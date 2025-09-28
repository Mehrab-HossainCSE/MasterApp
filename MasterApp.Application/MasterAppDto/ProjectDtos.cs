namespace MasterApp.Application.MasterAppDto
{
    public class ProjectDtos
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NavigateUrl { get; set; }
        public string LoginUrl { get; set; }
        public bool IsActive { get; set; }
        
        public string LogoUrl { get; set; }

        public string ? Password { get; set; }
        public bool IsChecked { get; set; }
    }
    public class ProjectJsonDtos
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? NavigateUrl { get; set; }
        public string? LoginUrl { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
