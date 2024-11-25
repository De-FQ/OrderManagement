using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("SupplierItem")]
    public class SupplierItemController : BaseController
    {
        public SupplierItemController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(SupplierItemController).Name), apiHelper, PermissionTypes.PriceTypeCategories) { }

        [HttpGet, Route("SupplierItemList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/SupplierItem/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/SupplierItem/AddEdit.cshtml");
        }
    }
}
