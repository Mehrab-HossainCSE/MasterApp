using System.Text.Json.Serialization;

namespace MasterApp.Application.SlaveDto;

public class BillingUserDto
{
    [JsonPropertyName("Id")]   
    
    public int Id { get; set; }
    [JsonPropertyName("Username")]
    public string Username { get; set; }

    [JsonPropertyName("RoleId")]
    public string RoleId { get; set; }
}


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