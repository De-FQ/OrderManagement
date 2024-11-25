using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("SubCategory")]
    public class SubCategoryController : BaseController
    {
        public SubCategoryController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(SubCategoryController).Name), apiHelper, PermissionTypes.SubCategories) { }

        [HttpGet, Route("SubCategoryList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/SubCategory/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/SubCategory/AddEdit.cshtml");
        }
    }
}
