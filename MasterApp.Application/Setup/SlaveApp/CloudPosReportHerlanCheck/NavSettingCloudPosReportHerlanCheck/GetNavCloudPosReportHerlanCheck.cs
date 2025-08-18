using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.NavSettingCloudPosReportHerlanCheck;

public class GetNavCloudPosReportHerlanCheck
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetNavCloudPosReportHerlanCheck(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<List<NavDto>> GetNavsAsync()
    {
        // Get all menu items from JSON file (complete menu structure)
        var jsonMenuItems = await GetNavsJsonAsync();

        // Get checked/active menu items from database
        var dbMenuItems = await GetNavsFromDB();

        // Create a HashSet of database menu IDs for faster lookup
        var dbMenuIds = new HashSet<decimal>(dbMenuItems.Select(x => x.SERIAL));

        // Merge: Set IsChecked = true for items that exist in database
        var mergedMenuItems = SetCheckedStatus(jsonMenuItems, dbMenuIds);

        return mergedMenuItems;
    }
    public async Task<List<NavDto>> GetNavsFromDB()
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");
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
                    SHOW_EDIT_PERMISSION,
                    SERIAL AS ID
                FROM MENU_1
                ORDER BY ORDER_BY, SERIAL";

        var navItems = (await connection.QueryAsync<NavDto>(sql)).ToList();

        // Clean up any self-referencing items
        foreach (var item in navItems.Where(n => n.SERIAL == n.PARENT_ID))
        {
            item.PARENT_ID = 0;
        }

        return navItems;
    }
    public async Task<List<NavDto>> GetNavsJsonAsync()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "CloudPosReportHerlanCheck.json");
        if (!File.Exists(filePath))
            return new List<NavDto>();

        var json = await File.ReadAllTextAsync(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var menuItems = JsonSerializer.Deserialize<List<NavDto>>(json, options);


        // If JSON is flat structure, build hierarchy
        if (menuItems != null && menuItems.Any() && menuItems.All(x => x.Children == null || !x.Children.Any()))
        {
            return BuildHierarchyFromFlat(menuItems);
        }

        return menuItems ?? new List<NavDto>();
    }
    
    private List<NavDto> BuildHierarchyFromFlat(List<NavDto> flatItems)
    {
        // Get root items (PARENT_ID is null or 0)
        var rootItems = flatItems.Where(x => x.PARENT_ID == null || x.PARENT_ID == 0).ToList();

        foreach (var rootItem in rootItems)
        {
            rootItem.Children = GetChildrenRecursive(flatItems, rootItem.SERIAL).ToList();
        }

        return rootItems.OrderBy(x => x.ORDER_BY).ThenBy(x => x.SERIAL).ToList();
    }
    private IEnumerable<NavDto> GetChildrenRecursive(List<NavDto> allItems, decimal parentId)
    {
        var children = allItems.Where(x => x.PARENT_ID == parentId)
                              .OrderBy(x => x.ORDER_BY)
                              .ThenBy(x => x.SERIAL)
                              .ToList();

        foreach (var child in children)
        {
            child.Children = GetChildrenRecursive(allItems, child.SERIAL).ToList();
        }

        return children;
    }
    private List<NavDto> SetCheckedStatus(List<NavDto> menuItems, HashSet<decimal> dbMenuIds)
    {
        if (menuItems == null) return new List<NavDto>();

        foreach (var menuItem in menuItems)
        {
            // Set IsChecked = true if this menu item exists in database
            menuItem.IsChecked = dbMenuIds.Contains(menuItem.SERIAL);

            // Recursively set checked status for children
            if (menuItem.Children != null && menuItem.Children.Any())
            {
                var childrenList = menuItem.Children.ToList();
                SetCheckedStatus(childrenList, dbMenuIds);
                menuItem.Children = childrenList;
            }
        }

        return menuItems;
    }
    
    private List<NavDto> BuildTree(List<NavDto> navItems, decimal parentId, HashSet<decimal> visited = null)
    {
        if (visited == null)
            visited = new HashSet<decimal>();

        return navItems
            .Where(n => n.PARENT_ID == parentId && !visited.Contains(n.SERIAL))
            .OrderBy(n => n.ORDER_BY)
            .ThenBy(n => n.SERIAL)
            .Select(n =>
            {
                var newVisited = new HashSet<decimal>(visited) { n.SERIAL };
                return new NavDto
                {
                    SERIAL = n.SERIAL,
                    PARENT_ID = n.PARENT_ID,
                    DESCRIPTION = n.DESCRIPTION,
                    URL = n.URL,
                    PER_ROLE = n.PER_ROLE,
                    ENTRY_BY = n.ENTRY_BY,
                    ENTRY_DATE = n.ENTRY_DATE,
                    ORDER_BY = n.ORDER_BY,
                    FA_CLASS = n.FA_CLASS,
                    MENU_TYPE = n.MENU_TYPE,
                    SHOW_EDIT_PERMISSION = n.SHOW_EDIT_PERMISSION,
                    ID = n.ID,
                    IsChecked = true, // Items from DB are checked
                    Children = BuildTree(navItems, n.SERIAL, newVisited)
                };
            })
            .ToList();
    }
  
    private List<NavDto> FlattenMenuItems(List<NavDto> menuItems)
    {
        var flatList = new List<NavDto>();

        foreach (var item in menuItems)
        {
            flatList.Add(item);

            if (item.Children != null && item.Children.Any())
            {
                flatList.AddRange(FlattenMenuItems(item.Children.ToList()));
            }
        }

        return flatList;
    }
}
