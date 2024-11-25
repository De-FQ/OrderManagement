
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Utility.Helpers;
using Utility.ResponseMapper;
using Services.Backend.UserManagement;
using Utility.API;
using Utility.Enum;
using Utility.Models.Base;
using Microsoft.AspNetCore.Antiforgery;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.DataProtection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Data.UserManagement;

namespace API.Areas.Backend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    // [AllowCrossSiteAttribute]
    [IgnoreAntiforgeryToken]
    public class BaseController : ControllerBase
    {
        //protected ILogger _logger;
        /// <summary>
        /// for every post request __RequestVerificationToken parameter should be exists, 
        /// <br></br>otherwise javascript injection foreach loop for multiple submission cannot be stopped
        /// <br></br>__RequestVerificationToken parameter is added in "submitHiddenData" method of site.js file,
        /// through-out this project "submitHiddenData" is called in "saveData()" of each "?_CRUD.js" file
        /// </summary>

        protected string ControllerName;
        protected readonly AppSettingsModel AppSettings;
        protected readonly IUserService<User> UserService;
        protected readonly IAPIHelper _apiHelper;
        protected IMemoryCache _memoryCache;
        protected ResponseMapper<User> accessResponse = new();
        protected string FullName = string.Empty;
        protected long UserId = 0;
        protected long RoleId = 0;
        protected long RoleTypeId = 0;
        protected int PermissionType;
        protected bool TokenExpired { get; set; }
        public BaseController(
        AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            PermissionTypes permissionType = (int)PermissionTypes.None)
        {
            AppSettings = options;
            UserService = systemUserService;
            _apiHelper = apiHelper;
            PermissionType = (int)permissionType;

        }
        protected string GetHeaderValueByName(string headerName)
        {
            string headerValue = string.Empty;
            if (Request.Headers.ContainsKey(headerName))
            {
                headerValue = Request.Headers[headerName];
            }

            return headerValue;
        }

        protected Tuple<long, long, long> AuthenticateUser
        {
            get
            {
                this.UserId = 0;
                this.RoleId = 0;
                this.RoleTypeId = 0;
                var temp = new Tuple<long, long, long>(0, 0, 0);
                var headers = Request.Headers;
                if (headers.ContainsKey("Authorization"))
                {


                    headers.TryGetValue("Authorization", out StringValues tokens);
                    var token = tokens.ToString().Replace("Bearer ", "");
                    if (!string.IsNullOrEmpty(token))
                    {
                        if (token.ToLower() == "Bearer".ToLower())
                        {
                            // TokenExpired = true;
                            return temp;
                        }
                        UserModel userModel = _apiHelper.GetUserModelFromToken(token);
                        if (userModel is null)
                        {
                            return temp;
                        }
                        if (userModel.TokenExpired)
                        {
                            TokenExpired = true;
                            // accessResponse = setExpiredObject("Token is Expired, please login again", 250);
                            return temp;

                        }
                        else
                        {
                            this.UserId = long.Parse(userModel.Id, 0);
                            this.RoleId = long.Parse(userModel.RoleId, 0);
                            this.RoleTypeId = long.Parse(userModel.RoleTypeId, 0);
                        }
                    }
                }

                return new Tuple<long, long, long>(UserId, RoleId, RoleTypeId);
            }
        }


        /// <summary>
        /// The purpose of the end-point to validate the user session is valid 
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        //[HttpGet, Route("/api/base/Allowed")]
        protected async Task<bool> Allowed(int permissionId = (int)PermissionTypes.None)
        {
            accessResponse = new();
            var auth = AuthenticateUser;

            if (permissionId != (int)PermissionTypes.None)
            {
                this.PermissionType = permissionId;
            }

            if (AppSettings.AppSettings.EnableAuthorization)
            {
                if (TokenExpired)
                {
                    //setResponseObject("Token is Expired, please login again", 250, false);
                    //accessResponse.Message = "Token is Expired, please login again";
                    //accessResponse.Success = false;
                    //accessResponse.StatusCode = 250;
                    return false;
                }

                var allowed = await UserService.Allowed(RoleId, this.PermissionType);
                if (!allowed)
                {
                    setResponseObject("UnAuthorized", "You are not authorized, Access right is denied", 401, false);

                    //accessResponse.Message = "You are not authorized, Access right is denied";
                    //accessResponse.Success = false;
                    //accessResponse.StatusCode = 401;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// The purpose of the end-point to check the logged-in user permission on specific end-point
        /// <list type="">
        /// <item>0-None</item>
        /// <item>1-List</item>
        /// <item>2-DisplayOrder</item>
        /// <item>3-Add</item>
        /// <item>4-Edit</item>
        /// <item>5-Delete</item>
        /// <item>6-View</item>
        /// <item>7-Active</item>
        /// <item>8-Allowed</item>
        /// </list>
        /// </summary>
        /// <param name="permissiontype"></param>
        /// <param name="allowPermission"></param>
        /// <returns></returns>
        [HttpGet, Route("AccessPermission")]
        public async Task<bool> AccessPermission(PermissionTypes permissiontype = PermissionTypes.None, AllowPermission allowPermission = AllowPermission.None)
        {
            accessResponse = new();
            var auth = AuthenticateUser;

            if (AppSettings.AppSettings.EnableAuthorization)
            {
                if (TokenExpired)
                {
                    //setResponseObject("Token is Expired, please login again", 250, false);
                    //accessResponse.Message = "Token is Expired, please login again";
                    //accessResponse.Success = false;
                    //accessResponse.StatusCode = 250;
                    return false;
                }
                var allowed = await UserService.AccessPermission(RoleId, permissiontype, allowPermission);
                if (!allowed)
                {
                    setResponseObject("UnAuthorized", "You are not authorized, Access right is denied", 401, false);

                    //accessResponse.Message = "You are not authorized, Access right is denied";
                    //accessResponse.Success = false;
                    //accessResponse.StatusCode = 401;
                    return false;
                }
            }

            return true;
        }

        [HttpGet, Route("AllowedForExceptionRole")]
        public bool AllowedForExceptionRole(int[] ExceptionRoleIds)
        {
            var success = false;
            foreach (var roleId in ExceptionRoleIds)
            {
                if (this.RoleId == roleId) // This condition is for special role permission, RoleId=5
                {
                    success = true;
                }
            }
            return success;
        }
        #region Default Language
        public bool IsEnglish
        {
            get
            {
                var headers = Request.Headers;
                if (headers.ContainsKey(Constants.Common.Lang))
                {
                    headers.TryGetValue(Constants.Common.Lang, out StringValues langs);

                    var lang = langs.FirstOrDefault();
                    if (lang != null)
                    {
                        if (lang.ToUpper().Contains("EN-US") || lang.ToUpper().Contains("EN"))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Base Image Url
        protected string GetBaseUrl()
        {
            return AppSettings.AppSettings.APIBaseUrl;
        }
        protected string GetBaseImageUrl(string folderName)
        {
            return AppSettings.AppSettings.APIBaseUrl + folderName;
        }
        protected string GetImageUrl(string folderName, string imageName)
        {
            return AppSettings.AppSettings.APIBaseUrl + folderName + imageName;
        }
        #endregion

        #region DataTable
        public DataTableParam GetDataTableParameters
        {
            get
            {
                return new DataTableParam()
                {
                    Draw = Draw,
                    IsEnglish = IsEnglish,
                    SearchValue = SearchValue,
                    SortColumn = SortColumn,
                    SortColumnDirection = SortColumnDirection,
                    Skip = Skip,
                    PageSize = PageSize
                };
            }
        }
        public string Draw
        {
            get
            {
                try
                {
                    return Request.Form["draw"].FirstOrDefault();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string SortColumn
        {
            get
            {
                try
                {
                    return Request.Form["columns[" + (Request.Form["order[0][column]"].FirstOrDefault() ?? "0") + "][name]"].FirstOrDefault() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string SortColumnDirection
        {
            get
            {
                try
                {
                    return Request.Form["order[0][dir]"].FirstOrDefault() ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public string SearchValue
        {
            get
            {
                try
                {
                    return (Request.Form["search[value]"].FirstOrDefault() ?? string.Empty).ToLower();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        public int PageSize
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int Skip
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        [HttpGet]
        private void setResponseObject(string title, string message, int status, bool expired)
        {
            TokenExpired = expired;
            accessResponse.Title = title;
            accessResponse.Message = message;
            accessResponse.Success = false;
            accessResponse.StatusCode = status;
            //return accessResponse;
        }

        [HttpGet, Route("Loginfo")]
        protected void LogInfo(string message, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Information)
        {
            switch (level)
            {
                case LogEventLevel.Information:
                    Log.Information(ControllerName + ":" + message);
                    break;
                case LogEventLevel.Warning:
                    Log.Warning(ControllerName + ":" + message);
                    break;
                case LogEventLevel.Debug:
                    Log.Debug(ControllerName + ":" + message);
                    break;
                case LogEventLevel.Fatal:
                    Log.Fatal(ControllerName + ":" + message);
                    break;
                case LogEventLevel.Error:
                    Log.Error(ControllerName + ":" + message);
                    break;
                default:
                    break;
            }

        }

        protected string GetFormattedSeo(string Name)
        {
            var SeoName = Regex.Replace(Name, @"[^A-Za-z0-9\s-]", ""); // replace all special charecters with emptyspace
            SeoName = Regex.Replace(SeoName, @"\s+", " ").Trim();  //replace consecutive empty spaces with single space
            SeoName = Regex.Replace(SeoName, @"\s", "-"); // replace single empty space with hyphen('-')

            if (!string.IsNullOrEmpty(SeoName))
            {
                SeoName = SeoName.ToLower();
            }

            return SeoName;
        }
        protected void RemoveCache(string key)
        {
            try
            {
                _memoryCache.Remove(key);
            }
            catch { }
        }
        protected void RemoveCaches(string keys)
        {
            try
            {
                var arrKey = keys.Split(',');
                foreach (var key in arrKey)
                {
                    _memoryCache.Remove(key);
                }
            }
            catch { }
        }

        protected void ClearCache()
        {
            _memoryCache.Remove("Categories");
        }

    //    protected DataTableParam GetDataTableParameters()
    //{
    //    // Assuming DataTableParam is the correct type
    //    // Populate the parameters as required
    //    var parameters = new DataTableParam
    //    {
    //        // Set properties of parameters as required
    //    };

    //    return parameters;
    //}
    }
}

