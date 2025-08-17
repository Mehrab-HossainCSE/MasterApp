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
            var query = "SELECT Id, Title, NavigateUrl,LoginUrl, LogoUrl,IsActive FROM ProjectList";
            var projects = (await connection.QueryAsync<ProjectDtos>(query)).ToList();

            return Result<List<ProjectDtos>>.Success(projects, "Projects retrieved successfully.");
        }
        catch (Exception ex)
        {
            return Result<List<ProjectDtos>>.Fail($"Failed to retrieve projects: {ex.Message}");
        }
    }

}
