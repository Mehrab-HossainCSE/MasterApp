using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Runtime.ExceptionServices;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class AssignUserMenuCloudPosDBKMART
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public AssignUserMenuCloudPosDBKMART(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<Result<string>> AssignUserMenu(UserMenuDto dto)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("CloudPosDBKMART");

            var sqlUpdate = "UPDATE AC_UserMenu SET MenuIdList = @MenuIdList WHERE ID = @ID";
            await connection.ExecuteAsync(sqlUpdate, new
            {
                dto.MenuIdList,
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
