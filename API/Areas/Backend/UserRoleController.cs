using AutoMapper;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Backend.UserManagement;
using Utility.API;
using Utility.Enum;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace API.Areas.Backend.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserRoleController : BaseController
    {
        //private readonly IMapper _mapper;
        public readonly IUserRoleService<UserRole> _get;
        public UserRoleController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            // ILoggerFactory logger,
            IMapper mapper,
            IUserRoleService<UserRole> get) :
             base(options, apiHelper, systemUserService, PermissionTypes.UserRoles)
        {
            base.ControllerName = typeof(UserRoleController).Name;
            // _mapper = mapper; 
            _get = get;
        }


        #region System User Role

        [HttpGet, Route("api/UserRole/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<UserRole> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (guid.HasValue)
                {
                    var item = await _get.GetByGuid(guid.Value);
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


        [HttpPost, Route("api/UserRole/AddEdit")]
        public async Task<IActionResult> AddEditAsync([FromForm] UserRole item)
        {
            ResponseMapper<UserRole> response = new();
            try
            {


                if (item.Guid.HasValue)
                {
                    if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.Edit)) { return Ok(accessResponse); }


                    item.ModifiedBy = this.UserId;
                    await _get.Update(item);
                    response.Update(item);
                }
                else
                {
                    if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.Add)) { return Ok(accessResponse); }

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

        [HttpPost, Route("api/UserRole/ToggleActive")]
        public async Task<IActionResult> ToggleActiveRoleAsync(Guid guid)
        {
            ResponseMapper<User> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.Active)) { return Ok(accessResponse); }


                var item = await _get.ToggleActive(guid);
                // var item1 = await _get.Delete(guid);
                response.ToggleActive(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpPost, Route("api/UserRole/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<User> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.DisplayOrder)) { return Ok(accessResponse); }


                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
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
        [HttpPost, Route("api/UserRole/GetForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAllForDataTable()
        {
            ResponseMapper<dynamic> response = new();
            try
            {

                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.List)) { return Ok(accessResponse); }


                var items = await _get.GetForDataTable(param: base.GetDataTableParameters, roleTypeId: this.RoleTypeId);

                if (items.Data != null)
                {
                    foreach (var item in items.Data)
                    {
                        item.Name = item.Name;
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



        //[HttpGet, Route("api/UserRole/GetViewPermission")]
        //public async Task<IActionResult> GetViewPermission(string navigationUrl)
        //{
        //    ResponseMapper<UserRolePermission> response = new();
        //    try
        //    {
        //        if (!await Allowed())
        //        {
        //            return Ok(accessResponse);
        //        }

        //        var item = await _get.GetViewPermission(this.RoleId, navigationUrl);
        //        response.GetAll(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.CacheException(ex);
        //        LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
        //    }

        //    return Ok(response);
        //}

        [HttpGet, Route("api/UserRole/GetAllPermissionByRoleId")]
        public async Task<IActionResult> ByRoleId(long id)
        {
            ResponseMapper<List<UserRolePermission>> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.List)) { return Ok(accessResponse); }


                var item = await _get.GetAllPermissionByRoleId(id);
                response.GetAll(item);

            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/UserRole/GetAllPermission")]
        public async Task<IActionResult> GetAllPermission()
        {
            ResponseMapper<List<UserPermission>> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.List)) { return Ok(accessResponse); }


                var item = await _get.GetPermissions();
                response.GetAll(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpPost, Route("api/UserRole/UpdatePermission")]
        public async Task<IActionResult> Update([FromForm] UserRole updatePermission)
        {
            ResponseMapper<List<UserRolePermission>> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.Edit)) { return Ok(accessResponse); }


                var item = await _get.UpdateRolePermission(updatePermission);
                response.Update(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpDelete, Route("api/UserRole/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid guid)
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.UserRoles, AllowPermission.Delete))
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

