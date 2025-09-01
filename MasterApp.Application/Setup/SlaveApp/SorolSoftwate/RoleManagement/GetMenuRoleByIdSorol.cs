using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement;

public class GetMenuRoleByIdSorol
{
    private readonly IDbConnectionFactory _connectionFactory;
    public GetMenuRoleByIdSorol
        (IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<SorolNavDto>> GetMenusByRoleAsync(int roleId)
    {
        using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

        try
        {
            // Step 1: Get all menu items from MENU table
            const string menuQuery = @"
                SELECT 
                    MenuID, 
                    ParentID, 
                    ModuleID, 
                    Text, 
                    URL, 
                    REL, 
                    Serial, 
                    IcoClass 
                FROM [AC_MenuURL_1] 
                WHERE Text IS NOT NULL AND Text != ''
                ORDER BY Serial, MenuID";

            var allMenus = (await connection.QueryAsync<SorolNavDto>(menuQuery)).ToList();

            // Debug: Log retrieved menus
            Console.WriteLine($"Retrieved {allMenus.Count} menus from database");

            // Step 2: Get MenuList for the given Role from RoleMenu
            const string roleQuery = @"SELECT MenuListID FROM RoleMenu WHERE RoleId = @RoleId";
            var menuIdListString = await connection.QueryFirstOrDefaultAsync<string>(roleQuery, new { RoleId = roleId });

            // Handle null or empty MenuListID
            menuIdListString = menuIdListString ?? "";

            // Debug: Log role menu data
            Console.WriteLine($"MenuListID for Role {roleId}: '{menuIdListString}'");

            // Step 3: Parse the comma-separated list into ints
            var menuIdsFromRole = new HashSet<int>();
            if (!string.IsNullOrWhiteSpace(menuIdListString))
            {
                menuIdsFromRole = menuIdListString
                    .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => id.Trim())
                    .Where(id => int.TryParse(id, out _))
                    .Select(int.Parse)
                    .ToHashSet();
            }

            // Debug: Log parsed menu IDs
            Console.WriteLine($"Parsed menu IDs: [{string.Join(", ", menuIdsFromRole)}]");

            // Step 4: Mark IsChecked = true if MenuID in role's list
            foreach (var menu in allMenus)
            {
                menu.IsChecked = menuIdsFromRole.Contains(menu.MenuID);

                // Debug: Log menu check status
                if (menu.IsChecked)
                {
                    Console.WriteLine($"Menu '{menu.Text}' (ID: {menu.MenuID}) is checked");
                }
            }

            // Step 5: Build hierarchy
            var menuTree = BuildTree(allMenus, null);

            // Debug: Log final tree structure
            Console.WriteLine($"Built tree with {menuTree.Count} root items");

            return menuTree;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetMenusByRoleAsync: {ex.Message}");
            throw;
        }
    }

    private List<SorolNavDto> BuildTree(List<SorolNavDto> items, int? parentId)
    {
        var result = items
            .Where(x =>
                (parentId == null && (x.ParentID == null || x.ParentID == 0)) ||
                (parentId != null && x.ParentID == parentId)
            )
            .OrderBy(x => x.Serial ?? 999) // Handle null Serial values
            .ThenBy(x => x.MenuID)
            .Select(x => new SorolNavDto
            {
                MenuID = x.MenuID,
                ParentID = x.ParentID,
                ModuleID = x.ModuleID,
                Text = x.Text,
                URL = x.URL,
                REL = x.REL,
                Serial = x.Serial,
                IcoClass = x.IcoClass,
                IsChecked = x.IsChecked,
                children = BuildTree(items, x.MenuID) // Recursive call for children
            })
            .ToList();

        // Debug: Log items at this level
        if (parentId == null)
        {
            Console.WriteLine($"Root level items: {result.Count}");
            foreach (var item in result)
            {
                Console.WriteLine($"- {item.Text} (ID: {item.MenuID}, Children: {item.children?.Count ?? 0})");
            }
        }

        return result;
    }

}
