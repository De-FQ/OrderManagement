using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.InventoryManagement
{
    public class InventoryTransaction : BaseCommon
    {
        public int Quantity { get; set; }
        public DateTime Transaction { get; set; } = DateTime.Now;
        public string TransactionType { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)] 
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
