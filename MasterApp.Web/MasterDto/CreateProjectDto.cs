namespace MasterApp.Web.MasterDto
{
    public class CreateProjectDto
    {
        public string Title { get; set; }
        public string NavigateUrl { get; set; }
        public string LoginUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class UpdateProjectDto   
    { public int Id { get; set; }   
        public string Title { get; set; }
        public string NavigateUrl { get; set; }
        public string LoginUrl { get; set; }
        public IFormFile? LogoFile { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string? Password { get; set; }
    }
}
