using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using System.Reflection;
using System.Security.Cryptography;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace MasterApp.Application.Setup.MasterApp;

public class SSOUserCreate(IVatProSoftUserCreate vatProSoftUserCreate, IDbConnectionFactory _context, IEncryption encryption)
{
    public async Task<IResult> Handle(SSOUserCreateDto request)
    {
        try
        {
            using var connection = _context.CreateConnection("MasterAppDB");

            // 1. Get all projects
            var projectsQuery = "SELECT UserName,Password FROM ProjectList where Id=30";
            var projects = (await connection.QueryAsync <VatProTokenDto>(projectsQuery)).FirstOrDefault();
            var Password=  encryption.Decrypt(projects.password);
            var dtoToken = new VatProTokenDto
            {
                username = projects.username,
                password = Password
            };
            var token = await vatProSoftUserCreate.getVatProToken(dtoToken);
            var dto = new VatProUserCreateDto
            {

                USER_NAME = request.userName,
                USER_PASS=request.password,
                FullName=request.fullName,
                ExcelPermission=false,
                BranchID =request.branch,
                NID= request.NID,
                RoleId=request.RoleId,
                EMAIL = request.email,
                DES_ID = request.designationID,
                MOBILE = request.mobileNo,
                ADDRESS = request.address,
                IsActive= true,
                userImages = new UserImages
                {
                    UserImageId="string",
                    USER_ID="string",
                    UserImageData="stringsss",
                    NIDImageData="stringsss",
                    NIDImageData2="stringss"
                },
                OldPassword= "string",
                DES_TITLE= "string",
                RecordCount=0,
                RecordFilter=0,
                CREATE_BY = "string",
                CREATE_DATE = DateTime.Now,
                UPDATE_BY = "string",
                UPDATE_DATE = DateTime.Now
            };
            var result = await vatProSoftUserCreate.CreateUserVatPro(dto,token);


            if (result.Succeeded) // ✅ instead of result.IsSuccess
            {
                return Result.Success(result.Messages.FirstOrDefault() ?? "User created successfully");
            }
            else
            {
                return Result.Fail(result.Messages.FirstOrDefault() ?? "Failed to create user");
            }

        }
        catch (Exception ex)
        {
            return Result.Fail("Error: " + ex.Message);
        }
    }
}