
using System.ComponentModel.DataAnnotations;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class UserRoleDto : BaseEntityCommonDto
    {
        public string Name { get; set; }
        public long RoleTypeId { get; set; }


    }
}
