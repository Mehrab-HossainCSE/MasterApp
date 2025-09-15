using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;

namespace MasterApp.Application.Setup.MasterApp;

public class UpdateMasterUser(IPasswordHash _passwordHash, IDbConnectionFactory _dbConnectionFactory, IEncryption _encryption)
{
    public async Task<Result<string>> HandleAsync(UserUpdateDto userDto)
    {
        try
        {
            using var connection = _dbConnectionFactory.CreateConnection("MasterAppDB");

            // ✅ Step 1: Check if user exists by UserName
            var existingUser = await connection.QueryFirstOrDefaultAsync<UserCreateDto>(
                "SELECT * FROM Users WHERE UserName = @UserName",
                new { userDto.UserName }
            );

            if (existingUser == null)
            {
                return Result<string>.Fail("User not found with the given username.");
            }

            // ✅ Step 2: Update only the provided fields (keep old values if new is null/empty)
            var updatedUser = new UserCreateDto
            {
                UserName = existingUser.UserName, // cannot change username
                ShopID = string.IsNullOrEmpty(userDto.ShopID) ? existingUser.ShopID : userDto.ShopID,
                EmployeeID = string.IsNullOrEmpty(userDto.EmployeeID) ? existingUser.EmployeeID : userDto.EmployeeID,
                FullName = string.IsNullOrEmpty(userDto.FullName) ? existingUser.FullName : userDto.FullName,
                Email = string.IsNullOrEmpty(userDto.Email) ? existingUser.Email : userDto.Email,
                DesignationID = string.IsNullOrEmpty(userDto.DesignationID) ? existingUser.DesignationID : userDto.DesignationID,
                MobileNo = string.IsNullOrEmpty(userDto.MobileNo) ? existingUser.MobileNo : userDto.MobileNo,
                Address = string.IsNullOrEmpty(userDto.Address) ? existingUser.Address : userDto.Address,
                InActive = userDto.InActive ?? existingUser.InActive,
                Password = string.IsNullOrEmpty(userDto.Password) ? null : userDto.Password,
                PasswordEncrypted = string.IsNullOrEmpty(userDto.Password)
                                    ? existingUser.PasswordEncrypted
                                    : _encryption.Encrypt(userDto.Password)
            };

            // ✅ Step 3: Hash password if new password provided
            string passwordHash = existingUser.PasswordHash;
            string passwordSalt = existingUser.PasswordSalt;

            if (!string.IsNullOrEmpty(userDto.Password))
            {
                _passwordHash.CreateHash(userDto.Password, ref passwordHash, ref passwordSalt);
            }

            // ✅ Step 4: Update user record
            var sql = @"UPDATE Users 
                        SET ShopID = @ShopID, 
                            EmployeeID = @EmployeeID, 
                            FullName = @FullName, 
                            Email = @Email, 
                            DesignationID = @DesignationID, 
                            MobileNo = @MobileNo, 
                            Address = @Address, 
                            InActive = @InActive, 
                            PasswordHash = @PasswordHash, 
                            PasswordSalt = @PasswordSalt, 
                            PasswordEncrypted = @PasswordEncrypted,
                            UpdateDate = @UpdateDate,
                            UpdateBy = @UpdateBy
                        WHERE UserName = @UserName";

            await connection.ExecuteAsync(sql, new
            {
                updatedUser.UserName,
                updatedUser.ShopID,
                updatedUser.EmployeeID,
                updatedUser.FullName,
                updatedUser.Email,
                updatedUser.DesignationID,
                updatedUser.MobileNo,
                updatedUser.Address,
                updatedUser.InActive,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                updatedUser.PasswordEncrypted,
                UpdateDate = DateTime.Now,
                UpdateBy = "Admin"
            });

            return Result<string>.Success($"User '{userDto.UserName}' updated successfully.");
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Error: " + ex.Message);
        }
    }
}