using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MasterApp.Application.SlaveDto.SorolSoftACMasterDB
{
    public class GetAllUserSorolDto
    {

        [JsonPropertyName("UserName")]
        public string UserName { get; set; }
        [JsonPropertyName("UserId")]
        public string UserId { get; set; }

        [JsonPropertyName("MenuIdList")]
        public string MenuIdList { get; set; } = null!;
        [JsonPropertyName("Designation")]
        public string Designation { get; set; }
        [JsonPropertyName("CompanyId")]
        public string CompanyId { get; set; }
      
    }
}
