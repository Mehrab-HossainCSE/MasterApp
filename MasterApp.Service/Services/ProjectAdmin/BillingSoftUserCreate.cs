using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
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
        var url = $"{_apiSettings.BillingBaseUrl}/SaveUser";

        // Attach Bearer token in the request headers
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };
       

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResopnseBilling>();


        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.success) // ✅ success regardless of message
        {
            return Result.Success(string.IsNullOrWhiteSpace(apiResponse.msg)
                ? "User created successfully"
                : apiResponse.msg);
        }

        // ❌ failed case
        return Result.Fail(string.IsNullOrWhiteSpace(apiResponse.msg)
            ? "Failed to create user"
            : apiResponse.msg);
    }
    public async Task<Result<ApiResopnseBilling>> GetRoleBilling()
    {
        var url = $"{_apiSettings.BillingBaseUrl}/GetRole";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Result<ApiResopnseBilling>.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResopnseBilling>();

        if (apiResponse == null)
        {
            return Result<ApiResopnseBilling>.Fail("Invalid response from server");
        }

        if (apiResponse.success)
        {
            return Result<ApiResopnseBilling>.Success(apiResponse);
        }

        return Result<ApiResopnseBilling>.Fail(string.IsNullOrWhiteSpace(apiResponse.msg)
            ? "Failed to retrieve roles"
            : apiResponse.msg);
    }
    public async  Task<IResult> UpdateUserBilling(BillingUserUpdateDto dto)
    {
        var url = $"{_apiSettings.BillingBaseUrl}SaveUser";

        // Attach Bearer token in the request headers
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };


        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResopnseBilling>();


        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.success) // ✅ success regardless of message
        {
            return Result.Success(string.IsNullOrWhiteSpace(apiResponse.msg)
                ? "User created successfully"
                : apiResponse.msg);
        }

        // ❌ failed case
        return Result.Fail(string.IsNullOrWhiteSpace(apiResponse.msg)
            ? "Failed to create user"
            : apiResponse.msg);
    }

    public async Task<Result<bool>> GetUserByUserNameBilling(string username)
    {
        var url = $"{_apiSettings.BillingBaseUrl}GetUser?searchQuery=&pageNo=0&itemPerPage=2000";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Result<bool>.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<GetUserBillingByUserNameDto>();

        if (apiResponse == null)
        {
            return Result<bool>.Fail("Invalid response from server");
        }

        if (apiResponse.success && apiResponse.data != null)
        {
            // Check if user exists
            var exists = apiResponse.data.Any(u =>
                u.username.Equals(username, StringComparison.OrdinalIgnoreCase));

            return Result<bool>.Success(exists);
        }

        return Result<bool>.Fail(string.IsNullOrWhiteSpace(apiResponse.msg)
            ? "Failed to retrieve users"
            : apiResponse.msg);
    }




}
