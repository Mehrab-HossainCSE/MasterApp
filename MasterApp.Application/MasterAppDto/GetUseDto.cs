namespace MasterApp.Application.MasterAppDto;

public class GetUseDto
{ 
    public int userID { get; set; }
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
