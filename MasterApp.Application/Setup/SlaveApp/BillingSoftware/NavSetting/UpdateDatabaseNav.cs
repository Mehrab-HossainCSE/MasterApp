using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.NavSetting;

public class UpdateDatabaseNav
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UpdateDatabaseNav(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<Result<string>> UpdateNavAsync(List<BillingSoftwareDatabaseDto> navDtos)
    {
        try
        {
            // FIXED: Removed * from _connectionFactory and connection string
            using var connection = _connectionFactory.CreateConnection("BillingSoft");

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
                await connection.ExecuteAsync("TRUNCATE TABLE [Management].[Menu_1]", transaction: transaction);

                // 2. Flatten the hierarchical data - THIS IS THE KEY FIX
                var allMenuItems = FlattenMenuItems(navDtos);

                // 3. Insert all items (parents and children)
                string insertQuery = @"
                    INSERT INTO [Management].[Menu_1]
                    (MenuId, ParentMenuId, MenuName, Url, Sorting, IsActive, ApplicationId, CreatorId, CreateDate)
                    VALUES 
                    (@MenuId, @ParentMenuId, @MenuName, @Url, @Sorting, @IsActive, @ApplicationId, @CreatorId, @CreateDate)
                ";

                var dbDtos = allMenuItems.Select(x => new BillingSoftNevCreateDto
                {
                    MenuId = x.menuId,
                    ParentMenuId = x.parentMenuId ,
                    MenuName = x.menuName,
                    Url = x.url,
                    Sorting = x.sorting,
                    IsActive = x.isActive,
                    ApplicationId = x.applicationId,
                    CreatorId = x.creatorId,
                    CreateDate = DateTime.Now.Date,
                   
                }).ToList();

                var rowsAffected = await connection.ExecuteAsync(insertQuery, dbDtos, transaction: transaction);

                transaction.Commit();

                return Result<string>.Success($"Nav menu updated successfully. {rowsAffected} records inserted (including {allMenuItems.Count(x => x.parentMenuId > 0)} child items).");
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



    private List<BillingSoftwareDatabaseDto> FlattenMenuItems(List<BillingSoftwareDatabaseDto> menuItems)
    {
        var flattenedItems = new List<BillingSoftwareDatabaseDto>();

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
