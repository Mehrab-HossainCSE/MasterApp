namespace MasterApp.Application.MasterAppDto;

public class SSOUserCreateDto
{
    public string userName { get; set; }
    public string password { get; set; }
    public string? fullName { get; set; }
    public string? City { get; set; }
    public string email { get; set; }
  
    public int designationID { get; set; }
    public string? mobileNo { get; set; }
    public string? address { get; set; }
    public string? ExpairsOn { get; set; }
    public bool IsMobileAppUser { get; set; }
    public string? IMEI { get; set; }
    public bool? StatusBilling { get; set; }
    public int RoleId { get; set; }
      
    public string? NID { get; set; }
    public string? branch { get; set; }
    public string? sorolMenuIdList { get; set; }
    public string ? RoleIdBilling { get; set; }
    public string? companyIdSorol { get; set; }
    public string ProjectListId { get; set; }
    public string RoleIdSorol { get; set; }
}
