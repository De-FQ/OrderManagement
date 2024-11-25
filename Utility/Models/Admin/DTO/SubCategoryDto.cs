using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class SubCategoryDto : BaseEntityImageDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ShowInMenu { get; set; }

        public bool DiscountActive { get; set; }
        public double? DiscountPercentage { get; set; }
    }
}
