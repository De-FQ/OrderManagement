using Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace Admin.Controllers.Settings
{
    [Route("Settings/Country")]
    public class CountryController : BaseController
    {
        public CountryController(
            IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) :
            base(razorViewEngine, options,
                logger.CreateLogger(typeof(CountryController).Name), apiHelper, PermissionTypes.Country)
        {

        }


        [HttpGet, Route("CountryList")]
        public   async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/Settings/Country/List.cshtml");


        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public   async Task<ViewResult> AddEdit(Guid? guid)
        {
            return await base.AddEditView(guid, "~/Views/Settings/Country/AddEdit.cshtml");

        }

    }
}