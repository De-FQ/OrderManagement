using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Utility.API;
using Utility.Enum;
using Serilog.Events;
using Utility.LoggerService;
using Microsoft.Extensions.Caching.Memory;
using Utility.Models.Frontend.Category;
using Utility.ResponseMapper;
namespace API.Areas.Frontend.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Memory Cache variables [_memoryCache,semaphore]
        /// </summary>
        protected IMemoryCache _memoryCache;
        protected static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        protected void AddToMemoryCache(string cacheKey, object o)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(3));
            _memoryCache.Set(cacheKey, o, cacheEntryOptions);
        }

        protected string ControllerName; 
        protected readonly AppSettingsModel AppSettings;
        private string ApiBaseUrl { get; set; }
        public BaseController(AppSettingsModel options)
        {
            AppSettings = options;
            
        }
        //protected string GetImageUrl(string folderName, string imageName)
        //{
        //    return ApiBaseUrl + folderName + imageName;
        //}
        //protected string GetBaseImageUrl(string folderName)
        //{
        //    return ApiBaseUrl + folderName;
        //}

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


        public bool isEnglish
        {
            get
            {
                var headers = Request.Headers;
                if (headers.ContainsKey("lang"))
                {
                    StringValues langs;
                    headers.TryGetValue("lang", out langs);

                    var lang = langs.FirstOrDefault();
                    if (lang.ToUpper() == "EN")
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public DeviceType HeaderDeviceTypeId
        {
            get
            {
                var headers = Request.Headers;
                if (headers.ContainsKey("deviceTypeId"))
                {
                    StringValues deviceTypeIds;
                    headers.TryGetValue("deviceTypeId", out deviceTypeIds);

                    var deviceTypeId = deviceTypeIds.FirstOrDefault();

                    Enum.TryParse(deviceTypeId, out DeviceType typeId);
                    return typeId;
                }

                return 0;
            }
        }
        public string ServiceAPIKey
        {
            get
            {
                var headers = Request.Headers;
                if (headers.ContainsKey("serviceAPIKey"))
                {
                    StringValues serviceAPIKeys;
                    headers.TryGetValue("serviceAPIKey", out serviceAPIKeys);

                    var serviceAPIKey = serviceAPIKeys.FirstOrDefault();
                    return serviceAPIKey;
                }

                return string.Empty;
            }
        }

        [HttpGet, Route("Loginfo")]
        protected void LogInfo(string message, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Information)
        {
            switch (level)
            {
                case LogEventLevel.Information: 
                    AppSeriLog.LogInfo(message, ControllerName, Serilog.Events.LogEventLevel.Information);
                    break;
                case LogEventLevel.Warning:
                    AppSeriLog.LogInfo(message, ControllerName, Serilog.Events.LogEventLevel.Warning);
                    break;
                case LogEventLevel.Debug:
                    AppSeriLog.LogInfo(message, ControllerName, Serilog.Events.LogEventLevel.Debug);
                    break;
                case LogEventLevel.Fatal:
                    AppSeriLog.LogInfo(message, ControllerName, Serilog.Events.LogEventLevel.Fatal);
                    break;
                case LogEventLevel.Error:
                    AppSeriLog.LogInfo(message, ControllerName, Serilog.Events.LogEventLevel.Error);
                    break;
                default:
                    break;
            }

        }

       

       
    }
}

