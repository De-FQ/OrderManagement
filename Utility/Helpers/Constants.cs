using Microsoft.AspNetCore.Mvc.Controllers;

namespace Utility.Helpers
{
    public static class Constants
    {
        public static class Cookie
        {
            public const string AuthenticationScheme = "Cookies";
        }
        public static class Roles
        {
            public const string Root = "Root";
            public const string Administrator = "Administrator";
            public const string RootOrAdministrator = Root + "," + Administrator;
            public const string Supervisor = "Supervisor";
            public const string Customer = "Customer";
            public const string AdministratorOrSupervisor = Administrator +"," + Supervisor;
        }
        public static class RoleTypes
        {
            /// <summary>
            /// fixed Role ID system-wise for internal system root only
            /// </summary>
            public readonly static int ROOT_ROLE_ID = 1;
            public readonly static int DRIVER_ROLE_ID = 4;
            public readonly static int ROLE_ID_SALES_PERSON = 5;

            /// <summary>
            /// This permission can only be visible to internal system root user
            /// </summary>
            public readonly static int ROOT_PERMISSION_ID = 5;
        }
        public static class Common
        {
            public static int InvalidLoginAttemptsCount { get; set; } = 0;
            public const int MaxLoginAttempts = 3;
            public const string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public const string AlphaNumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            public const string Lang = "lang";
            public const string APIForUrlHub = "/hubs/info";
            public const string WehHubUrl = "/webhubs/info";
            public const string APIUrlForAntiforgeryToken = "/api/heartbit";
        }
        public static class ClaimTypes
        {
             

            public const string Id = "Id";
            public const string EncryptedId = "EncryptedId";
            public const string Guid = "Guid"; 
            //public const string FullName = "FullName";
            public const string RoleId = "RoleId";
            public const string RoleName = "RoleName"; 
            public const string RoleTypeId = "RoleTypeId";
            public const string ImageUrl = "ImageUrl";
            public const string AuthenticationToken = "AuthenticationToken"; //this name should be same in "auth-helper.js" file
            public const string RefreshToken = "refreshToken";
            public const string X_XSRF_TOKEN = "X-XSRF-TOKEN";
            /// <summary>
            /// assigned UserType based on at Token Generation (Admin/WebUser)
            /// </summary>
            public const string UserType = "UserType"; //admin or web user
            /// <summary>
            /// for admin site user
            /// </summary>
            public const string Admin = "Admin";
            /// <summary>
            /// for website user
            /// </summary>
            public const string WebUser = "WebUser";

            //frontend website
            public const string Name = "Name";
            //public const string Email = "Email"; //USER ID
            public const string PhoneNumber = "PhoneNumber";
            // public const string ApplicantGuid = "ApplicantGuid";
            //public const string EntityGuid = "EntityGuid";
            //public const string EntityId = "EntityId";
        }
        public static class DataSize
        {
            public const int ColorCode = 7;
            public const int CivilId = 12;
            public const int Password = 50;
            public const int Long = 64;
            /// <summary>
            /// All images name is genrated by GUID, its size is 36 + ext that is dot + 3 latters=40
            ///<br></br> 10 more charaters for extra space, we might remove it later on 
            /// </summary>
            public const int ImageName = 50; 
            public const int Mobile = 20;
            public const int Email = 200;
            public const int FileExtension = 5;
            public const int Title = 200;
            public const int TitleSmall = 50;
            public const int TitleSmallest = 10;
            public const int TitleMedium = 100;
            public const int TitleExtraMedium = 150;
            public const int TitleLarge = 200;
            public const int TitleExtraLarge = 400;
            public const int DescriptionExtraLarge = 500;
            public const string Numeric = "numeric(18, 0)";
            public const string Decimal3 = "decimal(18, 3)";
            public const string Decimal2 = "decimal(18, 2)";
            public const string HtmlContent = "nText";
        }
        public static class DateTimeFormat
        {
            public const string DisplayDate = "dd/MM/yyyy";
            public const string DisplayTime = "hh:mm:ss tt";
            public const string DisplayDateTime = DisplayDate + " " + DisplayTime;
        }
        public static class Redirection
        {
           // public static class Controller {
                /// <summary>
                /// Redirect to Account Controller
                /// </summary>
                public const string HomeIndex = "/Home/Index";
                public const string HomeCategory = "/Home/Category";

                public const string AccountLogin = "/Account/Login";
                public const string AccountIndex = "/Account/Index";
                public const string AccountAccessDenied = "/Account/AccessDenied";
                public const string AccountSessionExpired = "/Account/SessionExpired";
                public const string AccountLogout = "/Account/Logout";
            public const string AccountSignOut = "/Account/SignOut";
                 
          //  }

            //public static class Action {
            //   // public const string Index = "/Index";
            //   // public const string Login = "/Login";
            //    /// <summary>
            //    /// Controller Method Logout
            //    /// </summary>
                
            //  //  public const string AccessDenied = "/Home/AccessDenied";
                
            //}
           

            
        }
    }
}
