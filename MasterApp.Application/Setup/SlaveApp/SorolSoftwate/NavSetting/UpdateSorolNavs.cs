using MasterApp.Application.SlaveDto;
using System.Text.Json.Serialization;
using System.Text.Json;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting;

public class UpdateSorolNavs
{
    private readonly string _jsonFilePath;
    public UpdateSorolNavs()
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "SorolSoftACMasterDB.json");
    }

    public async Task<int> UpdateAsync(CreateSorolSoftNavDto dto)
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

            var menus = JsonSerializer.Deserialize<List<SorolNavDto>>(json, options);
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

    private bool UpdateMenuRecursive(List<SorolNavDto> menus, CreateSorolSoftNavDto dto)
    {
        foreach (var menu in menus)
        {
            if (menu.MenuID == dto.MenuID)
            {
                // Update matching menu
                menu.ParentID = dto.ParentID;
                menu.ModuleID = dto.ModuleID;
                menu.Text = dto.Text;
                menu.URL = dto.URL;
                menu.REL = dto.REL;
                menu.Serial = dto.Serial;
               

                return true;
            }

            // Recursive update in children
            if (menu.children != null && menu.children.Count > 0)
            {
                bool childUpdated = UpdateMenuRecursive(menu.children, dto);
                if (childUpdated)
                    return true;
            }
        }
        return false;
    }
}
