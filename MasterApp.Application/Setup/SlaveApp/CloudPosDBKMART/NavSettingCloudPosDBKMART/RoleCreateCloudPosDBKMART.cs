using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosDBKMART.NavSettingCloudPosDBKMART;

public class RoleCreateCloudPosDBKMART
{
    private readonly IDbConnectionFactory _connectionFactory;

    public RoleCreateCloudPosDBKMART(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task<Result<int>> CreateRoleAsync(RoleCreateDto dto)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection("CloudPosDBKMART");

            // Get Max ID
            string getMaxIdSql = "SELECT ISNULL(MAX(ID), 0) FROM ROLE_1";
            int maxId = await connection.ExecuteScalarAsync<int>(getMaxIdSql);
            int newId = maxId + 1;

            // Insert Query
            string insertSql = @"
                INSERT INTO ROLE_1 (ID, ROLENAME, MENULISTID)
                VALUES (@ID, @ROLENAME, @MENULISTID)";

            int rowsAffected = await connection.ExecuteAsync(insertSql, new
            {
                ID = newId,
                ROLENAME = dto.ROLENAME,
                MENULISTID = dto.MENULISTID
            });

            if (rowsAffected > 0)
                return Result<int>.Success(newId);
            else
                return Result<int>.Fail("No rows were inserted.");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error creating role: {ex.Message}");
        }
    }


}
