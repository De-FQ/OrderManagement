using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Base;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Services.Base;
using Serilog;
using Utility.Models.Admin.DTO;
using Data.UserManagement;

namespace Services.Backend.UserManagement
{
    public class UserPermissionService : BaseService, IUserPermissionService<UserPermission> // IUserRoleService :
    {
        private readonly IMapper _mapper;
        public UserPermissionService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        #region Permissions & LoggedIn Menu Permissions

        public async Task<List<UserPermission>> GetMenuByRoleId(long roleId, bool isEnglish, long? userId = null)
        {
            var permissions = await (from role in _dbcontext.UserRolePermissions
                                     join permission in _dbcontext.UserPermissions
                                     on role.UserPermissionId equals permission.Id
                                     where role.UserRoleId == roleId
                                     && role.Allowed == true
                                     // && permission.Active == true
                                     && permission.ShowInMenu == true
                                     && permission.Active == true && permission.Deleted == false

                                     select new UserPermission
                                     {
                                         Id = permission.Id,
                                         Title = permission.Title,
                                         NavigationUrl = permission.NavigationUrl,
                                         Icon = permission.Icon,
                                         Active = permission.Active,
                                         UserPermissionId = permission.UserPermissionId,
                                         DisplayOrder = permission.DisplayOrder,


                                     }).OrderBy(x => x.DisplayOrder)
                                        .AsNoTracking()
                                        .ToListAsync();

            if (userId.HasValue)
            {
                var systemUser = await _dbcontext.Users.Include(a => a.Roles).Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (systemUser != null)
                {
                    foreach (var permission in permissions)
                    {

                    }
                }
            }

            //permissions = permissions.Where(a => a.Active).ToList();

            var menuItems = new UserPermission().GetMenuTree(permissions, null);

            return menuItems;
        }

