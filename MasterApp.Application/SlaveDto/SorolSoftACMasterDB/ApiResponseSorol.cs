namespace MasterApp.Application.SlaveDto.SorolSoftACMasterDB;

public class ApiResponseSorol
{
    public string token { get; set; }
}

public class ApiResponseSorolCreateUser
{
    public bool status { get; set; }
    public int code { get; set; }
    public string data { get; set; }
    public string message { get; set; }
}