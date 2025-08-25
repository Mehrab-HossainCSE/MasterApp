using MasterApp.Application.SlaveDto;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;

public class UpdateNav
{
    private readonly string _jsonFilePath;
    public UpdateNav()
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "BillingSoftware.json");
    }

    public async Task<int> UpdateAsync(BillingSoftNevCreateDto dto)
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

            var menus = JsonSerializer.Deserialize<List<BillingSoftNevCreateJsonDto>>(json, options);
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

    private bool UpdateMenuRecursive(List<BillingSoftNevCreateJsonDto> menus, BillingSoftNevCreateDto dto)
    {
        foreach (var menu in menus)
        {
            if (menu.MenuId == dto.MenuId)
            {
                // Update matching menu
                menu.ParentMenuId = dto.ParentMenuId;
                menu.MenuName = dto.MenuName;
                menu.Url = dto.Url;
                menu.Sorting = dto.Sorting;
                menu.IsActive = dto.IsActive;
                menu.ApplicationId = dto.ApplicationId;
                menu.CreatorId = dto.CreatorId;
                menu.CreateDate = DateTime.Now.Date;

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
