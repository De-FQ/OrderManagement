using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.InventoryManagement
{
    public class Stock : BaseCommon
    {
        [ForeignKey("InventoryItemId")]
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }

        public string Unit { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal NetUnitCost { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal CompanyCostMargin { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int TotalQuantity { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2 )]
        public decimal TotalUnitNetCost { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal TotalUnitCompanyPrice { get; set; }
    }
}
