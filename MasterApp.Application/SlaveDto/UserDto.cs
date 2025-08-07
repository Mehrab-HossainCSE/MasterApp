namespace MasterApp.Application.SlaveDto;

public class UserDto
{
    public int ID { get; set; }
    public string UserId { get; set; }
    public string MenuIdList { get; set; } = null!; 
}
public class UserMenuDto
{
    public int ID { get; set; }
   
    public string MenuIdList { get; set; } = null!;
}
