using Data.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.FrontEnd.GeneralWeb.PeiceTypeCategoryWeb;
using System;
using System.Threading.Tasks;
using Utility.Models.Frontend.Category;

namespace API.Areas.Frontend
{
    public class PriceTypeCategoriesController : Controller
    {
        private readonly IPriceTypeCategoryWebService _priceTypeCategoryWebService;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;

        public PriceTypeCategoriesController(IMemoryCache cache, IPriceTypeCategoryWebService priceTypeCategoryWebService, ApplicationDbContext dbcontext)
        {
            _priceTypeCategoryWebService = priceTypeCategoryWebService;
            _dbcontext = dbcontext;
            _memoryCache = cache;
        }

        [HttpGet, Route("webapi/PriceTypeCategories/GetActivePriceTypeCategories")]
        public async Task<IActionResult> GetActivePriceTypeCategories([FromQuery] int childCategoryId)
        {
            var priceTypeCategories = await _priceTypeCategoryWebService.GetActivePriceTypeCategories(childCategoryId);
            return Ok(priceTypeCategories);
        }

        [HttpPost, Route("webapi/PriceTypeCategories/TogglePriceTypeCategoryStatus")]
        public async Task<IActionResult> TogglePriceTypeCategoryStatus([FromBody] ToggleCategoryStatusRequest request)
        {
            try
            {
                var priceTypeCategory = await _dbcontext.PriceTypeCategories.FindAsync(request.ChildCategoryId); // Use appropriate request model
                if (priceTypeCategory == null)
                {
                    return NotFound();
                }

                priceTypeCategory.Active = request.IsActive;
                await _dbcontext.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception (implement logging as per your logging strategy)
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
