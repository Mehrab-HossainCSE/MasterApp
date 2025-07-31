using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace MasterApp.Application.MasterAppDto
{
    public class CreateProjectCommand
    {
        public string Title { get; set; }
        public string ApiUrl { get; set; }
        public string LoginUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? WebRootPath { get; set; }
        public bool IsActive { get; set; }
    }
}
