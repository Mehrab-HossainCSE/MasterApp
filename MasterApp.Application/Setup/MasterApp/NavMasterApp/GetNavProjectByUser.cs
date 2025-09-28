using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.Extensions.Options;
using System.Text.Json;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace MasterApp.Application.Setup.MasterApp.NavMasterApp;


public class GetNavProjectByUser(IDbConnectionFactory _dbConnectionFactory,IOptions<ApplicationUsers> _applicationUser)
{
    public async Task<IResult<List<ProjectNavDto>>> HandleAsync(string UserID)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

            List<ProjectNavDto> projects;

            if (UserID == "0" && _applicationUser.Value.IsMediaSoftUser) // ✅ Special condition: return all projects
            {
               
                var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProjectList", "Project.json");
                if (!File.Exists(jsonPath))
                    throw new FileNotFoundException("Project.json not found at " + jsonPath);

                var json = await File.ReadAllTextAsync(jsonPath);
                projects = JsonSerializer.Deserialize<List<ProjectNavDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ProjectNavDto>();
            }
            else
            {
                // 1. Get ProjectListId string from user table
                var projectListIdCsv = await connection.ExecuteScalarAsync<string>(
                    "SELECT ProjectListId FROM [Users] WHERE UserID = @UserID",
                    new { UserID }
                );

                if (string.IsNullOrWhiteSpace(projectListIdCsv))
                    return Result<List<ProjectNavDto>>.Success(new List<ProjectNavDto>(), "No projects found.");

                // 2. Split into list of ids
                var projectIds = projectListIdCsv.Split(',').Select(id => id.Trim()).ToList();

                // 3. Fetch matching projects
                var query = @"
                    SELECT Id, Title, NavigateUrl, LoginUrl, LogoUrl, IsActive , Password, UserName
                    FROM ProjectList
                    WHERE Id IN @Ids AND IsActive = 1";

                projects = (await connection.QueryAsync<ProjectNavDto>(query, new { Ids = projectIds })).ToList();
            }

            return Result<List<ProjectNavDto>>.Success(projects, "Projects retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<List<ProjectNavDto>>.Fail($"Failed to retrieve projects: {ex.Message}");
        }
    }
}