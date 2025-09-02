using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.UserManagement;




public class GetMenuByRoleSorol
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetMenuByRoleSorol(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<List<GetMenuByRoleSorolDto>> GetAllMenuDetailsAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection("SorolSoftACMasterDB");

        var sql = @"
            SELECT 
                r.RoleName,
                m.MenuID,
                m.Text,
                m.ParentID,
                ISNULL(p.Text, 'ROOT') AS ParentText,
                m.URL,
                m.IcoClass,
                m.Serial
            FROM RoleMenu r
            JOIN [AC_MenuURL_1] m 
                ON ',' + REPLACE(r.MenuListID, '-', '') + ',' LIKE '%,' + CAST(m.MenuID AS VARCHAR) + ',%'
            LEFT JOIN [AC_MenuURL_1] p 
                ON m.ParentID = p.MenuID
            WHERE r.RoleName IS NOT NULL AND r.RoleName <> ''
              AND m.Text IS NOT NULL AND m.Text <> ''
              AND ISNULL(p.Text, 'ROOT') <> 'ROOT' -- ✅ only exclude ROOT, not Setup/Admin/Accounts
            ORDER BY m.Serial, m.MenuID";

        var lookup = new Dictionary<string, GetMenuByRoleSorolDto>();

        var result = await connection.QueryAsync<GetMenuByRoleSorolDto, MenuDetails, GetMenuByRoleSorolDto>(
            sql,
            (role, menu) =>
            {
                // safety: normalize any leading '-' from MenuID string form
                menu.MenuID = int.TryParse(menu.MenuID.ToString().TrimStart('-'), out var id) ? id : menu.MenuID;

                if (!lookup.TryGetValue(role.ROLENAME, out var roleDto))
                {
                    roleDto = new GetMenuByRoleSorolDto { ROLENAME = role.ROLENAME };
                    lookup[role.ROLENAME] = roleDto;
                }

                roleDto.menuRoles.Add(menu);
                return roleDto;
            },
            splitOn: "MenuID"
        );

        return lookup.Values.ToList();
    }
}