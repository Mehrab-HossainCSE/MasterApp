using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Transactions;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class UpdateMenuIdToTheRoleCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;
    public UpdateMenuIdToTheRoleCloudPosDBKMART( IDbConnectionFactory connectionFactory )
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<Result<string>> UpdateMenuIdsForRoleAsync(RoleUpdateDto dto)
    {
        using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            

            if (!string.IsNullOrWhiteSpace(dto.MENULISTID))
            {
                var deleteQuery = @"
                    UPDATE ROLE_1
                    SET MENULISTID = NULL
                    WHERE ID = @ID;
                ";

                 await connection.ExecuteAsync(deleteQuery, new { ID = dto.ID }, transaction);

                            // Step 2: Set new MENULISTID value
                 var updateQuery = @"
                    UPDATE ROLE_1
                    SET MENULISTID = @MENULISTID
                    WHERE ID = @ID;
                ";

           await connection.ExecuteAsync(updateQuery, new
                  {
                                ID = dto.ID,
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
