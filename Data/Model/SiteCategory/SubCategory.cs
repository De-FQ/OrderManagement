using Data.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model.SiteCategory
{
    public class SubCategory : BaseImage
    {
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool ShowInMenu { get; set; }

        public bool DiscountActive { get; set; }
        [Column(TypeName = Utility.Helpers.Constants.DataSize.Decimal2)]
        public double DiscountPercentage { get; set; } 

        // Navigation
        public ICollection<ChildCategory> ChildCategories { get; set; }
    }
}
