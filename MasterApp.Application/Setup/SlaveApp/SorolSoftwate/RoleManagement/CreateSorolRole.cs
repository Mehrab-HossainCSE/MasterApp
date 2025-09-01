using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

namespace MasterApp.Application.Setup.SlaveApp.SorolSoftwate.RoleManagement;

public class CreateSorolRole(IDbConnectionFactory _connectionFactory)
{
    public async Task<IResult> InsertRoleAsync(CreateSorolRoleDto dto)
    {
        try
        {
            var sql = @"
                        DECLARE @NewId INT;

                        SELECT @NewId = ISNULL(MAX(RoleId), 0) + 1 FROM [RoleMenu];

                        INSERT INTO [RoleMenu] 
                            (RoleId, RoleName, IsActive, CreatedBy, CreateDate, UpdateBy, UpdateDate)
                        VALUES
                            (@NewId, @RoleName, @IsActive, @CreatedBy, @CreateDate, @UpdateBy, @UpdateDate);

                        SELECT @NewId;
                    ";

            using var connection = _connectionFactory.CreateConnection("SorolSoftACMasterDB");

            var newId = await connection.ExecuteScalarAsync<int>(sql, dto);

            dto.RoleId = newId; // assign the generated ID back to the DTO

            return Result.Success("Role created successfully.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error creating role: {ex.Message}");
        }
    }
}
