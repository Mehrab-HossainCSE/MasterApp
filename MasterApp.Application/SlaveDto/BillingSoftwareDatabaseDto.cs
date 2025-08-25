namespace MasterApp.Application.SlaveDto;

public class BillingSoftwareDatabaseDto
{
    public int menuId { get; set; }
    public int parentMenuId { get; set; }
    public string menuName { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
    public int sorting { get; set; }
    public bool isActive { get; set; }
    public int applicationId { get; set; }
    public string creatorId { get; set; } = string.Empty;
    public bool isChecked { get; set; }

    // Use string for safe JSON binding of "yyyy-MM-dd"
    public string createDate { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

    public List<BillingSoftwareDatabaseDto> children { get; set; } 
}
