using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;

public class GetNav
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetNav(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Merge JSON menu + DB checked items
    /// </summary>
    //public async Task<List<BillingSoftNavDto>> GetNavsAsync()
    //{
    //    var jsonMenuItems = await GetNavsJsonAsync();
    //    var dbMenuItems = await GetNavsFromDB();

    //    var dbMenuIds = new HashSet<int>(dbMenuItems.Select(x => x.MenuId));

    //    return SetCheckedStatus(jsonMenuItems, dbMenuIds);
    //}

    /// <summary>
    /// Load JSON menu (complete structure)
    /// </summary>
    public async Task<List<BillingSoftNavDto>> GetNavsJsonAsync()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "BillingSoftware.json");
        if (!File.Exists(filePath))
            return new List<BillingSoftNavDto>();

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var menuItems = JsonSerializer.Deserialize<List<BillingSoftNavDto>>(json, options);

        return menuItems ?? new List<BillingSoftNavDto>();
    }

    /// <summary>
    /// Load DB menu items
    /// </summary>
    //public async Task<List<BillingSoftNavDto>> GetNavsFromDB()
    //{
    //    using var connection = _connectionFactory.CreateConnection("BillingSoft");

    //    const string sql = @"
    //        SELECT 
    //            MenuId,
    //            ParentMenuId,
    //            MenuName,
    //            Url,
    //            Sorting,
    //            IsActive,
    //            ApplicationId,
    //            CreatorId,
    //            CreateDate
    //        FROM [Management].[Menu_1]
    //        ORDER BY Sorting";

    //    var navItems = (await connection.QueryAsync<BillingSoftNavDto>(sql)).ToList();

    //    // Fix any self-referencing rows
    //    foreach (var item in navItems.Where(n => n.MenuId == n.ParentMenuId))
    //    {
    //        item.ParentMenuId = 0;
    //    }

    //    return navItems;
    //}

    /// <summary>
    /// Set IsChecked status using DB menu IDs
    /// </summary>
    //private List<BillingSoftNavDto> SetCheckedStatus(List<BillingSoftNavDto> menuItems, HashSet<int> dbMenuIds)
    //{
    //    if (menuItems == null) return new List<BillingSoftNavDto>();

    //    foreach (var menuItem in menuItems)
    //    {
    //        menuItem.IsChecked = dbMenuIds.Contains(menuItem.MenuId);

    //        if (menuItem.children != null && menuItem.children.Any())
    //        {
    //            menuItem.children = SetCheckedStatus(menuItem.children, dbMenuIds);
    //        }
    //    }

    //    return menuItems;
    //}

    /// <summary>
    /// Build tree from DB items
    /// </summary>
    //public async Task<List<BillingSoftNavDto>> GetCheckedNavsWithHierarchyAsync()
    //{
    //    var dbMenuItems = await GetNavsFromDB();
    //    return BuildTree(dbMenuItems, 0);
    //}

    //private List<BillingSoftNavDto> BuildTree(List<BillingSoftNavDto> items, decimal parentId)
    //{
    //    return items
    //        .Where(n => n.ParentMenuId == parentId)
    //        .OrderBy(n => n.Sorting)
    //        .Select(n => new BillingSoftNavDto
    //        {
    //            MenuId = n.MenuId,
    //            ParentMenuId = n.ParentMenuId,
    //            MenuName = n.MenuName,
    //            Url = n.Url,
    //            Sorting = n.Sorting,
    //            IsActive = n.IsActive,
    //            ApplicationId = n.ApplicationId,
    //            CreatorId = n.CreatorId,
    //            CreateDate = n.CreateDate,
    //            IsChecked = false,
    //            children = BuildTree(items, n.MenuId)
    //        })
    //        .ToList();
    //}
}
