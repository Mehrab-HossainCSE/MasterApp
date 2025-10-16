using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp;

public class GetAllUser(IDbConnectionFactory _dbConnectionFactory)
{
    public async Task<Result<List<GetUseDto>>> HandleAsync()
    {
        try
        {  

            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");
            var sql = @"
                    SELECT 
                       UserID,
                       UserName,
                      
                      
                       FullName,
                       Email,
                       RoleId,
                       MobileNo,
                       Address,
                       
                       ProjectListId,
                       RoleIdBilling,
                       ExpairsOnBilling as ExpairsOn,
                       IsMobileAppUserBilling as IsMobileAppUser,
                       IMEIBilling as IMEI,
                       RoleIdSorol,
                       CompanyIdSorol,
                       DES_IDVatPro,
                       RoleIdVatPro as RoleId,
                       NIDVatPro as NID,
                       BranchIDVatPro as branch,
                       
                       BranchVatPro as branch,
                       CityCloudPos as City,
                       StatusBilling
                    FROM Users";
            var users = await connection.QueryAsync<GetUseDto>(sql);
            return Result<List<GetUseDto>>.Success(users.ToList());
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return Result<List<GetUseDto>>.Fail("Error: " + ex.Message);
        }
    }
}
