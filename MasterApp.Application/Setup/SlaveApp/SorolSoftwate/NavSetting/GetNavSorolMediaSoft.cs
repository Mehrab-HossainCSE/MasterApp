using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting
{
    public class GetNavSorolMediaSoft
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNavSorolMediaSoft(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// ✅ Load navigation menu from JSON file
        /// </summary>
        public async Task<List<SorolNavDto>> GetNavsAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "SorolSoftACMasterDB.json");

            if (!File.Exists(filePath))
                return new List<SorolNavDto>();

            var json = await File.ReadAllTextAsync(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var menuItems = JsonSerializer.Deserialize<List<SorolNavDto>>(json, options);

            return menuItems ?? new List<SorolNavDto>();
        }

        /// <summary>
        /// ✅ Build hierarchical tree from flat list (if you need to rebuild from flat data)
        /// </summary>
        private List<SorolNavDto> BuildTree(List<SorolNavDto> items, int parentId)
        {
            return items
                .Where(n => n.ParentID == parentId)
                .OrderBy(n => n.Serial)
                .Select(n => new SorolNavDto
                {
                    MenuID = n.MenuID,
                    ParentID = n.ParentID,
                    ModuleID = n.ModuleID,
                    Text = n.Text,
                    URL = n.URL,
                    Serial = n.Serial,
                    IcoClass = n.IcoClass,
                    REL = n.REL,
                    IsChecked = false,
                    children = BuildTree(items, n.MenuID)
                })
                .ToList();
        }
    }
}
