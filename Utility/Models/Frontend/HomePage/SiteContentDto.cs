using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;
using Utility.Models.Frontend.Common;

namespace Utility.Models.Frontend.HomePage
{

    public class SiteContentDto
    {
        public string Content { get; set; }
        public AppContentType AppContentType { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string ImageUrl { get; set; }
        public string PosterFileUrl { get; set; }

        public SEOModel SEOModel { get; set; }

    }


    public class RightAccesstDto
    {
        public string Content { get; set; }
        public AppContentType AppContentType { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string ImageUrl { get; set; }
        public string PosterFileUrl { get; set; }
        public List<RightAccesstFileDto> RightAccesstFiles { get; set; }

}

    public class RightAccesstFileDto
    {
        public string Title { get; set; }
        public string DownloadFileUrl { get; set; }

    }

}
