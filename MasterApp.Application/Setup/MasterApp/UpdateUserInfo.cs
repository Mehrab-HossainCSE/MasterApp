using Dapper;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.Setup.MasterApp;

public class UpdateUserInfo(IDbConnectionFactory dbConncetionFactory)
{
    private readonly IDbConnectionFactory _dbConnectionFactory = dbConncetionFactory;

    public async Task<int> UpdateUserAsync(UserCreateDto userDto)
    {
        using var connection = _dbConnectionFactory.CreateConnection("MasterAppDb");

        string query;

        if (!string.IsNullOrEmpty(userDto.Password)) // ✅ Password provided
        {
            query = @"
                UPDATE Users
                SET UserName = @UserName,
                    ShopID = @ShopID,
                    EmployeeID = @EmployeeID,
                    FullName = @FullName,
                    Email = @Email,
                    DesignationID = @DesignationID,
                    MobileNo = @MobileNo,
                    Address = @Address,
                    UpdateBy = @UpdateBy,
                    UpdateDate = @UpdateDate,
                    InActive = @InActive,
                    ProjectListId = @ProjectListId,
                    Password = @Password,
                    PasswordHash = @PasswordHash,
                    PasswordSalt = @PasswordSalt
                WHERE UserID = @UserID";
        }
        else // ✅ Password not provided
        {
            query = @"
                UPDATE Users
                SET UserName = @UserName,
                    ShopID = @ShopID,
                    EmployeeID = @EmployeeID,
                    FullName = @FullName,
                    Email = @Email,
                    DesignationID = @DesignationID,
                    MobileNo = @MobileNo,
                    Address = @Address,
                    UpdateBy = @UpdateBy,
                    UpdateDate = @UpdateDate,
                    InActive = @InActive,
                    ProjectListId = @ProjectListId
                WHERE UserID = @UserID";
        }

        var rowsAffected = await connection.ExecuteAsync(query, new
        {
            userDto.UserID,
            userDto.UserName,
            userDto.ShopID,
            userDto.EmployeeID,
            userDto.FullName,
            userDto.Email,
            userDto.DesignationID,
            userDto.MobileNo,
            userDto.Address,
            userDto.UpdateBy,
            userDto.UpdateDate,
            userDto.InActive,
            userDto.ProjectListId,
            userDto.Password,
            userDto.PasswordHash,
            userDto.PasswordSalt
        });

        return rowsAffected;
    }
}

