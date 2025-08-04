using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetUserCloudPosDBKMART
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetUserCloudPosDBKMART(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<List<UserDto>> GetAllUserAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection("CloudPosDBKMART");

        var sql = "SELECT ID,UserId, MenuIdList  FROM AC_UserMenu";

        var result = await connection.QueryAsync<UserDto>(sql);

        return result.AsList();
    }
}
