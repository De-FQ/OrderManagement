using Utility.API;

namespace Utility.Models.Frontend.Activity
{
    public class ActivityModel
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string WorkingHour { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
       
        public string GoogleLocationLink { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool ShowInMenu { get; set; }
        public List<ActivityGalleryModel> ActivityGalleryModels { get; set; }
        public List<ActivityBannerModel> ActivityBannerModels { get; set; }
    }
}
