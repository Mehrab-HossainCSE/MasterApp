using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System.Data;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.NavSettingCloudPosReportHerlanCheck;

public class UpdateDatabaseNavCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{
    public async Task<Result<string>> UpdateNavAsync(List<CreateNavInputDto> navDtos)
    {
        try
        {
            // FIXED: Removed * from _connectionFactory and connection string
            using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");

            // Check if connection string is properly configured
            if (string.IsNullOrEmpty(connection.ConnectionString))
            {
                return Result<string>.Fail("Connection string 'CloudPosReportHerlanCheck' is not configured.");
            }

            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // 1. Truncate existing data
                await connection.ExecuteAsync("TRUNCATE TABLE MENU_1", transaction: transaction);

                // 2. Flatten the hierarchical data - THIS IS THE KEY FIX
                var allMenuItems = FlattenMenuItems(navDtos);

                // 3. Insert all items (parents and children)
                string insertQuery = @"
                    INSERT INTO MENU
                    (SERIAL, PARENT_ID, DESCRIPTION, URL, PER_ROLE, ENTRY_BY, ENTRY_DATE, ORDER_BY, FA_CLASS, MENU_TYPE, SHOW_EDIT_PERMISSION)
                    VALUES 
                    (@SERIAL, @PARENT_ID, @DESCRIPTION, @URL, @PER_ROLE, @ENTRY_BY, @ENTRY_DATE, @ORDER_BY, @FA_CLASS, @MENU_TYPE, @SHOW_EDIT_PERMISSION)
                ";

                var dbDtos = allMenuItems.Select(x => new CreateNavCloudPosDBKMARTDto
                {
                    SERIAL = x.serial,
                    PARENT_ID = x.parenT_ID ?? 0,
                    DESCRIPTION = x.description,
                    URL = x.url,
                    PER_ROLE = x.peR_ROLE,
                    ENTRY_BY = x.entrY_BY,
                    ENTRY_DATE = DateTime.Now.Date,
                    ORDER_BY = x.ordeR_BY,
                    FA_CLASS = x.fA_CLASS,
                    MENU_TYPE = x.menU_TYPE,
                    SHOW_EDIT_PERMISSION = x.shoW_EDIT_PERMISSION
                }).ToList();

                var rowsAffected = await connection.ExecuteAsync(insertQuery, dbDtos, transaction: transaction);

                transaction.Commit();

                return Result<string>.Success($"Nav menu updated successfully. {rowsAffected} records inserted (including {allMenuItems.Count(x => x.parenT_ID > 0)} child items).");
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Update failed: " + ex.Message);
        }
    }
    private List<CreateNavInputDto> FlattenMenuItems(List<CreateNavInputDto> menuItems)
    {
        var flattenedItems = new List<CreateNavInputDto>();

        foreach (var item in menuItems)
        {
            // Add the parent item
            flattenedItems.Add(item);

            // Add all children recursively
            if (item.children != null && item.children.Any())
            {
                var childItems = FlattenMenuItems(item.children);
                flattenedItems.AddRange(childItems);
            }
        }

        return flattenedItems;
    }
}
