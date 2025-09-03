using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.UserManagement;

public class AssignUserMenuSorol
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public AssignUserMenuSorol(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<Result<string>> AssignUserMenu(UserMenuSorolDto dto)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("SorolSoftACMasterDB");

            var sqlUpdate = "UPDATE AC_UserMenu SET MenuIdList = @MenuIdList WHERE UserId = @UserId";
            await connection.ExecuteAsync(sqlUpdate, new
            {
                MenuIdList = "-" + dto.MenuIdList,
                dto.UserId
            });



            return Result<string>.Success("Updated MENULISTIDs successfully.");
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return Result<string>.Fail("Error: " + ex.Message);
        }
    }
}
