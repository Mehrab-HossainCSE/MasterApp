using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace MasterApp.Service.Services.ProjectAdmin;

public class CloudePosUserCreate : ICloudePosUserCreate
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public CloudePosUserCreate(HttpClient httpClient, IOptions<ApiSettings> options, IDbConnectionFactory dbConnectionFactory)
    {
        _httpClient = httpClient;
        _apiSettings = options.Value;
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<IResult> CreateUserCloudePos(CloudPosUserDto dto, string apiKey)
    {
        var url = $"{_apiSettings.CloudPosBaseUrl}/api/Users/SaveExternalUser";

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };

        // Correct way to add Authorization header
        request.Headers.TryAddWithoutValidation("Authorization", $"{dto.ApiKeyUser}:{apiKey}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"API call failed: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            var apiResponse = JsonSerializer.Deserialize<ApiResponseUserCreateCloudePos>(responseContent);

            if (apiResponse != null && apiResponse.Success)
            {
                return Result.Success(string.IsNullOrWhiteSpace(apiResponse.Message)
                    ? "User created successfully"
                    : apiResponse.Message);
            }

            return Result.Fail(apiResponse?.Message ?? "Failed to create user");
        }
        catch (JsonException)
        {
            // fallback if API doesn't match your DTO
            var fallback = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
            var message = fallback?["Message"] ?? "Unknown error from server";

            return Result.Fail(message);
        }
    }

    public async Task<IResult> CreateMenuCloudePos(MenuCreateCoudPos dto, string apiKey)
    {
        var url = $"{_apiSettings.CloudPosBaseUrl}/api/Users/SaveExternalUserMenu";

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };

        request.Headers.TryAddWithoutValidation("Authorization", $"{dto.ApiKeyUser}:{apiKey}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return Result.Fail($"API call failed: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            var apiResponse = JsonSerializer.Deserialize<ApiResponseUserCreateCloudePos>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (apiResponse != null && apiResponse.Success)
            {
                return Result.Success(string.IsNullOrWhiteSpace(apiResponse.Message)
                    ? "Menu created successfully"
                    : apiResponse.Message);
            }

            return Result.Fail(apiResponse?.Message ?? "Failed to create Menu");
        }
        catch (JsonException)
        {
            var fallback = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
            var message = fallback?["Message"] ?? "Unknown error from server";
            return Result.Fail(message);
        }
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

    public async Task<Result<CloudPosApiResponse>> GetUserByUsernameCloudPos(CloudPosApiKeyDto dto)
    {
        var url = $"{_apiSettings.CloudPosBaseUrl}/api/external/login?username={dto.username}&password={dto.password}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Result<CloudPosApiResponse>.Fail($"API call failed: {response.StatusCode}");
        }

        var rawContent = await response.Content.ReadAsStringAsync();

        // Case 1: Error message (not JSON)
        if (rawContent.StartsWith("\"") || !rawContent.TrimStart().StartsWith("{"))
        {
            return Result<CloudPosApiResponse>.Fail(rawContent.Trim('"'));
        }

        // Case 2: JSON response
        var apiResponse = JsonSerializer.Deserialize<CloudPosApiResponse>(rawContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (apiResponse == null || string.IsNullOrEmpty(apiResponse.ApiKey))
        {
            return Result<CloudPosApiResponse>.Fail("Invalid response from server");
        }

        return Result<CloudPosApiResponse>.Success(apiResponse);
    }


    public async Task<IResult> UpdatePasswordCloudePos(MenuCreateCoudPos dto, string apiKey)
    {
        var url = $"{_apiSettings.CloudPosBaseUrl}/api/Users/SaveExternalUserMenu";

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };

        request.Headers.TryAddWithoutValidation("Authorization", $"{dto.ApiKeyUser}:{apiKey}");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return Result.Fail($"API call failed: {response.StatusCode}");

        var responseContent = await response.Content.ReadAsStringAsync();

        try
        {
            var apiResponse = JsonSerializer.Deserialize<ApiResponseUserCreateCloudePos>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (apiResponse != null && apiResponse.Success)
            {
                return Result.Success(string.IsNullOrWhiteSpace(apiResponse.Message)
                    ? "Menu created successfully"
                    : apiResponse.Message);
            }

            return Result.Fail(apiResponse?.Message ?? "Failed to create Menu");
        }
        catch (JsonException)
        {
            var fallback = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
            var message = fallback?["Message"] ?? "Unknown error from server";
            return Result.Fail(message);
        }
    }

}
