using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.Category
{
    public class ToggleCategoryStatusRequest
    {
        public int CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public int ChildCategoryId { get; set; }
        public int PriceTypeCategoryId { get; set; }
        public int PriceTypeId { get; set; }

        public bool IsActive { get; set; }
    }
}
