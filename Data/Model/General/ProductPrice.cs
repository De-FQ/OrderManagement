using Data.Base;
using Data.Model.SiteCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model.General
{
    public class ProductPrice : BaseCommon
    {
        [ForeignKey("ChildCategoryId")]
        public int ChildCategoryId { get; set; }
        public ChildCategory ChildCategories { get; set; }

        [ForeignKey("PriceTypeId")]
        public int PriceTypeId { get; set; }
        public PriceType PriceTypes { get; set; }

        [ForeignKey("PriceTypeCategoryId")]
        public int PriceTypeCategoryId { get; set; }
        public virtual PriceTypeCategory PriceTypeCategory { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public decimal Price { get; set; }

        
    }
}
