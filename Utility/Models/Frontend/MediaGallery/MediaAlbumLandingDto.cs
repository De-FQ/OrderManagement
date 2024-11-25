using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.MediaGallery
{

    public class MediaAlbumLandingDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public List<MediaAlbumSummaryDto> Albums { get; set; }
    }
    public class MediaAlbumSummaryDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public long DisplayOrder { get; set; }

    }

}