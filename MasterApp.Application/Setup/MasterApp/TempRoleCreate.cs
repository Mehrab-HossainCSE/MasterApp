using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using System.Text.Json;

namespace MasterApp.Application.Setup.MasterApp;

public class TempRoleCreate
{
    private readonly IDbConnectionFactory _context;

    public TempRoleCreate(IDbConnectionFactory context)
    {
        _context = context;
    }

    public async Task<Result> UpdateRoleWiseMenuAsync(TemRoleCreateDto dto)
    {
        try
        {
            using var connection = _context.CreateConnection("MasterAppDB");

            // Convert full projectMenus list into JSON
            string projectMenusJson = JsonSerializer.Serialize(dto.projectMenus);

            string query = @"
            UPDATE [MasterAppDB].[dbo].[RoleWiseMenu]
            SET MenuIdList = @MenuIdList
            WHERE Id = @RoleId";

            var parameters = new
            {
                MenuIdList = projectMenusJson,
                RoleId = dto.roleId
            };

            int rowsAffected = await connection.ExecuteAsync(query, parameters);

            return rowsAffected > 0
                ? Result.Success("Role menus updated successfully.")
                : Result.Warning("No matching role found or no changes made.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"An error occurred while updating role menus: {ex.Message}");
        }
    }

}
