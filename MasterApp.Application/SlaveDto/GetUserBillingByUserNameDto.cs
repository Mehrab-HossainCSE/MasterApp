namespace MasterApp.Application.SlaveDto;

public class GetUserBillingByUserNameDto
{
    public bool success { get; set; }
    public string msg { get; set; }
    public List<BillingUser> data { get; set; }
}

public class BillingUser
{
    public int id { get; set; }
    public string username { get; set; }
    public string fullname { get; set; }
    public string phoneNo { get; set; }
    public string roleId { get; set; }
    public string roleName { get; set; }
    public DateTime expairsOn { get; set; }
    public string imei { get; set; }
    public bool isMobileAppUser { get; set; }
    public bool isActive { get; set; }
    public string user_Status { get; set; }
    public string payrollUsername { get; set; }
}