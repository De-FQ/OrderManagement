using Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Diagnostics;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace Admin.Controllers.Settings
{
    [Route("Settings/WebSiteMap")]
    public class WebSiteMapController : BaseController
    {
        public WebSiteMapController(
            IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) :
            base(razorViewEngine, options, logger.CreateLogger(typeof(WebSiteMapController).Name), apiHelper, PermissionTypes.WebSiteMap)

        { }

        [HttpGet, Route("WebSiteMapList")]
        public   async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/Settings/WebSiteMap/List.cshtml");
            //await GetPermission();
            //if (permission.AllowView == false)
            //{
            //    return base.GetViewBy(Constants.Redirection.AccountAccessDenied);
            //}
            //return View("~/Views/Settings/WebSiteMap/List.cshtml", permission);
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public   async Task<ViewResult> AddEdit(Guid? guid)
        {
            return await base.AddEditView(guid, "~/Views/Settings/WebSiteMap/AddEdit.cshtml");

            //await GetPermission();
            //if (guid == null && permission.AllowAdd == false)
            //{
            //    return GetViewBy(Constants.Redirection.AccountAccessDenied);
            //}
            //else if (guid.HasValue && permission.AllowEdit == false)
            //{
            //    return GetViewBy(Constants.Redirection.AccountAccessDenied);
            //}
            //if (!guid.HasValue) guid = Guid.Empty;

            //return View("~/Views/Settings/WebSiteMap/AddEdit.cshtml", new BaseModel { Guid = guid });
        }

    }
}