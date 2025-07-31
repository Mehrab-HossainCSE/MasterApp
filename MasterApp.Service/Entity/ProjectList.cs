namespace MasterApp.Service.Entity;

public class ProjectList
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string ApiUrl { get; set; } = null!;
    public string LoginUrl { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;
    public bool IsActive { get; set; } = true;

}
