using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.General
{
    public class OrderItem : BaseCommon
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string PriceTypeName { get; set; }

        public int Quantity { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal Total { get; set; }


    }
}
