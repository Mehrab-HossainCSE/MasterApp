namespace MasterApp.Application.MasterAppDto
{
    public class ClientAdminUserCreateDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
       public List<ProjectJsonDtos> projectJsonDtos { get; set; }= new List<ProjectJsonDtos>();
        public string MenuList { get; set; }
    }
}
