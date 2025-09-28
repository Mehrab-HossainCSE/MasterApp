using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement
{
    public class GetRoleSorolUserAssing(IDbConnectionFactory _connectionFactory)
    {
        public async Task<List<GetSorolRoleDto>> GetAllRolesAsync()
        {
            using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

            var sql = "SELECT RoleId, RoleName,MenuListID ,IsActive FROM [RoleMenu]";

            var result = await connection.QueryAsync<GetSorolRoleDto>(sql);

            return result.AsList();
        }
    }
}
