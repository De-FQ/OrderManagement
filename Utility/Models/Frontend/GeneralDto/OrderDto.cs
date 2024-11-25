using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.GeneralDto
{
    public class OrderDto : BaseEntityCommonDto
    {

        public string CustomerName { get; set; }
        public string CustomerContact { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ChangeToReturn { get; set; }
        public DateTime OrderDate { get; set; }
        public string SerialNumber { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    }
}
