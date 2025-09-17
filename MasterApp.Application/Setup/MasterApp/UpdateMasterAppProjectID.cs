using Dapper;
using MasterApp.Application.Interface;

namespace MasterApp.Application.Setup.MasterApp;

public class UpdateMasterAppProjectID(IDbConnectionFactory dbConnectionFactory) 
{
    public async Task UpdateMasterAppProjectListAsync(string userName, IEnumerable<int> successfulProjectIds)
    {
        if (successfulProjectIds == null || !successfulProjectIds.Any())
            return; // nothing to update

        var projectListId = string.Join(",", successfulProjectIds);

        using var connection = dbConnectionFactory.CreateConnection("MasterAppDB");

        var sql = @"UPDATE Users 
                SET ProjectListId = @ProjectListId 
                WHERE UserName = @UserName";

        await connection.ExecuteAsync(sql, new
        {
            ProjectListId = projectListId,
            UserName = userName
        });
    }

}
