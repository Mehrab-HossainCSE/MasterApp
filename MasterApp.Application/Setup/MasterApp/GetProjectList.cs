using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.MasterApp;

public class GetProjectList
{

    private readonly IDbConnectionFactory _context;

    public GetProjectList(IDbConnectionFactory context)
    {
        _context = context;
    }

    public async Task<IResult<List<ProjectDtos>>> HandleAsync()
    {
        try
        {
            using var connection = _context.CreateConnection("MasterAppDB");

            // 1. Get all projects
            var projectsQuery = "SELECT Id, Title, NavigateUrl, LoginUrl, LogoUrl, IsActive, CAST(0 AS BIT) AS IsChecked FROM ProjectList";
            var projects = (await connection.QueryAsync<ProjectDtos>(projectsQuery)).ToList();

            if (projects == null || !projects.Any())
            {
                return Result<List<ProjectDtos>>.Success(new List<ProjectDtos>(), "No projects found.");
            }

            // 2. Get the comma-separated list of project IDs for the user
            // Use a parameterized query to prevent SQL injection.
            //var userProjectsQuery = "SELECT ProjectListId FROM Users WHERE UserID = @UserID";
            //var userProjectIdsString = await connection.QueryFirstOrDefaultAsync<string>(userProjectsQuery, new { UserID = UserID });

            //// 3. Check if the user has any assigned projects
            //if (!string.IsNullOrEmpty(userProjectIdsString))
            //{
            //    // Split the string into a list of individual project IDs
            //    var userProjectIds = userProjectIdsString.Split(',')
            //                                             .Select(id => id.Trim())
            //                                             .Where(id => !string.IsNullOrEmpty(id))
            //                                             .ToList();

            //    // 4. Mark projects in the main list that match the user's projects as checked
            //    foreach (var project in projects)
            //    {
            //        if (userProjectIds.Contains(project.Id.ToString()))
            //        {
            //            project.IsChecked = true;
            //        }
            //    }
            //}

            return Result<List<ProjectDtos>>.Success(projects, "Projects retrieved and checked successfully.");
        }
        catch (Exception ex)
        {
            return Result<List<ProjectDtos>>.Fail($"Failed to retrieve projects: {ex.Message}");
        }
    }
}

    


