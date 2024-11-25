using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("PriceType")]
    public class PriceTypeController : BaseController
    {
        public PriceTypeController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(PriceTypeController).Name), apiHelper, PermissionTypes.PriceTypeCategories) { }

        [HttpGet, Route("PriceTypeList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/PriceType/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/PriceType/AddEdit.cshtml");
        }
    }
}
