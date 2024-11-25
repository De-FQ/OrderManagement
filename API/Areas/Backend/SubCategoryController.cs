using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.SiteCategory;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.SubCategories;
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
    public class SubCategoryController : BaseController
    {
        private readonly ISubCategoryService<SubCategory> _subCategoryService;
        private readonly IMapper _mapper;

        public SubCategoryController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            ISubCategoryService<SubCategory> subCategoryService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.SubCategories)
        {
            base.ControllerName = typeof(SubCategoryController).Name;
            _subCategoryService = subCategoryService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region SubCategory Management

        [HttpGet, Route("api/SubCategory/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<SubCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _subCategoryService.GetByGuid(guid.Value);
                    response.GetById(item);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetByGuid");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SubCategory/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] SubCategory item)
        {
            ResponseMapper<SubCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (await _subCategoryService.Exists(item.Name, item.Guid))
                {
                    accessResponse.Message = item.Name + ResourceHelper.GetResource("subcategory_name_already_taken", IsEnglish);
                    accessResponse.Success = false;
                    accessResponse.StatusCode = 300;
                    return Ok(accessResponse);
                }

                SaveImage(ref item);

                if ( item.DiscountActive && item.DiscountPercentage>0)
                {
                    await _subCategoryService.UpdateSubCategoryDiscount(item.Guid.Value, item.DiscountActive, item.DiscountPercentage);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _subCategoryService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("subcategory_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _subCategoryService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("subcategory_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SubCategory/UpdateSubCategoryDiscount")]
        public async Task<IActionResult> UpdateSubCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var success = await _subCategoryService.UpdateSubCategoryDiscount(guid, isDiscountActive, discountPercentage);

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

        [HttpPost, Route("api/SubCategory/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<SubCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _subCategoryService.ToggleActive(guid);
                response.ToggleActive(item);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ToggleActiveAsync");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SubCategory/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<SubCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.DisplayOrder))
                {
                    return Ok(accessResponse);
                }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _subCategoryService.UpdateDisplayOrders(items);
                    response.DisplayOrder(true);
                }

                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in UpdateDisplayOrders");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SubCategory/GetSubCategoryForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetSubCategoryForDataTable([FromForm] int? categoryId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters; // Using as a method
                var items = await _subCategoryService.GetSubCategoryForDataTable(parameters, categoryId);
                foreach (var item in items.Data)
                {
                    item.ImageUrl = GetBaseImageUrl(AppSettings.ImageSettings.SubCategories) + (string.IsNullOrEmpty(item.ImageName) ? "default.png" : item.ImageName);
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetSubCategoryForDataTable");
            }

            return Ok(response);
        }


        [HttpDelete, Route("api/SubCategory/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _subCategoryService.Delete(guid);
                response.Delete(item);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in Delete");
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SubCategory/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _subCategoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SubCategory/ImportFromExcel")]
        public async Task<IActionResult> ImportFromExcel([FromForm] IFormFile file, [FromForm] int categoryId)
        {
            ResponseMapper<bool> response = new();
            try
            {
                // Check if the user has permission to import subcategories
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                // Validate the file and category ID
                if (file == null || file.Length == 0)
                {
                    response.Message = "Please upload a valid Excel file.";
                    response.Success = false;
                    return Ok(response);
                }

                if (categoryId <= 0)
                {
                    response.Message = "Invalid category ID.";
                    response.Success = false;
                    return Ok(response);
                }

                bool result = await _subCategoryService.ImportSubCategoriesFromExcel(file, categoryId);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Subcategories imported successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to import subcategories.";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ImportFromExcel");
            }

            return Ok(response);
        }


        private void ClearCache()
        {
            _memoryCache.Remove("SubCategories");
        }

        private SubCategory BuildUrl(SubCategory item)
        {
            if (!string.IsNullOrEmpty(item.ImageName))
            {
                item.ImageUrl = GetImageUrl(AppSettings.ImageSettings.SubCategories, item.ImageName);
            }

            return item;
        }

        private void SaveImage(ref SubCategory item)
        {
            if (item.Image != null && item.Image.Length > 0)
            {
                string fileName = MediaHelper.ConvertImageToWebp(item.ImageName, item.Image, AppSettings.ImageSettings.SubCategories);
                if (!string.IsNullOrEmpty(fileName))
                    item.ImageName = fileName;
            }
        }
        #endregion
    }
}
