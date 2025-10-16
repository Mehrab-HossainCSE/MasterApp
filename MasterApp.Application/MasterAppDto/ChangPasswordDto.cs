namespace MasterApp.Application.MasterAppDto
{
    public class ChangPasswordDto
    {
        public string UserName { get; set; }
        public string previousPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmNewPassword { get; set; }
    }
    public class PasswordDetails
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
