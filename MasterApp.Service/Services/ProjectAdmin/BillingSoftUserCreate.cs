using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.Setup.MasterApp;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using MasterApp.Service.Entity;
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

    public async Task<Result<int>> GetUserByUserNameBilling(string username)
    {
        var url = $"{_apiSettings.BillingBaseUrl}GetUser?searchQuery=&pageNo=0&itemPerPage=2000";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return Result<int>.Fail($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<GetUserBillingByUserNameDto>();

        if (apiResponse == null)
        {
            return Result<int>.Fail("Invalid response from server");
        }

        if (apiResponse.success && apiResponse.data != null)
        {
            var user = apiResponse.data
                .FirstOrDefault(u => u.username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                // ✅ return the ID instead of just true/false
                return Result<int>.Success(user.id);
            }

            // user not found
            return Result<int>.Success(0);
        }

        return Result<int>.Fail(string.IsNullOrWhiteSpace(apiResponse.msg)
            ? "Failed to retrieve users"
            : apiResponse.msg);
    }


    public async Task<IResult> UpdateUserPassword(BillingUserUpdateDto dto)
    {
        var url = $"{_apiSettings.BillingBaseUrl}ChangeUserPassword?oldpassword=&username=lovely&password=shukla";

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
    public async Task<IResult> SaveRoleAsync(BillingRoleCreateDto dto)
    {
        var url = $"{_apiSettings.BillingBaseUrl}SaveRole";

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(dto)
        };

        // Optional: Add bearer token if needed
        // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return Result.Fail($"API call failed: {response.StatusCode}");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResopnseBilling>();

        if (apiResponse == null)
            return Result.Fail("Invalid response from server");

        return apiResponse.success
            ? Result.Success(string.IsNullOrWhiteSpace(apiResponse.msg) ? "Role created successfully" : apiResponse.msg)
            : Result.Fail(string.IsNullOrWhiteSpace(apiResponse.msg) ? "Failed to create role" : apiResponse.msg);
    }
    public async Task<IResult> AddMenuPagesToRoleAsync(int roleId, List<int> menuIds)
    {
        var url = $"{_apiSettings.BillingBaseUrl}AddRangMenuPagesRole?roleid={roleId}";

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(menuIds)
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            return Result.Fail($"API call failed: {response.StatusCode}");

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResopnseBilling>();

        if (apiResponse == null)
            return Result.Fail("Invalid response from server");

        return apiResponse.success
            ? Result.Success(string.IsNullOrWhiteSpace(apiResponse.msg) ? "Menus assigned successfully" : apiResponse.msg)
            : Result.Fail(string.IsNullOrWhiteSpace(apiResponse.msg) ? "Failed to assign menus" : apiResponse.msg);
    }



}
