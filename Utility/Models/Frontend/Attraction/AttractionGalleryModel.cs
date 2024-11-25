using Utility.API;
using Utility.Enum;

namespace Utility.Models.Frontend.Attraction
{
    public class AttractionGalleryModel
    {
        public bool ShowBanner { get; set; } = false;
        public string BannerURL { get; set; } = string.Empty;
        public MediaFileType FileType { get; set; }

    }
}
