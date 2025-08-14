using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class GetCompanyInfoCloudPosDBKMART(IDbConnectionFactory _connectionFactory)
{
    public async Task<CompanyInfoDto> GetCompanyInfoAsync()
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
        const string query = @"
            SELECT *
              
            FROM COMPANY_INFO";
        var companyInfo = await connection.QueryFirstOrDefaultAsync<CompanyInfoDto>(query);
        return companyInfo ?? new CompanyInfoDto();
    }
}