        public async Task<bool> Exists(string name, Guid? guid = null)
        {

            var result = await _dbcontext.UserPermissions
                .Select(x => new { x.Guid, x.Title })
                .Where(a => a.Title.ToLower() == name.ToLower())
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (result != null)
            {
                if (guid.HasValue && result.Guid == guid)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        public async Task<UserPermission> GetByGuid(Guid guid)
        {
            var item = await _dbcontext
                          .UserPermissions
                          .Where(x => x.Deleted == false && x.Guid == guid)
                          .AsNoTracking()
                          .FirstOrDefaultAsync();
            if (item is null) { return new UserPermission(); } else { return item; }
        }
        public async Task<UserPermission> GetById(long id)
        {
            var item = await _dbcontext
                          .UserPermissions
                          .Where(x => x.Deleted == false && x.Id == id)
                          .AsNoTracking()
                          .FirstOrDefaultAsync();
            if (item is null) { return new UserPermission(); } else { return item; }
        }
        public async Task<List<UserPermission>> GetAllPermissions()
        {
            var data = await _dbcontext
                          .UserPermissions
                          .Where(x => x.Deleted == false)
                          .AsNoTracking()
                          .ToListAsync();
            return data;
        }
        public async Task<UserPermission> Create(UserPermission model)
        {
            model.Guid = Guid.NewGuid();
            model.DisplayOrder = await GetNextDisplayOrder();
            model.CreatedOn = DateTime.Now;
            await _dbcontext.UserPermissions.AddAsync(model);
            await _dbcontext.SaveChangesAsync();
            return model;
        }
        public async Task<bool> Update(UserPermission model)
        {
            var data = await _dbcontext
                          .UserPermissions
                          .Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.Title = model.Title;
                data.UserPermissionId = model.UserPermissionId;
                data.NavigationUrl = model.NavigationUrl;
                data.Icon = model.Icon;
                data.AccessList = model.AccessList;
                data.DisplayOrder = model.DisplayOrder;
                data.ShowInMenu = model.ShowInMenu;
                data.ModifiedBy = model.ModifiedBy;
                data.ModifiedOn = DateTime.Now;
                await UpdatePermission(model);
                return await _dbcontext.SaveChangesAsync() > 0;
            }
            return false;
        }
        /// <summary>
        /// update Users Permission in all roles
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task UpdatePermission(UserPermission model)
        {
            var items = await _dbcontext.UserRolePermissions
                            .Where(x => x.UserPermissionId == model.Id)
                            .ToListAsync();

            //var allowedPermssion = model.AccessList.Split(",");
            foreach (var item in items)
            {
                item.Allowed = model.AccessList.Contains("Allowed") ? true : false;
                item.AllowList = model.AccessList.Contains("List") ? true : false;
                item.AllowDisplayOrder = model.AccessList.Contains("DisplayOrder") ? true : false;
                item.AllowActive = model.AccessList.Contains("Active") ? true : false;
                item.AllowAdd = model.AccessList.Contains("Add") ? true : false;
                item.AllowEdit = model.AccessList.Contains("Edit") ? true : false;
                item.AllowDelete = model.AccessList.Contains("Delete") ? true : false;
                item.AllowView = model.AccessList.Contains("View") ? true : false;
            }
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.UserPermissions.Where(x => x.Guid == guid).FirstOrDefaultAsync();

                if (data is not null)
                {
                    // it will trigger softDelete operation, when SaveChangesAsync() is called
                    _dbcontext.Remove(data);
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserRUserPermissionsole-->Delete action Failed Reason: " + ex.Message.ToString());
            }
            return false;
        }
        //public async Task<bool> Delete(Guid guid)
        //{
        //    var data = await _dbcontext.UserPermissions.Where(x => x.Guid == guid).FirstOrDefaultAsync();
        //    if (data is not null)
        //    {
        //        data.Deleted = true;
        //        //data.Active = !data.Active;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }

        //    return false;
        //}
        //public async Task<bool> Delete(UserPermission model)
        //{
        //    var data = await _dbcontext
        //                 .UserPermissions
        //                 .Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();
        //    if (data is not null)
        //    {
        //        data.Deleted = true;
        //        data.ModifiedOn = DateTime.Now;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}

        //public async Task<bool> ToggleActive(long id)
        //{
        //    var data = await _dbcontext.UserPermissions.FindAsync(id);
        //    if (data is not null)
        //    {
        //        data.ModifiedOn = DateTime.Now;
        //        data.Active = !data.Active;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}
        public async Task<bool> ToggleActive(Guid guid)
        {
            var data = await _dbcontext.UserPermissions.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.ModifiedOn = DateTime.Now;
                data.Active = !data.Active;
                return await _dbcontext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            bool modified = false;
            var orderList = rowOrders.Select(x => x.Id).ToList();

            var items = await _dbcontext.UserPermissions.Where(x => orderList.Contains(x.Id)).ToListAsync();
            foreach (var item in items)
            {
                var row = rowOrders.Where(p => p.Id == item.Id).FirstOrDefault();
                if (row is not null)
                {
                    modified = true;
                    item.ModifiedBy = row.UserId;
                    item.ModifiedOn = DateTime.Now;
                    item.DisplayOrder = (int)row.DisplayOrder;
                }


            }
            if (modified)
            {
                return await _dbcontext.SaveChangesAsync() > 0;
            }
            return false;
        }


        public async Task<bool> UpdateDisplayOrder(Guid guid, int num = 0)
        {
            var data = await _dbcontext.UserPermissions.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.DisplayOrder = num;
                data.ModifiedOn = DateTime.Now;
                return await _dbcontext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<DataTableResult<List<UserPermissionDto>>> GetForDataTable(DataTableParam param)
        {
            DataTableResult<List<UserPermissionDto>> result = new() { Draw = param.Draw };
            try
            {
                var items = _dbcontext.UserPermissions
                         .Select(x => new UserPermissionDto
                         {
                             Id = x.Id,
                             Guid = x.Guid,
                             DisplayOrder = x.DisplayOrder,
                             Title = x.Title,
                             NavigationUrl = x.NavigationUrl,
                             Icon = x.Icon,
                             ParentPermissionId = x.UserPermissionId,
                             CreatedBy = x.CreatedBy,
                             CreatedOn = x.CreatedOn,
                             ModifiedBy = x.ModifiedBy,
                             ModifiedOn = x.ModifiedOn,
                             ShowInMenu = x.ShowInMenu,
                             Active = x.Active,
                             Deleted = x.Deleted
                         }).Where(x => x.Deleted == false);

                //User Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var SearchValue = param.SearchValue.ToLower();
                    items = items.Where(obj =>
                     obj.Title.ToLower().Contains(SearchValue) ||
                     obj.NavigationUrl.ToLower().Contains(SearchValue) ||
                     obj.DisplayOrder.ToString().Contains(SearchValue)
                     );
                }
                else { items = items.OrderBy(x => x.DisplayOrder); }

                //Sorting
                if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
                {
                    //using System.Linq.Dynamic.Core;
                    //NEEDS TO BE INSTALLED FROM NUGET PACKAGE MANAGER
                    items = items.OrderBy(param.SortColumn + " " + param.SortColumnDirection);//.ToList();
                }
                else
                {
                    items = items.OrderBy(x => x.DisplayOrder); //default

                }

                result.RecordsTotal = items.Count();
                result.RecordsFiltered = items.Count();
                result.Data = await items.Skip(param.Skip).Take(param.PageSize).ToListAsync();

                return result;
            }
            catch (Exception err)
            {
                result.Error = err;
            }

            return result;
        }
        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            var items = await _dbcontext.UserPermissions
                .Where(x => x.Deleted == false)
                .Select(x => new { x.Id, Name =  x.Title })
                .AsNoTracking()
                .ToListAsync();

            return items;
        }
        public async Task<long> GetNextDisplayOrder()
        {
            var item = await _dbcontext.UserPermissions.OrderByDescending(x => x.DisplayOrder).AsNoTracking().FirstOrDefaultAsync();
            if (item is not null)
            {
                return item.DisplayOrder + 1;
            }
            return 1;

        }
        #endregion

        #region not in used
        //public async Task<bool> UpdateDisplayOrder(long id, int num = 0)
        //{
        //    var data = await _dbcontext.UserPermissions.FindAsync(id);
        //    if (data is not null)
        //    {
        //        data.DisplayOrder = num;
        //        data.ModifiedOn = DateTime.Now;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}
        #endregion

    }
}
