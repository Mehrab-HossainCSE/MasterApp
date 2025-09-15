using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;

public class GetUserByUserName
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetUserByUserName(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<BillingUserUpdateDto?> GetUserAsync(string userName)
    {
        using var connection = _dbConnectionFactory.CreateConnection("BillingSoft");

        var sql = "SELECT Id, Username, RoleId FROM [Management].[User] WHERE Username = @UserName";

        var result = await connection.QueryFirstOrDefaultAsync<BillingUserUpdateDto>(sql, new { UserName = userName });

        return result;
    }
}
