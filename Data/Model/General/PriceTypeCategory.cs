using Data.Base;
using Data.Model.SiteCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Data.Model.General
{
    public class PriceTypeCategory : BaseCommon
    {
        public string Name { get; set; }

        [ForeignKey("ChildCategoryId")]
        public int ChildCategoryId { get; set; }
        public virtual ChildCategory ChildCategory { get; set; }

        //Navigation
        public virtual ICollection<PriceType> PriceTypes { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }

    }
}
