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
