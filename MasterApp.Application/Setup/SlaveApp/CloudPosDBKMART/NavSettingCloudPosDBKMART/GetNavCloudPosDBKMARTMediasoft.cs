using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetNavCloudPosDBKMARTMediasoft
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetNavCloudPosDBKMARTMediasoft(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Load navigation menu from JSON file and build parent-child hierarchy.
    /// </summary>
    public async Task<List<NavDto>> GetNavsJsonAsync()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "menuData.json");

        if (!File.Exists(filePath))
            return new List<NavDto>();

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
            // Remove this line: PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var menuItems = JsonSerializer.Deserialize<List<NavDto>>(json, options);

        if (menuItems == null || !menuItems.Any())
            return new List<NavDto>();

        // The Children property should now be populated from JSON
        return menuItems;
    }

    /// <summary>
    /// Build hierarchical tree from flat JSON data.
    /// </summary>
    private List<NavDto> BuildHierarchyFromFlat(List<NavDto> allItems, decimal parentId)
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
                IsChecked = false,
                Children = BuildHierarchyFromFlat(allItems, x.SERIAL)
            })
            .ToList();
    }
}
