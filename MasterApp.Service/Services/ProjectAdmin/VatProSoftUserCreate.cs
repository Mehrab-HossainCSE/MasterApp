using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.SlaveDto;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace MasterApp.Service.Services.ProjectAdmin;

public class VatProSoftUserCreate : IVatProSoftUserCreate
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public VatProSoftUserCreate(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _apiSettings = options.Value;
    }
    public async Task<IResult> CreateUserVatPro(VatProUserCreateDto dto, string token)
    {
        var url = $"{_apiSettings.VatProBaseUrl}api/Setup/User_Insert";

        // Attach Bearer token in the request headers
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseVatPro>();

       
        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.Status) // ✅ success regardless of message
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


    public async Task<string> getVatProToken(VatProTokenDto dto)
    {
        // Now url comes from appsettings.json
        var url = $"{_apiSettings.VatProBaseUrl}token?username={dto.username}&password={dto.password}";

        var response = await _httpClient.PostAsJsonAsync(url, dto);
        if (!response.IsSuccessStatusCode)
        {
            return response.StatusCode.ToString();
        }
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseVatProToken>();
        if (apiResponse == null)
        {
            return "Invalid response from server";
        }
        return apiResponse.token;
    }

    public async Task<GetUserVatProDto> GetUserByUsername(string userName,string token)
    {
        var url = $"{_apiSettings.VatProBaseUrl}api/Setup/User_FindByUserId?username={userName}";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.SendAsync(request);
       return await response.Content.ReadFromJsonAsync<GetUserVatProDto>();
    }
    public async Task<IResult> UpdateUserVatPro(VatProUserUpdateDto dto, string token)
    {
        var url = $"{_apiSettings.VatProBaseUrl}api/Setup/User_Update";

        // Attach Bearer token in the request headers
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseVatPro>();


        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.Status) // ✅ success regardless of message
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
}
