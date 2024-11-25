using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics;
using Utility.API;
using Web.Models;

namespace Web.Controllers
{
    
    public class BaseController : Controller
     {
        private readonly IRazorViewEngine _razorViewEngine;
        protected readonly IAPIHelper _apiHelper;
        protected readonly AppSettingsModel _appSettingsModel;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController(IRazorViewEngine razorViewEngine, IAPIHelper apiHelper, AppSettingsModel options, IHttpContextAccessor httpContextAccessor)
        {
            _razorViewEngine = razorViewEngine;
            _apiHelper = apiHelper;
            _appSettingsModel = options; 
            _httpContextAccessor = httpContextAccessor;
            //HttpContext.Session.SetString("apiurl", _appSettingsModel.AppSettings.APIBaseUrl);
            //ViewData["APIBaseUrl"] = _appSettingsModel.AppSettings.APIBaseUrl;
            httpContextAccessor.HttpContext.Response.Cookies.Append("api", _appSettingsModel.AppSettings.APIBaseUrl);
            httpContextAccessor.HttpContext.Response.Cookies.Append("lang", System.Globalization.CultureInfo.CurrentCulture.Name);
            //ViewBag.APIBaseUrl = _appSettingsModel.AppSettings.APIBaseUrl;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        protected virtual async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            //create action context
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

            //set view name as action name in case if not passed
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            //set model
            ViewData.Model = model;

            //try to get a view by the name
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
            if (viewResult.View == null)
            {
                //or try to get a view by the path
                viewResult = _razorViewEngine.GetView(null, viewName, false);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} view was not found");
            }
            await using var stringWriter = new StringWriter();
            var viewContext = new ViewContext(actionContext, viewResult.View, ViewData, TempData, stringWriter, new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.GetStringBuilder().ToString();
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
 
 
