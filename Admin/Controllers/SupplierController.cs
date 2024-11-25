using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("Supplier")]
    public class SupplierController : BaseController
    {
        public SupplierController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(SupplierController).Name), apiHelper, PermissionTypes.Categories) { }

        [HttpGet, Route("SupplierList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/Supplier/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/Supplier/AddEdit.cshtml");
        }
    }
}
