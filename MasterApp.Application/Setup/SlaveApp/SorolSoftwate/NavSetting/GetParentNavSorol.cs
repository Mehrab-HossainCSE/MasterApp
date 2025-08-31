using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting
{
    public class GetParentNavSorol()
    {

        public async Task<IEnumerable<ParentSorolSoftNavDto>> GetParentsAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "SorolSoftACMasterDB.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Menu file not found: {filePath}");

            var json = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON into list of menu objects
            var menus = JsonSerializer.Deserialize<List<SorolNavDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (menus == null)
                return Enumerable.Empty<ParentSorolSoftNavDto>();

            // Filter only parent menus where ParentID is null
            var parentMenus = menus
                .Where(m => m.ParentID == 0)
                .Select(m => new ParentSorolSoftNavDto
                {
                    MenuID = m.MenuID,
                    Text = m.Text
                });

            return parentMenus;
        }


    }
}
