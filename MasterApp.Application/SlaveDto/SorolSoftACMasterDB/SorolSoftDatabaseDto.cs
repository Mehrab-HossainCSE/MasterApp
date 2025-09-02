using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.SlaveDto.SorolSoftACMasterDB
{
    public class SorolSoftDatabaseDto
    {       
            public int menuID { get; set; }
            public int parentID { get; set; }
            public int? moduleID { get; set; }
            public string? text { get; set; }
            public string? url { get; set; }
            public string? rel { get; set; }
            public int? serial { get; set; }
            public string? icoClass { get; set; }
            public bool IsChecked { get; set; }
            public List<SorolSoftDatabaseDto>? children { get; set; }
        
    }
}
