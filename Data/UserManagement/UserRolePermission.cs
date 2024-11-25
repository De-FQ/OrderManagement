using Data.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.UserManagement
{
    public partial class UserRolePermission : BaseDate
    {
        [ForeignKey("UserPermissionId")]
        public long UserPermissionId { get; set; }
        public virtual UserPermission Permission { get; set; }

        [ForeignKey("UserRoleId")]
        public long UserRoleId { get; set; }
        public virtual UserRole Role { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>to show side menu option with role</b>
        /// </summary>
        public bool Allowed { get; set; } 
        
        
        /////////////////////////////////////////////////////////////////
        /// start BKD - modified on 31-aug-2023 for admin view permission
        ////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This permission will allow end-user to <b>View information in Datatable</b>
        /// </summary>
        public bool AllowList { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Change display order in Datatable</b>
        /// </summary>
        public bool AllowDisplayOrder { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Add</b> records
        /// </summary>
        public bool AllowAdd { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Edit</b> records
        /// </summary>
        public bool AllowEdit { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Delete</b> records
        /// </summary>
        public bool AllowDelete { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>View/Print/Download</b>
        /// </summary>
        public bool AllowView { get; set; }
        /// <summary>
        /// This permission will allow to show a <b>link in Admin side-Menu</b>
        /// </summary>
        public bool AllowActive { get; set; }

        public int GetAllowList()
        {
            return AllowList ? 1 : 0;
        }
        public int GetAllowDisplayOrder()
        {
            return AllowDisplayOrder ? 1 : 0;
        }
        public int GetAllowAdd()
        {
            return AllowAdd ? 1 : 0;
        }
        public int GetAllowEdit()
        {
            return AllowEdit ? 1 : 0;
        }
        public int GetAllowDelete()
        {
            return AllowDelete ? 1 : 0;
        }
        public int GetAllowActive()
        {
            return AllowActive ? 1 : 0;
        }
        public int GetAllowView()
        {
            return AllowView ? 1 : 0;
        }
        /////////////////////////////////////////////////////////////////
        /// end BKD - modified on 31-aug-2023 for admin view permission
        ////////////////////////////////////////////////////////////////////////////
    }
}
