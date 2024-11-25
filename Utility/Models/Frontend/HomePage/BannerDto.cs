using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.HomePage
{
    public class BannerDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string IconActiveColor { get; set; }
        public string IconMouseOverColor { get; set; }
        public string IconURL { get; set; } = string.Empty;
        public string IconTitle { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string BannerURL { get; set; } = string.Empty;
        public string BannerPosterURL { get; set; } = string.Empty;
        public string BannerButtonText { get; set; } = string.Empty;
        /// <summary>
        /// From attraction type table if ExternalLink:true then redirect to new tab with  External URL else internal web page 
        /// </summary>
       // public string ExternalURL { get; set; } = string.Empty;
        /// <summary>
        /// If SubCategory:true then redirected to listing page else detail page
        /// </summary>
        public bool ExternalLink { get; set; }
        public bool SubCategory { get; set; }
        
        public string RedirectURL { get; set; } = string.Empty;
        public string ExternalURL { get; set; }
        public SiteAttractionType Type { get; set; }

    }
}
