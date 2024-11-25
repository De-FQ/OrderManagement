using Utility.Enum;

namespace Utility.Models.Frontend.HomePage
{
    public class HomePageModel
    {
        public IEnumerable<BannerDto> Banners { get; set; }
        public IEnumerable<ActivityTypeDto> Activities { get; set; }
        public List<NewsDto> News { get; set; }
        public List<EventListDto> Events { get; set; }
        public GetInTouchInfo GetInTouch { get; set; }
        public IEnumerable<FaisalTypeDto> Faisals { get; set; }

    }

    public class MenuModel
    {
        public IEnumerable<MenuDto> Menus { get; set; }


    }

    public class MenuDto
    {
        public List<MenuModelDto> MenuItems { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string TwitterLink { get; set; }
        public string YoutubeLink { get; set; }
        public string LinkedIn { get; set; }
        public string WhatsAppLink { get; set; }
        public string TiktokLink { get; set; }
        public string SnapchatLink { get; set; }
    }
    public class MenuModelDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string RedirectURL { get; set; }
        public bool ChildMenu { get; set; }
        public bool ExternalLink { get; set; }

        public bool SubCategory { get; set; }

        public List<MenuModelDto> SubMenuItems { get; set; }
    }

    public class GetInTouchInfo
    {

        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string PosterImageUrl { get; set; }
        public string Address { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string TwitterLink { get; set; }
        public string YoutubeLink { get; set; }
        public string LinkedIn { get; set; }
        public string WhatsAppLink { get; set; }
        public string TiktokLink { get; set; }
        public string SnapchatLink { get; set; }
        public string InformationRequestContent { get; set; }
        public string GrievanceFormContent { get; set; }
        public string GoogleLocationLink { get; set; }
    }


}
