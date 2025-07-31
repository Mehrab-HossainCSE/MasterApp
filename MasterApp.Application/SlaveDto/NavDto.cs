using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ICollection<NavDto>? Children { get; set; }
    }
}
