using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.SiteCategory;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.Categorys;
using Services.Backend.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace API.Areas.Backend.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService<Category> _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            ICategoryService<Category> categoryService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.Categories)
        {
            base.ControllerName = typeof(CategoryController).Name;
            _categoryService = categoryService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region Category Management

        [HttpGet, Route("api/Category/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<Category> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (guid.HasValue)
                {
                    var item = await _categoryService.GetByGuid(guid.Value);
                    response.GetById(item);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost, Route("api/Category/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] Category item)
        {
            ResponseMapper<Category> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (await _categoryService.Exists(item.Name, item.Guid))
                {
                    accessResponse.Message = item.Name + ResourceHelper.GetResource("category_name_already_taken", IsEnglish);
                    accessResponse.Success = false;
                    accessResponse.StatusCode = 300;
                    return Ok(accessResponse);
                }

                SaveImage(ref item); 

                if (item.DiscountActive && item.DiscountPercentage > 0)
                {
                    await _categoryService.UpdateCategoryDiscount(item.Guid.Value, item.DiscountActive, item.DiscountPercentage);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _categoryService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("category_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _categoryService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("category_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/Category/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<Category> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.Active)) { return Ok(accessResponse); }

                var item = await _categoryService.ToggleActive(guid);
                response.ToggleActive(item);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/Category/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<Category> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.DisplayOrder)) { return Ok(accessResponse); }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _categoryService.UpdateDisplayOrders(items);
                    response.DisplayOrder(true);
                }

                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }


        [HttpPost, Route("api/Category/GetCategoryForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetCategoryForDataTable()
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var items = await _categoryService.GetCategoryForDataTable(param: base.GetDataTableParameters);
                foreach (var item in items.Data)
                {
                    item.ImageUrl = GetBaseImageUrl(AppSettings.ImageSettings.Categories) + (string.IsNullOrEmpty(item.ImageName) ? "default.png" : item.ImageName);

                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/Category/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.Delete)) { return Ok(accessResponse); }

                var item = await _categoryService.Delete(guid);
                response.Delete(item);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/Category/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _categoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/Category/UpdateCategoryDiscount")]
        public async Task<IActionResult> UpdateCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var success = await _categoryService.UpdateCategoryDiscount(guid, isDiscountActive, discountPercentage);

                response.Success = success;
                response.Message = success ? "Discount updated successfully" : "Failed to update discount";
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpPost, Route("api/Category/ImportFromExcel")]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Categories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (file == null || file.Length == 0)
                {
                    response.Success = false;
                    response.Message = "No file selected or file is empty.";
                    return Ok(response);
                }

                var result = await _categoryService.ImportCategoriesFromExcel(file);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Categories imported successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to import categories.";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error($"ImportFromExcel API failed: {ex.Message}");
            }

            return Ok(response);
        }



        private void ClearCache()
        {
            _memoryCache.Remove("Categories");
        }


        private Category BuildUrl(Category item)
        {
            if (!string.IsNullOrEmpty(item.ImageName))
            {
                item.ImageUrl = GetImageUrl(AppSettings.ImageSettings.Categories, item.ImageName);
            }

            return item;
        }

        private void SaveImage(ref Category item)
        {
            if (item.Image != null && item.Image.Length > 0)
            {
                string fileName = MediaHelper.ConvertImageToWebp(item.ImageName, item.Image, AppSettings.ImageSettings.Categories);
                if (!string.IsNullOrEmpty(fileName))
                    item.ImageName = fileName;
            }
        }
        #endregion


    }
}
