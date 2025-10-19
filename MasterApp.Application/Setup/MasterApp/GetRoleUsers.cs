using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp;

public class GetRoleUsers(IDbConnectionFactory _connectionFactory)
{
    public async Task<List<MasterAppRoleDto>> GetAllRolesAsync()
    {
        using var connection = _connectionFactory.CreateConnection("MasterAppDB");

        var sql = "SELECT Id, RoleName  FROM [dbo].[RoleWiseMenu] WHERE MenuIdList IS NOT NULL";

        var result = await connection.QueryAsync<MasterAppRoleDto>(sql);

        return result.AsList();
    }
}