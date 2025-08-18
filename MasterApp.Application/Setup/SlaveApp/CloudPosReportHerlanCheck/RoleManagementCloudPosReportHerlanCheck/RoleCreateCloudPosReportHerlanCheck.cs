using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.SlaveDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.SlaveApp.CloudPosReportHerlanCheck.RoleManagementCloudPosReportHerlanCheck;

public class RoleCreateCloudPosReportHerlanCheck(IDbConnectionFactory _connectionFactory)
{
    public async Task<Result<int>> CreateRoleAsync(RoleCreateCloudPosReportHerlanCheckDto dto)
    {
        try
        {
            using var connection = _connectionFactory.CreateConnection("CloudPosReportHerlanCheck");
           

            // Insert Query
            string insertSql = @"
             INSERT INTO ROLE_1 ( ROLE_NAME, MENULISTID)
             VALUES ( @ROLENAME, @MENULISTID)";

            int rowsAffected = await connection.ExecuteAsync(insertSql, new
            {
               
                ROLENAME = dto.ROLE_NAME,
                MENULISTID = dto.MENULISTID
            });

            if (rowsAffected > 0)
                return Result<int>.Success(0);
            else
                return Result<int>.Fail("No rows were inserted.");
        }
        catch (Exception ex)
        {
            return Result<int>.Fail($"Error creating role: {ex.Message}");
        }
    }
}