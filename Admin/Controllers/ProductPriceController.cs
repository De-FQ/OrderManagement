using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("ProductPrice")]
    public class ProductPriceController : BaseController
    {
        public ProductPriceController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(ProductPriceController).Name), apiHelper, PermissionTypes.ProductPrices) { }

        [HttpGet, Route("ProductPriceList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/ProductPrice/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/ProductPrice/AddEdit.cshtml");
        }
    }
}
