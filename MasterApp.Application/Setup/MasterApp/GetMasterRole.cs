using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.MasterApp;

public class GetMasterRole(IDbConnectionFactory _connectionFactory)
{
    public async Task<List<MasterAppRoleDto>> GetAllRolesAsync()
    {
        using var connection = _connectionFactory.CreateConnection("MasterAppDB");

        var sql = "SELECT Id, RoleName  FROM [dbo].[RoleWiseMenu]";

        var result = await connection.QueryAsync<MasterAppRoleDto>(sql);

        return result.AsList();
    }
}