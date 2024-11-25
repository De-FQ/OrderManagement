using Data.Base;
using System.ComponentModel.DataAnnotations;
using Utility.Helpers;

namespace Data.UserManagement
{
    public partial class UserRoleType : BaseCommon
    {
        [StringLength(Constants.DataSize.TitleSmall)]
        public string Name { get; set; }

        //Navigation
        public virtual ICollection<UserRole> Roles { get; set;}
    }
}
