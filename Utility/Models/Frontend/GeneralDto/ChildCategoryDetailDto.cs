using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.GeneralDto
{
    public class ChildCategoryDetailsDto : BaseEntityImageDto
    {
        public string Name { get; set; }
        public List<WebPriceTypeCategoryDto> PriceTypeCategories { get; set; }
    }


}
