using Dapper;
using MasterApp.Application.Interface;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;

public class GetNavBilling
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetNavBilling(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// ✅ Only retrieve hierarchical navigation data from database
    /// </summary>
    public async Task<List<BillingSoftNavDto>> GetNavsAsync()
    {
        var dbMenuItems = await GetNavsFromDB();
        return BuildTree(dbMenuItems, 0);
    }

    /// <summary>
    /// 🔹 Fetch all menu items from DB
    /// </summary>
    private async Task<List<BillingSoftNavDto>> GetNavsFromDB()
    {
        using var connection = _connectionFactory.CreateConnection("BillingSoft");

        const string sql = @"
                SELECT 
                    MenuId,
                    ParentMenuId,
                    MenuName,
                    Url,
                    Sorting,
                    IsActive,
                    ApplicationId,
                    CreatorId,
                    CreateDate
                FROM [Management].[Menu_1]
                ORDER BY Sorting";

        var navItems = (await connection.QueryAsync<BillingSoftNavDto>(sql)).ToList();

        // Fix any self-referencing rows
        foreach (var item in navItems.Where(n => n.MenuId == n.ParentMenuId))
        {
            item.ParentMenuId = 0;
        }

        return navItems;
    }

    /// <summary>
    /// 🔹 Convert flat list to hierarchical structure
    /// </summary>
    private List<BillingSoftNavDto> BuildTree(List<BillingSoftNavDto> items, decimal parentId)
    {
        return items
            .Where(n => n.ParentMenuId == parentId)
            .OrderBy(n => n.Sorting)
            .Select(n => new BillingSoftNavDto
            {
                MenuId = n.MenuId,
                ParentMenuId = n.ParentMenuId,
                MenuName = n.MenuName,
                Url = n.Url,
                Sorting = n.Sorting,
                IsActive = n.IsActive,
                ApplicationId = n.ApplicationId,
                CreatorId = n.CreatorId,
                CreateDate = n.CreateDate,
                children = BuildTree(items, n.MenuId)
            })
            .ToList();
    }
}

