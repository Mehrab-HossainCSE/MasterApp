namespace MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

//public class GetMenuByRoleSorolDto
//{
//    public Dictionary<string, List<RoleMenuGroup>> RoleDetails { get; set; } = new();
//}

//public class RoleMenuGroup
//{
//    public ParentMenuInfo Parent { get; set; }
//    public List<MenuDetails> Menus { get; set; } = new();
//}
//public class ParentMenuInfo
//{
//    public int ParentID { get; set; }
//    public string Text { get; set; }
//}

//public class MenuDetails
//{
//    public int MenuID { get; set; }
//    public string Text { get; set; }
//    public string? URL { get; set; }
//    public string? IcoClass { get; set; }
//    public int? Serial { get; set; }
//}

public class GetMenuByRoleSorolDto
{
    public string ROLENAME { get; set; } = null!;
    public List<MenuDetails> menuRoles { get; set; } = new();
}

public class MenuDetails
{
    public int MenuID { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ParentID { get; set; }
    public string ParentText { get; set; } = string.Empty;
    public string? URL { get; set; }
    public string? IcoClass { get; set; }
    public int? Serial { get; set; }
}