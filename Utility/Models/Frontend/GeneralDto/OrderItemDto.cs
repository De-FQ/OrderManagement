using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.GeneralDto
{
    public class OrderItemDto : BaseEntityCommonDto
    {
        
        public string PriceTypeName { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
