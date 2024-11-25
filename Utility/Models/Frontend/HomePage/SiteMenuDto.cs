using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.HomePage
{

    public class SiteMenuDto
    {
        public string Title { get; set; }
        public bool External { get; set; }
        public string ExternalURL { get; set; }
        public string RedirectURL { get; set; }
        public AppContentType AppContentType { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string BannerImageUrl { get; set; }
        public string PosterFileUrl { get; set; }

    }
     
}
