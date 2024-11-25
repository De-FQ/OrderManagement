using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Admin.UserManagement
{
    public class AdminUserPermissionModel
    {

        public long PermissionId { get; set; }
        public long RoleId { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Call API Request</b>
        /// </summary>
        public bool Allowed { get; set; }
        //public bool AllowEdit { get; set; }
        //public bool AllowActive { get; set; }
        //public bool AllowView { get; set; }

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
        public int GetAllowList() { return AllowList ? 1 : 1; }
        public int GetAllowDisplayOrder() { return AllowDisplayOrder ? 1 : 0; }
        public int GetAllowActive() { return AllowActive ? 1 : 0; }
        public int GetAllowAdd() { return AllowAdd ? 1 : 0; }
        public int GetAllowEdit() { return AllowEdit ? 1 : 0; }
        public int GetAllowDelete() { return AllowDelete ? 1 : 0; }
        public int GetAllowView() { return AllowView ? 1 : 0; }
        public string Title { get; set; }
        public string AddEditPath { get; set; }

        public dynamic DynamicModel { get; set; }

    }
}
