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
                //var projectListIdCsv = await connection.ExecuteScalarAsync<string>(
                //    "SELECT ProjectListId FROM [Users] WHERE UserID = @UserID",
                //    new { UserID }
                //);

                //if (string.IsNullOrWhiteSpace(projectListIdCsv))
                //    return Result<List<NavsDto>>.Success(new List<NavsDto>(), "No projects found.");

                // 2. Split into list of ids
                //var projectIds = projectListIdCsv.Split(',')
                //                                 .Select(id => id.Trim())
                //                                 .ToList();

                // 3. Fetch matching projects
                //var query = @"
                //    SELECT Title AS title, NavigateUrl AS navigateUrl, LogoUrl AS logoUrl
                //    FROM ProjectList
                //    WHERE Id IN @Ids AND IsActive = 1";

                //projects = (await connection.QueryAsync<NavsDto>(query, new { Ids = projectIds })).ToList();

                //return Result<List<NavsDto>>.Success(projects, "Projects retrieved successfully.");


                var projectListCsv = await connection.ExecuteScalarAsync<string>(
                 "SELECT MenuList FROM [Users] WHERE UserID = @UserID",
                 new { UserID }
                );

                if (string.IsNullOrWhiteSpace(projectListCsv))
                    return Result<List<NavsDto>>.Success(new List<NavsDto>(), "No projects found.");

                // Split each "title:..." block
                var blocks = projectListCsv.Split("title:", StringSplitOptions.RemoveEmptyEntries);

                var projectss = new List<NavsDto>();

                foreach (var block in blocks)
                {
                    // Example block = "userCreate,navigateUrl:project/UserCreate"
                    var parts = block.Split(",", StringSplitOptions.RemoveEmptyEntries);

                    var title = parts.FirstOrDefault(p => p.Trim().StartsWith("userCreate", StringComparison.OrdinalIgnoreCase)
                                                       || p.Contains("title:", StringComparison.OrdinalIgnoreCase));
                    var navigateUrl = parts.FirstOrDefault(p => p.Contains("navigateUrl:", StringComparison.OrdinalIgnoreCase));

                    if (string.IsNullOrEmpty(title))
                        title = parts.FirstOrDefault()?.Trim(); // fallback

                    title = title?.Replace("title:", "").Trim();
                    navigateUrl = navigateUrl?.Replace("navigateUrl:", "").Trim();

                    if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(navigateUrl))
                    {
                        projectss.Add(new NavsDto
                        {
                            title = title,
                            navigateUrl = navigateUrl,
                            logoUrl = "" // optional
                        });
                    }
                }

                return Result<List<NavsDto>>.Success(projectss, "Projects retrieved successfully.");
            }
        }
        catch (Exception ex)
        {
            return Result<List<NavsDto>>.Fail($"Failed to retrieve projects: {ex.Message}");
        }
    }
}
