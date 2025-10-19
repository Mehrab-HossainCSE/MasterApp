using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART
{
    public class GetNavCloudPosDBKMARTMediasoft
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GetNavCloudPosDBKMARTMediasoft(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Loads navigation menu directly from JSON file (including nested children).
        /// </summary>
        public async Task<List<NavDto>> GetNavsJsonAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "menuData.json");

            // If file doesn't exist → return empty list
            if (!File.Exists(filePath))
                return new List<NavDto>();

            var json = await File.ReadAllTextAsync(filePath);

            // If JSON is empty or whitespace → return empty list
            if (string.IsNullOrWhiteSpace(json))
                return new List<NavDto>();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var menuItems = JsonSerializer.Deserialize<List<NavDto>>(json, options);

                // If no data found → return empty list
                if (menuItems == null || !menuItems.Any())
                    return new List<NavDto>();

                // ✅ JSON already contains children, return as is
                return menuItems;
            }
            catch (JsonException ex)
            {
                // Log error but don't crash
                Console.WriteLine($"⚠️ Invalid JSON in file '{filePath}': {ex.Message}");
                return new List<NavDto>();
            }
        }
    }
}
