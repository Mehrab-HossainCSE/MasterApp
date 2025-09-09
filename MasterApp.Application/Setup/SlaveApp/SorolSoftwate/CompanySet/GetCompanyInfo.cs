using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.CompanySet;

public class GetCompanyInfo(IDbConnectionFactory _connectionFactory)
{
    public async Task<List<CompanySorolDto>> GetAllCompanyList()
    {
        using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

        var sql = "SELECT CFullName,CShortName FROM [AC_CompanyList]";

        var result = await connection.QueryAsync<CompanySorolDto>(sql);

        return result.AsList();
    }
}

public class CompanySorolDto
{
    public string CFullName { get; set; }
    public string CShortName { get; set; }
}