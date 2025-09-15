namespace MasterApp.Application.MasterAppDto;

public class SSOUserUpdateDto
{
    public string userName { get; set; }
    public string password { get; set; }
    public string? fullName { get; set; }
    public string email { get; set; }
    public string? shopID { get; set; } = "0";
    public string? employeeID { get; set; } = "0";
    public string? employeeName { get; set; }
    public int? designationID { get; set; }
    public string? mobileNo { get; set; }
    public string? address { get; set; }
    public string? ExpairsOn { get; set; }
    public bool? IsMobileAppUser { get; set; }
    public string? IMEI { get; set; }
    public bool? inActive { get; set; }
    public int? RoleId { get; set; }
    public string? companyCode { get; set; } = "0";
    public string? productPricePermission { get; set; } = "0";
    public string? userLavel { get; set; } = "0";
    public string? type { get; set; } = "0";
    public string? NID { get; set; }
    public string? branch { get; set; }
    public string? sorolMenuIdList { get; set; }
    public string? RoleIdBilling { get; set; }
    public string? companyIdSorol { get; set; }
    public string ProjectListId { get; set; }
    public string RoleIdSorol { get; set; }
}

public class SorolUserUpdateDto
{
    public string Username { get; set; }
    public string Designation { get; set; }
    public string Password { get; set; }
    public string CompanyId { get; set; }
    public string Menulist { get; set; }
}

public class VatProUserUpdateDto
{
    public string USER_NAME { get; set; }
    public string USER_PASS { get; set; }
    public string FullName { get; set; }
    public bool ExcelPermission { get; set; }
    public string? BranchID { get; set; }
    public string NID { get; set; }
    public int? RoleId { get; set; }
    public string EMAIL { get; set; }
    public int? DES_ID { get; set; }
    public string MOBILE { get; set; }
    public string ADDRESS { get; set; }
    public bool IsActive { get; set; }
    public string UPDATE_BY { get; set; }
    public DateTime UPDATE_DATE { get; set; }
}

public class UserUpdateDto
{
    public string UserName { get; set; }
    public string? ShopID { get; set; }
    public string? EmployeeID { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string DesignationID { get; set; }
    public string MobileNo { get; set; }
    public string Address { get; set; }
    public bool? InActive { get; set; }
    public string Password { get; set; }
    public string ProjectListId { get; set; }
    public string PasswordEncrypted { get; set; }
    public string UpdateBy { get; set; }
    public DateTime UpdateDate { get; set; }
}

public class BillingUserUpdateDto
{
    public string Username { get; set; }
    public string FullName { get; set; }
    public string PhoneNo { get; set; }
    public string Password { get; set; }
    public string RoleId { get; set; }
    public string IsActive { get; set; }
    public string ExpairsOn { get; set; }
    public bool? IsMobileAppUser { get; set; }
    public string IMEI { get; set; }
    public string PayrollUsername { get; set; }
}