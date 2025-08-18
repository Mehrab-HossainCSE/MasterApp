using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.RoleManagementCloudPosReportHerlanCheck;

public class GetMenuIdToTheRoleCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{
    public async Task<List<NavDto>> GetMenusByRoleAsync(string Role_Name)
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");

        // Step 1: Get all menu items from MENU_1
        const string menuQuery = @"SELECT * FROM MENU_1 ORDER BY ORDER_BY, SERIAL";
        var allMenus = (await connection.QueryAsync<NavDto>(menuQuery)).ToList();

        // Step 2: Get MenuIdList for the given Role from ROLE_2
        const string roleQuery = @"SELECT MENULISTID FROM ROLE_1 WHERE ROLE_NAME = @Role_Name";
        var menuIdListString = await connection.QueryFirstOrDefaultAsync<string>(roleQuery, new { Role_Name = Role_Name });

        if (string.IsNullOrWhiteSpace(menuIdListString))
            menuIdListString = ""; // fallback

        // Step 3: Parse the comma-separated list
        var menuIdsFromRole = new HashSet<decimal>(
            menuIdListString.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(id => decimal.TryParse(id, out var val) ? val : -1)
                            .Where(id => id != -1)
        );

        // Step 4: Mark IsChecked = true if SERIAL in menuIdsFromRole
        foreach (var menu in allMenus)
        {
            if (menuIdsFromRole.Contains(menu.SERIAL))
                menu.IsChecked = true;

            if (menu.SERIAL == menu.PARENT_ID)
                menu.PARENT_ID = 0;
        }

        // Step 5: Build hierarchy
        var menuTree = BuildTree(allMenus, 0);

        return menuTree;
    }

    private List<NavDto> BuildTree(List<NavDto> items, decimal parentId)
    {
        return items
            .Where(x => x.PARENT_ID == parentId)
            .OrderBy(x => x.ORDER_BY)
            .ThenBy(x => x.SERIAL)
            .Select(x => new NavDto
            {
                SERIAL = x.SERIAL,
                PARENT_ID = x.PARENT_ID,
                DESCRIPTION = x.DESCRIPTION,
                URL = x.URL,
                PER_ROLE = x.PER_ROLE,
                ENTRY_BY = x.ENTRY_BY,
                ENTRY_DATE = x.ENTRY_DATE,
                ORDER_BY = x.ORDER_BY,
                FA_CLASS = x.FA_CLASS,
                MENU_TYPE = x.MENU_TYPE,
                SHOW_EDIT_PERMISSION = x.SHOW_EDIT_PERMISSION,
                ID = x.ID,
                IsChecked = x.IsChecked,
                Children = BuildTree(items, x.SERIAL)
            })
            .ToList();
    }
}
