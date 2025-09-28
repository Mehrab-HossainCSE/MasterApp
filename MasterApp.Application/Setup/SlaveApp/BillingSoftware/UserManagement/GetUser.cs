using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using MasterApp.Application.SlaveDto;
using System.Net.Http;
using System.Net.Http.Json;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;

public class GetUser
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly HttpClient _httpClient;
    public GetUser(IDbConnectionFactory dbConnectionFactory, HttpClient httpClient)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _httpClient = httpClient;
    }
    //public async Task<List<BillingUserDto>> GetAllUserAsync()
    //{
    //    using var connection = _dbConnectionFactory.CreateConnection("BillingSoft");

    //    var sql = "SELECT Id,Username ,RoleId FROM [Management].[User]";

    //    var result = await connection.QueryAsync<BillingUserDto>(sql);

    //    return result.AsList();
    //}

    public async Task<List<BillingUserDto>> GetAllUserAsync()
    {
        var url = $"https://software.mediasoftbd.com/testbill/api/User/GetUser?searchQuery=&pageNo=0&itemPerPage=2000";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API call failed: {response.StatusCode}");
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<BillingUserResponse>();

        if (apiResponse?.data == null || apiResponse.data.Count == 0)
        {
            throw new Exception("Invalid or empty response from server");
        }

        return apiResponse.data;
    }



}
