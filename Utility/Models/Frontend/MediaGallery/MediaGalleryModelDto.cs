using Utility.API;
using Utility.Enum;

namespace Utility.Models.Frontend.MediaGallery
{

    public class MediaGalleryModelDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string PosterFileUrl { get; set; }

        public MediaGalleryDetailDto Album { get; set; }
    }

    public class MediaGalleryDetailDto
    {
        public string SeoName { get; set; }
    
       
        public string Name { get; set; }

      

       
        public List<AlbumGalleryDto> Galleries { get; set; }
      
    }


    //public class NewsModelDto
    //{
    //    public string SeoName { get; set; }
    //    public string Name { get; set; }
    //    public string ShortDescription { get; set; }
    //    public string WorkingHour { get; set; }
    //    public string ThumbImageURL { get; set; } = string.Empty;
    //}

    

    public class AlbumGalleryDto
    {

        public MediaFileType MediaFileType { get; set; }
        public string MediaFileURL { get; set; } = string.Empty;
        public string ImagePosterURL { get; set; } = string.Empty;



    }


}
