using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Base
{
    public class RefreshResult
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } 
        public string AntiforgeryToken { get; set; }
        public bool Success { get; set; } = false;
        public string X_XSRF_TOKEN { get; set; }
    }
}
