namespace MasterApp.Application.MasterAppDto;

 public class ApiSettings
  {
    public string VatProBaseUrl { get; set; }
    public string SorolBaseUrl { get; set; }
    public string BillingBaseUrl { get; set; }
}

public class ApiSettingsEncryption
{
    public string Key { get; set; }
}