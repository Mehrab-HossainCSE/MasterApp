using System.Text.Json.Serialization;

namespace MasterApp.Application.MasterAppDto;

public class UserCreateDto
{
    public string? UserID { get; set; }
    public string? UserName { get; set; }
   
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int? DesignationID { get; set; }
    public string? MobileNo { get; set; }
    public string? Address { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool? StatusBilling { get; set; }
    public string? Password { get; set; } 
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public string? PasswordEncrypted { get; set; }
    public string? ProjectListId { get; set; }
    public string? RoleIdBilling { get; set; }
    public string? ExpairsOnBilling { get; set; }
    public bool IsMobileAppUserBilling { get; set; }
    public string? IMEIBilling { get; set; }
    public string RoleIdSorol { get; set; }
    public string? CompanyIdSorol { get; set; }
    public int? DES_IDVatPro { get; set; }
    public int RoleIdVatPro { get; set; }
    public string? NIDVatPro { get; set; }
    public string? BranchIDVatPro { get; set; }
    public string? CityCloudPos { get; set; }
    public string? BranchVatPro { get; set; } = "0";
    public string? MenuList { get; set; }
}

public class ProjectUpdateDto
{
    public string? UserID { get; set; }
    public string? ProjectListId { get; set; }
}