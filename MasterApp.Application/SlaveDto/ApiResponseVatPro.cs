using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApp.Application.SlaveDto
{
    public class ApiResponseVatPro
    {
        public string Data { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
    public class ApiResponseVatProToken
    {
        public string token { get; set; }
        
    }
}
