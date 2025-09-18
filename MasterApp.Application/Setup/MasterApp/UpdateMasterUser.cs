using Dapper;
using MasterApp.Application.Common.Models;
using MasterApp.Application.Interface;
using MasterApp.Application.MasterAppDto;
using Microsoft.IdentityModel.Tokens;

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
                UserName = existingUser.UserName,

                FullName = string.IsNullOrEmpty(userDto.FullName) ? existingUser.FullName : userDto.FullName,
                Email = string.IsNullOrEmpty(userDto.Email) ? existingUser.Email : userDto.Email,
                DesignationID = userDto?.DesignationID ?? existingUser.DesignationID,
                MobileNo = string.IsNullOrEmpty(userDto.MobileNo) ? existingUser.MobileNo : userDto.MobileNo,
                Address = string.IsNullOrEmpty(userDto.Address) ? existingUser.Address : userDto.Address,

                ProjectListId = string.IsNullOrEmpty(userDto.ProjectListId) ? existingUser.ProjectListId : userDto.ProjectListId,
                RoleIdBilling = string.IsNullOrEmpty(userDto.RoleIdBilling) ? existingUser.RoleIdBilling : userDto.RoleIdBilling,
                ExpairsOnBilling = string.IsNullOrEmpty(userDto.ExpairsOnBilling) ? existingUser.ExpairsOnBilling : userDto.ExpairsOnBilling,
                IsMobileAppUserBilling = userDto.IsMobileAppUserBilling ,
                IMEIBilling = string.IsNullOrEmpty(userDto.IMEIBilling) ? existingUser.IMEIBilling : userDto.IMEIBilling,

                RoleIdSorol = string.IsNullOrEmpty(userDto.RoleIdSorol) ? existingUser.RoleIdSorol : userDto.RoleIdSorol,
                CompanyIdSorol = string.IsNullOrEmpty(userDto.CompanyIdSorol) ? existingUser.CompanyIdSorol : userDto.CompanyIdSorol,

                DES_IDVatPro = userDto.DES_IDVatPro,
                RoleIdVatPro =  userDto.RoleIdVatPro,
                NIDVatPro = string.IsNullOrEmpty(userDto.NIDVatPro) ? existingUser.NIDVatPro : userDto.NIDVatPro,
                BranchIDVatPro = string.IsNullOrEmpty(userDto.BranchIDVatPro) ? existingUser.BranchIDVatPro : userDto.BranchIDVatPro,
                BranchVatPro = string.IsNullOrEmpty(userDto.BranchVatPro) ? existingUser.BranchVatPro : userDto.BranchVatPro,

                CityCloudPos = string.IsNullOrEmpty(userDto.CityCloudPos) ? existingUser.CityCloudPos : userDto.CityCloudPos,
                StatusBilling =  userDto.StatusBilling,

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
            var sql = @"
                    UPDATE Users 
                    SET  
                        FullName               = @FullName, 
                        Email                  = @Email, 
                        DesignationID          = @DesignationID, 
                        MobileNo               = @MobileNo, 
                        Address                = @Address, 
                        PasswordHash           = @PasswordHash, 
                        PasswordSalt           = @PasswordSalt, 
                        PasswordEncrypted      = @PasswordEncrypted,
                        ProjectListId          = @ProjectListId,
                        RoleIdBilling          = @RoleIdBilling,
                        ExpairsOnBilling       = @ExpairsOnBilling,
                        IsMobileAppUserBilling = @IsMobileAppUserBilling,
                        IMEIBilling            = @IMEIBilling,
                        RoleIdSorol            = @RoleIdSorol,
                        CompanyIdSorol         = @CompanyIdSorol,
                        DES_IDVatPro           = @DES_IDVatPro,
                        RoleIdVatPro           = @RoleIdVatPro,
                        NIDVatPro              = @NIDVatPro,
                        BranchIDVatPro         = @BranchIDVatPro,
                        BranchVatPro           = @BranchVatPro,
                        CityCloudPos           = @CityCloudPos,
                        StatusBilling          = @StatusBilling,                      
                        UpdateDate             = @UpdateDate,
                        UpdateBy               = @UpdateBy
                    WHERE UserName = @UserName;
                ";

            await connection.ExecuteAsync(sql, new
            {
                updatedUser.UserName,
                updatedUser.FullName,
                updatedUser.Email,
                updatedUser.DesignationID,
                updatedUser.MobileNo,
                updatedUser.Address,

                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                updatedUser.PasswordEncrypted,

                updatedUser.ProjectListId,
                updatedUser.RoleIdBilling,
                updatedUser.ExpairsOnBilling,
                updatedUser.IsMobileAppUserBilling,
                updatedUser.IMEIBilling,

                updatedUser.RoleIdSorol,
                updatedUser.CompanyIdSorol,
                updatedUser.DES_IDVatPro,
                updatedUser.RoleIdVatPro,
                updatedUser.NIDVatPro,
                updatedUser.BranchIDVatPro,

                updatedUser.BranchVatPro,
                updatedUser.CityCloudPos,
                updatedUser.StatusBilling,

                
                UpdateDate = DateTime.Now,
                UpdateBy = userDto.UpdateBy  // pass the updater info
            });


            return Result<string>.Success($"User '{userDto.UserName}' updated successfully.");
        }
        catch (Exception ex)
        {
            return Result<string>.Fail("Error: " + ex.Message);
        }
    }
}