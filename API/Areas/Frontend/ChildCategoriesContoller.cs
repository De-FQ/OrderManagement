using Data.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;  // Added for IConfiguration
using Services.FrontEnd.Categories.ChildCategoryWeb;
using Services.Web.Categorys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Frontend.Category;
using Utility.ResponseMapper;

namespace API.Areas.Frontend.Controllers
{
    public class ChildCategoriesController : Controller
    {
        private readonly IWebChildCategoryService _webChildCategoryService;
        private readonly ApplicationDbContext _dbcontext;
        private readonly AppSettingsModel AppSettings;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private string imageBaseUrl = string.Empty;

        public ChildCategoriesController(IMemoryCache cache, IConfiguration configuration, AppSettingsModel appSettings, IWebChildCategoryService webChildCategoryService, ApplicationDbContext dbcontext)
        {
            _webChildCategoryService = webChildCategoryService;
            _dbcontext = dbcontext;
            AppSettings = appSettings;
            _memoryCache = cache;
            _configuration = configuration;
            this.imageBaseUrl = AppSettings.AppSettings.APIBaseUrl + AppSettings.ImageSettings.ChildCategories;
        }

        protected string GetImageUrl(string imageName)
        {
            return imageBaseUrl + imageName;
        }

        [HttpGet, Route("webapi/ChildCategories/GetActiveChildCategories")]
        public async Task<IActionResult> GetActiveChildCategories([FromQuery] int subCategoryId)
        {
            var childCategories = await _webChildCategoryService.GetActiveChildCategories(subCategoryId);
            foreach (var childCategory in childCategories)
            {
                childCategory.ImageUrl = GetImageUrl(childCategory.ImageName);
            }
            return Ok(childCategories);
        }

        [HttpPost, Route("webapi/ChildCategories/ToggleChildCategoryStatus")]
        public async Task<IActionResult> ToggleChildCategoryStatus([FromBody] ToggleCategoryStatusRequest request)
        {
            try
            {
                // Use appropriate property from request model, assuming `ChildCategoryId` for this case
                var childCategory = await _dbcontext.ChildCategories.FindAsync(request.CategoryId);
                if (childCategory == null)
                {
                    return NotFound();
                }

                childCategory.Active = request.IsActive;
                await _dbcontext.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Implement logging here using a logging service or any preferred logging framework
                return StatusCode(500, "Internal server error");
            }
        }

        //private string GetChildCategoryImageUrl(string imageName, bool useLocal = true)
        //{
        //    if (string.IsNullOrEmpty(imageName))
        //    {
        //        imageName = _configuration["ImageSettings:DefaultImage"];
        //    }

        //    string baseUrl = useLocal ? _configuration["ImageSettings:LocalBaseUrl"] : _configuration["ImageSettings:ServerBaseUrl"];
        //    string imagePath = _configuration["ImageSettings:ChildCategories"];

        //    return $"{baseUrl}{imagePath}{imageName}";
        //}


    }
}
