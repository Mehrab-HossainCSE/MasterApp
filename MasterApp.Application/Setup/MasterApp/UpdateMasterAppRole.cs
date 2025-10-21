using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp
{
    public class UpdateMasterAppRole
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UpdateMasterAppRole(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Result<string>> HandleAsync(UserCreateDto dto)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

                // ✅ Step 1: Check if user exists by UserName
                var userExists = await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM Users WHERE UserName = @UserName",
                    new { dto.UserName }
                );

                if (userExists == 0)
                    return Result<string>.Fail($"User not found with the username '{dto.UserName}'.");

                // ✅ Step 2: Update RoleId using UserName
                var sql = @"
                    UPDATE Users
                    SET 
                        RoleId = @RoleId,
                        UpdateBy = @UpdateBy,
                        UpdateDate = @UpdateDate
                    WHERE UserName = @UserName;
                ";

                int rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    dto.UserName,
                    dto.RoleId,
                    dto.UpdateBy,
                    UpdateDate = DateTime.Now
                });

                if (rowsAffected == 0)
                    return Result<string>.Fail("Failed to update user role. Please try again.");

                // ✅ Step 3: Success response
                return Result<string>.Success($"Role updated successfully for user '{dto.UserName}'.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Error updating user role: " + ex.Message);
            }
        }
    }
}
