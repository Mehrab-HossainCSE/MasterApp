using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class UpdateNavCloudPosDBKMART
{
    private readonly string _jsonFilePath;

    public UpdateNavCloudPosDBKMART()
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "menuData.json");
    }

    public async Task<int> UpdateAsync(CreateNavCloudPosDBKMARTDto dto)
    {
        try
        {
            if (!File.Exists(_jsonFilePath))
                throw new FileNotFoundException("Menu data file not found.");

            string json = await File.ReadAllTextAsync(_jsonFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            var menus = JsonSerializer.Deserialize<List<CreateNavCloudPosDBKMARTJsonDto>>(json, options);
            if (menus == null)
                return 0;

            bool updated = UpdateMenuRecursive(menus, dto);

            if (!updated)
                return 0;

            string updatedJson = JsonSerializer.Serialize(menus, new JsonSerializerOptions
            {
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            await File.WriteAllTextAsync(_jsonFilePath, updatedJson);

            return 1;
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating menu data in JSON", ex);
        }
    }

    private bool UpdateMenuRecursive(List<CreateNavCloudPosDBKMARTJsonDto> menus, CreateNavCloudPosDBKMARTDto dto)
    {
        foreach (var menu in menus)
        {
            if (menu.SERIAL == dto.SERIAL)
            {
                // Update matching menu
                menu.PARENT_ID = dto.PARENT_ID;
                menu.DESCRIPTION = dto.DESCRIPTION;
                menu.URL = dto.URL;
                menu.PER_ROLE = dto.PER_ROLE;
                menu.ENTRY_BY = dto.ENTRY_BY;
                menu.ENTRY_DATE = dto.ENTRY_DATE;
                menu.ORDER_BY = dto.ORDER_BY;
                menu.FA_CLASS = dto.FA_CLASS;
                menu.ID = dto.ID;
                menu.MENU_TYPE = dto.MENU_TYPE;
                menu.SHOW_EDIT_PERMISSION = dto.SHOW_EDIT_PERMISSION;
                return true;
            }

            // Recursive update in children
            if (menu.CHILDREN != null && menu.CHILDREN.Count > 0)
            {
                bool childUpdated = UpdateMenuRecursive(menu.CHILDREN, dto);
                if (childUpdated)
                    return true;
            }
        }
        return false;
    }

    // Match the JSON structure exactly
  
}