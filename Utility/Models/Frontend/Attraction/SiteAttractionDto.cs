using Utility.Enum;

namespace Utility.Models.Frontend.Attraction
{
    public class SiteAttractionDto
    {
        public long? ParentId { get; set; }
        public string ParentSeoName { get; set; }
        public string ParentName { get; set; }
        public string SeoName { get; set; }
        public string Name { get; set; }
        public SiteAttractionType Type { get; set; }
        public SiteAttractionType ChildType { get; set; }
        public string WorkingHour { get; set; }
        public string WorkingHourAr { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string GoogleLocationLink { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LongDescription { get; set; }
        public string LongDescriptionAr { get; set; }
        public string ExternalURL { get; set; }
        public bool ShowInMenu { get; set; }
        public bool ShowInFooter { get; set; }
        public MediaFileType BannerMediaFileType { get; set; }
        public long? SEOId { get; set; }
        public string BannerImageUrl { get; set; }
        public string PosterImageUrl { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string DetailURL { get; set; }
        public string PreviousRedirectionURL { get; set; }
        public string NextRedirectionURL { get; set; }
        public List<GalleryDto> Galleries { get; set; }
        public List<ActivityModelDto> Activities { get; set; }
        public List<EventModelDto> Events { get; set; }
        public List<FaqModelDto> Faqs { get; set; }
    }
}
