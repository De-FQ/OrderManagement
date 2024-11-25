using Data.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.FrontEnd.GeneralWeb.PriceTypeWeb;
using System;
using System.Threading.Tasks;
using Utility.Models.Frontend.Category;

namespace API.Areas.Frontend.Controllers
{
    public class PriceTypesController : Controller
    {
        private readonly IPriceTypeWebService _priceTypeWebService;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;

        public PriceTypesController(IMemoryCache cache, IPriceTypeWebService priceTypeWebService, ApplicationDbContext dbcontext)
        {
            _priceTypeWebService = priceTypeWebService;
            _dbcontext = dbcontext;
            _memoryCache = cache;
        }

        [HttpGet, Route("webapi/PriceTypes/GetActivePriceTypes")]
        public async Task<IActionResult> GetActivePriceTypes([FromQuery] int priceTypeCategoryId)
        {
            var priceTypes = await _priceTypeWebService.GetActivePriceTypes(priceTypeCategoryId);
            return Ok(priceTypes);
        }

        [HttpPost, Route("webapi/PriceTypes/TogglePriceTypeStatus")]
        public async Task<IActionResult> TogglePriceTypeStatus([FromBody] ToggleCategoryStatusRequest request)
        {
            try
            {
                var priceType = await _dbcontext.PriceTypes.FindAsync(request.PriceTypeCategoryId); // Use appropriate request model
                if (priceType == null)
                {
                    return NotFound();
                }

                priceType.Active = request.IsActive;
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
