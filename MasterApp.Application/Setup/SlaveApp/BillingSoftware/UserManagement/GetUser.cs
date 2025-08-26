using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;

public class GetUser
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetUser(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<List<BillingUserDto>> GetAllUserAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection("BillingSoft");

        var sql = "SELECT Id,Username ,RoleId FROM [Management].[User]";

        var result = await connection.QueryAsync<BillingUserDto>(sql);

        return result.AsList();
    }
}
