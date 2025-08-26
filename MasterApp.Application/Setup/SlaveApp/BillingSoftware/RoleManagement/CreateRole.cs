using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.BillingSoftware.RoleManagement;

public class CreateRole(IDbConnectionFactory _connectionFactory)
{
    public async Task<IResult> InsertRoleAsync(CreateRoleBilling dto)
    {
        try
        {
            var sql = @"
                

                INSERT INTO [Management].[Role_1]
                    ( RoleName, Description, IsActive, CreatorId, CreateDate, UpdatorId, UpdateDate)
                VALUES
                    ( @RoleName, @Description, @IsActive, @CreatorId, @CreateDate, @UpdatorId, @UpdateDate);

                
            ";

            using var connection = _connectionFactory.CreateConnection("BillingSoft");

            var newId = await connection.ExecuteScalarAsync<int>(sql, dto);

            dto.RoleId = newId; // assign the generated ID back to the DTO

            return Result.Success( "Role created successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error creating role: {ex.Message}");
        }
    }
}

