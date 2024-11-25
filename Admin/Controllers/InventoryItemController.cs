using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("InventoryItem")]
    public class InventoryItemController : BaseController
    {
        public InventoryItemController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(InventoryItemController).Name), apiHelper, PermissionTypes.PriceTypeCategories) { }

        [HttpGet, Route("InventoryItemList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/InventoryItem/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/InventoryItem/AddEdit.cshtml");
        }
    }
}
