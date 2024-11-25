using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.General
{
    public class Order : BaseCommon
    {
        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string PaymentMethod { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal AmountReceived { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal ChangeToReturn { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string SerialNumber { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
