using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;

public class GetParentNav
{
    private readonly IDbConnectionFactory _connectionFactory;

    public GetParentNav(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ParentBillingSoftNavDto>> GetParentsAsync()
    {
        var sql = @"
            SELECT DISTINCT menuId, menuName
            FROM [Management].[Menu]
            WHERE ParentMenuId = 0";

        using var connection = _connectionFactory.CreateConnection("BillingSoft");

        return await connection.QueryAsync<ParentBillingSoftNavDto>(sql);
    }
}
