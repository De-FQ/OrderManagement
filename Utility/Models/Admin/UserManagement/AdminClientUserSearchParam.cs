using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;

namespace Utility.Models.Admin.UserManagement
{
    public class AdminClientUserSearchParam
    {
        public bool IsEnglish { get; set; }
        public int SelectedTab { get; set; }
        public DataTableParam DatatableParam { get; set; }
        public Guid? CompanyGuid { get; set; }
        public long? CompanyId { get; set; }
        public Guid? LevelOneGuid { get; set; }
        public long? LevelOneId { get; set; }
        public Guid? LevelTwoGuid { get; set; }
        public long? LevelTwoId { get; set; }
        public Guid? LevelThreeGuid { get; set; }
        public long? LevelThreeId { get; set; }
        public long? CreatedBy { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? CreatedOn { get; set; } = null;
        public int? Status { get; set; }
    }
}
