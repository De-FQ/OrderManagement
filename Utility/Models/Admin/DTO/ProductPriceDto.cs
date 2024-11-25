using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class ProductPriceDto : BaseEntityImageDto
    {
        public decimal Price { get; set; }
        public int ChildCategoryId { get; set; }
        public int PriceTypeCategoryId { get; set; }
        public int PriceTypeId { get; set; }

        public decimal DiscountedPrice { get; set; }
    }
}
