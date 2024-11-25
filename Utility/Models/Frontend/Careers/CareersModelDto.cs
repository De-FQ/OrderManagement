using Utility.API;
using Utility.Enum;

namespace Utility.Models.Frontend.Careers
{

    public class CareersModelDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string PosterFileUrl { get; set; }
        public CareersDetailDto Careers { get; set; }
    }

    public class CareersDetailDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }

        public string LocationName { get; set; }
        public string LongDescription { get; set; }

        public bool FullTime { get; set; }
        public string DetailURL { get; set; }
        public string FormatedPostDate { get; set; }

        public MediaFileType MediaFileType { get; set; }
        public string imageURL { get; set; } = string.Empty;
        public string imagePosterURL { get; set; } = string.Empty;

       

      
    }




}
