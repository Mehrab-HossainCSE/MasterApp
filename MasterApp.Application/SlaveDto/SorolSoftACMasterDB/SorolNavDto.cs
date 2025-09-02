namespace MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

public class SorolNavDto
{
    public int MenuID { get; set; }
    public int? ParentID { get; set; }
    public int? ModuleID { get; set; }
    public string? Text { get; set; }
    public string? URL { get; set; }
    public string? REL { get; set; }
    public int? Serial { get; set; }
    public string? IcoClass { get; set; }
    public bool IsChecked { get; set; }
    public List<SorolNavDto>? children { get; set; }
}
