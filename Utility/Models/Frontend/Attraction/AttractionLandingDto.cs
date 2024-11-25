using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.Attraction
{

    public class AttractionLandingDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string BannerPosterURL { get; set; } = string.Empty;
        public string ListName { get; set; } = string.Empty;
        public string ListSeoName { get; set; } = string.Empty;
        public List<LandingDto> Attractions { get; set; }
    }

    public class LandingDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
        public string RedirectURL { get; set; } = string.Empty;
        public bool ExternalLink { get; set; }
        public string ExternalURL { get; set; }
        public SiteAttractionType Type { get; set; }
    }

}
