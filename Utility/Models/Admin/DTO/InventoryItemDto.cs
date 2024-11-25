using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class InventoryItemDto : BaseEntityCommonDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int SupplierId { get; set; }
        public string Supplier { get; set; }

        public string Unit { get; set; }

        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
    }
}
