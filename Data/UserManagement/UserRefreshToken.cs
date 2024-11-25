using Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Data.UserManagement
{
    public class UserRefreshToken
    {
        [Key]
        public  long Id { get; set; }
        public long UserId { get; set; } 
        public Guid Guid { get; set; }
        public string Email { get; set; } 
        public string RefreshToken { get; set; } 
        public bool  Active { get; set; } = true;
         
    }
}