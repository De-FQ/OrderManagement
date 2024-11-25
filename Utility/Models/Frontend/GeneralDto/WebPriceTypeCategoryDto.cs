using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.GeneralDto
{
    public class WebPriceTypeCategoryDto : BaseEntityCommonDto
    {
        public string Name { get; set; }
        public int ChildCategoryId { get; set; }
        public List<WebPriceTypeDto> PriceTypes { get; set; }
    }
}
