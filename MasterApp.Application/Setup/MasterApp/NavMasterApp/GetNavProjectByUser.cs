using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace MasterApp.Application.Setup.MasterApp.NavMasterApp;

//public class GetNavProjectByUser(IDbConnectionFactory _dbConnectionFactory)
//{
//    public async Task<IResult<List<ProjectNavDto>>> HandleAsync(string UserID)
//    {
//        try
//        {
//            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

//            // 1. Get ProjectListId string from user table
//            var projectListIdCsv = await connection.ExecuteScalarAsync<string>(
//                "SELECT ProjectListId FROM [Users] WHERE UserID = @UserID",
//                new { UserID = UserID }
//            );

//            if (string.IsNullOrWhiteSpace(projectListIdCsv))
//                return Result<List<ProjectNavDto>>.Success(new List<ProjectNavDto>(), "No projects found.");

//            // 2. Split into list of ids
//            var projectIds = projectListIdCsv.Split(',').Select(id => id.Trim()).ToList();

//            // 3. Fetch matching projects
//            var query = @"
//            SELECT Id, Title, NavigateUrl, LoginUrl, LogoUrl, IsActive
//            FROM ProjectList
//            WHERE Id IN @Ids AND IsActive = 1";

//            var projects = (await connection.QueryAsync<ProjectNavDto>(query, new { Ids = projectIds })).ToList();

//            return Result<List<ProjectNavDto>>.Success(projects, "Projects retrieved successfully.");
//        }
//        catch (Exception ex)
//        {
//            return Result<List<ProjectNavDto>>.Fail($"Failed to retrieve projects: {ex.Message}");
//        }
//    }

//}
public class GetNavProjectByUser(IDbConnectionFactory _dbConnectionFactory)
{
    public async Task<IResult<List<ProjectNavDto>>> HandleAsync(string UserID)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

            List<ProjectNavDto> projects;

            if (UserID == "0") // ✅ Special condition: return all projects
            {
                var queryAll = @"
                    SELECT Id, Title, NavigateUrl, LoginUrl, LogoUrl, IsActive, Password, UserName
                    FROM ProjectList
                    WHERE IsActive = 1";

                projects = (await connection.QueryAsync<ProjectNavDto>(queryAll)).ToList();
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