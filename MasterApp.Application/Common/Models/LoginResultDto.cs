namespace MasterApp.Application.Common.Models
{
    public class LoginResultDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
    }
}
