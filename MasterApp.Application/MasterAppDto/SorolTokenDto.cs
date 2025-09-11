namespace MasterApp.Application.MasterAppDto;

public class SorolTokenDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class ProjectTokenDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Title { get; set; }
}

public class TokenDto
{
    public Dictionary<int, ProjectTokenDto> Tokens { get; set; } = new Dictionary<int, ProjectTokenDto>();
}