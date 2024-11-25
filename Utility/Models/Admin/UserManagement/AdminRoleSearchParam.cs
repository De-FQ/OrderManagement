using System;

namespace Utility.Models.Admin.UserManagement
{
    public class AdminRoleSearchParam
    {
        public Guid? CompanyGuid { get; set; }
        public long? CompanyId { get; set; }
        public long? SystemUserRoleTypeId { get; set; }
        public long? CreatedBy { get; set; }
        public int? Active { get; set; }
        public long UserTypeId { get; set; }
    }
}
