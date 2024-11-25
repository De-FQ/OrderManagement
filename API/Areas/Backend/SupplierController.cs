using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.InventoryManagement;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.Inventory;
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
    public class SupplierController : BaseController
    {
        private readonly ISupplierService<Supplier> _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            ISupplierService<Supplier> supplierService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.Suppliers)
        {
            base.ControllerName = typeof(SupplierController).Name;
            _supplierService = supplierService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region Supplier Management

        [HttpGet, Route("api/Supplier/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<Supplier> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Suppliers, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (guid.HasValue)
                {
                    var item = await _supplierService.GetByGuid(guid.Value);
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

        [HttpPost, Route("api/Supplier/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] Supplier item)
        {
            ResponseMapper<Supplier> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Suppliers, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (await _supplierService.Exists(item.Name, item.Guid))
                {
                    accessResponse.Message = item.Name + ResourceHelper.GetResource("supplier_name_already_taken", IsEnglish);
                    accessResponse.Success = false;
                    accessResponse.StatusCode = 300;
                    return Ok(accessResponse);
                }

                if (item.Guid.HasValue)
                {
                    item.ModifiedBy = this.UserId;
                    await _supplierService.Update(item);
                    response.Update(item);
                    response.Message = item.Name + ResourceHelper.GetResource("supplier_edited_successfully", IsEnglish);
                }
                else
                {
                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await _supplierService.Create(item);
                    response.Create(item);
                    response.Message = item.Name + ResourceHelper.GetResource("supplier_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/Supplier/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<Supplier> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Suppliers, AllowPermission.Active)) { return Ok(accessResponse); }

                var item = await _supplierService.ToggleActive(guid);
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

        [HttpPost, Route("api/Supplier/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<Supplier> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Suppliers, AllowPermission.DisplayOrder)) { return Ok(accessResponse); }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _supplierService.UpdateDisplayOrders(items);
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

        [HttpPost, Route("api/Supplier/GetSupplierForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetSupplierForDataTable()
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.Suppliers, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var items = await _supplierService.GetSupplierForDataTable(param: base.GetDataTableParameters);
                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/Supplier/Delete")]
        public async Task<IActionResult> Delete(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Suppliers, AllowPermission.Delete)) { return Ok(accessResponse); }

                var item = await _supplierService.Delete(guid);
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

        [HttpGet, Route("api/Supplier/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _supplierService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        private void ClearCache()
        {
            _memoryCache.Remove("Suppliers");
        }

        #endregion
    }
}
