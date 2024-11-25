using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.InventoryManagement;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.InventoryManagement.Stocks;
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
    public class StockController : BaseController
    {
        private readonly IStockService<Stock> _stockService;
        private readonly IMapper _mapper;

        public StockController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IStockService<Stock> stockService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.Stocks)
        {
            base.ControllerName = typeof(StockController).Name;
            _stockService = stockService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region Stock Management

        [HttpGet, Route("api/Stock/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<Stock> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Stocks, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (guid.HasValue)
                {
                    var item = await _stockService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/Stock/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] Stock item)
        {
            ResponseMapper<Stock> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Stocks, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _stockService.Update(item);
                    response.Update(item);
                    response.Message = "Stock " + ResourceHelper.GetResource("edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _stockService.Create(item);
                    response.Create(item);
                    response.Message = "Stock " + ResourceHelper.GetResource("created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in AddEdit");
            }

            return Ok(response);
        }

        [HttpPost, Route("api/Stock/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<Stock> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Stocks, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }

                var item = await _stockService.ToggleActive(guid);
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

        [HttpPost, Route("api/Stock/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid guid)
        {
            ResponseMapper<Stock> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Stocks, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var result = await _stockService.Delete(guid);
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

        [HttpPost, Route("api/Stock/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromBody] List<BaseRowOrder> rowOrders)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Stocks, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var result = await _stockService.UpdateDisplayOrders(rowOrders);
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

        [HttpPost, Route("api/Stock/GetStocksForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetStocksForDataTable([FromForm] int? inventoryItemId)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.Stocks, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var parameters = base.GetDataTableParameters;
                var items = await _stockService.GetStocksForDataTable(parameters, inventoryItemId);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetStocksForDataTable");
            }

            return Ok(response);
        }

        [HttpGet, Route("api/Stock/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList(bool isEnglish = true)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var result = await _stockService.GetAllForDropDownList(isEnglish);
                response.GetById(result);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                Log.Error(ex, "Error in GetAllForDropDownList");
            }

            return Ok(response);
        }

        #endregion

        private void ClearCache()
        {
            _memoryCache.Remove("Stocks");
        }
    }
}
