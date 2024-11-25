using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model.SiteCategory
{
    public class Category : BaseImage
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool ShowInMenu { get; set; }

        public bool DiscountActive { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public double DiscountPercentage { get; set; } 

        // Navigation
        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
