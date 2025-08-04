using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class UpdateNavCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UpdateNavCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> UpdateAsync(CreateNavCloudPosDBKMARTDto dto)
    {
        try
        {
            var sql = @"
                    UPDATE MENU 
                    SET 
                        PARENT_ID = @PARENT_ID,
                        DESCRIPTION = @DESCRIPTION,
                        URL = @URL,
                        PER_ROLE = @PER_ROLE,
                        ENTRY_BY = @ENTRY_BY,
                        ENTRY_DATE = GETDATE(),
                        ORDER_BY = @ORDER_BY,
                        FA_CLASS = @FA_CLASS,
                        MENU_TYPE = @MENU_TYPE,
                        SHOW_EDIT_PERMISSION = @SHOW_EDIT_PERMISSION
                    WHERE SERIAL = @SERIAL;";


            using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");
            return await connection.ExecuteAsync(sql, dto);
        }
        catch (Exception ex)
        {
            // Log the exception (not implemented here)
            throw new Exception("An error occurred while Update data into CloudPosDBKMART", ex);
        }


    }
}
