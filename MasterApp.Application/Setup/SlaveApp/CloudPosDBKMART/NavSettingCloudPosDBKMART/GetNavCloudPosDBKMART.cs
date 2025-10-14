
using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART
{
    public class GetNavCloudPosDBKMART
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Get all menu items from database and build hierarchical tree.
        /// </summary>
        public async Task<List<NavDto>> GetNavsAsync()
        {
            using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");

            const string sql = @"
                SELECT 
                    SERIAL,
                    PARENT_ID,
                    DESCRIPTION,
                    URL,
                    PER_ROLE,
                    ENTRY_BY,
                    ENTRY_DATE,
                    ORDER_BY,
                    FA_CLASS,
                    MENU_TYPE,
                    SHOW_EDIT_PERMISSION
                FROM MENU
                ORDER BY ORDER_BY, SERIAL";

            var dbMenuItems = (await connection.QueryAsync<NavDto>(sql)).ToList();

            // Clean up any self-referencing items
            foreach (var item in dbMenuItems.Where(n => n.SERIAL == n.PARENT_ID))
            {
                item.PARENT_ID = 0;
            }

            // Build tree structure from flat DB data
            var tree = BuildTree(dbMenuItems, 0);

            return tree;
        }

        /// <summary>
        /// Build tree structure recursively from flat list.
        /// </summary>
        private List<NavDto> BuildTree(List<NavDto> allItems, decimal parentId)
        {
            return allItems
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
                    IsChecked = false, // all from DB are considered checked
                    Children = BuildTree(allItems, x.SERIAL)
                })
                .ToList();
        }
    }
}





//using Dapper;
//using MasterApp.Application.Interface;
//using MasterApp.Application.SlaveDto;
//using System.Text.Json;

//namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

//public class GetNavCloudPosDBKMART
//{
//    private readonly IDbConnectionFactory _connectionFactory;

//    public GetNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
//    {
//        _connectionFactory = connectionFactory;
//    }

//    /// <summary>
//    /// Main method that merges JSON menu structure with database checked status
//    /// JSON provides all menu items, Database determines which are checked
//    /// </summary>
//    public async Task<List<NavDto>> GetNavsAsync()
//    {
//        // Get all menu items from JSON file (complete menu structure)
//        var jsonMenuItems = await GetNavsJsonAsync();

//        // Get checked/active menu items from database
//        var dbMenuItems = await GetNavsFromDB();

//        // Create a HashSet of database menu IDs for faster lookup
//        var dbMenuIds = new HashSet<decimal>(dbMenuItems.Select(x => x.SERIAL));

//        // Merge: Set IsChecked = true for items that exist in database
//        var mergedMenuItems = SetCheckedStatus(jsonMenuItems, dbMenuIds);

//        return mergedMenuItems;
//    }

//    /// <summary>
//    /// Get complete menu structure from JSON file
//    /// </summary>
//    public async Task<List<NavDto>> GetNavsJsonAsync()
//    {
//        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "menuData.json");
//        if (!File.Exists(filePath))
//            return new List<NavDto>();

//        var json = await File.ReadAllTextAsync(filePath);
//        var options = new JsonSerializerOptions
//        {
//            PropertyNameCaseInsensitive = true,
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//        };

//        var menuItems = JsonSerializer.Deserialize<List<NavDto>>(json, options);

//        // If JSON is flat structure, build hierarchy
//        if (menuItems != null && menuItems.Any() && menuItems.All(x => x.Children == null || !x.Children.Any()))
//        {
//            return BuildHierarchyFromFlat(menuItems);
//        }

//        return menuItems ?? new List<NavDto>();
//    }

//    /// <summary>
//    /// Get active/selected menu items from database (these will be marked as checked)
//    /// </summary>
//    public async Task<List<NavDto>> GetNavsFromDB()
//    {
//        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
//        const string sql = @"
//                SELECT 
//                    SERIAL,
//                    PARENT_ID,
//                    DESCRIPTION,
//                    URL,
//                    PER_ROLE,
//                    ENTRY_BY,
//                    ENTRY_DATE,
//                    ORDER_BY,
//                    FA_CLASS,
//                    MENU_TYPE,
//                    SHOW_EDIT_PERMISSION,
//                    SERIAL AS ID
//                FROM MENU_1
//                ORDER BY ORDER_BY, SERIAL";

//        var navItems = (await connection.QueryAsync<NavDto>(sql)).ToList();

//        // Clean up any self-referencing items
//        foreach (var item in navItems.Where(n => n.SERIAL == n.PARENT_ID))
//        {
//            item.PARENT_ID = 0;
//        }

//        return navItems;
//    }

//    /// <summary>
//    /// Build hierarchy from flat JSON structure
//    /// </summary>
//    private List<NavDto> BuildHierarchyFromFlat(List<NavDto> flatItems)
//    {
//        // Get root items (PARENT_ID is null or 0)
//        var rootItems = flatItems.Where(x => x.PARENT_ID == null || x.PARENT_ID == 0).ToList();

//        foreach (var rootItem in rootItems)
//        {
//            rootItem.Children = GetChildrenRecursive(flatItems, rootItem.SERIAL).ToList();
//        }

//        return rootItems.OrderBy(x => x.ORDER_BY).ThenBy(x => x.SERIAL).ToList();
//    }

