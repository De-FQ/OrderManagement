using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("Category")]
    public class CategoryController : BaseController
    {
        public CategoryController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(CategoryController).Name), apiHelper, PermissionTypes.Categories) { }

        [HttpGet, Route("CategoryList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/Category/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/Category/AddEdit.cshtml");
        }
    }
}
