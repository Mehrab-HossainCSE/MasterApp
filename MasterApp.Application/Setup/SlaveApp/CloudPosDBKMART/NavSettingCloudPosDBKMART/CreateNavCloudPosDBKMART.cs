using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class CreateNavCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;

    public CreateNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> InsertAsync(CreateNavCloudPosDBKMARTDto dto)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
            var checkSql = "SELECT COUNT(1) FROM MENU WHERE SERIAL = @SERIAL";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { dto.SERIAL });

            if (exists > 0)
            {
                return 0;
            }
            var sql = @"
            INSERT INTO MENU 
            (SERIAL, PARENT_ID, DESCRIPTION, URL, PER_ROLE, ENTRY_BY, ENTRY_DATE, ORDER_BY, FA_CLASS, MENU_TYPE, SHOW_EDIT_PERMISSION)
            VALUES 
            (@SERIAL, @PARENT_ID, @DESCRIPTION, @URL, @PER_ROLE, @ENTRY_BY, GETDATE(), @ORDER_BY, @FA_CLASS, @MENU_TYPE, @SHOW_EDIT_PERMISSION);";

           
            return await connection.ExecuteAsync(sql, dto);
        }
        catch(Exception ex)
        {
            // Log the exception (not implemented here)
            throw new Exception("An error occurred while inserting data into CloudPosDBKMART", ex);
        }
          

    }
}
