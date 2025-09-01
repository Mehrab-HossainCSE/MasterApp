using System.Text.Json.Serialization;

namespace MasterApp.Application.SlaveDto.SorolSoftACMasterDB
{
    public class CreateSorolRoleDto
    {
       

            [JsonPropertyName("RoleId")]
            public int? RoleId { get; set; }

            [JsonPropertyName("RoleName")]
            public string RoleName { get; set; }


            [JsonPropertyName("IsActive")]
            public bool IsActive { get; set; }

            [JsonPropertyName("CreatedBy")]
            public string? CreatedBy { get; set; } = "Mehrab";

            [JsonPropertyName("CreateDate")]
            public DateTime? CreateDate { get; set; } = DateTime.Now;

            [JsonPropertyName("UpdateBy")]
            public string? UpdateBy { get; set; } = "Mehrab";

            [JsonPropertyName("UpdateDate")]
            public DateTime? UpdateDate { get; set; } = DateTime.Now;
        
    }
}
