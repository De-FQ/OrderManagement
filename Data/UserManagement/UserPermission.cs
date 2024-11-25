using Data.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Data.UserManagement
{
    public partial class UserPermission : BaseCommon
    {
        [StringLength(Constants.DataSize.TitleSmall)]
        public string Title { get; set; }
        [StringLength(Constants.DataSize.TitleSmall)]
        public string NavigationUrl { get; set; }

        [StringLength(Constants.DataSize.TitleSmall)]
        public string Icon { get; set; }
        /// <summary>
        /// AccessList List will contains
        /// <list type="">
        /// <item>Allowed</item>
        /// <item>List</item>
        /// <item>DisplayOrder</item>
        /// <item>Add</item>
        /// <item>Edit</item>
        /// <item>Delete</item>
        /// <item>View</item>
        /// </list>
        /// </summary>
        [StringLength(Constants.DataSize.TitleMedium)]
        public string AccessList { get; set; } = ""; // "Allowed,List,DisplayOrder,Add,Edit,Delete,View";
        public int? UserPermissionId { get; set; }
        public virtual UserPermission ParentPermission { get; set; }
        public bool ShowInMenu { get; set; }
        
        public ICollection<UserPermission> ChildPermissions { get; set; }

        public virtual ICollection<UserRolePermission> RolePermissions { get; set; }

        //Navigation
        public List<UserPermission> GetMenuTree(List<UserPermission> list, long? parentId)
        {
            return list.Where(x => x.UserPermissionId == parentId)
                .Select(x => new UserPermission()
                {
                    Id = x.Id,
                    Title = x.Title,
                    NavigationUrl = x.NavigationUrl,
                    Icon = x.Icon,
                    UserPermissionId = x.UserPermissionId,
                    Active = x.Active,
                    ChildPermissions = GetMenuTree(list, x.Id),

                })
                .ToList();
        }

        #region Not mapped
        [NotMapped]
        public virtual int NotificationBadgeCount { get; set; }

        [NotMapped]
        public virtual int SalesOrderBadgeCount { get; set; }

        [NotMapped]
        public virtual int SubscriptionBadgeCount { get; set; }

        [NotMapped]
        public virtual int PrepaidCardBadgeCount { get; set; }

        [NotMapped]
        public virtual int DeliveryDBBadgeCount { get; set; }

        [NotMapped]
        public virtual int PaymentBadgeCount { get; set; }

        [NotMapped]
        public virtual long BadgeCount { get; set; }
        #endregion
    }
}
