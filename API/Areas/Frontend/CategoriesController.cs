using Data.EntityFramework;
using Data.Model.SiteCategory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Web.Categorys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Frontend.Category;
using Utility.ResponseMapper;

namespace API.Areas.Frontend.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IWebCategoryService _webCategoryService;
        private readonly ApplicationDbContext _dbcontext;
        private readonly AppSettingsModel AppSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private string imageBaseUrl=string.Empty;
        public CategoriesController(IMemoryCache cache, IConfiguration configuration, AppSettingsModel appSettings, IWebCategoryService webCategoryService, ApplicationDbContext dbcontext)
        {
            _webCategoryService = webCategoryService;
            _dbcontext = dbcontext;
            AppSettings = appSettings;
            _memoryCache = cache;
            _configuration = configuration;
             
            this.imageBaseUrl = AppSettings.AppSettings.APIBaseUrl + AppSettings.ImageSettings.Categories;
             
        }
        protected string GetImageUrl(string imageName)
        {
            return imageBaseUrl + imageName;
        }

        [HttpGet, Route("webapi/Categories/GetActiveCategories")]
        public async Task<IActionResult> GetActiveCategories()
        {
            var categories = await _webCategoryService.GetActiveCategories();
            foreach (var category in categories)
            {
                category.ImageUrl = GetImageUrl(category.ImageName);
            }
            return Ok(categories);
        }


        [HttpPost, Route("webapi/Categories/ToggleCategoryStatus")]
        public async Task<IActionResult> ToggleCategoryStatus([FromBody] ToggleCategoryStatusRequest request)
        {
            try
            {
                var category = await _dbcontext.Categories.FindAsync(request.CategoryId);
                if (category == null)
                {
                    return NotFound();
                }

                category.Active = request.IsActive;
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
