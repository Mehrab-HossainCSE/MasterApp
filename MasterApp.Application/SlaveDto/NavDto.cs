using MasterApp.Application.SlaveDto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterApp.Application.SlaveDto
{
    public class NavDto
    {
        public Decimal SERIAL { get; set; }
        public Decimal? PARENT_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string? URL { get; set; }
        public string PER_ROLE { get; set; }
        public string ENTRY_BY { get; set; }
        public DateTime ENTRY_DATE { get; set; }
        public Decimal ORDER_BY { get; set; }
        public string? FA_CLASS { get; set; }
        public int ID { get; set; }
        public string? MENU_TYPE { get; set; }
        public bool SHOW_EDIT_PERMISSION { get; set; }
        public bool IsChecked { get; set; }
       
        public ICollection<NavDto>? Children { get; set; }
    }

    }

public class BillingSoftNavDto
{
    public int MenuId { get; set; }
    public int ParentMenuId { get; set; }
    public string MenuName { get; set; }
    public string Url { get; set; }
    public int Sorting { get; set; }
    public bool IsActive { get; set; }
    public int ApplicationId { get; set; }
    public string CreatorId { get; set; }
    public bool IsChecked { get; set; }
    public DateTime CreateDate { get; set; }
    public List<BillingSoftNavDto>? children { get; set; }
}

public class NavDtoCloudPos
{
    public Decimal SERIAL { get; set; }
    public Decimal? PARENT_ID { get; set; }
    public string DESCRIPTION { get; set; }
    public string? URL { get; set; }
    public string PER_ROLE { get; set; }
    public string ENTRY_BY { get; set; }
    public DateTime ENTRY_DATE { get; set; }
    public Decimal ORDER_BY { get; set; }
    public string? FA_CLASS { get; set; }
    public int ID { get; set; }
    public string? MENU_TYPE { get; set; }
    public bool SHOW_EDIT_PERMISSION { get; set; }
    public bool IsChecked { get; set; }
    public int ActualParentID { get; set; }
    public ICollection<NavDtoCloudPos>? Children { get; set; }
}