using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.GeneralDto
{
    public class WebProductPriceDto : BaseEntityCommonDto
    {
        public int ChildCategoryId { get; set; }
        public int PriceTypeId { get; set; }
        public int PriceTypeCategoryId { get; set; }
        public decimal Price { get; set; }
    }
}
