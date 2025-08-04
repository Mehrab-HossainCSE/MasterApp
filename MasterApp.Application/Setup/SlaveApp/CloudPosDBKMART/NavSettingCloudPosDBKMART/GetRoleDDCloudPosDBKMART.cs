using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetRoleDDCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetRoleDDCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<RoleDDDto>> GetAllRolesAsync()
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");

        var sql = @"
            SELECT 
                r.ROLENAME,
                m.SERIAL,
                m.DESCRIPTION
            FROM ROLE_1 r
            JOIN MENU_1 m 
                ON ',' + r.MENULISTID + ',' LIKE '%,' + CAST(m.SERIAL AS VARCHAR) + ',%'
        ";

        var lookup = new Dictionary<string, RoleDDDto>();

        var result = await connection.QueryAsync<RoleDDDto, MenuRole, RoleDDDto>(
            sql,
            (role, menu) =>
            {
                if (!lookup.TryGetValue(role.ROLENAME, out var roleDto))
                {
                    roleDto = new RoleDDDto { ROLENAME = role.ROLENAME };
                    lookup[role.ROLENAME] = roleDto;
                }
                roleDto.menuRoles.Add(menu);
                return roleDto;
            },
            splitOn: "SERIAL"
        );

        return lookup.Values.ToList();
    }
}

