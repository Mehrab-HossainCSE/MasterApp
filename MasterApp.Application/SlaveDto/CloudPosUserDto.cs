using System.Text.Json.Serialization;

namespace MasterApp.Application.SlaveDto;

public class CloudPosUserDto
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? ApiKeyUser { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Password { get; set; }
}

public class CloudPosApiKeyDto
{
    public string? username { get; set; }
    public string? password { get; set; }
}
public class MenuCreateCoudPos
{
    public string UserName { get; set; }
    public List<int> MenuIdList { get; set; }
    public string? ApiKeyUser { get; set; }
}

public class ChangeCloudPosPasswordDto
{
    public string UserName { get; set; }

    public string? ApiKeyUser { get; set; }
    public string NewPassword { get; set; }
}