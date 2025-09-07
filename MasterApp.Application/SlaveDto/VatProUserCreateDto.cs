namespace MasterApp.Application.SlaveDto;

public class VatProUserCreateDto
{
    public string USER_ID { get; set; }
    public string USER_NAME { get; set; }
    public string USER_PASS { get; set; }
    public string MOBILE { get; set; }
    public string EMAIL { get; set; }
    public string ADDRESS { get; set; }
    public int DES_ID { get; set; }
    public string FullName { get; set; }
    public string BranchID { get; set; }
    public bool ExcelPermission { get; set; }
    public string NID { get; set; }
    public int RoleId { get; set; }
    public bool IsActive { get; set; }
    public UserImages userImages { get; set; }
    public string OldPassword { get; set; }
    public string BranchName { get; set; }
    public string DES_TITLE { get; set; }
    public int RecordCount { get; set; }
    public int RecordFilter { get; set; }
    public string CREATE_BY { get; set; }
    public DateTime CREATE_DATE { get; set; }
    public string UPDATE_BY { get; set; }
    public DateTime UPDATE_DATE { get; set; }
}
public class UserImages
{
    public string UserImageId { get; set; }
    public string USER_ID { get; set; }
    public string UserImageData { get; set; }
    public string NIDImageData { get; set; }
    public string NIDImageData2 { get; set; }
}