using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.MasterAppDto
{
    public class ProjectMenuDto
    {
        public string ProjectId { get; set; }
        public List<string> MenuIds { get; set; } = new();
    }
    public class TemRoleCreateDto
    {
        public int RoleId { get; set; }
        public List<ProjectMenuDto> ProjectMenus { get; set; } = new();
    }

}
