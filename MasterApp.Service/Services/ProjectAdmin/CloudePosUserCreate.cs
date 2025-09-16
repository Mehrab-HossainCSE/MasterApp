using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;

namespace MasterApp.Service.Services.ProjectAdmin;

public class CloudePosUserCreate : ICloudePosUserCreate
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public CloudePosUserCreate(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _apiSettings = options.Value;
    }
    public async Task<IResult> CreateUserCloudePos(CloudPosUserDto dto, string apiKey)
    {
        var url = $"{_apiSettings.CloudPosBaseUrl}/api/Users/SaveExternalUser";

        // Build request with JSON body
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };

        // Authorization header -> "UserName:ApiKey"
        request.Headers.Add("Authorization", $"{dto.UserName}:{apiKey}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseUserCreateCloudePos>();

        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.Status) // ✅ success
        {
            return Result.Success(string.IsNullOrWhiteSpace(apiResponse.Message)
                ? "User created successfully"
                : apiResponse.Message);
        }

        // ❌ failed case
        return Result.Fail(string.IsNullOrWhiteSpace(apiResponse.Message)
            ? "Failed to create user"
            : apiResponse.Message);
    }
    public async Task<string> GetCloudePosApiKey(CloudPosApiKeyDto dto)
    {
        var url = $"{_apiSettings.CloudPosBaseUrl}/api/external/login?username={dto.username}&password={dto.password}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToString();
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<CloudPosApiResponse>();
        if (apiResponse == null || string.IsNullOrEmpty(apiResponse.ApiKey))
        {
            return "Invalid response from server";
        }

        return apiResponse.ApiKey;
    }

}
