using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.General;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.Price;
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
    public class PriceTypeCategoryController : BaseController
    {
        private readonly IPriceTypeCategoryService<PriceTypeCategory> _priceTypeCategoryService;
        private readonly IMapper _mapper;

        public PriceTypeCategoryController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IPriceTypeCategoryService<PriceTypeCategory> priceTypeCategoryService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.PriceTypeCategories)
        {
            base.ControllerName = typeof(PriceTypeCategoryController).Name;
            _priceTypeCategoryService = priceTypeCategoryService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region PriceTypeCategory Management

        [HttpGet, Route("api/PriceTypeCategory/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<PriceTypeCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _priceTypeCategoryService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/PriceTypeCategory/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] PriceTypeCategory item)
        {
            ResponseMapper<PriceTypeCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                // Removed the existence validation

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _priceTypeCategoryService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("PriceTypeCategory_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _priceTypeCategoryService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("PriceTypeCategory_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }


        [HttpPost, Route("api/PriceTypeCategory/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<PriceTypeCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _priceTypeCategoryService.ToggleActive(guid);
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

        [HttpPost, Route("api/PriceTypeCategory/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<PriceTypeCategory> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.DisplayOrder))
                {
                    return Ok(accessResponse);
                }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _priceTypeCategoryService.UpdateDisplayOrders(items);
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

        [HttpPost, Route("api/PriceTypeCategory/GetPriceTypeCategoryForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPriceTypeCategoryForDataTable([FromForm] int? childCategoryId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters; 
                var items = await _priceTypeCategoryService.GetPriceTypeCategoryForDataTable(parameters, childCategoryId);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetPriceTypeCategoryForDataTable");
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/PriceTypeCategory/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _priceTypeCategoryService.Delete(guid);
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

        [HttpGet, Route("api/PriceTypeCategory/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _priceTypeCategoryService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/PriceTypeCategory/ImportPriceTypeCategoriesFromExcel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportPriceTypeCategoriesFromExcel([FromForm] IFormFile file, [FromForm] int childCategoryId)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypeCategories, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (file == null || file.Length == 0)
                {
                    response.Message = "No file uploaded.";
                    response.Success = false;
                    return Ok(response);
                }

                var result = await _priceTypeCategoryService.ImportPriceTypeCategoriesFromExcel(file, childCategoryId);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Price type categories imported successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to import price type categories.";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ImportPriceTypeCategoriesFromExcel");
            }

            return Ok(response);
        }


        private void ClearCache()
        {
            _memoryCache.Remove("PriceTypeCategories");
        }
        #endregion
    }
}
