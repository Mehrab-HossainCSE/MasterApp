namespace MasterApp.Application.SlaveDto;

public class RoleCreateDto
{
    public int ID { get; set; }
    public string ROLENAME { get; set; }
    public string? MENULISTID { get; set; } = null;
}

public class RoleUpdateDto
{
    public int ID { get; set; }
   
    public string? MENULISTID { get; set; } = null;
}