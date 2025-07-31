using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.MasterAppDto
{
    public class ProjectDtos
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ApiUrl { get; set; }
        public bool IsActive { get; set; }
        public string LogoUrl { get; set; }
    }
}
