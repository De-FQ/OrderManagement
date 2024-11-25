using Data.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.FrontEnd.GeneralWeb.ProductPriceWeb;
using System;
using System.Threading.Tasks;
using Utility.Models.Frontend.Category;

namespace API.Areas.Frontend.Controllers
{
    public class ProductPricesController : Controller
    {
        private readonly IProductPriceWebService _productPriceWebService;
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMemoryCache _memoryCache;

        public ProductPricesController(IMemoryCache cache, IProductPriceWebService productPriceWebService, ApplicationDbContext dbcontext)
        {
            _productPriceWebService = productPriceWebService;
            _dbcontext = dbcontext;
            _memoryCache = cache;
        }

        [HttpGet, Route("webapi/ProductPrices/GetActiveProductPrices")]
        public async Task<IActionResult> GetActiveProductPrices([FromQuery] int priceTypeId)
        {
            var productPrices = await _productPriceWebService.GetActiveProductPrices(priceTypeId);
            return Ok(productPrices);
        }

        [HttpPost, Route("webapi/ProductPrices/ToggleProductPriceStatus")]
        public async Task<IActionResult> ToggleProductPriceStatus([FromBody] ToggleCategoryStatusRequest request)
        {
            try
            {
                var productPrice = await _dbcontext.ProductPrices.FindAsync(request.PriceTypeId); // Use appropriate request model
                if (productPrice == null)
                {
                    return NotFound();
                }

                productPrice.Active = request.IsActive;
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