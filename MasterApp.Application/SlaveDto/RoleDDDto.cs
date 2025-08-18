namespace MasterApp.Application.SlaveDto;

public class RoleDDDto
{
    public string ROLENAME { get; set; } = null!;
    public List<MenuRole> menuRoles { get; set; } = new();
}

public class MenuRole
{
    public decimal SERIAL { get; set; }
    public string DESCRIPTION { get; set; } = null!;
}

public class RoleCloudPosReportHerlanCheckDDDto
{
    public string ROLE_NAME { get; set; } = null!;
    public List<MenuRole> menuRoles { get; set; } = new();
}
