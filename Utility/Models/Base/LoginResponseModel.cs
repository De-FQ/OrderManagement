using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Base
{
    public class LoginResponseModel
    {
        public bool Success { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string RedirectionUrl { get; set; }
    }
}
