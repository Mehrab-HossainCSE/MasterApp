using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace MasterApp.Service.Services.ProjectAdmin;

public class BillingSoftUserCreate : IBillingSoftUserCreate
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public BillingSoftUserCreate(HttpClient httpClient, IOptions<ApiSettings> options)
    {
        _httpClient = httpClient;
        _apiSettings = options.Value;
    }
    public async Task<IResult> CreateUserBilling(BillingUserCreateDto dto)
    {
        var url = $"{_apiSettings.BillingBaseUrl}api/Setup/User_Insert";

        // Attach Bearer token in the request headers
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer");

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
