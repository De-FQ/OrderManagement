using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Base;

namespace Admin.Controllers.UserMgmt
{
    [Route("UserMgmt/User")]
    public class UserController : BaseController
    {
        public UserController(
            IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) :
            base(razorViewEngine, options, logger.CreateLogger(typeof(UserController).Name), apiHelper, PermissionTypes.Users)
        { }

        [HttpGet, Route("UserList")]
        public   async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/UserMgmt/User/List.cshtml");
            //await GetPermission();
            //if (permission.AllowList == false)
            //{
            //    return base.GetViewBy(Constants.Redirection.AccountAccessDenied);
            //}
            //return View("~/Views/UserMgmt/User/List.cshtml", permission);
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public   async Task<ViewResult> AddEdit(Guid? guid)
        {
            return await base.AddEditView(guid, "~/Views/UserMgmt/User/AddEdit.cshtml");


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

            //return View("~/Views/UserMgmt/User/AddEdit.cshtml", new BaseModel { Guid = guid });
        }

        
    }
}
