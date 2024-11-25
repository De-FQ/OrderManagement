using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;
using Utility.Models.Frontend.Events;

namespace Utility.Models.Frontend.Faisal
{

    public class FaisalLandingDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string PosterFileUrl { get; set; }
        public List<LandingDto> Faisals { get; set; }
    }
    public class LandingDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
        public string RedirectURL { get; set; } = string.Empty;
    }

}