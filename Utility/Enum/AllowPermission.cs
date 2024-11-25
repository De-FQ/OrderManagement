using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Enum
{
    public enum AllowPermission
    {
        None = 0,
        /// <summary>
        /// This permission will allow end-user to <b> while creating role that should be allwed</b>
        /// </summary>
        Allowed = 1,
        /// <summary>
        /// This permission will allow end-user to <b>View information in Datatable</b>
        /// </summary>
        List = 2,
        /// <summary>
        /// This permission will allow end-user to <b>Change display order in Datatable</b>
        /// </summary>
        DisplayOrder = 3,
        /// <summary>
        /// This permission will allow to <b>Toggle Active status in Datatable</b>
        /// </summary>
        Active = 4,
        /// <summary>
        /// This permission will allow end-user to <b>Add</b> records
        /// </summary>
        Add = 5,
        /// <summary>
        /// This permission will allow end-user to <b>Edit records in Datatable Menu</b>
        /// </summary>
        Edit = 6,
        /// <summary>
        /// This permission will allow end-user to <b>Delete records in Datatable Menu</b>
        /// </summary>
        Delete = 7,
        /// <summary>
        /// This permission will allow end-user to <b>View/Print/Download</b>
        /// </summary>
        View = 8, 
       
    }
     
}
