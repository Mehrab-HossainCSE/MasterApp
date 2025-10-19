using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetParentNavCloudPosDBKMART
{
    private readonly string _jsonFilePath;

    public GetParentNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Menu", "menuData.json");
    }

    public async Task<IEnumerable<ParentCloudPosDBKMARTNavDto>> GetParentsAsync()
    {
        if (!File.Exists(_jsonFilePath))
            return Enumerable.Empty<ParentCloudPosDBKMARTNavDto>();

        var jsonContent = await File.ReadAllTextAsync(_jsonFilePath);

        // If file is empty or whitespace, return empty list
        if (string.IsNullOrWhiteSpace(jsonContent))
            return Enumerable.Empty<ParentCloudPosDBKMARTNavDto>();

        List<ParentCloudPosDBKMARTNavDto>? allMenus = null;

        try
        {
            allMenus = JsonSerializer.Deserialize<List<ParentCloudPosDBKMARTNavDto>>(
                jsonContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (JsonException)
        {
            // Log or handle invalid JSON here if needed
            return Enumerable.Empty<ParentCloudPosDBKMARTNavDto>();
        }

        if (allMenus == null)
            return Enumerable.Empty<ParentCloudPosDBKMARTNavDto>();

        // Return only parent items (PARENT_ID = 0)
        return allMenus
            .Where(m => m.PARENT_ID == 0)
            .GroupBy(m => m.SERIAL)
            .Select(g => g.First())
            .ToList();
    }
}
