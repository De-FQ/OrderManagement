using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.News
{

    public class NewsLandingDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public List<NewsSummaryDto> News { get; set; }
    }
    public class NewsSummaryDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string FormatedPostDate { get; set; }
        public long DisplayOrder { get; set; }

        public bool Featured { get; set; }
    }

}