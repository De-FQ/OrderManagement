using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.GeneralDto
{
    public class WebPriceTypeDto : BaseEntityCommonDto
    {
        public string Name { get; set; }
        public int PriceTypeCategoryId { get; set; }

        public List<WebProductPriceDto> ProductPrices { get; set; }
    }
}
