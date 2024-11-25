using System.Drawing.Printing;

namespace Utility.API
{
    public partial class AppSettingsModel
    {
        public string APIBaseUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;

        public HttpClientSettings HttpClientSettings { get; set; }
        public CookieSettings CookieSettings { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public AppSettings AppSettings { get; set; }

        public PageSettings PageSettings { get; set; }

        public EmailSettings EmailSettings { get; set; }
        public ImageSettings ImageSettings { get; set; }
        public PushSettings PushSettings { get; set; }    
    }

    public class HttpClientSettings
    {
        public int ExpirationMinutes { get; set; } = 4;
    }
    public class JwtSettings
    {
        public string APIKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 30;

    }
    public class CookieSettings
    {
        public int ExpirationMinutes { get; set; } = 30;
        public int ExpirationDays { get; set; } = 1;
    }
    public class AppSettings
    {
        public string APIBaseUrl { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string AdminUrl { get; set; } = string.Empty;

        public bool EnableAuthorization { get; set; }
        public string CorsAllowedUrls { get; set; } = string.Empty;

        public string DefaultLang { get; set; } = "EN";
        public string Domain { get; set; } = "localhost";
        public double CustomerCookieTimeout { get; set; } = 30;

        public bool EnableSwagger { get; set; }
        

        /// <summary>
        /// SMS Valid minutes for end-user to validate 
        /// </summary>
        public int OTPValidMinutes { get; set; } = 3;

        public string AttractionsURL { get; set; } = "Attractions";
        public string AttractionsListURL { get; set; }
        public string AttractionsDetailURL { get; set; }


        public string ActivityURL { get; set; } = "Activities";
        public string ActivitiesListURL { get; set; }
        public string ActivitiesDetailURL { get; set; }

        public string EventsDetailURL { get; set; }

        public string NewsDetailURL { get; set; }
       
        public string FaisalsListURL { get; set; }
        public string FaisalsDetailURL { get; set; }
        public string OrderURL { get; set; }
       
        public string ProjectDetailURL { get; set; }

        public string CareersListURL { get; set; }
        public string CareersDetailURL { get; set; }
        public bool EnableRedirectToWwwRule { get; set; }

    }
    public class PageSettings
    {
        public int HomePageNewsTakeTop { get; set; } = 5;
        public int HomePageFaisalTakeTop { get; set; } = 5;
        public int LandingPageNewsTakeTop { get; set; } = 10;

    }
    public class ImageSettings
    {
        public string UploadForms { get; set; }

        public string Users { get; set; }
        public string Categories { get; set; }
        public string SubCategories { get; set; }
        public string ChildCategories { get; set; }
        public string AttractionIcons { get; set; }
        public string AttractionBanners { get; set; }

        
        public string HomeBannerIcons { get; set; }
        public string HomeBanners { get; set; }


        public string AttractionAlbums { get; set; }
        public string AttractionThumbs { get; set; }

        public string AttractionLanding { get; set; }
        public string AttractionGallery { get; set; }

		public string ActivityThumbs { get; set; }
		public string ActivityTypes { get; set; }
        public string ActivityBanners { get; set; }
        public string ActivityGalleries { get; set; }

        public string FaisalThumbs { get; set; }
        public string FaisalTypes { get; set; }
        public string FaisalBanners { get; set; }
        public string FaisalGalleries { get; set; }

        public string CategoryThumbs { get; set; }
        public string CategoryTypes { get; set; }
        public string CategoryBanners { get; set; }
        public string CategoryGalleries { get; set; }

        public string Events { get; set; }

        public string EventBanners { get; set; }
        public string EventGalleries { get; set; }

        public string LandingBanners { get; set; }
        public string News { get; set; }
        public string NewsBanners { get; set; }
        public string NewsGalleries { get; set; }

        public string Projects { get; set; }
        public string ProjectBanners { get; set; }
        public string ProjectGalleries { get; set; }


        public string Media { get; set; }

        public string LandingPages { get; set; }

        public string GrievanceForms { get; set; }

        public string InfomationRequestForms { get; set; }
        public string RightAccessInformations { get; set; }

        public string Careers { get; set; }
        public string SiteContent { get; set; }
        public string SiteSeo { get; set; }


        public string MediaAlbumBanners { get; set; }
        public string MediaAlbumThumbs { get; set; }
        public string MediaAlbumGalleries { get; set; }

    }
    public class EmailSettings
    {
        //Email Configuration
        public bool SendInBackground { get; set; }
        public string DefaultOTPEmailIds { get; set; } = string.Empty;
        public string EmailFromAddress { get; set; } = string.Empty;
        public string EmailSMTP { get; set; } = string.Empty;
        public int EmailPortNo { get; set; }
        public string EmailPassword { get; set; } = string.Empty;
        public string EmailDisplayName { get; set; } = string.Empty;
        public bool EmailSSLEnabled { get; set; }
        public bool EmailUseDefaultCredentials { get; set; }        
        public string DefaultOTPMobileNumbers { get; set; } = string.Empty;
        public string DefaultOTPValue { get; set; } = string.Empty;
        public int DefaultCountryId { get; set; }
    }

    public class PushSettings
    {
        ////Push notification
        public string BaseUrl { get; set; } = string.Empty;
        public string AuthorizationKey { get; set; } = string.Empty;
        public string SenderKey { get; set; } = string.Empty;
        public bool EnableRedirectToWwwRule { get; set; }
        public bool EnableSwagger { get; set; }
        public int RequestIntervalInSec { get; set; }
        public int MaxVerifyOTPTries { get; set; }
        public bool EnableDosAttackMiddleware { get; set; }
    }

    public class TabbySettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string MerchantCode { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string DefaultCurrency { get; set; } = string.Empty;
        public string SuccessUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public string FailureUrl { get; set; } = string.Empty;
    }

    public class AramexSettings
    {
        public string RateCalculatorUrl { get; set; } = string.Empty;
        public string CreateShippingUrl { get; set; } = string.Empty;
        public string AccountCountryCode { get; set; } = string.Empty;
        public string AccountEntity { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountPin { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public bool RateCalculatorEnabled { get; set; }
        public bool CreateShippingEnabled { get; set; }
        public string ClientCity { get; set; } = string.Empty;
        public string ClientAddressLine1 { get; set; } = string.Empty;
        public string ClientCountryCode { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientCompanyName { get; set; } = string.Empty;
        public string ClientCellPhone { get; set; } = string.Empty;
        public string ClientPhoneNumber1 { get; set; } = string.Empty;
        public string ShipmentDetailsProductGroup { get; set; } = string.Empty;
        public string ShipmentDetailsProductType { get; set; } = string.Empty;
        public string ShipmentDetailsPaymentType { get; set; } = string.Empty;
        public string ShipmentDetailsDescriptionOfGoods { get; set; } = string.Empty;
        public string WeightUnit { get; set; } = string.Empty;
        public bool UseAramxRateCalculator { get; set; }

    }

    public class MyFatoorah
    {
        public string Token { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string CallBackUrl { get; set; } = string.Empty;
        public decimal ProductMaxWeight { get; set; } = 4000;
    }

    public class MasterCard
    {
        public bool Enabled { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Merchant { get; set; } = string.Empty;
        public string MerchantName { get; set; } = string.Empty;
        public string MerchantAddressLine1 { get; set; } = string.Empty;
        public string MerchantAddressLine2 { get; set; } = string.Empty;
        public string InteractionOperation { get; set; } = string.Empty;
        public string InteractionReturnUrl { get; set; } = string.Empty;
        public string InteractionRequestUrl { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public double Fee { get; set; }
    }
}
