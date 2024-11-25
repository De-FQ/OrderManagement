using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class SupplierItemDto : BaseEntityCommonDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
    }
}
