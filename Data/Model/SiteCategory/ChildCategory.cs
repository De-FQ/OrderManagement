using Data.Base;
using Data.Model.General;
using Data.Model.InventoryManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model.SiteCategory
{
    public class ChildCategory : BaseImage
    {
        [ForeignKey("SubCategory")]
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool ShowInMenu { get; set; }

        public bool DiscountActive { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public double DiscountPercentage { get; set; } 

        // Navigation
        public virtual ICollection<PriceTypeCategory> PriceTypeCategories { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}
