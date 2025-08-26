using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.ComponentModel;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;

public class RoleWiseMenu
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public RoleWiseMenu(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<List<BillingUserMenuDto>> GetAllUserAsync(int roleId)
    {
        using var connection = _dbConnectionFactory.CreateConnection("BillingSoft");

        var sql = @"
        ;WITH MenuCTE AS (
            SELECT m.MenuId, m.ParentMenuId, m.MenuName, m.Url, m.Sorting, m.IsActive, p.CanView
            FROM [Management].[Menu_1] m
            INNER JOIN [Management].[Permission] p ON m.MenuId = p.MenuId
            WHERE p.RoleId = @RoleId

            UNION ALL

            SELECT m.MenuId, m.ParentMenuId, m.MenuName, m.Url, m.Sorting, m.IsActive, CAST(1 AS bit) AS CanView
            FROM [Management].[Menu_1] m
            INNER JOIN MenuCTE c ON m.MenuId = c.ParentMenuId
        )
        SELECT DISTINCT MenuId, ParentMenuId, MenuName, Url, Sorting, IsActive, CanView
        FROM MenuCTE;
    ";

        var menus = await connection.QueryAsync<BillingUserMenuDto>(sql, new { RoleId = roleId });

        // Build hierarchy
        var menuList = menus.ToList();
        var menuDict = menuList.ToDictionary(m => m.MenuId);

        foreach (var menu in menuList)
        {
            if (menu.ParentMenuId != 0 && menuDict.ContainsKey(menu.ParentMenuId))
            {
                menuDict[menu.ParentMenuId].Children.Add(menu);
            }
        }

        // Return only root menus (ParentMenuId == 0)
        return menuList.Where(m => m.ParentMenuId == 0).OrderBy(m => m.Sorting).ToList();
    }

}
