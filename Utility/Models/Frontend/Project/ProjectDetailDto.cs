using Utility.API;
using Utility.Enum;
using Utility.Models.Frontend.News;

namespace Utility.Models.Frontend.Project
{

    public class ProjectModelDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string PosterFileUrl { get; set; }
        public ProjectDetailDto Projects { get; set; }
    }

    public class ProjectDetailDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }


        public string DetailURL { get; set; }


        public MediaFileType MediaFileType { get; set; }
        public string imageURL { get; set; } = string.Empty;
        public string imagePosterURL { get; set; } = string.Empty;

        public List<ProjectGalleryDto> Galleries { get; set; }
      
    }


    

    public class ProjectGalleryDto
    {

        public string MediaFileURL { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string ThumbImageURL { get; set; }
        public int DisplayOrder { get; set; }

    }


}
