using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using Newtonsoft.Json.Linq;
using System.ComponentModel.Design;
using System.Reflection;
using System.Security.Cryptography;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace MasterApp.Application.Setup.MasterApp;

public class SSOUserCreate(IVatProSoftUserCreate vatProSoftUserCreate, IDbConnectionFactory _context, IEncryption encryption, UserCreate userCreate, ISorolSoftUserCreate 
    sorolSoftUserCreate,IBillingSoftUserCreate billingSoftUserCreate)
{
    public async Task<IResult> Handle(SSOUserCreateDto request)
    {
        try
        {
            var projectIds = request.ProjectListId
                .Trim()
                .Split(',')
                .Select(int.Parse)
                .ToList();

            using var connection = _context.CreateConnection("MasterAppDB");

            var query = "SELECT Id, UserName as Username, Password FROM ProjectList";
            var allProjects = (await connection.QueryAsync<ProjectTokenDto>(query)).ToList();


            var tokenDictionary = allProjects.ToDictionary(p => p.Id, p => p);

            // Run all project tasks in parallel
            var tasks = projectIds.Select(async projectId =>
            {
                try
                {
                  
                    if (projectId == 1035)
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

                        if (dtoToken == null)
                            return new ProjectUserCreationResult { ProjectId = projectId, Title= projectConfig.Title, Success = false, Message = "Project config not found" };

                       

                        var token = await sorolSoftUserCreate.getSorolToken(dtoToken);
                        if (string.IsNullOrEmpty(token))
                            return new ProjectUserCreationResult { ProjectId = projectId, Success = false, Message = "Authentication failed / API unavailable" };

                        var dto = new SorolUserCreateDto
                        {
                            Username = request.userName,
                            Designation = request.RoleIdSorol,
                            Password = request.password,
                            CompanyId = request.companyIdSorol,
                            Menulist = request.sorolMenuIdList,
                        };

                        var result = await sorolSoftUserCreate.CreateUserSorol(dto, token);

                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,
                            Success = result.Succeeded,
                            Message = result.Messages.FirstOrDefault() ??
                                      (result.Succeeded ? "User created successfully" : "Failed to create user")
                        };

                    }


                  else  if (projectId == 30) // Example project VatPro
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

                        if (dtoToken == null)
                            return new ProjectUserCreationResult { ProjectId = projectId, Title = projectConfig.Title, Success = false, Message = "Project config not found" };
                    
                        var token = await vatProSoftUserCreate.getVatProToken(dtoToken);
                        if (string.IsNullOrEmpty(token))
                            return new ProjectUserCreationResult { ProjectId = projectId, Success = false, Message = "Authentication failed / API unavailable" };

                        var dto = new VatProUserCreateDto
                        {
                            USER_NAME = request.userName,
                            USER_PASS = request.password,
                            FullName = request.fullName,
                            ExcelPermission = false,
                            BranchID = request.branch,
                            NID = request.NID,
                            RoleId = request.RoleId,
                            EMAIL = request.email,
                            DES_ID = request.designationID,
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
                            CREATE_BY = "string",
                            CREATE_DATE = DateTime.Now,
                            UPDATE_BY = "string",
                            UPDATE_DATE = DateTime.Now
                        };

                        var result = await vatProSoftUserCreate.CreateUserVatPro(dto, token);

                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,
                            Success = result.Succeeded,
                            Message = result.Messages.FirstOrDefault() ??
                                      (result.Succeeded ? "User created successfully" : "Failed to create user")
                        };
                    }
                    else if (projectId == 25) // ✅ Local MasterAppDB user creation
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
                        var passwordEnc = encryption.Encrypt(request.password);
                        var newa = passwordEnc;
                        var dto = new UserCreateDto
                        {
                            UserName = request.userName,
                            ShopID = request.shopID,
                            EmployeeID = request.employeeID,
                            FullName = request.fullName,
                            Email = request.email,
                            DesignationID = request.designationID.ToString(),
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

                        var localResult = await userCreate.HandleAsync(dto);

                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,                      
                            Title = projectConfig.Title,
                            Success = localResult.Succeeded,
                            Message = localResult.Messages.FirstOrDefault() ??
                                      (localResult.Succeeded ? "User created in MasterAppDB successfully."
                                                             : "Failed to create user in MasterAppDB.")
                        };
                    }
                   
                    else if (projectId==26)
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

                        var dto = new BillingUserCreateDto
                        {
                            Username = request.userName,
                            FullName=request.fullName,
                            PhoneNo = request.mobileNo,
                            Password = request.password,
                            RoleId=request.RoleIdBilling,
                            IsActive=request.inActive.ToString(),
                            ExpairsOn=request.ExpairsOn,
                            IsMobileAppUser = request.IsMobileAppUser,
                            IMEI = request.IMEI,
                            PayrollUsername = request.userName,
                        };

                        var result = await billingSoftUserCreate.CreateUserBilling(dto);

                        return new ProjectUserCreationResult
                        {
                            ProjectId = projectId,                           
                            Title = projectConfig.Title,
                            Success = result.Succeeded,
                            Message = result.Messages.FirstOrDefault() ??
                                      (result.Succeeded ? "User created successfully" : "Failed to create user")
                        };
                    }
                    else
                    {
                        return new ProjectUserCreationResult { ProjectId = projectId, Success = false, Message = "Handler not implemented" };
                    }
                   

                }
                catch (Exception ex)
                {
                    return new ProjectUserCreationResult { ProjectId = projectId, Success = false, Message = $"Exception: {ex.Message}" };
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
                        ShopID = request.shopID,
                        EmployeeID = request.employeeID,
                        FullName = request.fullName,
                        Email = request.email,
                        DesignationID = request.designationID.ToString(),
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

                    var localResult = await userCreate.HandleAsync(dto);

                    return new ProjectUserCreationResult
                    {
                        ProjectId = 25,
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

            return Result<ProjectUserCreationResponse>.Success(response, "User creation process completed");
        }
        catch (Exception ex)
        {
            return Result<ProjectUserCreationResponse>.Fail("Fatal Error: " + ex.Message);
        }
    }


}