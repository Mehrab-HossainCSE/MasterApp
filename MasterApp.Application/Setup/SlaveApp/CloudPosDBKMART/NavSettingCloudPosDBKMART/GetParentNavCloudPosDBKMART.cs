using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetParentNavCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetParentNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ParentCloudPosDBKMARTNavDto>> GetParentsAsync()
    {
        var sql = @"
            SELECT DISTINCT SERIAL, DESCRIPTION
            FROM MENU
            WHERE PARENT_ID = 0";

        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");

        return await connection.QueryAsync<ParentCloudPosDBKMARTNavDto>(sql);
    }
}
