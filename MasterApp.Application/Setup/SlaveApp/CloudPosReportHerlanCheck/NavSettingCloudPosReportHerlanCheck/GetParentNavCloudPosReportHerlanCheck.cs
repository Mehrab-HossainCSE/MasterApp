using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.NavSettingCloudPosReportHerlanCheck;

public class GetParentNavCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{
 

    public async Task<IEnumerable<ParentCloudPosDBKMARTNavDto>> GetParentsAsync()
    {
        var sql = @"
            SELECT DISTINCT SERIAL, DESCRIPTION
            FROM MENU
            WHERE PARENT_ID = 0";

        using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");

        return await connection.QueryAsync<ParentCloudPosDBKMARTNavDto>(sql);
    }
}
