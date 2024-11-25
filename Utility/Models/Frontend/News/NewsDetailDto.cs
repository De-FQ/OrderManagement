using Utility.API;
using Utility.Enum;

namespace Utility.Models.Frontend.News
{

    public class NewsModelDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string BannerPosterURL { get; set; } = string.Empty;
        public NewsDetailDto News { get; set; }
    }

    public class NewsDetailDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }

        public string FormatedPostDate { get; set; }
        public string DetailURL { get; set; }


        public MediaFileType MediaFileType { get; set; }
        public string imageURL { get; set; } = string.Empty;
        public string imagePosterURL { get; set; } = string.Empty;

       

       
        public List<NewsGalleryDto> Galleries { get; set; }
      
    }


    //public class NewsModelDto
    //{
    //    public string SeoName { get; set; }
    //    public string Name { get; set; }
    //    public string ShortDescription { get; set; }
    //    public string WorkingHour { get; set; }
    //    public string ThumbImageURL { get; set; } = string.Empty;
    //}

    

    public class NewsGalleryDto
    {

        public string MediaFileURL { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string ThumbImageURL { get; set; }
        public int DisplayOrder { get; set; }

    }


}
