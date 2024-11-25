
using System.ComponentModel.DataAnnotations;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class UserPermissionDto : BaseEntityCommonDto
    {
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string NavigationUrl { get; set; }
        public string Icon { get; set; }
        public long? ParentPermissionId { get; set; }
        public bool ShowInMenu { get; set; }
        public bool PermissionCreate { get; set; }
        public bool PermissionEdit { get; set; }
        public bool PermissionView { get; set; }
        public bool PermissionDelete { get; set; }
    }
}
