using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.SiteCategory;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.ChildCategories;
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
    public class ChildCategoryController : BaseController
    {
        private readonly IChildCategoryService<ChildCategory> _childCategoryService;
        private readonly IMapper _mapper;

        public ChildCategoryController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IChildCategoryService<ChildCategory> childCategoryService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.ChildCategories)
        {
            base.ControllerName = typeof(ChildCategoryController).Name;
            _childCategoryService = childCategoryService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region ChildCategory Management

        [HttpGet, Route("api/ChildCategory/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<ChildCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _childCategoryService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/ChildCategory/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] ChildCategory item)
        {
            ResponseMapper<ChildCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (await _childCategoryService.Exists(item.Name, item.Guid))
                {
                    accessResponse.Message = item.Name + ResourceHelper.GetResource("Childcategory_name_already_taken", IsEnglish);
                    accessResponse.Success = false;
                    accessResponse.StatusCode = 300;
                    return Ok(accessResponse);
                }

                SaveImage(ref item);

                if ( item.DiscountActive && item.DiscountPercentage>0)
                {
                    await _childCategoryService.UpdateChildCategoryDiscount(item.Guid.Value, item.DiscountActive, item.DiscountPercentage);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _childCategoryService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("Childcategory_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _childCategoryService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("Childcategory_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/ChildCategory/UpdateChildCategoryDiscount")]
        public async Task<IActionResult> UpdateChildCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SubCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var success = await _childCategoryService.UpdateChildCategoryDiscount(guid, isDiscountActive, discountPercentage);

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

        [HttpPost, Route("api/ChildCategory/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<ChildCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _childCategoryService.ToggleActive(guid);
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

        [HttpPost, Route("api/ChildCategory/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<ChildCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.DisplayOrder))
                {
                    return Ok(accessResponse);
                }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _childCategoryService.UpdateDisplayOrders(items);
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

        [HttpPost, Route("api/ChildCategory/GetChildCategoryForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetChildCategoryForDataTable([FromForm] int? categoryId, [FromForm] int? subCategoryId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters; // Using as a method
                var items = await _childCategoryService.GetChildCategoryForDataTable(parameters, categoryId, subCategoryId);
                foreach (var item in items.Data)
                {
                    item.ImageUrl = GetBaseImageUrl(AppSettings.ImageSettings.ChildCategories) + (string.IsNullOrEmpty(item.ImageName) ? "default.png" : item.ImageName);
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetChildCategoryForDataTable");
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/ChildCategory/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _childCategoryService.Delete(guid);
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

        [HttpGet, Route("api/ChildCategory/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _childCategoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        [HttpGet, Route("api/ChildCategory/GetSubCategoriesByCategoryId")]
        public async Task<IActionResult> GetSubCategoriesByCategoryId(int categoryId)
        {
            ResponseMapper<List<SubCategory>> response = new();
            try
            {
                var items = await _childCategoryService.GetSubCategoriesByCategoryId(categoryId);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetSubCategoriesByCategoryId");
            }

            return Ok(response);
        }

        [HttpGet, Route("api/ChildCategory/GetChildCategoriesBySubCategoryId")]
        public async Task<IActionResult> GetChildCategoriesBySubCategoryId(int subCategoryId)
        {
            ResponseMapper<List<ChildCategory>> response = new();
            try
            {
                var items = await _childCategoryService.GetChildCategoriesBySubCategoryId(subCategoryId);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetChildCategoriesBySubCategoryId");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/ChildCategory/ImportFromExcel")]
        public async Task<IActionResult> ImportChildCategoriesFromExcel(IFormFile file, int categoryId, int subCategoryId)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.ChildCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var result = await _childCategoryService.ImportChildCategoriesFromExcel(file, categoryId, subCategoryId);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Child categories imported successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to import child categories.";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ImportChildCategoriesFromExcel");
            }

            return Ok(response);
        }

        private void ClearCache()
        {
            _memoryCache.Remove("ChildCategories");
        }

        private ChildCategory BuildUrl(ChildCategory item)
        {
            if (!string.IsNullOrEmpty(item.ImageName))
            {
                item.ImageUrl = GetImageUrl(AppSettings.ImageSettings.ChildCategories, item.ImageName);
            }

            return item;
        }

        private void SaveImage(ref ChildCategory item)
        {
            if (item.Image != null && item.Image.Length > 0)
            {
                string fileName = MediaHelper.ConvertImageToWebp(item.ImageName, item.Image, AppSettings.ImageSettings.ChildCategories);
                if (!string.IsNullOrEmpty(fileName))
                    item.ImageName = fileName;
            }
        }
        #endregion
    }
}
