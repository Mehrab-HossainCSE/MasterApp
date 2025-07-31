using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class UpdateDatabaseNavCloudPosDBKMART
{

    private readonly IDbConnectionFactory _connectionFactory;

    public UpdateDatabaseNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<Result<string>> UpdateNavAsync(List<CreateNavInputDto> navDtos)
    {
        var connection = _connectionFactory.CreateConnection("CloudPos_DB_KMART");
        var transaction = connection.BeginTransaction();

        try
        {
            // 1. Truncate existing data
            await connection.ExecuteAsync("TRUNCATE TABLE MENU1", transaction: transaction);

            // 2. Map and insert new data
            string insertQuery = @"
                INSERT INTO MENU1 
                (SERIAL, PARENT_ID, DESCRIPTION, URL, PER_ROLE, ENTRY_BY, ENTRY_DATE, ORDER_BY, FA_CLASS, ID, MENU_TYPE, SHOW_EDIT_PERMISSION)
                VALUES 
                (@SERIAL, @PARENT_ID, @DESCRIPTION, @URL, @PER_ROLE, @ENTRY_BY, @ENTRY_DATE, @ORDER_BY, @FA_CLASS, @ID, @MENU_TYPE, @SHOW_EDIT_PERMISSION)
            ";

            var dbDtos = navDtos.Select(x => new CreateNavCloudPosDBKMARTDto
            {
                SERIAL = x.serial,
                PARENT_ID = x.parenT_ID ?? 0,
                DESCRIPTION = x.description,
                URL = x.url,
                PER_ROLE = x.peR_ROLE,
                ENTRY_BY = x.entrY_BY,
                ENTRY_DATE = x.entrY_DATE,
                ORDER_BY = x.ordeR_BY,
                FA_CLASS = x.fA_CLASS,
                ID = x.id,
                MENU_TYPE = x.menU_TYPE,
                SHOW_EDIT_PERMISSION = x.shoW_EDIT_PERMISSION
            });

            await connection.ExecuteAsync(insertQuery, dbDtos, transaction: transaction);

            transaction.Commit();
            return Result<string>.Success("Nav menu updated successfully.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return Result<string>.Fail("Update failed: " + ex.Message);
        }
    }
}
