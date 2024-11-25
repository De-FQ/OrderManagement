 
namespace Admin.Models
{
    public class DataTableModel
    {
        public string Title { get;set; }
        public string SubTitle { get;set; }
        public string NavigationPath { get;set; }        
        public Utility.Models.Admin.UserManagement.AdminUserPermissionModel Permission { get;set; }
        public List<TableHeader> Headers { get;set; }
    }

    public class PermissionModel
    { 
        public long PermissionId { get; set; }
        public long RoleId { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Call API Request</b>
        /// </summary>
        public bool Allowed { get; set; } 
        /// <summary>
        /// This permission will allow end-user to <b>View information in Data table</b>
        /// </summary>
        public bool AllowList { get; set; }
        /// <summary>
        /// This permission will allow end-user to <b>Change display order in Data table</b>
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
        public int GetAllowList() { return AllowList ? 1 : 0; }
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

    public class TableHeader
    {
        public string Title { get; set; } = string.Empty;
        public int  Priority { get; set; } = 0;
    }
}