using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement
{
    public class GetSorolRole(IDbConnectionFactory _connectionFactory)
    {
        public async Task<List<CreateSorolRoleDto>> GetAllRolesAsync()
        {
            using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

            var sql = "SELECT RoleId, RoleName ,IsActive FROM [RoleMenu]";

            var result = await connection.QueryAsync<CreateSorolRoleDto>(sql);

            return result.AsList();
        }
    }
}
