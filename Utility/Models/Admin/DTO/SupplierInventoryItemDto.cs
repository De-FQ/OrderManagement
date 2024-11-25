using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class SupplierInventoryItemDto : BaseEntityCommonDto
    {
       
        public int SupplierId { get; set; }
        public string Supplier { get; set; }

        public int InventoryItemId { get; set; }
        public string InventoryItem { get; set; }

        public int Quantity { get; set; }
        public DateTime DateSupplied { get; set; } = DateTime.Now;
    }
}
