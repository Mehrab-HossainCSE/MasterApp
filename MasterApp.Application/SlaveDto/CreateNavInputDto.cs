using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.SlaveDto
{
    public class CreateNavInputDto
    {
        public decimal serial { get; set; }
        public decimal? parenT_ID { get; set; }
        public string description { get; set; }
        public string? url { get; set; }
        public string peR_ROLE { get; set; }
        public string entrY_BY { get; set; }
        public DateTime entrY_DATE { get; set; }
        public decimal ordeR_BY { get; set; }
        public string? fA_CLASS { get; set; }
        public int id { get; set; }
        public string? menU_TYPE { get; set; }
        public bool shoW_EDIT_PERMISSION { get; set; }
        public List<CreateNavInputDto> children { get; set; }
    }
}
