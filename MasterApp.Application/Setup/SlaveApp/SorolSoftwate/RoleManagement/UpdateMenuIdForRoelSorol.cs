using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement;

public class UpdateMenuIdForRoelSorol(IDbConnectionFactory _connectionFactory)
{
    public async Task<Result<string>> UpdateMenuIdsForRoleAsync(RoleMenuListUpdateDto dto)
    {
        using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {

            if (!string.IsNullOrWhiteSpace(dto.MenuListID))
            {
               

                // Step 2: Set new MENULISTID value
                var updateQuery = @"
                    UPDATE RoleMenu
                    SET MenuListID = @MenuListID
                    WHERE RoleId = @RoleId;
                ";

                await connection.ExecuteAsync(updateQuery, new
                {
                    RoleId = dto.RoleId,
                    MenuListID = "-"+dto.MenuListID
                }, transaction);

            }

            transaction.Commit();
            return Result<string>.Success("Updated MENULISTIDs successfully.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Result<string>.Fail("Error: " + ex.Message);
        }
    }
}
