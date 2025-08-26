using System.Text.Json.Serialization;

namespace MasterApp.Application.SlaveDto
{
    public class GetRoleBilling
    {
       
            [JsonPropertyName("RoleId")]
            public int RoleId { get; set; }

            [JsonPropertyName("RoleName")]
            public string RoleName { get; set; }

            [JsonPropertyName("Description")]
            public string Description { get; set; }

            [JsonPropertyName("IsActive")]
            public bool IsActive { get; set; }

            [JsonPropertyName("CreatorId")]
            public string CreatorId { get; set; }

            [JsonPropertyName("CreateDate")]
            public DateTime CreateDate { get; set; }

            [JsonPropertyName("UpdatorId")]
            public string UpdatorId { get; set; }

            [JsonPropertyName("UpdateDate")]
            public DateTime UpdateDate { get; set; }
       
    }
}

public class CreateRoleBilling
{

    [JsonPropertyName("RoleId")]
    public int? RoleId { get; set; }

    [JsonPropertyName("RoleName")]
    public string RoleName { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("IsActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("CreatorId")]
    public string? CreatorId { get; set; } = "Mehrab";

    [JsonPropertyName("CreateDate")]
    public DateTime? CreateDate { get; set; }= DateTime.Now;

    [JsonPropertyName("UpdatorId")]
    public string? UpdatorId { get; set; } = "Mehrab";

    [JsonPropertyName("UpdateDate")]
    public DateTime? UpdateDate { get; set; }=DateTime.Now;
}