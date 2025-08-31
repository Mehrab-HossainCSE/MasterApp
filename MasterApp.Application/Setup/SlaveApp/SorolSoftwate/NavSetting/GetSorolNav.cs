using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting;

public class GetSorolNav
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetSorolNav(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Merge JSON menu + DB checked items
    /// </summary>
    public async Task<List<SorolNavDto>> GetNavsAsync()
    {
        var jsonMenuItems = await GetNavsJsonAsync();
        var dbMenuItems = await GetNavsFromDB();

        var dbMenuIds = new HashSet<int>(dbMenuItems.Select(x => x.MenuID));

        return SetCheckedStatus(jsonMenuItems, dbMenuIds);
    }

    /// <summary>
    /// Load JSON menu (complete structure)
    /// </summary>
    public async Task<List<SorolNavDto>> GetNavsJsonAsync()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "SorolSoftACMasterDB.json");
        if (!File.Exists(filePath))
            return new List<SorolNavDto>();

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var menuItems = JsonSerializer.Deserialize<List<SorolNavDto>>(json, options);

        return menuItems ?? new List<SorolNavDto>();
    }

    /// <summary>
    /// Load DB menu items
    /// </summary>
    public async Task<List<SorolNavDto>> GetNavsFromDB()
    {
        using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

        const string sql = @"
            SELECT 
                MenuID,
                ParentID,
                ModuleID
                URL,
                Text,
                REL,
                Serial,
                IcoClass
           From [AC_Master_DB].[dbo].[AC_MenuURL_1]
            ORDER BY Serial";

        var navItems = (await connection.QueryAsync<SorolNavDto>(sql)).ToList();

        // Fix any self-referencing rows
        foreach (var item in navItems.Where(n => n.MenuID == n.ParentID))
        {
            item.ParentID = 0;
        }

        return navItems;
    }

    /// <summary>
    /// Set IsChecked status using DB menu IDs
    /// </summary>
    private List<SorolNavDto> SetCheckedStatus(List<SorolNavDto> menuItems, HashSet<int> dbMenuIds)
    {
        if (menuItems == null) return new List<SorolNavDto>();

        foreach (var menuItem in menuItems)
        {
            menuItem.IsChecked = dbMenuIds.Contains(menuItem.MenuID);

            if (menuItem.children != null && menuItem.children.Any())
            {
                menuItem.children = SetCheckedStatus(menuItem.children, dbMenuIds);
            }
        }

        return menuItems;
    }

    /// <summary>
    /// Build tree from DB items
    /// </summary>
    public async Task<List<SorolNavDto>> GetCheckedNavsWithHierarchyAsync()
    {
        var dbMenuItems = await GetNavsFromDB();
        return BuildTree(dbMenuItems, 0);
    }

    private List<SorolNavDto> BuildTree(List<SorolNavDto> items, decimal parentId)
    {
        return items
            .Where(n => n.ParentID == parentId)
            .OrderBy(n => n.Serial)
            .Select(n => new SorolNavDto
            {
                MenuID = n.MenuID,
                ParentID = n.ParentID,
                Text = n.Text,
                URL = n.URL,
                Serial = n.Serial,
                IcoClass=n.IcoClass,
                REL=n.REL,
                IsChecked = true,
                children = BuildTree(items, n.MenuID)
            })
            .ToList();
    }
}
