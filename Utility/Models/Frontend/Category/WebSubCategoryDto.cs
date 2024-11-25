using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Frontend.Category
{
    public class WebSubCategoryDto : BaseEntityImageDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
