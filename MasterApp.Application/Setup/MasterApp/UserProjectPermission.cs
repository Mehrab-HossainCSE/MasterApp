using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp;

public class UserProjectPermission(IDbConnectionFactory dbConnectionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

    public async Task<int> UpdateUserMenuListAsync(ProjectUpdateDto userDto)
    {
        using var connection = _dbConnectionFactory.CreateConnection("MasterAppDb");

        var query = @"
            UPDATE Users
            SET ProjectListId = @ProjectListId
            WHERE UserID = @UserID";

        // ExecuteAsync returns number of affected rows
        var rowsAffected = await connection.ExecuteAsync(query, new
        {
            UserID = userDto.UserID,
            ProjectListId = userDto.ProjectListId,
        });

        return rowsAffected;
    }
}