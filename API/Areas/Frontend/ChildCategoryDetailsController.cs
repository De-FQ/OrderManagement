using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Services.Web.Categorys;
using Utility.Models.Frontend.Category;
using Services.FrontEnd.GeneralWeb.ChildCategoryDetails;

namespace API.Areas.Frontend.Controllers
{
    [Route("webapi/[controller]")]
    [ApiController]
    public class ChildCategoryDetailsController : ControllerBase
    {
        private readonly IWebChildCategoryDetailsService _webChildCategoryDetailsService;

        public ChildCategoryDetailsController(IWebChildCategoryDetailsService webChildCategoryDetailsService)
        {
            _webChildCategoryDetailsService = webChildCategoryDetailsService;
        }

        [HttpGet("GetChildCategoryDetails")]
        public async Task<IActionResult> GetChildCategoryDetails(int childCategoryId)
        {
            var childCategoryDetails = await _webChildCategoryDetailsService.GetChildCategoryDetails(childCategoryId);
            if (childCategoryDetails == null)
            {
                return NotFound();
            }

            return Ok(childCategoryDetails);
        }
    }
}
