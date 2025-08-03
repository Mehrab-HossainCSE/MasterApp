using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetRoleCloudPosDBKMART
{
    
         
        private readonly IDbConnectionFactory _connectionFactory;

        public GetRoleCloudPosDBKMART(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<RoleCreateDto>> GetAllRolesAsync()
        {
            using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");

            var sql = "SELECT ID, ROLENAME, MENULISTID FROM ROLE_1"; 

            var result = await connection.QueryAsync<RoleCreateDto>(sql);

            return result.AsList();
        }
    
}

