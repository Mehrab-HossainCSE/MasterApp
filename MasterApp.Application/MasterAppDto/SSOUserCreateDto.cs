namespace MasterApp.Application.MasterAppDto;

public class SSOUserCreateDto
{
    public string userName { get; set; }
    public string password { get; set; }
    public string fullName { get; set; }
    public string email { get; set; }
    public string shopID { get; set; }
    public string employeeID { get; set; }
    public string employeeName { get; set; }
    public int designationID { get; set; }
    public string mobileNo { get; set; }
    public string address { get; set; }

    public bool inActive { get; set; }
    public string roleName { get; set; }
    public string companyCode { get; set; }
    public string productPricePermission { get; set; }
    public string userLavel { get; set; }
    public string type { get; set; }
    public string NID { get; set; }
    public string branch { get; set; }
   
    public string ProjectListId { get; set; }
}
