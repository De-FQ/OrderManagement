using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.General;
using Data.Model.InventoryManagement;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.InventoryManagement.SupplierItems;
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
    public class SupplierItemController : BaseController
    {
        private readonly ISupplierItemService<SupplierItem> _supplierItemService;
        private readonly IMapper _mapper;

        public SupplierItemController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            ISupplierItemService<SupplierItem> supplierItemService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.SupplierItems)
        {
            base.ControllerName = typeof(SupplierItemController).Name;
            _supplierItemService = supplierItemService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region SupplierItem Management

        [HttpGet, Route("api/SupplierItem/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<SupplierItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _supplierItemService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/SupplierItem/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] SupplierItem item)
        {
            ResponseMapper<SupplierItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                // Removed the existence validation

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _supplierItemService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("SupplierItem_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _supplierItemService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("SupplierItem_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }


        [HttpPost, Route("api/SupplierItem/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<SupplierItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _supplierItemService.ToggleActive(guid);
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

        [HttpPost, Route("api/SupplierItem/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<SupplierItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.DisplayOrder))
                {
                    return Ok(accessResponse);
                }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _supplierItemService.UpdateDisplayOrders(items);
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

        [HttpPost, Route("api/SupplierItem/GetSupplierItemForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetSupplierItemForDataTable([FromForm] int? supplierId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters;
                var items = await _supplierItemService.GetSupplierItemForDataTable(parameters, supplierId);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetSupplierItemForDataTable");
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/SupplierItem/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _supplierItemService.Delete(guid);
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

        [HttpGet, Route("api/SupplierItem/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _supplierItemService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SupplierItem/ImportSupplierItemsFromExcel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportSupplierItemsFromExcel([FromForm] IFormFile file, [FromForm] int supplierId)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SupplierItems, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (file == null || file.Length == 0)
                {
                    response.Message = "No file uploaded.";
                    response.Success = false;
                    return Ok(response);
                }

                var result = await _supplierItemService.ImportSupplierItemsFromExcel(file, supplierId);
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
                Log.Error(ex, "Error in ImportSupplierItemsFromExcel");
            }

            return Ok(response);
        }


        private void ClearCache()
        {
            _memoryCache.Remove("SupplierItems");
        }
        #endregion
    }
}
