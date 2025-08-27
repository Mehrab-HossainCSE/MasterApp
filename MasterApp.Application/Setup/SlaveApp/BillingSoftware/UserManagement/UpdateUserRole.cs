using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.UserManagement;

public class UpdateUserRole(IDbConnectionFactory _connectionFactory)
{
    public async Task<IResult> UpdateRoleAsync(UpdateRoleBillingDto dto)
    {
        try
        {
            var sql = @"
                UPDATE [Management].[User]
                SET 
                RoleId = @RoleId                                      
                WHERE Id   = @Id
            ";

            using var connection = _connectionFactory.CreateConnection("BillingSoft");

            var affectedRows = await connection.ExecuteAsync(sql, dto);

            if (affectedRows > 0)
                return Result.Success("Role updated successfully.");
            else
                return Result.Fail("No role found with the given ID.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error creating role: {ex.Message}");
        }
    }
}
