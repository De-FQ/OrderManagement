using Utility.API;

namespace Utility.Models.Admin.UserManagement
{
    public class SystemUserSearchParamModel
    {
        public long RoleTypeId { get; set; }
        public long UserTypeId { get; set; }
        public bool IsEnglish { get; set; }
        public int SelectedTab { get; set; }
        public DataTableParam DatatableParam { get; set; }
        public long? CreatedBy { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? CreatedOn { get; set; } = null;
        public int? Status { get; set; }
    }
}
