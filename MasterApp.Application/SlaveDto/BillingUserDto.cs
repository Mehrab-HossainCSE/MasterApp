using System.Text.Json.Serialization;

namespace MasterApp.Application.SlaveDto;

//public class BillingUserDto
//{
//    [JsonPropertyName("Id")]   
    
//    public int Id { get; set; }
//    [JsonPropertyName("Username")]
//    public string Username { get; set; }

//    [JsonPropertyName("RoleId")]
//    public string RoleId { get; set; }
//}


public class BillingUserMenuDto
{
    public int MenuId { get; set; }
    public string MenuName { get; set; }
    public int ParentMenuId { get; set; }
    public string Url { get; set; }
    public int Sorting { get; set; }
    public bool IsActive { get; set; }
    public bool CanView { get; set; }

    public List<BillingUserMenuDto> Children { get; set; } = new();
}

public record UpdateRoleBillingDto 
{
    public int Id { get; set; }
    public int RoleId { get; set; }
   
}


public class BillingUserResponse
{
    public bool success { get; set; }
    public string? msg { get; set; }
    public int counts { get; set; }
    public List<BillingUserDto>? data { get; set; }
}

public class BillingUserDto
{
    public int id { get; set; }
    public string? username { get; set; }
    public string? fullname { get; set; }
    public string? phoneNo { get; set; }
    public string? roleId { get; set; }
    public string? roleName { get; set; }
    public DateTime expairsOn { get; set; }
    public string? imei { get; set; }
    public bool isMobileAppUser { get; set; }
    public bool isActive { get; set; }
    public string? user_Status { get; set; }
    public string? payrollUsername { get; set; }
}
