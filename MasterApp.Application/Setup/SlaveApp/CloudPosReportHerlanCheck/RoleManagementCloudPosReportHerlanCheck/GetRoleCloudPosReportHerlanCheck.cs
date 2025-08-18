using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.RoleManagementCloudPosReportHerlanCheck;

public class GetRoleCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{

    public async Task<List<RoleCreateCloudPosReportHerlanCheckDto>> GetAllRolesAsync()
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");

        var sql = "SELECT  ROLE_NAME, MENULISTID FROM ROLE_1";

        var result = await connection.QueryAsync<RoleCreateCloudPosReportHerlanCheckDto>(sql);

        return result.AsList();
    }
}