//    /// <summary>
//    /// Recursively get children for hierarchy building
//    /// </summary>
//    private IEnumerable<NavDto> GetChildrenRecursive(List<NavDto> allItems, decimal parentId)
//    {
//        var children = allItems.Where(x => x.PARENT_ID == parentId)
//                              .OrderBy(x => x.ORDER_BY)
//                              .ThenBy(x => x.SERIAL)
//                              .ToList();

//        foreach (var child in children)
//        {
//            child.Children = GetChildrenRecursive(allItems, child.SERIAL).ToList();
//        }

//        return children;
//    }

//    /// <summary>
//    /// Set IsChecked status based on database menu IDs
//    /// Recursively processes parent and children
//    /// </summary>
//    private List<NavDto> SetCheckedStatus(List<NavDto> menuItems, HashSet<decimal> dbMenuIds)
//    {
//        if (menuItems == null) return new List<NavDto>();

//        foreach (var menuItem in menuItems)
//        {
//            // Set IsChecked = true if this menu item exists in database
//            menuItem.IsChecked = dbMenuIds.Contains(menuItem.SERIAL);

//            // Recursively set checked status for children
//            if (menuItem.Children != null && menuItem.Children.Any())
//            {
//                var childrenList = menuItem.Children.ToList();
//                SetCheckedStatus(childrenList, dbMenuIds);
//                menuItem.Children = childrenList;
//            }
//        }

//        return menuItems;
//    }

//    /// <summary>
//    /// Alternative method: Get only checked items with hierarchy (for display purposes)
//    /// </summary>
//    public async Task<List<NavDto>> GetCheckedNavsWithHierarchyAsync()
//    {
//        var dbMenuItems = await GetNavsFromDB();
//        return BuildTree(dbMenuItems, 0);
//    }

//    /// <summary>
//    /// Build tree structure from database items
//    /// </summary>
//    private List<NavDto> BuildTree(List<NavDto> navItems, decimal parentId, HashSet<decimal> visited = null)
//    {
//        if (visited == null)
//            visited = new HashSet<decimal>();

//        return navItems
//            .Where(n => n.PARENT_ID == parentId && !visited.Contains(n.SERIAL))
//            .OrderBy(n => n.ORDER_BY)
//            .ThenBy(n => n.SERIAL)
//            .Select(n =>
//            {
//                var newVisited = new HashSet<decimal>(visited) { n.SERIAL };
//                return new NavDto
//                {
//                    SERIAL = n.SERIAL,
//                    PARENT_ID = n.PARENT_ID,
//                    DESCRIPTION = n.DESCRIPTION,
//                    URL = n.URL,
//                    PER_ROLE = n.PER_ROLE,
//                    ENTRY_BY = n.ENTRY_BY,
//                    ENTRY_DATE = n.ENTRY_DATE,
//                    ORDER_BY = n.ORDER_BY,
//                    FA_CLASS = n.FA_CLASS,
//                    MENU_TYPE = n.MENU_TYPE,
//                    SHOW_EDIT_PERMISSION = n.SHOW_EDIT_PERMISSION,
//                    ID = n.ID,
//                    IsChecked = true, // Items from DB are checked
//                    Children = BuildTree(navItems, n.SERIAL, newVisited)
//                };
//            })
//            .ToList();
//    }

//    /// <summary>
//    /// Save selected menu items to database
//    /// Call this method when user selects/deselects menu items
//    /// </summary>
//    public async Task<bool> SaveSelectedMenusAsync(List<NavDto> selectedMenus)
//    {
//        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
//        using var transaction = connection.BeginTransaction();

//        try
//        {
//            // Clear existing menu items
//            await connection.ExecuteAsync("DELETE FROM MENU_1", transaction: transaction);

//            // Get all selected items (including children) in flat structure
//            var flatSelectedItems = FlattenMenuItems(selectedMenus).Where(x => x.IsChecked).ToList();

//            // Insert selected items
//            const string insertSql = @"
//                INSERT INTO MENU_1 (SERIAL, PARENT_ID, DESCRIPTION, URL, PER_ROLE, ENTRY_BY, 
//                                   ENTRY_DATE, ORDER_BY, FA_CLASS, MENU_TYPE, SHOW_EDIT_PERMISSION)
//                VALUES (@SERIAL, @PARENT_ID, @DESCRIPTION, @URL, @PER_ROLE, @ENTRY_BY, 
//                        @ENTRY_DATE, @ORDER_BY, @FA_CLASS, @MENU_TYPE, @SHOW_EDIT_PERMISSION)";

//            await connection.ExecuteAsync(insertSql, flatSelectedItems, transaction: transaction);

//            transaction.Commit();
//            return true;
//        }
//        catch (Exception)
//        {
//            transaction.Rollback();
//            return false;
//        }
//    }

//    /// <summary>
//    /// Flatten hierarchical menu structure to flat list
//    /// </summary>
//    private List<NavDto> FlattenMenuItems(List<NavDto> menuItems)
//    {
//        var flatList = new List<NavDto>();

//        foreach (var item in menuItems)
//        {
//            flatList.Add(item);

//            if (item.Children != null && item.Children.Any())
//            {
//                flatList.AddRange(FlattenMenuItems(item.Children.ToList()));
//            }
//        }

//        return flatList;
//    }
//}