using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.InventoryManagement;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.InventoryManagement;
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
    public class InventoryItemController : BaseController
    {
        private readonly IInventoryItemService<InventoryItem> _inventoryItemService;
        private readonly IMapper _mapper;

        public InventoryItemController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IInventoryItemService<InventoryItem> inventoryItemService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.InventoryItems)
        {
            base.ControllerName = typeof(InventoryItemController).Name;
            _inventoryItemService = inventoryItemService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region InventoryItem Management

        [HttpGet, Route("api/InventoryItem/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<InventoryItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _inventoryItemService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/InventoryItem/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] InventoryItem item)
        {
            ResponseMapper<InventoryItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _inventoryItemService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("InventoryItem_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _inventoryItemService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("InventoryItem_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/InventoryItem/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<InventoryItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _inventoryItemService.ToggleActive(guid);
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

        [HttpPost, Route("api/InventoryItem/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid guid)
        {
            ResponseMapper<InventoryItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var result = await _inventoryItemService.Delete(guid);
                response.Delete(result);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in DeleteAsync");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/InventoryItem/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromBody] List<BaseRowOrder> rowOrders)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var result = await _inventoryItemService.UpdateDisplayOrders(rowOrders);
                response.Update(result);
                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in UpdateDisplayOrders");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/InventoryItem/GetInventoryItemsForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetInventoryItemsForDataTable([FromForm] int? supplierId, [FromForm] DateTime? date)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters;
                var items = await _inventoryItemService.GetInventoryItemsForDataTable(parameters, supplierId, date);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetInventoryItemsForDataTable");
            }

            return Ok(response);
        }




        [HttpGet, Route("api/InventoryItem/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList(bool isEnglish = true)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var result = await _inventoryItemService.GetAllForDropDownList(isEnglish);
                response.GetById(result);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        //[HttpPost, Route("api/SupplierBalance/CalculateAndSaveBalance")]
        //public async Task<IActionResult> CalculateAndSaveBalance(int supplierId, decimal paidAmount)
        //{
        //    ResponseMapper<SupplierBalance> response = new();
        //    try
        //    {
        //        if (!await base.AccessPermission(PermissionTypes.InventoryItems, AllowPermission.Edit))
        //        {
        //            return Ok(accessResponse);
        //        }

        //        var supplierBalance = await _inventoryItemService.CalculateAndSaveBalance(supplierId, paidAmount);
        //        response.GetById(supplierBalance);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        Log.Error(ex, "Error in CalculateAndSaveBalance");
        //    }

        //    return Ok(response);
        //}

        #endregion

        private void ClearCache()
        {
            _memoryCache.Remove("InventoryItems");
        }
    }
}
