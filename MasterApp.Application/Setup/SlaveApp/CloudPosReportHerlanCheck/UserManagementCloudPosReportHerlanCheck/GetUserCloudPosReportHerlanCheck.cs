using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.UserManagementCloudPosReportHerlanCheck;

public class GetUserCloudPosReportHerlanCheck(IDbConnectionFactory _dbConnectionFactory)
{
    public async Task<List<UserDto>> GetAllUserAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection("CloudPosReportHerlanCheck");

        var sql = "SELECT ID,UserId, MenuIdList  FROM AC_UserMenu";

        var result = await connection.QueryAsync<UserDto>(sql);

        return result.AsList();
    }
}
