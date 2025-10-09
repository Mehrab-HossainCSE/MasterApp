using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.MasterAppDto;

public class TemRoleCreateDto
{
    public string projectId { get; set; } = string.Empty;
    public string roleId { get; set; } = string.Empty;
    public List<ProjectMenuDto> projectMenus { get; set; } = new();
}

public class ProjectMenuDto
{
    public string projectId { get; set; } = string.Empty;
    public List<string> menuIds { get; set; } = new();
}
