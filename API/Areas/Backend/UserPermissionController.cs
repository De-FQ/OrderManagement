using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Backend.UserManagement;
using UAParser;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.ResponseMapper;
using Utility.Models.Base;
using API.Helpers;
using Utility.Models.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using Data.UserManagement;

namespace API.Areas.Backend.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserPermissionController : BaseController
    {
        //private readonly IMapper _mapper;
        IUserPermissionService<UserPermission> _get;
        public UserPermissionController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IUserPermissionService<UserPermission> get
            //,  IMapper mapper 
            ) :
             base(options, apiHelper, systemUserService, PermissionTypes.UserPermissions)
        {
            base.ControllerName = typeof(UserPermissionController).Name;
            // _mapper = mapper;
            _get = get;
        }

        #region Permissions
        [HttpGet, Route("api/UserPermission/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            ResponseMapper<UserPermission> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserPermissions, AllowPermission.Edit)) { return Ok(accessResponse); }

                var items = await _get.GetByGuid(guid);
                response.GetById(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/UserPermission/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] UserPermission item)
        {
            ResponseMapper<UserPermission> response = new();
            try
            {

                if (item.Guid.HasValue)
                {

                    if (!await base.AccessPermission(PermissionTypes.UserPermissions, AllowPermission.Edit)) { return Ok(accessResponse); }

                    item.ModifiedBy = this.UserId;
                    await _get.Update(item);
                    response.Update(item);
                }
                else
                {

                    if (!await base.AccessPermission(PermissionTypes.UserPermissions, AllowPermission.Add)) { return Ok(accessResponse); }

                    item.CreatedBy = this.UserId;
                    await _get.Create(item);
                    response.Create(item);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/UserPermission/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<UserPermission> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserPermissions, AllowPermission.DisplayOrder)) { return Ok(accessResponse); }

                if (items.Count > 0)
                {
                    await _get.UpdateDisplayOrders(items);
                    response.DisplayOrder(true);
                }

            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/UserPermission/ToggleActive")]
        public async Task<IActionResult> ToggleActive(Guid guid)
        {
            ResponseMapper<User> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserPermissions, AllowPermission.Active)) { return Ok(accessResponse); }

                var item = await _get.ToggleActive(guid);
                response.ToggleActive(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }


        [HttpPost, Route("api/UserPermission/GetForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAllForDataTable()
        {
            ResponseMapper<dynamic> response = new();
            try
            {

                if (!await AccessPermission(PermissionTypes.UserPermissions, AllowPermission.List)) { return Ok(accessResponse); }

                var items = await _get.GetForDataTable(base.GetDataTableParameters);
                if (items.Data != null)
                {
                    foreach (var item in items.Data)
                    {
                        item.FormattedCreatedOn = await this.UserService.GetForDisplay(item.CreatedBy, item.CreatedOn, isEnglish: IsEnglish);
                        item.FormattedModifiedOn = await base.UserService.GetForDisplay(item.ModifiedBy, item.ModifiedOn, isEnglish: IsEnglish);
                    }
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

        [HttpGet, Route("api/UserPermission/GetAllPermission")]
        public async Task<IActionResult> GetAllPermission()
        {
            ResponseMapper<List<UserPermission>> response = new();
            try
            {
                if (!await Allowed())
                {
                    return Ok(accessResponse);
                }

                var item = await _get.GetAllPermissions();
                response.GetAll(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }


        [HttpDelete, Route("api/UserPermission/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserPermissions, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                var item = await _get.Delete(guid);
                response.Delete(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        #endregion


    }
}

