using Data.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;  // Added for IConfiguration
using Services.FrontEnd.Categories.SubCategoryWeb;
using Services.Web.Categorys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Frontend.Category;
using Utility.ResponseMapper;

namespace API.Areas.Frontend.Controllers
{
    public class SubCategoriesController : Controller
    {
        private readonly IWebSubCategoryService _webSubCategoryService;
        private readonly ApplicationDbContext _dbcontext;
        private readonly AppSettingsModel AppSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;  // Added IConfiguration for base URL
        private string imageBaseUrl = string.Empty;

        public SubCategoriesController(IMemoryCache cache, IConfiguration configuration, AppSettingsModel appSettings, IWebSubCategoryService webSubCategoryService, ApplicationDbContext dbcontext)
        {
            _webSubCategoryService = webSubCategoryService;
            _dbcontext = dbcontext;
            AppSettings = appSettings;
            _memoryCache = cache;
            _configuration = configuration;
            this.imageBaseUrl = AppSettings.AppSettings.APIBaseUrl + AppSettings.ImageSettings.SubCategories;
        }

        protected string GetImageUrl(string imageName)
        {
            return imageBaseUrl + imageName;
        }

        [HttpGet, Route("webapi/SubCategories/GetActiveSubCategories")]
        public async Task<IActionResult> GetActiveSubCategories([FromQuery] int categoryId)
        {
            var subCategories = await _webSubCategoryService.GetActiveSubCategories(categoryId);
            foreach (var subCategory in subCategories)
            {
                subCategory.ImageUrl = GetImageUrl(subCategory.ImageName);
            }
            return Ok(subCategories);
        }

        [HttpPost, Route("webapi/SubCategories/ToggleSubCategoryStatus")]
        public async Task<IActionResult> ToggleSubCategoryStatus([FromBody] ToggleCategoryStatusRequest request)
        {
            try
            {
                var subCategory = await _dbcontext.SubCategories.FindAsync(request.CategoryId);
                if (subCategory == null)
                {
                    return NotFound();
                }

                subCategory.Active = request.IsActive;
                await _dbcontext.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Implement logging here using a logging service or any preferred logging framework
                return StatusCode(500, "Internal server error");
            }
        }

        //private string GetSubCategoryImageUrl(string imageName)
        //{
        //    if (string.IsNullOrEmpty(imageName))
        //    {
        //        imageName = "default.png"; 
        //    }

        //    return $"https://localhost:7000/Contents/Images/Categories/{imageName}";
        //}

        //private string GetSubCategoryImageUrl(string imageName, bool useLocal = true)
        //{
        //    if (string.IsNullOrEmpty(imageName))
        //    {
        //        imageName = _configuration["ImageSettings:DefaultImage"];
        //    }

        //    string baseUrl = useLocal ? _configuration["ImageSettings:LocalBaseUrl"] : _configuration["ImageSettings:ServerBaseUrl"];
        //    string imagePath = _configuration["ImageSettings:SubCategories"];

        //    return $"{baseUrl}{imagePath}{imageName}";
        //}

    }
}
