using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;
using static System.Net.Mime.MediaTypeNames;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.NavSetting
{
    public class UpdateSorolSoftDatabaseNav
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UpdateSorolSoftDatabaseNav(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<Result<string>> UpdateNavAsync(List<SorolSoftDatabaseDto> navDtos)
        {
            try
            {
                // FIXED: Removed * from _connectionFactory and connection string
                using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

                // Check if connection string is properly configured
                if (string.IsNullOrEmpty(connection.ConnectionString))
                {
                    return Result<string>.Fail("Connection string 'BillingSoft' is not configured.");
                }

                connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    // 1. Truncate existing data
                    await connection.ExecuteAsync("TRUNCATE TABLE [AC_Master_DB].[dbo].[AC_MenuURL_1]", transaction: transaction);

                    // 2. Flatten the hierarchical data - THIS IS THE KEY FIX
                    var allMenuItems = FlattenMenuItems(navDtos);

                    // 3. Insert all items (parents and children)
                    string insertQuery = @"
                    INSERT INTO [AC_Master_DB].[dbo].[AC_MenuURL_1]
                    (MenuID, ParentID, ModuleID, URL, REL, IcoClass, Text, Serial)
                    VALUES 
                    (@MenuID, @ParentID, @ModuleID, @URL, @REL, @IcoClass, @Text, @Serial)
                ";

                    var dbDtos = allMenuItems.Select(x => new CreateSorolSoftNavDto
                    {
                        MenuID = x.menuID,
                        ParentID = x.parentID,
                        ModuleID = x.moduleID,
                        URL = x.url,
                        REL = x.rel,
                        IcoClass = x.icoClass,
                        Text=x.text,
                        Serial=x.serial,
                    }).ToList();

                    var rowsAffected = await connection.ExecuteAsync(insertQuery, dbDtos, transaction: transaction);

                    transaction.Commit();

                    return Result<string>.Success($"Nav menu updated successfully. {rowsAffected} records inserted (including {allMenuItems.Count(x => x.parentID > 0)} child items).");
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



        private List<SorolSoftDatabaseDto> FlattenMenuItems(List<SorolSoftDatabaseDto> menuItems)
        {
            var flattenedItems = new List<SorolSoftDatabaseDto>();

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
}
