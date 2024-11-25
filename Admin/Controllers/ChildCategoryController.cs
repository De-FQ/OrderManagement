using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Utility.API;
using Utility.Enum;

namespace Admin.Controllers.Settings
{
    [Route("ChildCategory")]
    public class ChildCategoryController : BaseController
    {
        public ChildCategoryController(IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) : base(razorViewEngine, options, logger.CreateLogger(typeof(ChildCategoryController).Name), apiHelper, PermissionTypes.ChildCategories) { }

        [HttpGet, Route("ChildCategoryList")]
        public async Task<IActionResult> List()
        {
            return await base.ListView("~/Views/ChildCategory/List.cshtml");
        }

        [HttpGet, Route("AddEdit/{guid?}")]
        public async Task<ViewResult> AddEdit(Guid? guid, Guid? parentGuid)
        {
            ViewBag.ParentGuid = parentGuid;
            return await base.AddEditView(guid, "~/Views/ChildCategory/AddEdit.cshtml");
        }
    }
}
