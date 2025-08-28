using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp;

public class UserCreate(IPasswordHash _passwordHash, IDbConnectionFactory _dbConnectionFactory, IEncryption _encryption)
{
    public async Task<Result<string>> HandleAsync(UserCreateDto userDto)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");



            // ✅ Step 1: Check if user exists
            var checkUserExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Users WHERE UserName = @UserName OR Email = @Email",
                new { userDto.UserName, userDto.Email }
            );

            if (checkUserExists > 0)
            {
                return Result<string>.Fail("User with the same username or email already exists.");
            }

            // ✅ Step 2: Get Max UserID and generate next
            var maxUserId = await connection.ExecuteScalarAsync<string>(
                "SELECT TOP 1 UserID FROM Users ORDER BY UserID DESC"
            );

            if (string.IsNullOrEmpty(maxUserId))
            {
                maxUserId = "1001";
            }
            else if (int.TryParse(maxUserId, out int currentMaxId))
            {
                maxUserId = (currentMaxId + 1).ToString("D4"); // Pads with zeros
            }
            else
            {
                maxUserId = "1001"; // fallback
            }

            // ✅ Step 3: Hash password
            string passwordHash = string.Empty;
            string passwordSalt = string.Empty;
            _passwordHash.CreateHash(userDto.Password, ref passwordHash, ref passwordSalt);

            // ✅ Step 4: Insert user
            var sql = @"INSERT INTO Users 
                        (UserID, UserName, ShopID, EmployeeID, FullName, Email, DesignationID, MobileNo, Address, InActive, PasswordHash, PasswordSalt)
                        VALUES 
                        (@UserID, @UserName, @ShopID, @EmployeeID, @FullName, @Email, @DesignationID, @MobileNo, @Address, @InActive, @PasswordHash, @PasswordSalt)";

            var parameters = new
            {
                UserID = maxUserId,  // ✅ auto-generated ID
                userDto.UserName,
                userDto.ShopID,
                userDto.EmployeeID,
                userDto.FullName,
                userDto.Email,
                userDto.DesignationID,
                userDto.MobileNo,
                userDto.Address,
                InActive = userDto.InActive ?? false,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await connection.ExecuteAsync(sql, parameters);

            return Result<string>.Success($"User created successfully with ID {maxUserId}.");
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Error: " + ex.Message);
        }
    }
}
