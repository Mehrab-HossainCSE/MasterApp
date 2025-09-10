using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.SlaveDto
{
    public class ApiResopnseBilling
    {
        public List<RoleBilling> data { get; set; } = new();
        public bool success { get; set; }
        public string msg { get; set; }
    }
    public class RoleBilling
    {
        public int roleId { get; set; }
        public string roleName { get; set; }

    }
}
