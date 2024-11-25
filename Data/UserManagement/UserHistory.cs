using Data.Base;
using Data.UserManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.UserManagement
{
    public class UserHistory : BaseDate
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get;set; }

        [ForeignKey("UserId")]
        public int  UserId { get; set; }
        public virtual User SystemUser { get; set; }
        
        public DateTime LoginTime { get; set; }= DateTime.Now;
        public DateTime? LogoutTime { get; set; }
        public double Duration { get; set; }
        public string PublicIP { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Browser { get; set; }
        public string OperatingSystem { get; set; }
        public string Device { get; set; }
        //public string Action { get; set; }
        public string ActionStatus { get; set; }
    }
}
