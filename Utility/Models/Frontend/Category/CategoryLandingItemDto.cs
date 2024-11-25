using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.Category
{
    public class CategoryLandingItemDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
        public string RedirectURL { get; set; } = string.Empty;
    }

}
