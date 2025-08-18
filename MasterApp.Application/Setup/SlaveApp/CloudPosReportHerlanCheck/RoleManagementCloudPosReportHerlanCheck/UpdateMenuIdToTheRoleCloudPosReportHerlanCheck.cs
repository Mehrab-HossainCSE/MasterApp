using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.RoleManagementCloudPosReportHerlanCheck;

public class UpdateMenuIdToTheRoleCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{
    public async Task<Result<string>> UpdateMenuIdsForRoleAsync(RoleUpdateCloudPosReportHerlanCheckDto dto)
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {


            if (!string.IsNullOrWhiteSpace(dto.MENULISTID))
            {
                var deleteQuery = @"
                  UPDATE ROLE_1
                  SET MENULISTID = NULL
                  WHERE ROLE_NAME = @ROLE_NAME;
              ";

                await connection.ExecuteAsync(deleteQuery, new { ROLE_NAME = dto.ROLE_NAME }, transaction);

                // Step 2: Set new MENULISTID value
                var updateQuery = @"
                  UPDATE ROLE_1
                  SET MENULISTID = @MENULISTID
                  WHERE ROLE_NAME = @ROLE_NAME;
              ";

                await connection.ExecuteAsync(updateQuery, new
                {
                    ROLE_NAME = dto.ROLE_NAME,
                    MENULISTID = dto.MENULISTID
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
