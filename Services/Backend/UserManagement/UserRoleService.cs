using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Utility.API;
using Utility.Enum;
using System.Linq.Dynamic.Core;
using Utility.Models.Base;
using Services.Base;
using AutoMapper;
using Serilog;
using Utility.Models.Admin.DTO;
using Data.UserManagement;

namespace Services.Backend.UserManagement
{

    public class UserRoleService : BaseService, IUserRoleService<UserRole> // IUserRoleService :
    {
        private readonly IMapper _mapper;
        public UserRoleService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        //protected readonly ApplicationDbContext _dbcontext;
        //protected string ErrorMessage = string.Empty;
        //public UserRoleService(ApplicationDbContext dbcontext)
        //{
        //    _dbcontext = dbcontext;
        //}


        #region Roles 

        public async Task<bool> Exists(string name, Guid? guid = null)
        {

            var result = await _dbcontext.UserRoles
                .Select(x => new { x.Guid, x.Name })
                .Where(a => a.Name.ToLower() == name.ToLower())
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

        public async Task<UserRole> GetByGuid(Guid guid)
        {
            var item = await _dbcontext.UserRoles.Where(x => x.Guid == guid).AsNoTracking().FirstOrDefaultAsync();
            if (item is null) { return new UserRole(); } else { return item; }
        }
        public async Task<UserRole> GetById(long id)
        {
            var item = await _dbcontext.UserRoles.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (item is null) { return new UserRole(); } else { return item; }
        }

        public async Task<UserRole> Create(UserRole model)
        {
            try
            {
                model.Guid = Guid.NewGuid();
                model.CreatedOn = DateTime.Now;
                model.DisplayOrder = await GetNextDisplayOrder();
                await _dbcontext.UserRoles.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return model;
        }
        public async Task<bool> Update(UserRole model)
        {
            try
            {
                var data = await _dbcontext.UserRoles
                .Where(x => x.Guid == model.Guid)
                .FirstOrDefaultAsync();
                if (data is not null)
                {
                    data.Name = model.Name;
                    data.UserRoleTypeId = model.UserRoleTypeId;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    return await _dbcontext.SaveChangesAsync() > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return false;
        }

        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            bool modified = false;
            var orderList = rowOrders.Select(x => x.Id).ToList();

            var items = await _dbcontext.UserRoles.Where(x => orderList.Contains(x.Id)).ToListAsync();
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
            var data = await _dbcontext.UserRoles.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.DisplayOrder = num;
                data.ModifiedOn = DateTime.Now;
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }
        //public async Task<bool> Delete(UserRole model)
        //{
        //    var data = await _dbcontext.UserRoles.Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();
        //    if (data is not null)
        //    {
        //        data.Deleted = true;
        //        data.ModifiedBy = model.ModifiedBy;
        //        data.ModifiedOn = DateTime.Now;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }
        //    return false;

        //}
        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.UserRoles.Where(x => x.Guid == guid).FirstOrDefaultAsync();

                if (data is not null)
                {
                    // it will trigger softDelete operation, when SaveChangesAsync() is called
                    _dbcontext.Remove(data);
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserRole-->Delete action Failed Reason: " + ex.Message.ToString());
            }
            return false;
        }
        public async Task<bool> ToggleActive(Guid guid)
        {
            var data = await _dbcontext.UserRoles
                            .Where(x => x.Guid == guid)
                            .FirstOrDefaultAsync();
            if (data is not null)
            {
                data.ModifiedOn = DateTime.Now;
                data.Active = !data.Active;
                return await _dbcontext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<DataTableResult<List<UserRoleDto>>> GetForDataTable(DataTableParam param, long id = 0, long roleTypeId = 0)
        {
            DataTableResult<List<UserRoleDto>> result = new() { Draw = param.Draw };
            try
            {
                ///.Where(x => x.Deleted == false)
                var items = _dbcontext.UserRoles.AsNoTracking().AsQueryable();

                if (id > 0)
                {
                    items = items.Where(x => x.Id == id);
                }

                if (roleTypeId > 0)
                {
                    items = items.Where(x => x.UserRoleTypeId >= roleTypeId);
                }

                var records = items.Select(x => new UserRoleDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    Name = x.Name,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    Active = x.Active,
                    Deleted = x.Deleted,

                });



                //User Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var SearchValue = param.SearchValue.ToLower();
                    records = records.Where(obj => obj.Name.ToLower().Contains(SearchValue));
                }

                //Sorting
                if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
                {
                    //using System.Linq.Dynamic.Core;
                    //NEEDS TO BE INSTALLED FROM NUGET PACKAGE MANAGER
                    records = records.OrderBy(param.SortColumn + " " + param.SortColumnDirection);//.ToList();
                }
                else
                {
                    records = records.OrderBy(x => x.DisplayOrder);
                }

                result.RecordsTotal = await records.CountAsync();
                result.RecordsFiltered = await records.CountAsync();
                result.Data = await records.Skip(param.Skip).Take(param.PageSize).ToListAsync();

                return result;
            }
            catch (Exception err)
            {
                result.Error = err;
            }
            return result;
        }

        public async Task<List<UserRolePermission>> GetAllPermissionByRoleId(long roleId)
        {
            var data = _dbcontext
                          .UserRolePermissions
                          .Include(x => x.Role)
                          .Where(x => x.UserRoleId == roleId)
                          .AsNoTracking();

            data = data.Select(x => new UserRolePermission
            {
                Id = x.Id,
                UserPermissionId = x.UserPermissionId,
                UserRoleId = x.UserRoleId,
                Allowed = x.Allowed,
                AllowActive = x.AllowActive,
                AllowList = x.AllowList,
                AllowDisplayOrder = x.AllowDisplayOrder,
                AllowView = x.AllowView,
                AllowAdd = x.AllowAdd,
                AllowEdit = x.AllowEdit,
                AllowDelete = x.AllowDelete

                //Id = x.Id,
                //PermissionId = x.PermissionId,
                //RoleId = x.RoleId,
                //AllowAdd = x.AllowAdd,
                //AllowEdit = x.AllowEdit,
                //// AllowViewNoUsed = x.AllowViewNoUsed,
                //AllowActive = x.AllowActive,
                //AllowView = x.AllowView
            });

            return await data.ToListAsync();
        }
        public async Task<List<UserRolePermission>> UpdateRolePermission(UserRole Role)
        {
            var data = await _dbcontext
                            .UserRolePermissions
                            .Where(x => x.UserRoleId == Role.Id)
                            .ToListAsync();
            if (Role.RolePermissions is null) { return data; }
            foreach (var permission in Role.RolePermissions)
            {
                var find = data.Where(x => x.UserPermissionId == permission.Id).FirstOrDefault();
                if (find is not null)
                {
                    find.Allowed = permission.Allowed;
                    find.AllowActive = permission.AllowActive;
                    find.AllowList = permission.AllowList;
                    find.AllowDisplayOrder = permission.AllowDisplayOrder;
                    find.AllowView = permission.AllowView;
                    find.AllowAdd = permission.AllowAdd;
                    find.AllowEdit = permission.AllowEdit;
                    find.AllowDelete = permission.AllowDelete;

                    find.ModifiedOn = DateTime.Now;


                }
                else
                {
                    var newItem = new UserRolePermission
                    {
                        Allowed = permission.Allowed,
                        AllowActive = permission.AllowActive,
                        AllowList = permission.AllowList,
                        AllowDisplayOrder = permission.AllowDisplayOrder,
                        AllowView = permission.AllowView,
                        AllowAdd = permission.AllowAdd,
                        AllowEdit = permission.AllowEdit,
                        AllowDelete = permission.AllowDelete,
                        UserPermissionId = permission.Id,
                        UserRoleId = Role.Id,
                        CreatedOn = DateTime.Now,
                        Deleted = false
                    };
                    await _dbcontext.AddAsync(newItem);
                }
            }

            await _dbcontext.SaveChangesAsync();
            return data;
        }


        public async Task<List<UserPermission>> GetPermissions()
        {
            var data = await _dbcontext
                          .UserPermissions
                          .AsNoTracking()
                          .ToListAsync();
            return data;
        }
        /// <summary>
        /// Admin View Permission (Add,Edit,Delete,View)
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="navigationUrl"></param>
        /// <returns></returns>
        //public async Task<UserRolePermission?> GetViewAccessPermissionBy(long roleId, long permissionId)
        //{

        //    return await (from role in _dbcontext.UserRolePermissions
        //                      join permission in _dbcontext.UserPermissions
        //                      on role.PermissionId equals permission.Id
        //                      where role.RoleId == roleId
        //                      && permission.Id == permissionId
        //                      && role.AllowView == true
        //                      && permission.Active == true
        //                      && permission.ShowInMenu == true
        //                      select new UserRolePermission
        //                      {
        //                          Id = role.Id,
        //                          PermissionId = role.PermissionId,
        //                          AllowView = role.AllowView,
        //                          AllowAdd = role.AllowAdd,
        //                          AllowEdit = role.AllowEdit,
        //                          AllowActive = role.AllowActive,
        //                         // AllowViewNoUsed = role.AllowViewNoUsed
        //                      })
        //                      .AsNoTracking()
        //                      .FirstOrDefaultAsync();



        //    //return data;
        //}



        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            var items = await _dbcontext.UserRoles.Where(x => x.Deleted == false).Select(x => new
            {
                x.Id,
                Name = isEnglish ? x.Name : x.Name
            }).AsNoTracking().ToListAsync();

            return items;
        }

        public async Task<long> GetNextDisplayOrder()
        {
            var item = await _dbcontext.UserRoles.OrderByDescending(x => x.DisplayOrder).AsNoTracking().FirstOrDefaultAsync();
            if (item is not null)
            {
                return item.DisplayOrder + 1;
            }
            return 1;

        }

        #endregion

        #region need to verify, methods are used

        //public async Task Create(List<UserRole> models)
        //{
        //    try
        //    {
        //        await _dbcontext.UserRoles.AddRangeAsync(models);
        //        await _dbcontext.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message;
        //    }
        //}

        //public async Task<int> GetCount(long id)
        //{
        //    return await _dbcontext.Users
        //        .Where(x => !x.Deleted && x.RoleId == id)
        //        .AsNoTracking()
        //        .CountAsync();
        //}

        //public async Task<UserRole?> GetById(long id)
        //{
        //    var data = await _dbcontext.UserRoles
        //        .Where(x => x.Id == id)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync();
        //    return data;
        //}
        //public async Task<DataTableResult<List<UserPermissionDto>>> GetPermissionsForDataTable(DataTableParam param)
        //{
        //    DataTableResult<List<UserPermissionDto>> result = new() { Draw = param.Draw };
        //    try
        //    {
        //        var items = _dbcontext.UserPermissions
        //                 .Select(x => new UserPermissionDto
        //                 {
        //                     Id = x.Id,
        //                     Guid = x.Guid,
        //                     DisplayOrder = x.DisplayOrder,
        //                     Title = x.Title,
        //                     TitleAr = x.TitleAr,
        //                     NavigationUrl = x.NavigationUrl,
        //                     Icon = x.Icon,
        //                     ParentPermissionId = x.ParentPermissionId,
        //                     CreatedBy = x.CreatedBy,
        //                     CreatedOn = x.CreatedOn,
        //                     ModifiedBy = x.ModifiedBy,
        //                     ModifiedOn = x.ModifiedOn,
        //                     ShowInMenu = x.ShowInMenu,
        //                     Active = x.Active,
        //                     Deleted = x.Deleted
        //                 }).Where(x => x.Deleted == false);

        //        //User Search
        //        if (!string.IsNullOrEmpty(param.SearchValue))
        //        {
        //            var SearchValue = param.SearchValue.ToLower();
        //            items = items.Where(obj =>
        //             obj.Title.ToLower().Contains(SearchValue) ||
        //             obj.NavigationUrl.ToLower().Contains(SearchValue) ||
        //             obj.DisplayOrder.ToString().Contains(SearchValue)
        //             );
        //        }

        //        //Sorting
        //        if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
        //        {
        //            //using System.Linq.Dynamic.Core;
        //            //NEEDS TO BE INSTALLED FROM NUGET PACKAGE MANAGER
        //            items = items.OrderBy(param.SortColumn + " " + param.SortColumnDirection);//.ToList();
        //        }
        //        else
        //        {
        //            items = items.OrderBy(x => x.DisplayOrder); //default

        //        }

        //        result.RecordsTotal = items.Count();
        //        result.RecordsFiltered = items.Count();
        //        result.Data = await items.Skip(param.Skip).Take(param.PageSize).ToListAsync();

        //        return result;
        //    }
        //    catch (Exception err)
        //    {
        //        result.Error = err;
        //    }

        //    return result;
        //}


        /// <summary>
        /// Admin View Permission (Add,Edit,Delete,View)
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="navigationUrl"></param>
        /// <returns></returns>
        //public async Task<UserRolePermission> GetViewPermission(long roleId, string navigationUrl)
        //{

        //    var data = await (from role in _dbcontext.UserRolePermissions
        //                      join permission in _dbcontext.UserPermissions
        //                      on role.PermissionId equals permission.Id
        //                      where role.RoleId == roleId
        //                      && permission.NavigationUrl == navigationUrl
        //                      && role.Allowed == true
        //                      && permission.Active == true
        //                      && permission.ShowInMenu == true
        //                      select new UserRolePermission
        //                      {
        //                          Id = role.Id,
        //                          PermissionId = role.PermissionId,
        //                          Allowed = role.Allowed,
        //                          AllowAdd = role.AllowAdd,
        //                          AllowEdit = role.AllowEdit,
        //                          AllowDelete = role.AllowDelete,
        //                          AllowView = role.AllowView
        //                      }).AsNoTracking().FirstOrDefaultAsync();



        //    return data;
        //}

        //public async Task<UserRolePermission> CreateRolePermission(UserRolePermission model)
        //{


        //    model.CreatedOn = DateTime.Now;
        //    await _dbcontext.UserRolePermissions.AddAsync(model);
        //    await _dbcontext.SaveChangesAsync();
        //    return model;
        //}
        //public async Task CreateRolePermissions(List<UserRolePermission> models)
        //{
        //    await _dbcontext.UserRolePermissions.AddRangeAsync(models);
        //    await _dbcontext.SaveChangesAsync();
        //}
        //public async Task<List<UserRolePermission>> GetRolePermissionsById(long roleId)
        //{
        //    var userRoles = await _dbcontext
        //                   .UserRolePermissions
        //                   .Where(x => x.Deleted == false && x.RoleId == roleId)
        //                   .ToListAsync();

        //    return userRoles;
        //}


        //public async Task<List<UserRolePermission>> UpdateRolePermission(long roleId, List<long> permissions)
        //{
        //    var data = await _dbcontext
        //                     .UserRolePermissions
        //                     .Where(x => x.RoleId == roleId)
        //                     .AsNoTracking()
        //                     .ToListAsync();

        //    foreach (var permissionId in permissions)
        //    {
        //        var find = data.Where(x => x.PermissionId == permissionId).FirstOrDefault();
        //        if (find is not null)
        //        {
        //            find.AllowView = true;
        //            _dbcontext.Update(find);
        //        }
        //        else
        //        {
        //            var newItem = new UserRolePermission
        //            {
        //                AllowView = true,
        //                PermissionId = permissionId,
        //                RoleId = roleId,
        //                CreatedOn = DateTime.Now,
        //                Deleted = false
        //            };
        //            await _dbcontext.AddAsync(newItem);
        //        }
        //    }

        //    await _dbcontext.SaveChangesAsync();
        //    return data;
        //}

        //public async Task<bool> UpdateDisplayOrder(long id, int num = 0)
        //{
        //    var data = await _dbcontext.UserRoles.FindAsync(id);
        //    if (data is not null)
        //    {
        //        data.DisplayOrder = num;
        //        data.ModifiedOn = DateTime.Now;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }
        //    return false;
        //}

        //public async Task<List<UserRole>> GetAllByRole(long roleTypeId)
        //{
        //    List<UserRole> userRoles = new();
        //    if (roleTypeId == (long)Role.Root)
        //    {
        //        userRoles = await _dbcontext
        //                   .UserRoles
        //                   .Where(x => x.Deleted == false)
        //                   .AsNoTracking()
        //                   .ToListAsync();
        //    }
        //    else
        //    {
        //        userRoles = await _dbcontext
        //                   .UserRoles
        //                   .Where(x => x.Deleted == false && x.RoleTypeId > (long)Role.Root)
        //                   .AsNoTracking()
        //                   .ToListAsync();
        //    }

        //    return userRoles;
        //}

        //public async Task<List<UserRole>> GetByTypeId(long systemRoleTypeId)
        //{
        //    var systemUserRoles = await _dbcontext
        //                 .UserRoles
        //                 .Where(x => x.Deleted == false && x.RoleTypeId == systemRoleTypeId)
        //                 .AsNoTracking()
        //                 .ToListAsync();

        //    return systemUserRoles;
        //}
        #endregion

    }
}
