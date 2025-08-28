using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp;

public class GetAllUser(IDbConnectionFactory _dbConnectionFactory)
{
    public async Task<Result<List<UserCreateDto>>> HandleAsync()
    {
        try
        {  

            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");
            var sql = "SELECT UserID, UserName, EmployeeID, ShopID, FullName, Email,DesignationID,MobileNo,Address  FROM Users";
            var users = await connection.QueryAsync<UserCreateDto>(sql);
            return Result<List<UserCreateDto>>.Success(users.ToList());
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return Result<List<UserCreateDto>>.Fail("Error: " + ex.Message);
        }
    }
}
