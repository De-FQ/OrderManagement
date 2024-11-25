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
    [Route("Settings/Area")]
    public class AreaController : BaseController
    {
        public AreaController(
            IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) :
            base(razorViewEngine, options,
                logger.CreateLogger(typeof(AreaController).Name), apiHelper, PermissionTypes.Area)
        { }

        [HttpGet, Route("AreaList")]
        public   async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/Settings/Area/List.cshtml");

            //await GetPermission();
            //if (permission.AllowView == false)
            //{
            //    return base.GetViewBy(Constants.Redirection.AccountAccessDenied);
            //}
            //return View("~/Views/Settings/Area/List.cshtml", permission);
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public   async Task<ViewResult> AddEdit(Guid? guid)
        {
            return await base.AddEditView(guid, "~/Views/Settings/Area/AddEdit.cshtml");


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
            //return View("~/Views/Settings/Area/AddEdit.cshtml", new BaseModel { Guid = guid });
        }
        //[HttpGet, Route("AddEdit/{guid?}")]
        //public async Task<IActionResult> AddEdit(Guid? guid)
        //{
        //    await base.GetPermission();
        //    if (guid == null && base.permission.AllowAdd == false)
        //    {
        //        return base.GetViewBy(Constants.Redirection.AccountAccessDenied);
        //    }
        //    else if (guid.HasValue && base.permission.AllowEdit == false)
        //    {
        //        return base.GetViewBy(Constants.Redirection.AccountAccessDenied);
        //    }
        //    if (!guid.HasValue)
        //        guid = Guid.Empty;

        //    return View(new BaseModel { Guid = guid });
        //}

    }
}