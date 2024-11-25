using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("Stock")]
    public class StockController : BaseController
    {
        public StockController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(StockController).Name), apiHelper, PermissionTypes.PriceTypeCategories) { }

        [HttpGet, Route("StockList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/Stock/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/Stock/AddEdit.cshtml");
        }
    }
}
