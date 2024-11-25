using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.HomePage
{
    public class FaisalTypeDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string FormatedPostDate { get; set; }
        public bool Featured { get; set; }
        public long DisplayOrder { get; set; }
        public string RedirectURL { get; set; } = string.Empty;

    }
}
