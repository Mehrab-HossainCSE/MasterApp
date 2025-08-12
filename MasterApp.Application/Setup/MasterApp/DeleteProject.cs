using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;

namespace MasterApp.Application.Setup.MasterApp;

public class DeleteProject
{
    private readonly IDbConnectionFactory _context;

    public DeleteProject(IDbConnectionFactory context)
    {
        _context = context;
    }

    public async Task<IResult> HandleAsync(int projectId, string wwwRootPath)
    {
        using var connection = _context.CreateConnection("MasterAppDB");

        // 1. Get the existing logo path
        var logoUrl = await connection.QuerySingleOrDefaultAsync<string>(
            "SELECT LogoUrl FROM ProjectList WHERE Id = @Id",
            new { Id = projectId }
        );

        if (logoUrl == null)
        {
            return Result.Fail($"Project with ID {projectId} not found.");
        }

        // 2. Delete the record
        var rowsAffected = await connection.ExecuteAsync(
            "DELETE FROM ProjectList WHERE Id = @Id",
            new { Id = projectId }
        );

        if (rowsAffected == 0)
        {
            return Result.Fail("Failed to delete the project.");
        }

        // 3. Delete logo file from wwwroot
        if (!string.IsNullOrEmpty(logoUrl))
        {
            var filePath = Path.Combine(wwwRootPath, logoUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        return Result.Success("Project deleted successfully.");
    }
}
