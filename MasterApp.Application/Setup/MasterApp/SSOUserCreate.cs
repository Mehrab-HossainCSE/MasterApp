using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
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
                
                BranchID =request.branch,
                NID= request.NID,
                FullName = request.employeeName,
                EMAIL = request.email,
                DES_ID = request.designationID,
                MOBILE = request.mobileNo,
                ADDRESS = request.address
            };
            var result = await vatProSoftUserCreate.CreateUserVatPro(dto,token);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Fail("Error: " + ex.Message);
        }
    }
}