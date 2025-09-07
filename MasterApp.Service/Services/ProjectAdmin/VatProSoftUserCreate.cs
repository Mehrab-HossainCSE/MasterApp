using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
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

        return apiResponse.Status
            ? Result.Success(apiResponse.Message ?? "User created successfully")
            : Result.Fail(apiResponse.Message ?? "Failed to create user");
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
}
