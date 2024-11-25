
using Data.Base;
using Data.Model.General;
using Data.UserManagement;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Data.UserManagement
{
    public partial class User : BaseImage
    {
        [StringLength(Constants.DataSize.TitleMedium)]
        public string FullName { get; set; }

        [StringLength(Constants.DataSize.Email)]
        public string EmailAddress { get; set; }

        [StringLength(Constants.DataSize.Mobile)]
        public string MobileNumber { get; set; }

        [StringLength(Constants.DataSize.TitleMedium)]
        public string EncryptedPassword { get; set; }

        [ForeignKey("UserRoleId")]
        public int? UserRoleId { get; set; }
        public virtual UserRole Roles { get; set; }


        public DateTime LastLogin { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        //Navigation
        public virtual ICollection<UserHistory> UserHistories { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }

        #region Not mapped

        [StringLength(Constants.DataSize.TitleSmall)]
        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public string Token { get; set; }

        [NotMapped]
        public string FormattedRegisteredBy { get; set; }

        [NotMapped]
        public string FormattedLastLogin { get; set; }

        [NotMapped]
        public string RoleName { get; set; }

        #endregion
    }
}
