using Microsoft.AspNetCore.Http;
using Utility.Enum;

namespace Utility.Models.Admin.Common
{
    public class EntityBannerModel
    {
        public MediaFileType MediaFileType { get; set; }
        public string ImageName { get; set; }
        public IFormFile Image { get; set; }
        public string PosterFileName { get; set; }
        public IFormFile PosterFile { get; set; }
    }
}
