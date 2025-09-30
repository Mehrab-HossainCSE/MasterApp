using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.AspNetCore.Http;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Linq;

namespace MasterApp.Application.Setup.MasterApp;

public class ClientAdminUserCreate(IPasswordHash _passwordHash, IDbConnectionFactory _dbConnectionFactory, IEncryption _encryption)
{
    public async Task<Result<string>> HandleAsync(ClientAdminUserCreateDto userDto)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");
            connection.Open();

            using var transaction = connection.BeginTransaction();

            // ✅ Step 1: Check if user exists
            var checkUserExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Users WHERE UserName = @UserName",
                new { userDto.UserName }, transaction
            );

            if (checkUserExists > 0)
            {
                return Result<string>.Fail("User already exists.");
            }

            // ✅ Step 2: Generate new UserID
            var maxUserId = await connection.ExecuteScalarAsync<string>(
                "SELECT TOP 1 UserID FROM Users ORDER BY UserID DESC",
                transaction: transaction
            );

            if (string.IsNullOrEmpty(maxUserId))
            {
                maxUserId = "1001";
            }
            else if (int.TryParse(maxUserId, out int currentMaxId))
            {
                maxUserId = (currentMaxId + 1).ToString("D4");
            }
            else
            {
                maxUserId = "1001";
            }

            // ✅ Step 3: Hash password
            string passwordHash = string.Empty;
            string passwordSalt = string.Empty;
            _passwordHash.CreateHash(userDto.Password, ref passwordHash, ref passwordSalt);

            // ✅ Step 4: Insert user
            var userSql = @"INSERT INTO Users (UserID, UserName, PasswordHash, PasswordSalt, MenuList,ProjectListId) 
                        VALUES (@UserID, @UserName, @PasswordHash, @PasswordSalt, @MenuList,@ProjectListId)";

            await connection.ExecuteAsync(userSql, new
            {
                UserID = maxUserId,
                userDto.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                userDto.MenuList,
                ProjectListId = string.Join(",", userDto.projectJsonDtos.Select(x => x.Id)) // ✅ comma separated IDs
            }, transaction);


            // ✅ Step 5: Insert projects if not already in ProjectList
            if (userDto.projectJsonDtos != null && userDto.projectJsonDtos.Any())
            {
                foreach (var project in userDto.projectJsonDtos)
                {
                    // Check if project exists
                    

                  
                        var projectSql = @"
                        INSERT INTO ProjectList (Id, Title, NavigateUrl, LoginUrl, LogoUrl, IsActive, UserName, Password) 
                        VALUES (@Id, @Title, @NavigateUrl, @LoginUrl, @LogoUrl, @IsActive, @UserName, @Password)";

                        await connection.ExecuteAsync(projectSql, new
                        {
                            project.Id,
                            project.Title,
                            project.NavigateUrl,
                            project.LoginUrl,
                            project.LogoUrl,  // ✅ use directly from DTO
                            IsActive = true,
                            project.UserName,
                            Password = string.IsNullOrWhiteSpace(project.Password) ? null :(project.Password)
                        }, transaction);
                    

                    // ✅ Step 6: Insert mapping User ↔ Project
                   
                }
            }

            transaction.Commit();
            return Result<string>.Success($"User created successfully with ID {maxUserId}.");
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Error: " + ex.Message);
        }
    }



}
