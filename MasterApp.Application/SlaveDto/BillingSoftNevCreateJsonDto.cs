namespace MasterApp.Application.SlaveDto;

public class BillingSoftNevCreateJsonDto
{
    
        public int MenuId { get; set; }
        public int ParentMenuId { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int Sorting { get; set; }
        public bool IsActive { get; set; }
        public int ApplicationId { get; set; }
        public string CreatorId { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
    
       public List<BillingSoftNevCreateJsonDto> children { get; set; }
}

public class BillingSoftNevCreateDto
{

    public int MenuId { get; set; }
    public int ParentMenuId { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Sorting { get; set; }
    public bool IsActive { get; set; }
    public int ApplicationId { get; set; }
    public string CreatorId { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }


}