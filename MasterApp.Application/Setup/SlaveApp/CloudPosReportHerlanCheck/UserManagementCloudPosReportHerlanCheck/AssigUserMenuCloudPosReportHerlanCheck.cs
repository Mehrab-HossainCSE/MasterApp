using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.UserManagementCloudPosReportHerlanCheck;

public class AssigUserMenuCloudPosReportHerlanCheck(IDbConnectionFactory _dbConnectionFactory)
{
    public async Task<Result<string>> AssignUserMenu(UserMenuDto dto)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("CloudPosReportHerlanCheck");

            var sqlUpdate = "UPDATE AC_UserMenu SET MenuIdList = @MenuIdList WHERE ID = @ID";
            await connection.ExecuteAsync(sqlUpdate, new
            {
                MenuIdList = "-" + dto.MenuIdList,
                dto.ID
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
