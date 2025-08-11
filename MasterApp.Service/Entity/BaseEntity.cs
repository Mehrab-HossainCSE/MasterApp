namespace MasterApp.Service.Entity;

public abstract class BaseEntity
{
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? UpdateBy { get; set; }
    public DateTime? UpdateDate { get; set; }

}