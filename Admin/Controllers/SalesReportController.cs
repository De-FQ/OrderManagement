using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("SalesReport")]
    public class SalesReportController : BaseController
    {
        public SalesReportController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(SalesReportController).Name), apiHelper, PermissionTypes.Categories) { }

        [HttpGet, Route("SalesReportList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/SalesReport/List.cshtml");
        }

    }
}
