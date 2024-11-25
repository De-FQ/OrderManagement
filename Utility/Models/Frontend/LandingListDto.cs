using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend
{
    public class LandingListDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        public string WorkingHour { get; set; }

        public string AttractionName { get; set; }
        public long? AttractionId { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
        public long DisplayOrder { get; set; }
        public string RedirectURL { get; set; } = string.Empty;
    }
}
