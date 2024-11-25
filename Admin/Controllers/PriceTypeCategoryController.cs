using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("PriceTypeCategory")]
    public class PriceTypeCategoryController : BaseController
    {
        public PriceTypeCategoryController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(PriceTypeCategoryController).Name), apiHelper, PermissionTypes.PriceTypeCategories) { }

        [HttpGet, Route("PriceTypeCategoryList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/PriceTypeCategory/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/PriceTypeCategory/AddEdit.cshtml");
        }
    }
}
