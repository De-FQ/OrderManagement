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
    public class PriceTypeController : BaseController
    {
        private readonly IPriceTypeService<PriceType> _priceTypeService;
        private readonly IMapper _mapper;

        public PriceTypeController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IPriceTypeService<PriceType> priceTypeService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.PriceTypes)
        {
            base.ControllerName = typeof(PriceTypeController).Name;
            _priceTypeService = priceTypeService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region PriceType Management

        [HttpGet, Route("api/PriceType/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<PriceType> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _priceTypeService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/PriceType/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] PriceType item)
        {
            ResponseMapper<PriceType> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                // Removed the existence validation

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _priceTypeService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("PriceType_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _priceTypeService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("PriceType_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }


        [HttpPost, Route("api/PriceType/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<PriceType> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _priceTypeService.ToggleActive(guid);
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

        [HttpPost, Route("api/PriceType/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<PriceType> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.DisplayOrder))
                {
                    return Ok(accessResponse);
                }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _priceTypeService.UpdateDisplayOrders(items);
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

        [HttpPost, Route("api/PriceType/GetPriceTypeForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPriceTypeForDataTable([FromForm] int? priceTypeCategoryId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters;
                var items = await _priceTypeService.GetPriceTypeForDataTable(parameters, priceTypeCategoryId);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetPriceTypeForDataTable");
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/PriceType/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _priceTypeService.Delete(guid);
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

        [HttpGet, Route("api/PriceType/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _priceTypeService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/PriceType/ImportPriceTypeFromExcel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportPriceTypeFromExcel([FromForm] IFormFile file, [FromForm] int priceTypeCategoryId)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.PriceTypes, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (file == null || file.Length == 0)
                {
                    response.Message = "No file uploaded.";
                    response.Success = false;
                    return Ok(response);
                }

                var result = await _priceTypeService.ImportPriceTypeFromExcel(file, priceTypeCategoryId);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Price type imported successfully.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to import price type.";
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in ImportPriceTypeFromExcel");
            }

            return Ok(response);
        }

        private void ClearCache()
        {
            _memoryCache.Remove("PriceTypes");
        }
        #endregion
    }
}
