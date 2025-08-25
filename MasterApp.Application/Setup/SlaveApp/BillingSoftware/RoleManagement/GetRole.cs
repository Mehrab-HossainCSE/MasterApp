using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.RoleManagement;

public class GetRole(IDbConnectionFactory _connectionFactory)
{
    public async Task<List<GetRoleBilling>> GetAllRolesAsync()
    {
        using var connection = _connectionFactory.CreateConnection("BillingSoft");

        var sql = "SELECT RoleId, RoleName FROM [Management].[Role_1]";

        var result = await connection.QueryAsync<GetRoleBilling>(sql);

        return result.AsList();
    }
}
