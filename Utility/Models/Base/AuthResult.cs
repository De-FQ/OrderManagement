
using System.Security.Claims;
using Utility.Models.Admin.UserManagement;

namespace Utility.Models.Base
{
    public class AuthResult
    {
        public ClaimsPrincipal Principal { get; set; } = new();
        /// <summary>
        /// token is expired or empty
        /// </summary>
        public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
        public bool TokenExpired { get; set; }
        public string ErrorMessage { get; set; }
        public bool SecurityIssue { get; set; }
        public bool Success { get; set; } 
        public UserModel User { get; set; } = new();  
        public string RedirectionUrl { get; set; }
         
    }
}