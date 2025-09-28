using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.Extensions.Options;

namespace MasterApp.Application.Setup.MasterApp.NavMasterApp;

public class GetNavByUserId(IDbConnectionFactory _dbConnectionFactory, IOptions<ApplicationUsers> _applicationUser)
{
    public async Task<IResult<List<NavsDto>>> HandleAsync(string UserID)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

            List<NavsDto> projects;

            if (UserID == "0" && _applicationUser.Value.IsMediaSoftUser) // ✅ Special condition: return static navs
            {
                projects = new List<NavsDto>
                {
                    new NavsDto { title = "Menu Setup", navigateUrl = "project/menusetup", logoUrl = "/icons/menu.png" },
                    new NavsDto { title = "User Create", navigateUrl = "project/usercreate", logoUrl = "/icons/user.png" }
                };

                return Result<List<NavsDto>>.Success(projects, "Static navs for MediaSoft user");
            }
            else
            {
                // 1. Get ProjectListId string from user table
                var projectListIdCsv = await connection.ExecuteScalarAsync<string>(
                    "SELECT ProjectListId FROM [Users] WHERE UserID = @UserID",
                    new { UserID }
                );

                if (string.IsNullOrWhiteSpace(projectListIdCsv))
                    return Result<List<NavsDto>>.Success(new List<NavsDto>(), "No projects found.");

                // 2. Split into list of ids
                var projectIds = projectListIdCsv.Split(',')
                                                 .Select(id => id.Trim())
                                                 .ToList();

                // 3. Fetch matching projects
                var query = @"
                    SELECT Title AS title, NavigateUrl AS navigateUrl, LogoUrl AS logoUrl
                    FROM ProjectList
                    WHERE Id IN @Ids AND IsActive = 1";

                projects = (await connection.QueryAsync<NavsDto>(query, new { Ids = projectIds })).ToList();

                return Result<List<NavsDto>>.Success(projects, "Projects retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            return Result<List<NavsDto>>.Fail($"Failed to retrieve projects: {ex.Message}");
        }
    }
}
