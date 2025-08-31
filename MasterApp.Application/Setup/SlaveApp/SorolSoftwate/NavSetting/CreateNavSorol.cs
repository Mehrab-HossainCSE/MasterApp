using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting;

public class CreateNavSorol
{
    private readonly string _jsonFilePath;

    public CreateNavSorol()
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "SorolSoftACMasterDB.json");
    }

    public async Task<int> InsertAsync(CreateSorolSoftNavDto dto)
    {
        try
        {
            // 1. Load existing data from JSON
            var existingData = await LoadNavigationDataAsync();

            // 2. Check if item already exists
            if (ItemExists(existingData, dto.MenuID))
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
    private async Task<List<SorolNavDto>> LoadNavigationDataAsync()
    {
        try
        {
            if (!File.Exists(_jsonFilePath))
            {
                return new List<SorolNavDto>();
            }

            var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);

            if (string.IsNullOrEmpty(jsonContent))
            {
                return new List<SorolNavDto>();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<List<SorolNavDto>>(jsonContent, options) ?? new List<SorolNavDto>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error loading navigation data from JSON", ex);
        }

    }
    private bool ItemExists(List<SorolNavDto> items, int MenuId)
    {
        return FindItemBySerial(items, MenuId) != null;
    }
    private SorolNavDto FindItemBySerial(List<SorolNavDto> items, int MenuId)
    {
        foreach (var item in items)
        {
            if (item.MenuID == MenuId)
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
    private void AddItemToHierarchy(List<SorolNavDto> rootItems, CreateSorolSoftNavDto dto)
    {
        var newItem = new SorolNavDto
        {
            MenuID = dto.MenuID,
            ParentID = dto.ParentID,
            ModuleID = dto.ModuleID,
            Text = dto.Text,
            URL = dto.URL,
            REL = dto.REL,
            Serial = dto.Serial,
          

            children = new List<SorolNavDto>()
        };

        if (dto.ParentID == 0 || dto.ParentID == null)
        {
            rootItems.Add(newItem);
        }
        else
        {
            var parent = FindItemBySerial(rootItems, dto.ParentID);
            if (parent != null)
            {
                if (parent.children == null)
                    parent.children = new List<SorolNavDto>();

                parent.children.Add(newItem);
            }
            else
            {
                // Parent not found, add to root level
                rootItems.Add(newItem);
            }
        }
    }
    private async Task SaveNavigationDataAsync(List<SorolNavDto> data)
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
