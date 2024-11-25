using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Base
{
    public class RefreshTokenModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public bool Active { get; set; } = true;

    }
}
