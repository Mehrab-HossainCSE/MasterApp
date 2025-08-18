using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.NavSettingCloudPosReportHerlanCheck;

public class CreateNavCloudPosReportHerlanCheck
{
    private readonly string _jsonFilePath;

    public CreateNavCloudPosReportHerlanCheck()
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "CloudPosReportHerlanCheck.json");
    }

    public async Task<int> InsertAsync(CreateNavCloudPosDBKMARTDto dto)
    {
        try
        {
            // 1. Load existing data from JSON
            var existingData = await LoadNavigationDataAsync();

            // 2. Check if item already exists
            if (ItemExists(existingData, dto.SERIAL))
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
    private async Task<List<CreateNavCloudPosDBKMARTJsonDto>> LoadNavigationDataAsync()
    {
        try
        {
            if (!File.Exists(_jsonFilePath))
            {
                return new List<CreateNavCloudPosDBKMARTJsonDto>();
            }

            var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);

            if (string.IsNullOrEmpty(jsonContent))
            {
                return new List<CreateNavCloudPosDBKMARTJsonDto>();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };

            return JsonSerializer.Deserialize<List<CreateNavCloudPosDBKMARTJsonDto>>(jsonContent, options) ?? new List<CreateNavCloudPosDBKMARTJsonDto>();
        }
        catch (Exception ex)
        {
            throw new Exception("Error loading navigation data from JSON", ex);
        }

    }
    private bool ItemExists(List<CreateNavCloudPosDBKMARTJsonDto> items, decimal serial)
    {
        return FindItemBySerial(items, serial) != null;
    }
    private CreateNavCloudPosDBKMARTJsonDto FindItemBySerial(List<CreateNavCloudPosDBKMARTJsonDto> items, decimal serial)
    {
        foreach (var item in items)
        {
            if (item.SERIAL == serial)
                return item;

            if (item.CHILDREN != null)
            {
                var found = FindItemBySerial(item.CHILDREN, serial);
                if (found != null)
                    return found;
            }
        }
        return null;
    }
    private void AddItemToHierarchy(List<CreateNavCloudPosDBKMARTJsonDto> rootItems, CreateNavCloudPosDBKMARTDto dto)
    {
        var newItem = new CreateNavCloudPosDBKMARTJsonDto
        {
            SERIAL = dto.SERIAL,
            PARENT_ID = dto.PARENT_ID,
            DESCRIPTION = dto.DESCRIPTION,
            URL = dto.URL,
            PER_ROLE = dto.PER_ROLE,
            ENTRY_BY = dto.ENTRY_BY,
            ENTRY_DATE = DateTime.Now.Date,
            ORDER_BY = dto.ORDER_BY,
            FA_CLASS = dto.FA_CLASS,
            MENU_TYPE = dto.MENU_TYPE,
            SHOW_EDIT_PERMISSION = dto.SHOW_EDIT_PERMISSION,
            CHILDREN = new List<CreateNavCloudPosDBKMARTJsonDto>()
        };

        if (dto.PARENT_ID == 0 || dto.PARENT_ID == null)
        {
            rootItems.Add(newItem);
        }
        else
        {
            var parent = FindItemBySerial(rootItems, dto.PARENT_ID.Value);
            if (parent != null)
            {
                if (parent.CHILDREN == null)
                    parent.CHILDREN = new List<CreateNavCloudPosDBKMARTJsonDto>();

                parent.CHILDREN.Add(newItem);
            }
            else
            {
                // Parent not found, add to root level
                rootItems.Add(newItem);
            }
        }
    }
    private async Task SaveNavigationDataAsync(List<CreateNavCloudPosDBKMARTJsonDto> data)
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
