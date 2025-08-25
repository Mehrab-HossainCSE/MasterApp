using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;

public class CreateNav
{
    private readonly string _jsonFilePath;

    public CreateNav()
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "BillingSoftware.json");
    }

    public async Task<int> InsertAsync(BillingSoftNevCreateDto dto)
    {
        try
        {
            // 1. Load existing data from JSON
            var existingData = await LoadNavigationDataAsync();

            // 2. Check if item already exists
            if (ItemExists(existingData, dto.MenuId))
            {
                return 0; // Item already exists
            }

            // 3. Add new item to the hierarchical structure
            AddItemToHierarchy(existingData, dto);

            // 4. Save updated data back to JSON
            await SaveNavigationDataAsync(existingData);

            return 1; // Success
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while inserting data into JSON", ex);
        }
    }
    private async Task<List<BillingSoftNevCreateJsonDto>> LoadNavigationDataAsync()
    {
        try
        {
            if (!File.Exists(_jsonFilePath))
            {
                return new List<BillingSoftNevCreateJsonDto>();
            }

            var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);

            if (string.IsNullOrEmpty(jsonContent))
            {
                return new List<BillingSoftNevCreateJsonDto>();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<List<BillingSoftNevCreateJsonDto>>(jsonContent, options) ?? new List<BillingSoftNevCreateJsonDto>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error loading navigation data from JSON", ex);
        }

    }
    private bool ItemExists(List<BillingSoftNevCreateJsonDto> items, int MenuId)
    {
        return FindItemBySerial(items, MenuId) != null;
    }
    private BillingSoftNevCreateJsonDto FindItemBySerial(List<BillingSoftNevCreateJsonDto> items, int MenuId)
    {
        foreach (var item in items)
        {
            if (item.MenuId == MenuId)
                return item;

            if (item.children != null)
            {
                var found = FindItemBySerial(item.children, MenuId);
                if (found != null)
                    return found;
            }
        }
        return null;
    }
    private void AddItemToHierarchy(List<BillingSoftNevCreateJsonDto> rootItems, BillingSoftNevCreateDto dto)
    {
        var newItem = new BillingSoftNevCreateJsonDto
        {
            MenuId = dto.MenuId,
            ParentMenuId = dto.ParentMenuId,
            MenuName = dto.MenuName,
            Url = dto.Url,
            Sorting = dto.Sorting,
            IsActive = dto.IsActive,
            CreateDate = DateTime.Now.Date,
            ApplicationId = dto.ApplicationId,
            CreatorId = dto.CreatorId,
        
            children = new List<BillingSoftNevCreateJsonDto>()
        };

        if (dto.ParentMenuId == 0 || dto.ParentMenuId == null)
        {
            rootItems.Add(newItem);
        }
        else
        {
            var parent = FindItemBySerial(rootItems, dto.ParentMenuId);
            if (parent != null)
            {
                if (parent.children == null)
                    parent.children = new List<BillingSoftNevCreateJsonDto>();

                parent.children.Add(newItem);
            }
            else
            {
                // Parent not found, add to root level
                rootItems.Add(newItem);
            }
        }
    }
    private async Task SaveNavigationDataAsync(List<BillingSoftNevCreateJsonDto> data)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            var jsonContent = JsonSerializer.Serialize(data, options);
            await File.WriteAllTextAsync(_jsonFilePath, jsonContent);
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving navigation data to JSON", ex);
        }
    }
}
