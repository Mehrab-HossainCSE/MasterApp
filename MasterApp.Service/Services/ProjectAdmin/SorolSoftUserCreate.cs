using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace MasterApp.Service.Services.ProjectAdmin;

public class SorolSoftUserCreate : ISorolSoftUserCreate
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public SorolSoftUserCreate(HttpClient httpClient, IOptions<ApiSettings> options, IDbConnectionFactory dbConnectionFactory)
    {
        _httpClient = httpClient;
        _apiSettings = options.Value;
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<IResult> CreateUserSorol(SorolUserCreateDto dto, string token)
    {
        var url = $"{_apiSettings.SorolBaseUrl}api/UserRegistratoin";

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

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseSorolCreateUser>();



        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.status) // ✅ success regardless of message
        {
            return Result.Success(string.IsNullOrWhiteSpace(apiResponse.message)
                ? "User created successfully"
                : apiResponse.message);
        }

        // ❌ failed case
        return Result.Fail(string.IsNullOrWhiteSpace(apiResponse.message)
            ? "Failed to create user"
            : apiResponse.message);
    }


    public async Task<string> getSorolToken(SorolTokenDto dto)
    {
        var url = $"{_apiSettings.SorolBaseUrl}api/auth/login";

        // Create JSON content with exact property names as shown in Postman
        var jsonContent = new
        {
            Username = dto.Username,  // or use dto.username if that's the property name
            Password = dto.Password   // or use dto.password if that's the property name
        };

        var json = JsonSerializer.Serialize(jsonContent, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // This will make it "username", "password"
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };

        // Add headers if needed
        request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return $"API call failed: {response.StatusCode}, Body: {body}";
        }

        var apiResponse = JsonSerializer.Deserialize<ApiResponseSorol>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return apiResponse?.token ?? "Invalid response from server";
    }

    public async Task<IResult> UpdateUserSorol(SorolUserUpdateDto dto, string token)
    {
        var url = $"{_apiSettings.SorolBaseUrl}api/UpdateRegistratoin";

        Console.WriteLine(dto);
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

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseSorolCreateUser>();



        if (apiResponse == null)
        {
            return Result.Fail("Invalid response from server");
        }

        if (apiResponse.status) // ✅ success regardless of message
        {
            return Result.Success(string.IsNullOrWhiteSpace(apiResponse.message)
                ? "User created successfully"
                : apiResponse.message);
        }

        // ❌ failed case
        return Result.Fail(string.IsNullOrWhiteSpace(apiResponse.message)
            ? "Failed to create user"
            : apiResponse.message);
    }


    public async Task<GetAllUserSorolDto?> GetUserByUsername(string userName)
    {
        using var connection = _dbConnectionFactory.CreateConnection("SorolSoftACMasterDB");

        var sql = @"SELECT u.UserId, u.UserName, u.Designation, u.CompanyId ,UM.MenuIdList 
                FROM [AC_Users]  as u join  AC_UserMenu as UM on u.UserId =UM.UserId
                WHERE UserName = @UserName";

        return await connection.QueryFirstOrDefaultAsync<GetAllUserSorolDto>(sql, new { UserName = userName });
    }

    //public async Task<string> GetCompanySorol()
    //{

    //}

}
