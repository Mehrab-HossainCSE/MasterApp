namespace MasterApp.Application.SlaveDto
{
    public class GetRoleBilling
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdatorId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
