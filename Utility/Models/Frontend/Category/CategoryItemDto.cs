using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.Category
{
    public class CategoryItemDto
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public long DisplayOrder { get; set; }
        public bool ShowInMenu { get; set; }
    }
}
