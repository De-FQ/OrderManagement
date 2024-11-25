using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class SupplierDto : BaseEntityCommonDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
    }
}
