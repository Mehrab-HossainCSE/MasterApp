using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using Microsoft.Extensions.Configuration;

namespace MasterApp.Application.Setup.MasterApp
{
    public class ChangePasswordSSOClient
    {
        private readonly IDbConnectionFactory _context;
        private readonly IPasswordHash _passwordHash;
        private readonly IConfiguration _configuration;
        private readonly ISorolSoftUserCreate _sorolSoftUserCreate;
        private readonly IBillingSoftUserCreate _billingSoftUserCreate;
        private readonly ICloudePosUserCreate _cloudePosUserCreate;
        private readonly IEncryption _encryption;

        public ChangePasswordSSOClient(
            IDbConnectionFactory context,
            IPasswordHash passwordHash,
            IConfiguration configuration,
            ISorolSoftUserCreate sorolSoftUserCreate,
            IBillingSoftUserCreate billingSoftUserCreate,
            ICloudePosUserCreate cloudePosUserCreate,
            IEncryption encryption)
        {
            _context = context;
            _passwordHash = passwordHash;
            _configuration = configuration;
            _sorolSoftUserCreate = sorolSoftUserCreate;
            _billingSoftUserCreate = billingSoftUserCreate;
            _cloudePosUserCreate = cloudePosUserCreate;
            _encryption = encryption;
        }

        public async Task<IResult> Handle(ChangPasswordDto request)
        {
            try
            {
                using var connection = _context.CreateConnection("MasterAppDB");

                // Step 1: Get user details
                var user = await connection.QueryFirstOrDefaultAsync<PasswordDetails>(
                    "SELECT UserName, PasswordHash, PasswordSalt FROM Users WHERE UserName = @UserName",
                    new { request.UserName });

                if (user == null)
                    return Result.Fail("User not found");

                // Step 2: Validate old password
                if (!_passwordHash.ValidatePassword(request.previousPassword, user.PasswordHash, user.PasswordSalt))
                    return Result.Fail("Previous password is not valid");

                // Step 3: Get assigned projects
                var projectListString = await connection.QueryFirstOrDefaultAsync<string>(
                    "SELECT ProjectListId FROM Users WHERE UserName = @UserName",
                    new { request.UserName });

                if (string.IsNullOrWhiteSpace(projectListString))
                    return Result.Fail("User not assigned to any project");

                var allProjectIds = projectListString
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();

                // Step 4: Update master DB password
                string newHash = string.Empty;
                string newSalt = string.Empty;
                _passwordHash.CreateHash(request.confirmNewPassword, ref newHash, ref newSalt);

                await connection.ExecuteAsync(
                    "UPDATE Users SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt WHERE UserName = @UserName",
                    new { PasswordHash = newHash, PasswordSalt = newSalt, request.UserName });

                // Step 5: Load project credentials
                var allProjects = (await connection.QueryAsync<ProjectTokenDto>(
                    "SELECT Id, UserName AS Username, Password FROM ProjectList")).ToList();

                var tokenDictionary = allProjects.ToDictionary(p => p.Id, p => p);

                // ✅ Step 6: Create readable project name map
                var projectNames = new Dictionary<int, string>
                {
                    { 1, "CloudPOS" },
                    { 2, "BillingSoft" },
                    { 5, "SorolSoft" }
                };

                // Step 7: Call project APIs in parallel
                var tasks = allProjectIds.Select(projectId =>
                    UpdatePasswordInProjectAsync(projectId, request.UserName, request.previousPassword, request.newPassword, tokenDictionary)
                );

                var projectResults = await Task.WhenAll(tasks);

                // ✅ Step 8: Build readable summary
                var success = projectResults
                    .Where(x => x.Success)
                    .Select(x => projectNames.ContainsKey(x.ProjectId) ? projectNames[x.ProjectId] : $"Project-{x.ProjectId}");

                var failed = projectResults
                    .Where(x => !x.Success)
                    .Select(x => $"{(projectNames.ContainsKey(x.ProjectId) ? projectNames[x.ProjectId] : $"Project-{x.ProjectId}")}: {x.Message}");

                string summary = $"✅ Master password updated.\n" +
                                 $"Success: {string.Join(", ", success)}\n" +
                                 $"Failed: {string.Join(", ", failed)}";

                return Result.Success(summary);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error: {ex.Message}");
            }
        }

        private async Task<ProjectUserCreationResult> UpdatePasswordInProjectAsync(
            int projectId,
            string userName,
            string previousPassword,
            string newPassword,
            Dictionary<int, ProjectTokenDto> tokenDictionary)
        {
            try
            {
                HttpResponseMessage? response = null;

                switch (projectId)
                {
                    case 1:
                        // CloudPOS API call here
                        break;

                    case 2:
                        // Billing API call here
                        break;

                    case 5:
                        // SorolSoft API
                        if (!tokenDictionary.TryGetValue(projectId, out var config))
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "SorolSoft",
                                Success = false,
                                Message = "Sorol token not found"
                            };

                        var decryptedPass = _encryption.Decrypt(config.Password);
                        var tokenDto = new SorolTokenDto { Username = config.Username, Password = decryptedPass };
                        var token = await _sorolSoftUserCreate.getSorolToken(tokenDto);

                        if (string.IsNullOrEmpty(token))
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "SorolSoft",
                                Success = false,
                                Message = "Authentication failed"
                            };

                        var sorolResponse = await _sorolSoftUserCreate.UpdatePasswordSorol(
                            new SorolUserUpdateDto { Username = userName, Password = newPassword },
                            token
                        );

                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,
                            ProjectName = "SorolSoft",
                            Success = sorolResponse.Succeeded,
                            Message = sorolResponse.Succeeded
                                ? "Password updated successfully"
                                : "Failed to update user password"
                        };

                    default:
                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,
                            ProjectName = $"Project-{projectId}",
                            Success = false,
                            Message = $"No API handler found for Project ID {projectId}"
                        };
                }

                // Default fallback
                return new ProjectUserCreationResult
                {
                    ProjectId = projectId,
                    Success = response != null && response.IsSuccessStatusCode,
                    Message = response != null
                        ? (await response.Content.ReadAsStringAsync())
                        : "No response from API"
                };
            }
            catch (Exception ex)
            {
                return new ProjectUserCreationResult
                {
                    ProjectId = projectId,
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
