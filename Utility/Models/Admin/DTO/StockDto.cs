using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class StockDto : BaseEntityCommonDto
    {
        
        public int InventoryItemId { get; set; }
        public string InventoryItem { get; set; }

        public string Unit { get; set; }
        public decimal NetUnitCost { get; set; }
        public decimal CompanyCostMargin { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalUnitNetCost { get; set; }
        public decimal TotalUnitCompanyPrice { get; set; }
    }
}
