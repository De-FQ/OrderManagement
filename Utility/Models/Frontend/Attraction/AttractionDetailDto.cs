using Utility.API;
using Utility.Enum;

namespace Utility.Models.Frontend.Attraction
{
    public class AttractionDetailDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }
        public string WorkingHour { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
         public string WhatsAppLink { get; set; }

        public MediaFileType MediaFileType { get; set; }
        public string BannerURL { get; set; } = string.Empty;
        public string BannerPosterURL { get; set; } = string.Empty;


        public string AttractionTypeSeoName { get; set; }
        public string AttractionTypeName { get; set; }



        public string DetailURL { get; set; } = string.Empty;

        public string PreviousRedirectionURL { get; set; } = string.Empty;

        public string NextRedirectionURL { get; set; } = string.Empty;

        public string GoogleLocationLink { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<ActivityModelDto> Activities { get; set; }
        public List<FaisalModelDto> Faisals { get; set; }
        public List<EventModelDto> Events { get; set; }
        public List<GalleryDto> Galleries { get; set; }
        public List<FaqModelDto> Faqs { get; set; }

    }


    public class ActivityModelDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string WorkingHour { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
    }

    public class FaisalModelDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string WorkingHour { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
    }

    public class EventModelDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string FormattedDateTime { get; set; }
        public string ShortDescription { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;

    }

    public class FaqModelDto
    {
       
        public string Question { get; set; }
        public string Answer { get; set; }
        public int DisplayOrder { get; set; }
       

    }


    public class GalleryDto
    {

        public string MediaFileURL { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string ThumbImageURL { get; set; }
        public int DisplayOrder { get; set; }

    }


}
