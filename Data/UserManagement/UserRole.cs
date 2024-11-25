using Data.Base;
using Data.UserManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Data.UserManagement
{
    public partial class UserRole : BaseCommon
    {
        [StringLength(Constants.DataSize.TitleSmall)]
        public string Name { get; set; }
        [ForeignKey("UserRoleTypeId")]
        public int UserRoleTypeId { get; set; }
        public virtual UserRoleType RoleType { get; set; }

        // Navigation
        public ICollection<UserRolePermission> RolePermissions { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
