using Microsoft.AspNetCore.Http;
namespace MasterApp.Application.MasterAppDto
{
    public class CreateProjectCommand
    {
        public string Title { get; set; }
        public string NavigateUrl { get; set; }
        public string LoginUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? WebRootPath { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateProjectCommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NavigateUrl { get; set; }
        public string LoginUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? WebRootPath { get; set; }
        public bool IsActive { get; set; }
    }
}
