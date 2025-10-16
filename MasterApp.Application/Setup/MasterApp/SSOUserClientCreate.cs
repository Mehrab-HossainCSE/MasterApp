using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using System.Text.Json;

namespace MasterApp.Application.Setup.MasterApp;

public class SSOUserClientCreate(IVatProSoftUserCreate vatProSoftUserCreate, IDbConnectionFactory _context, IEncryption encryption, UserCreate userCreate, ISorolSoftUserCreate
    sorolSoftUserCreate, IBillingSoftUserCreate billingSoftUserCreate, ICloudePosUserCreate cloudePosUserCreate, UpdateMasterAppProjectID updateMasterAppProjectID)
{
    public async Task<IResult> Handle(SSOUserCreateClientDto request)
    {
        try
        {
            var projectIds = request.ProjectListId
                .Trim()
                .Split(',')
                .Select(int.Parse)
                .ToList();

           // var encrytption = encryption.Encrypt("123456");
           // Console.WriteLine(encrytption);
          //  var decryptedPassword = encryption.Decrypt(encrytption);
          //  Console.WriteLine(decryptedPassword);
            using var connection = _context.CreateConnection("MasterAppDB");

            var query = "SELECT Id, UserName as Username, Password FROM ProjectList";
            var allProjects = (await connection.QueryAsync<ProjectTokenDto>(query)).ToList();


            var tokenDictionary = allProjects.ToDictionary(p => p.Id, p => p);

            var menuListString = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT MenuIdList FROM RoleWiseMenu WHERE Id = @RoleId",
                new { request.RoleId }
            );

            // Step 2: Deserialize JSON into C# object
            var menuListFromDatabase = JsonSerializer.Deserialize<List<ProjectMenu>>(menuListString);

           


            // Run all project tasks in parallel
            var tasks = projectIds.Select(async projectId =>
            {
                try
                {

                    if (projectId == 5)
                    {
                        if (!tokenDictionary.TryGetValue(projectId, out var projectConfig))
                        {
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "Sorol",
                                Success = false,
                                Message = "Project config not found"
                            };
                        }
                        var decryptedPassword = encryption.Decrypt(projectConfig.Password);


                        var dtoToken = new SorolTokenDto
                        {
                            Username = projectConfig.Username,
                            Password = decryptedPassword
                        };

                        if (dtoToken == null)
                            return new ProjectUserCreationResult { ProjectId = projectId, ProjectName = "Sorol", Title = projectConfig.Title, Success = false, Message = "Project config not found" };



                        var token = await sorolSoftUserCreate.getSorolToken(dtoToken);
                        if (string.IsNullOrEmpty(token))
                            return new ProjectUserCreationResult { ProjectId = projectId, ProjectName = "Sorol", Success = false, Message = "Authentication failed / API unavailable" };

                        var companyInfo = await sorolSoftUserCreate.GetCompanySorol(token);
                        if(companyInfo == null || !companyInfo.Any())
                        {
                            return new ProjectUserCreationResult { ProjectId = projectId, ProjectName = "Sorol", Success = false, Message = "Failed to retrieve company info from Sorol" };
                        }
                        // ✅ Extract only menus for projectId == "5"
                        var projectMenu = menuListFromDatabase?.FirstOrDefault(x => x.projectId == "5");
                        var menuIds = projectMenu?.menuIds ?? new List<string>();
                        // Add leading '-' to the first menuId
                        var menuIdsString = menuIds.Count > 0
                            ? "-" + menuIds[0] + (menuIds.Count > 1 ? "," + string.Join(",", menuIds.Skip(1)) : "")
                            : "";
                        var dto = new SorolUserCreateDto
                        {
                            Username = request.userName,
                            Designation = request.RoleId.ToString(),
                            Password = request.password,
                            CompanyId = companyInfo,
                            Menulist = menuIdsString,
                        };

                        var result = await sorolSoftUserCreate.CreateUserSorol(dto, token);

                        return new ProjectUserCreationResult
                        {
                            ProjectName = "Sorol",
                            ProjectId = projectId,
                            Success = result.Succeeded,
                            Message = result.Messages.FirstOrDefault() ??
                                      (result.Succeeded ? "Sorol User created successfully" : "Sorol Failed to create user")
                        };

                    }
                    else if (projectId == 2)
                    {
                        return new ProjectUserCreationResult
                        {
                            ProjectName = "MIS",
                            ProjectId = projectId,
                            Success = true,
                            Message = "MIS User created successfully"
                        };
                    }

                    else if (projectId == 1)
                    {
                        if (!tokenDictionary.TryGetValue(projectId, out var projectConfig))
                        {
                            return new ProjectUserCreationResult
                            {
                                ProjectName = "CloudPos",
                                ProjectId = projectId,
                                Success = false,
                                Message = "Project config not found"
                            };
                        }
                        var decryptedPassword = encryption.Decrypt(projectConfig.Password);


                        var dtoToken = new CloudPosApiKeyDto
                        {
                            username = projectConfig.Username,
                            password = decryptedPassword
                        };

                        if (dtoToken == null)
                            return new ProjectUserCreationResult { ProjectId = projectId, ProjectName = "CloudPos", Title = projectConfig.Title, Success = false, Message = "Project config not found" };



                        var apiKey = await cloudePosUserCreate.GetCloudePosApiKey(dtoToken);
                        if (string.IsNullOrEmpty(apiKey))
                            return new ProjectUserCreationResult { ProjectId = projectId, ProjectName = "CloudPos", Success = false, Message = "Authentication failed / API unavailable" };

                        var dto = new CloudPosUserDto
                        {
                            UserName = request.userName,
                            Name = request.userName,
                            ApiKeyUser = dtoToken.username,
                            Password = request.password,
                            City= "Dhaka",
                            Address = "Bangladesh",
                            Phone = "01711111111",
                        };

                        var result = await cloudePosUserCreate.CreateUserCloudePos(dto, apiKey);
                        if (!result.Succeeded)
                        {
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "CloudPos",
                                Success = false,
                                Message = result.Messages.FirstOrDefault() ?? "Failed to create user in CloudPOS"
                            };
                        }
                        var dtoMenu = new MenuCreateCoudPos
                        {
                            UserName = request.userName,
                            MenuIdList = menuListFromDatabase?.FirstOrDefault(x => x.projectId == "1")?.menuIds.Select(int.Parse).ToList() ?? new List<int>(),
                            ApiKeyUser = dtoToken.username
                        };
                        var menuResult = await cloudePosUserCreate.CreateMenuCloudePos(dtoMenu, apiKey);

                        if (menuResult.Succeeded)
                        {
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "CloudPos",
                                Success = true,
                                Message = "User and menu created successfully"
                            };
                        }
                        else
                        {
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "CloudPos",
                                Success = false,
                                Message = menuResult.Messages.FirstOrDefault() ?? "User created but failed to create menu"
                            };
                        }

                    }
                 
                   

                    else if (projectId == 3)
                    {

                        if (!tokenDictionary.TryGetValue(projectId, out var projectConfig))
                        {
                            return new ProjectUserCreationResult
                            {
                                ProjectId = projectId,
                                ProjectName = "Billing",
                                Success = false,
                                Message = "Project config not found"
                            };
                        }

                        var dto = new BillingUserCreateDto
                        {
                            Username = request.userName,
                            FullName = request.userName,
                            PhoneNo = "01714535595",
                            Password = request.password,
                            RoleId = request.RoleId.ToString(),
                            IsActive = "true",
                            ExpairsOn = "2028/07/02",
                            IsMobileAppUser = true,
                            IMEI = "5639303",
                            PayrollUsername = request.userName,
                        };

                        var result = await billingSoftUserCreate.CreateUserBilling(dto);

                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,
                            ProjectName = "Billing",
                            Title = projectConfig.Title,
                            Success = result.Succeeded,
                            Message = result.Messages.FirstOrDefault() ??
                                      (result.Succeeded ? "User created successfully" : "Failed to create user")
                        };
                    }
                    else
                    {
                        return new ProjectUserCreationResult { ProjectName = "Billing", Success = false, Message = "Handler not implemented" };
                    }


                }
                catch (Exception ex)
                {
                    return new ProjectUserCreationResult { ProjectName = "Billing", Success = false, Message = $"Exception: {ex.Message}" };
                }
            }).ToList();

            if (!projectIds.Contains(25))
            {


                tasks.Add(Task.Run(async () =>
                {
                    var passwordEnc = encryption.Encrypt(request.password);
                    var dto = new UserCreateDto
                    {
                        UserName = request.userName,
                        FullName = request.userName,
                        RoleId = request.RoleId,
                        CreateBy = "Admin",
                        CreateDate = DateTime.Now,
                        UpdateBy = "Admin",
                        UpdateDate = DateTime.Now,
                       
                        Password = request.password,
                        ProjectListId = request.ProjectListId,
                        PasswordEncrypted = passwordEnc,

                    };

                    var localResult = await userCreate.HandleAsync(dto);

                    return new ProjectUserCreationResult
                    {
                        ProjectId = 25,
                        ProjectName = "MasterApp",
                        Success = localResult.Succeeded,
                        Message = localResult.Messages.FirstOrDefault() ??
                                  (localResult.Succeeded ? "User created in MasterAppDB successfully." : "Failed to create user in MasterAppDB.")
                    };
                }));
            }


            var results = await Task.WhenAll(tasks);

            var response = new ProjectUserCreationResponse
            {
                SuccessfulProjects = results.Where(r => r.Success),
                FailedProjects = results.Where(r => !r.Success)
            };
            var successfulProjectIds = response.SuccessfulProjects
               .Where(p => projectIds.Contains(p.ProjectId))
                .Select(p => p.ProjectId)
                .ToList();
            await updateMasterAppProjectID.UpdateMasterAppProjectListAsync(
                 request.userName,
                 successfulProjectIds
             );

            return Result<ProjectUserCreationResponse>.Success(response, "User creation process completed");
        }
        catch (Exception ex)
        {
            return Result<ProjectUserCreationResponse>.Fail("Fatal Error: " + ex.Message);
        }
    }
}
