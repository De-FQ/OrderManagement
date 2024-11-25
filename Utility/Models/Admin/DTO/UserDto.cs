using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class UserDto : BaseEntityImageDto
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public long? RoleId { get; set; }
        public DateTime LastLogin { get; set; }
        public string FormattedRegisteredBy { get; set; }
        public string FormattedLastLogin { get; set; }
        public string RoleName { get; set; }

    }
}
