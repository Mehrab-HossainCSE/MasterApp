using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.MasterApp;

public class SSOUserUpdate(IVatProSoftUserCreate vatProSoftUserCreate, IDbConnectionFactory _context, IEncryption encryption, UserCreate userCreate, ISorolSoftUserCreate
    sorolSoftUserUpdate, IBillingSoftUserCreate billingSoftUserCreate, IDbConnectionFactory _dbConnectionFactory, UpdateMasterUser userUpdate, GetUserByUserName getUserByUserName)
{

    public async Task<IResult> Handle(SSOUserUpdateDto request)
    {
        try
        {
            var projectIds = request.ProjectListId
                .Trim()
                .Split(',')
                .Select(int.Parse)
                .ToList();

            using var connection = _context.CreateConnection("MasterAppDB");

            var query = "SELECT Id, UserName as Username, Password, Title FROM ProjectList";
            var allProjects = (await connection.QueryAsync<ProjectTokenDto>(query)).ToList();

            var tokenDictionary = allProjects.ToDictionary(p => p.Id, p => p);

            // Run all project tasks in parallel
            var tasks = projectIds.Select(async projectId =>
            {
                try
                {
                    if (projectId == 1035) // Sorol Project
                    {
                        return await HandleSorolUserUpdate(projectId, tokenDictionary, request);
                    }
                    else if (projectId == 30) // VatPro Project
                    {
                        return await HandleVatProUserUpdate(projectId, tokenDictionary, request);
                    }
                    else if (projectId == 25) // Local MasterAppDB
                    {
                        return await HandleMasterAppUserUpdate(projectId, tokenDictionary, request, _dbConnectionFactory, userUpdate);
                    }
                    else if (projectId == 26) // Billing Project
                    {
                        return await HandleBillingUserUpdate(projectId, tokenDictionary, request, getUserByUserName);
                    }
                    else
                    {
                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,
                            Success = false,
                            Message = "Handler not implemented"
                        };

                    }
                }
                catch (Exception ex)
                {
                    return new ProjectUserCreationResult
                    {
                        ProjectId = projectId,
                        Success = false,
                        Message = $"Exception: {ex.Message}"
                    };
                }
            }).ToList();

            // Always handle MasterApp if not in project list
            if (!projectIds.Contains(25))
            {
                tasks.Add(Task.Run(async () =>
                {
                    return await HandleMasterAppUserUpdate(25, tokenDictionary, request, _dbConnectionFactory, userUpdate);
                }));
            }

            var results = await Task.WhenAll(tasks);

            var response = new ProjectUserCreationResponse
            {
                SuccessfulProjects = results.Where(r => r.Success),
                FailedProjects = results.Where(r => !r.Success)
            };

            return Result<ProjectUserCreationResponse>.Success(response, "User update process completed");
        }
        catch (Exception ex)
        {
            return Result<ProjectUserCreationResponse>.Fail("Fatal Error: " + ex.Message);
        }
    }



    #region Sorol Project Handler
    private async Task<ProjectUserCreationResult> HandleSorolUserUpdate(
        int projectId,
        Dictionary<int, ProjectTokenDto> tokenDictionary,
        SSOUserUpdateDto request)
    {
        if (!tokenDictionary.TryGetValue(projectId, out var projectConfig))
        {
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
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

        var token = await sorolSoftUserUpdate.getSorolToken(dtoToken);
        if (string.IsNullOrEmpty(token))
        {
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Success = false,
                Message = "Authentication failed / API unavailable"
            };
        }

        // Check if user exists
        var existingUser = await sorolSoftUserUpdate.GetUserByUsername(request.userName);

        if (existingUser == null)
        {
            // User doesn't exist, create new user
            var createDto = new SorolUserCreateDto
            {
                Username = request.userName,
                Designation = request.RoleIdSorol,
                Password = request.password,
                CompanyId = request.companyIdSorol,
                Menulist = request.sorolMenuIdList,
            };

            var createResult = await sorolSoftUserUpdate.CreateUserSorol(createDto, token);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig.Title,
                Success = createResult.Succeeded,
                Message = createResult.Succeeded ? "User created successfully" : "Failed to create user"
            };
        }
        else
        {
            // User exists, update with merged data
            var updateDto = new SorolUserUpdateDto
            {
                Username = request.userName,
                Designation = !string.IsNullOrEmpty(request.RoleIdSorol) ? request.RoleIdSorol : existingUser.Designation,
                Password = request.password,
                CompanyId = !string.IsNullOrEmpty(request.companyIdSorol) ? request.companyIdSorol : existingUser.CompanyId,
                Menulist = !string.IsNullOrEmpty(request.sorolMenuIdList) ? request.sorolMenuIdList : existingUser.MenuIdList,
            };

            var updateResult = await sorolSoftUserUpdate.UpdateUserSorol(updateDto, token);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig.Title,
                Success = updateResult.Succeeded,
                Message = updateResult.Succeeded ? "User updated successfully" : "Failed to update user"
            };
        }
    }
    #endregion

    #region VatPro Project Handler
    private async Task<ProjectUserCreationResult> HandleVatProUserUpdate(
        int projectId,
        Dictionary<int, ProjectTokenDto> tokenDictionary,
        SSOUserUpdateDto request)
    {
        if (!tokenDictionary.TryGetValue(projectId, out var projectConfig))
        {
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Success = false,
                Message = "Project config not found"
            };
        }

        var decryptedPassword = encryption.Decrypt(projectConfig.Password);
        var dtoToken = new VatProTokenDto
        {
            username = projectConfig.Username,
            password = decryptedPassword
        };

        var token = await vatProSoftUserCreate.getVatProToken(dtoToken);
        if (string.IsNullOrEmpty(token))
        {
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Success = false,
                Message = "Authentication failed / API unavailable"
            };
        }

        // Check if user exists
        var existingUser = await vatProSoftUserCreate.GetUserByUsername(request.userName, token);

        if (existingUser == null)
        {
            // User doesn't exist, create new user
            var createDto = new VatProUserCreateDto
            {
                USER_NAME = request.userName,
                USER_PASS = request.password,
                FullName = request.fullName,
                ExcelPermission = false,
                BranchID = request.branch,
                NID = request.NID,
                RoleId = request.RoleId ?? 0,
                EMAIL = request.email,
                DES_ID = request.designationID ?? 0,
                MOBILE = request.mobileNo,
                ADDRESS = request.address,
                IsActive = true,
                userImages = new UserImages
                {
                    UserImageId = "string",
                    USER_ID = "string",
                    UserImageData = "stringsss",
                    NIDImageData = "stringsss",
                    NIDImageData2 = "stringss"
                },
                OldPassword = "string",
                DES_TITLE = "string",
                RecordCount = 0,
                RecordFilter = 0,
                CREATE_BY = "Admin",
                CREATE_DATE = DateTime.Now,
                UPDATE_BY = "Admin",
                UPDATE_DATE = DateTime.Now
            };

            var createResult = await vatProSoftUserCreate.CreateUserVatPro(createDto, token);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig.Title,
                Success = createResult.Succeeded,
                Message = createResult.Succeeded ? "User created successfully" : "Failed to create user"
            };
        }
        else
        {
            // User exists, update with merged data
            var updateDto = new VatProUserUpdateDto
            {
                USER_NAME = request.userName,
                USER_PASS = request.password,
                FullName = !string.IsNullOrEmpty(request.fullName) ? request.fullName : existingUser.FullName,
                ExcelPermission = existingUser.ExcelPermission,
                BranchID = request.branch ?? existingUser.BranchID,
                NID = !string.IsNullOrEmpty(request.NID) ? request.NID : existingUser.NID,
                RoleId = request.RoleId ?? existingUser.RoleId,
                EMAIL = !string.IsNullOrEmpty(request.email) ? request.email : existingUser.EMAIL,
                DES_ID = request.designationID ?? existingUser.DES_ID,
                MOBILE = !string.IsNullOrEmpty(request.mobileNo) ? request.mobileNo : existingUser.MOBILE,
                ADDRESS = !string.IsNullOrEmpty(request.address) ? request.address : existingUser.ADDRESS,
                IsActive = true,
                UPDATE_BY = "Admin",
                UPDATE_DATE = DateTime.Now
            };

            var updateResult = await vatProSoftUserCreate.UpdateUserVatPro(updateDto, token);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig.Title,
                Success = updateResult.Succeeded,
                Message = updateResult.Succeeded ? "User updated successfully" : "Failed to update user"
            };
        }
    }
    #endregion

    #region MasterApp Project Handler
    private async Task<ProjectUserCreationResult> HandleMasterAppUserUpdate(
        int projectId,
        Dictionary<int, ProjectTokenDto> tokenDictionary,
        SSOUserUpdateDto request, IDbConnectionFactory _dbConnectionFactory, UpdateMasterUser userUpdate)
    {
        tokenDictionary.TryGetValue(projectId, out var projectConfig);

        // Check if user exists in MasterApp

        using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

        var sql = @"SELECT UserID, UserName, EmployeeID, ShopID, FullName, 
                   Email, DesignationID, MobileNo, Address  
            FROM Users 
            WHERE UserName = @UserName";

        var users = (await connection.QueryAsync<UserCreateDto>(sql, new { UserName = request.userName }))
                    .ToList();

        if (!users.Any())
        {
            // No user found
            return null;
        }

        // User found (take first since username should be unique)
        var existingUser = users.First();


        if (existingUser == null)
        {
            // User doesn't exist, create new user
            var passwordEnc = encryption.Encrypt(request.password);
            var createDto = new UserCreateDto
            {
                UserName = request.userName,
               
                FullName = request.fullName,
                Email = request.email,
                DesignationID = request.designationID,
                MobileNo = request.mobileNo,
                Address = request.address,
                CreateBy = "Admin",
                CreateDate = DateTime.Now,
                UpdateBy = "Admin",
                UpdateDate = DateTime.Now,
                InActive = false,
                Password = request.password,
                ProjectListId = request.ProjectListId,
                PasswordEncrypted = passwordEnc
            };

            var createResult = await userCreate.HandleAsync(createDto);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig?.Title ?? "MasterAppDB",
                Success = createResult.Succeeded,
                Message = createResult.Succeeded ? "User created in MasterAppDB successfully." : "Failed to create user in MasterAppDB."
            };
        }
        else
        {
            // User exists, update with merged data
            var passwordEnc = !string.IsNullOrEmpty(request.password) ? encryption.Encrypt(request.password) : existingUser.PasswordEncrypted;
            var updateDto = new UserUpdateDto
            {
                UserName = request.userName,
             
                FullName = !string.IsNullOrEmpty(request.fullName) ? request.fullName : existingUser.FullName,
                Email = !string.IsNullOrEmpty(request.email) ? request.email : existingUser.Email,
                DesignationID = (request.designationID ?? existingUser?.DesignationID),
                MobileNo = !string.IsNullOrEmpty(request.mobileNo) ? request.mobileNo : existingUser.MobileNo,
                Address = !string.IsNullOrEmpty(request.address) ? request.address : existingUser.Address,
                UpdateBy = "Admin",
                UpdateDate = DateTime.Now,
                InActive = request.inActive ?? existingUser.InActive,
                Password = !string.IsNullOrEmpty(request.password) ? request.password : existingUser.Password,
                ProjectListId = !string.IsNullOrEmpty(request.ProjectListId) ? request.ProjectListId : existingUser.ProjectListId,
                PasswordEncrypted = passwordEnc
            };

            var updateResult = await userUpdate.HandleAsync(updateDto);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig?.Title ?? "MasterAppDB",
                Success = updateResult.Succeeded,
                Message = updateResult.Succeeded ? "User updated in MasterAppDB successfully." : "Failed to update user in MasterAppDB."
            };
        }
    }
    #endregion

    #region Billing Project Handler
    private async Task<ProjectUserCreationResult> HandleBillingUserUpdate(
        int projectId,
        Dictionary<int, ProjectTokenDto> tokenDictionary,
        SSOUserUpdateDto request, GetUserByUserName getUserByUserName)
    {
        if (!tokenDictionary.TryGetValue(projectId, out var projectConfig))
        {
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Success = false,
                Message = "Project config not found"
            };
        }

        // Check if user exists
        var existingUser = await getUserByUserName.GetUserAsync(request.userName);

        if (existingUser == null)
        {
            // User doesn't exist, create new user
            var createDto = new BillingUserCreateDto
            {
                Username = request.userName,
                FullName = request.fullName,
                PhoneNo = request.mobileNo,
                Password = request.password,
                RoleId = request.RoleIdBilling,
                IsActive = request.inActive?.ToString() ?? "true",
                ExpairsOn = request.ExpairsOn,
                IsMobileAppUser = request.IsMobileAppUser ?? false,
                IMEI = request.IMEI,
                PayrollUsername = request.userName,
            };

            var createResult = await billingSoftUserCreate.CreateUserBilling(createDto);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig.Title,
                Success = createResult.Succeeded,
                Message = createResult.Succeeded ? "User created successfully" : "Failed to create user"
            };
        }
        else
        {
            // User exists, update with merged data
            var updateDto = new BillingUserUpdateDto
            {
                Username = request.userName,
                FullName = !string.IsNullOrEmpty(request.fullName) ? request.fullName : existingUser.FullName,
                PhoneNo = !string.IsNullOrEmpty(request.mobileNo) ? request.mobileNo : existingUser.PhoneNo,
                Password = !string.IsNullOrEmpty(request.password) ? request.password : existingUser.Password,
                RoleId = request.RoleIdBilling ?? existingUser.RoleId,
                IsActive = request.inActive?.ToString() ?? existingUser.IsActive,
                ExpairsOn = request.ExpairsOn ?? existingUser.ExpairsOn,
                IsMobileAppUser = request.IsMobileAppUser ?? existingUser.IsMobileAppUser,
                IMEI = request.IMEI ?? existingUser.IMEI,
                PayrollUsername = request.userName,
            };

            var updateResult = await billingSoftUserCreate.UpdateUserBilling(updateDto);
            return new ProjectUserCreationResult
            {
                ProjectId = projectId,
                Title = projectConfig.Title,
                Success = updateResult.Succeeded,
                Message = updateResult.Succeeded ? "User updated successfully" : "Failed to update user"
            };
        }
    }
    #endregion
}


