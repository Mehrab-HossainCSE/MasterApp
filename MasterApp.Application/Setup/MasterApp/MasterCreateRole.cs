using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using System.Xml;

namespace MasterApp.Application.Setup.MasterApp;

public class MasterCreateRole(IDbConnectionFactory _connectionFactory)
{
    public async Task<IResult> InsertRoleAsync(MasterAppRoleDto dto)
    {
               
        try
        {
            using var connection = _connectionFactory.CreateConnection("MasterAppDB");

            // Step 1: Get next Id manually
            var maxIdQuery = "SELECT ISNULL(MAX(Id), 0) + 1 FROM [dbo].[RoleWiseMenu]";
            var newId = await connection.ExecuteScalarAsync<int>(maxIdQuery);

            dto.Id = newId; // assign new Id to DTO

            // Step 2: Insert record
            var sql = @"
                INSERT INTO [dbo].[RoleWiseMenu]
                    (Id, RoleName)
                VALUES
                    (@Id, @RoleName);
            ";

            await connection.ExecuteAsync(sql, dto);

            return Result.Success("Role created successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error creating role: {ex.Message}");
        }
    }
}