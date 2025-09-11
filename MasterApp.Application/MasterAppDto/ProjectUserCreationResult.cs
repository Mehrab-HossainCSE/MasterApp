using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.MasterAppDto;

public class ProjectUserCreationResult
{
    public int ProjectId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public String Title { get; set; }
}
public class ProjectUserCreationResponse
{
    public IEnumerable<ProjectUserCreationResult> SuccessfulProjects { get; set; }
    public IEnumerable<ProjectUserCreationResult> FailedProjects { get; set; }
}