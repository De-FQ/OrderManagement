using Data.Model.SiteCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Backend
{
    public class DiscountHelper
    {
        public static double CalculateEffectiveDiscount(Category category, SubCategory subCategory, ChildCategory childCategory)
        {
            double categoryDiscount = category.DiscountActive ? category.DiscountPercentage : 0;
            double subCategoryDiscount = subCategory.DiscountActive ? subCategory.DiscountPercentage : 0;
            double childCategoryDiscount = childCategory.DiscountActive ? childCategory.DiscountPercentage : 0;
            return Math.Max(categoryDiscount, Math.Max(subCategoryDiscount, childCategoryDiscount));
        }
    }

}
