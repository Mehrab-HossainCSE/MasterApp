using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.UserManagement;

public class GetAllUserSorol
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetAllUserSorol(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<List<GetAllUserSorolDto>> GetAllUserAsync()
    {
        using var connection = _dbConnectionFactory.CreateConnection("SorolSoftACMasterDB");

        var sql = "SELECT UserId, UserName,Designation,CompanyId  FROM [AC_Users]";

        var result = await connection.QueryAsync<GetAllUserSorolDto>(sql);

        return result.AsList();
    }
}
