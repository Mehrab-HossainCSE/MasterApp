using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.UserManagementCloudPosReportHerlanCheck;

public class GetRoleDDCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{
    public async Task<List<RoleCloudPosReportHerlanCheckDDDto>> GetAllRolesAsync()
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");

        var sql = @"
         SELECT 
             r.ROLE_NAME,
             m.SERIAL,
             m.DESCRIPTION
         FROM ROLE_1 r
         JOIN MENU_1 m 
             ON ',' + r.MENULISTID + ',' LIKE '%,' + CAST(m.SERIAL AS VARCHAR) + ',%'
     ";

        var lookup = new Dictionary<string, RoleCloudPosReportHerlanCheckDDDto>();

        var result = await connection.QueryAsync<RoleCloudPosReportHerlanCheckDDDto, MenuRole, RoleCloudPosReportHerlanCheckDDDto>(
            sql,
            (role, menu) =>
            {
                if (!lookup.TryGetValue(role.ROLE_NAME, out var roleDto))
                {
                    roleDto = new RoleCloudPosReportHerlanCheckDDDto { ROLE_NAME = role.ROLE_NAME };
                    lookup[role.ROLE_NAME] = roleDto;
                }
                roleDto.menuRoles.Add(menu);
                return roleDto;
            },
            splitOn: "SERIAL"
        );

        return lookup.Values.ToList();
    }
}
