using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Base;

namespace Admin.Controllers.UserMgmt
{
    [Route("UserMgmt/Role")]
    public class RoleController : BaseController
    {
        public RoleController(
            IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) :
            base(razorViewEngine, options, logger.CreateLogger(typeof(RoleController).Name), apiHelper, PermissionTypes.UserRoles)
        { }

        [HttpGet, Route("RoleList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/UserMgmt/Role/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public   async Task<ViewResult> AddEdit(Guid? guid)
        {
            return await base.AddEditView(guid, "~/Views/UserMgmt/Role/AddEdit.cshtml");
        }

        
    }
}
