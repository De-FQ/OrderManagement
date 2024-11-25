using Data.EntityFramework;

using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Admin.UserManagement;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Utility.Models.Base;
using Services.Base;
using AutoMapper;
using Serilog;
using System.Reflection.Metadata;
using Utility.Models.Admin.DTO;
using Data.UserManagement;

namespace Services.Backend.UserManagement
{

    public class UserService :  BaseService,  IUserService<User>  
    { 
        private readonly IMapper _mapper;
        public UserService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }
      

        #region Users
        /// <summary>
        /// find the user with provided email and check guid also to verify
        /// </summary>
        /// <param name="emailAddress">must required</param>
        /// <param name="guid">optional</param>
        /// <returns>returns true or false</returns>
        public async Task<bool> Exists(string emailAddress, Guid? guid = null)
        {
            var result = await _dbcontext.Users
                .Select(x => new
                {
                    x.Guid,
                    x.EmailAddress
                })
                .Where(a => a.EmailAddress.ToLower() == emailAddress.ToLower())
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
  
        public async Task<User> GetByGuid(Guid guid)
        {
            var item = await _dbcontext
                            .Users
                            .Include(x => x.Roles)
                            .Where(x => x.Guid == guid)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            if (item != null)
            {
                item.Password = new EncryptionServices().DecryptString(item.EncryptedPassword);
            }
            if (item is null) { return new User(); } else { return item; }
        }
        public async Task<User> GetById(long id)
        {
            var item = await _dbcontext
                            .Users
                            .Include(x => x.Roles)
                            .Where(x => x.Id == id)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

            if (item is null) { return new User(); } else { return item; }
        }
        public async Task<User> GetByEmail(string email)
        {
            var item = await _dbcontext
                            .Users
                            .Include(x => x.Roles).ThenInclude(x => x.RoleType)
                            .Where(x => x.EmailAddress.ToLower() == email.ToLower())
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
         //   var item1 =   _dbcontext.Users.Include(x => x.Roles).ThenInclude(x => x.RoleType).Where(x => x.EmailAddress.ToLower() == email.ToLower()).AsNoTracking().ToList();

            if (item is null) { return new User(); } else { return item; }
        }
         
        public async Task<string> GetForDisplay(long? id, DateTime? date, bool isEnglish)
        {
            var info = "";
            var systemUser = await _dbcontext.Users
                  .Where(x => x.Id == id)
                  .Select(x => new { x.EmailAddress })
                  .AsNoTracking()
                  .FirstOrDefaultAsync();

            if (systemUser != null)
            {
                info = "<strong>" + systemUser.EmailAddress + "</strong>";
            }
            if (date.HasValue)
            {
                if (info.Length > 0) { info += "<br>"; }
                info += "at " + date.Value.ToString(Constants.DateTimeFormat.DisplayDateTime);
            }

            return info;
        }
        /// <summary>
        /// Get User with html formatted for Datatable grid column
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetLastLoginWithDate(long? id)
        {
            var info = "";
            if (id.HasValue)
            {
                var systemUserHistory = await _dbcontext.UserHistories
                    .Where(x => x.UserId == id.Value)
                    .AsNoTracking()
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();
                if (systemUserHistory != null)
                    info += "<br> at " + systemUserHistory.CreatedOn.ToString(Constants.DateTimeFormat.DisplayDateTime);
            }

            return info;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dbcontext.Users
                    .Where(x => x.Deleted == false)
                    .ToListAsync();
        }

        /// <summary>
        /// Create a User with provided model
        /// </summary>
        /// <param name="model"></param>
        /// <returns>returns created user model</returns>
        public async Task<User> Create(User model)
        {
            model.Guid = Guid.NewGuid();
            model.EncryptedPassword = new EncryptionServices().EncryptString(model.Password);
            model.CreatedOn = DateTime.Now;
            await _dbcontext.Users.AddAsync(model);
            await _dbcontext.SaveChangesAsync();
            return model; 
        }
        public async Task<bool> Update(User model)
        {
            var data = await _dbcontext.Users.Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.FullName = model.FullName;
                data.UserRoleId = model.UserRoleId;
                
                if (!string.IsNullOrEmpty(model.ImageName)) 
                { data.ImageName = model.ImageName; }

                if (model.Password != null)
                    data.EncryptedPassword = new EncryptionServices().EncryptString(model.Password);

                data.ModifiedBy = model.ModifiedBy;
                data.ModifiedOn = DateTime.Now;

                 
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> UpdatePassword(User model)
        {
            if(model.Guid == Guid.Empty) { return false; }
            if(model.Password == null) { return false; } 

            var data = await _dbcontext.Users.Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();

            if (data is not null)
            {
                data.EncryptedPassword = new EncryptionServices().EncryptString(model.Password); 
                data.ModifiedBy = model.ModifiedBy;
                data.ModifiedOn = DateTime.Now;

               // _dbcontext.Update(data);
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RevokeRefreshToken(User model)
        {
            var data = await _dbcontext.Users.Where(x => x.EmailAddress == model.EmailAddress).FirstOrDefaultAsync();

            if (data is not null)
            {
                data.RefreshToken = "";
                data.ModifiedBy = model.ModifiedBy;
                data.ModifiedOn = DateTime.Now;

                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;

        }

        public async Task<bool> RevokeAllRefreshToken()
        {
            var users =  await _dbcontext.Users.Where(x => x.Deleted == false).ToListAsync(); 
            foreach (var user in users)
            {
                user.RefreshToken = null;
                user.ModifiedOn = DateTime.Now;
            }
            return await _dbcontext.SaveChangesAsync() > 0;

        }

        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            bool modified = false;
            var orderList = rowOrders.Select(x => x.Id).ToList();

            var items = await _dbcontext.Users.Where(x => orderList.Contains(x.Id)).ToListAsync();
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
            var data = await _dbcontext.Users.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.DisplayOrder = num;
                data.ModifiedOn = DateTime.Now;
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> ToggleActive(Guid guid)
        {
            var data = await _dbcontext.Users.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.ModifiedOn = DateTime.Now;
                data.Active = !data.Active;
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }
        
        //public async Task<bool> Delete(User model)
        //{
        //    model.ModifiedOn = DateTime.Now;
        //    model.Deleted = true;
        //    return await Update(model);
        //}

      
        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.Users.Where(x => x.Guid == guid).FirstOrDefaultAsync();

                if (data is not null)
                {
                    // it will trigger softDelete operation, when SaveChangesAsync() is called
                    _dbcontext.Remove(data);
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Users-->Delete action Failed Reason: " + ex.Message.ToString());
            }
            return false;
        }
        //public async Task<bool> Delete(Guid guid)
        //{
        //    var data = await _dbcontext.Users.Where(x => x.Guid == guid).FirstOrDefaultAsync();
        //    if (data is not null)
        //    {
        //        data.Deleted = true;
        //        //data.Active = !data.Active;
        //        return await _dbcontext.SaveChangesAsync() > 0;
        //    }

        //    return false;
        //}

        /// <summary>
        /// Get all users for DataTable list
        /// </summary>
        /// <param name="param"></param>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        public async Task<DataTableResult<List<UserDto>>> GetForDataTable(DataTableParam param, SystemUserSearchParamModel? searchParam = null)
        {
            DataTableResult<List<UserDto>> result = new() { Draw = param.Draw };
            try
            {
                var items = _dbcontext.Users
                    .Include(x => x.Roles)
                    .Where(x => x.Deleted == false)
                    .AsNoTracking();

                if (searchParam != null)
                {
                    if (searchParam.RoleTypeId != (long)Role.Root)
                    {
                        items = items.Where(x => x.Roles.UserRoleTypeId > (long)Role.Root || !x.UserRoleId.HasValue);
                    }
                }

                var records = items.Select(x => new UserDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    EmailAddress = x.EmailAddress,
                    FullName = x.FullName,
                    RoleId = x.UserRoleId,
                    RoleName = x.Roles.Name,
                    ImageUrl=x.ImageName,
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
                    records = records.Where(obj =>
                    obj.FullName.Contains(SearchValue) ||
                    obj.EmailAddress.Contains(SearchValue) ||
                    obj.RoleName.Contains(SearchValue));
                }

                //Sorting
                if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
                {
                    //using System.Linq.Dynamic.Core;
                    //NEEDS TO BE INSTALLED FROM NUGET PACKAGE MANAGER
                    records = records.OrderBy(param.SortColumn + " " + param.SortColumnDirection);
                } else { records = records.OrderBy(x => x.DisplayOrder); }

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
        
        /// <summary>
        /// Get all users with Id,FullName columns
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        public async Task<dynamic> GetForDropDownList(string selectedIds = "")
        {
            var items = _dbcontext.Users
                .Include(x => x.Roles)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(selectedIds))
            {
                List<long> ids = selectedIds.Split(',').Select(long.Parse).ToList();
                if (ids.Count > 0)
                {
                    items = items.Where(x => !ids.Contains(x.Id));
                }
            }

            var data = await items.Where(x => x.Deleted == false).Select(x => new
            {
                x.Id,
                x.FullName
            }).ToListAsync();

            return data;
        }

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            var items = await _dbcontext.Users
                .Where(x => x.Deleted == false)
                .Select(x => new {x.Id, Name =  x.FullName   })
                .AsNoTracking().ToListAsync();

            return items;
        }

        #endregion

        #region View Permission
        public async Task<bool> Allowed(long roleId, long permissionId)
        {
            var rolePermission = await _dbcontext
                                .UserRolePermissions
                                .Where(a => a.UserRoleId == roleId && a.UserPermissionId == permissionId)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();
            if (rolePermission == null)
                return false;

            return rolePermission.Allowed;
        }

        /// <summary>
        /// The purpose of the end-point to check the logged-in user permission on specific end-point, before processing the api response
        /// <list type="">
        /// <item>0-None</item>
        /// <item>1-List</item>
        /// <item>2-DisplayOrder</item>
        /// <item>3-Add</item>
        /// <item>4-Edit</item>
        /// <item>5-Delete</item>
        /// <item>6-View</item>
        /// <item>7-Active</item>
        /// <item>8-Allowed</item>
        /// </list>
        /// </summary> 
        /// <param name="roleId">logged-in user roleId</param>
        /// <param name="permType">PermissionTypes for which resource</param>
        /// <param name="allowed"></param>
        /// <returns></returns>
        public async Task<bool> AccessPermission(long roleId, PermissionTypes permType, AllowPermission allowed)
        {
            var item = await _dbcontext.UserRolePermissions
                                .Where(a => a.UserRoleId == roleId && a.UserPermissionId == (int)permType)
                                .AsNoTracking()
                                .FirstOrDefaultAsync();

            if (item == null) return false;

            if (AllowPermission.Allowed == allowed) { return item.Allowed; } 
            else if (AllowPermission.List == allowed) { return item.AllowList; }
            else if (AllowPermission.DisplayOrder == allowed) { return item.AllowDisplayOrder; }
            else if (AllowPermission.Active == allowed) { return item.AllowActive; }
            else if (AllowPermission.Add == allowed) { return item.AllowAdd; }
            else if (AllowPermission.Edit == allowed) { return item.AllowEdit; }
            else if (AllowPermission.Delete == allowed) { return item.AllowDelete; }
            else if (AllowPermission.View == allowed) { return item.AllowView; }
            else { return false; }

            //else if (AllowPermission.Active == allowed)
            //{
            //    //root user cannot change its active status, and only Users Table Active status
            //    if (Constants.RoleTypes.ROOT_ROLE_ID == roleId && PermissionTypes.Users == permType)
            //    {   
            //        return false;
            //    }
            //    else
            //    {
            //        return item.AllowActive;
            //    }
            //}
        }

        /// <summary>
        /// This api method will be called by admin api for the following actions
        /// <list type="ddd">
        /// <item>List - for showing list records</item>
        /// <item>Add - for adding new records</item>
        /// <item>Edit - for editing existing records</item>
        /// </list>
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public async Task<AdminUserPermissionModel> GetViewPermissionBy(long roleId, long permissionId)
        {

            var item = await (from role in _dbcontext.UserRolePermissions
                              join permission in _dbcontext.UserPermissions
                              on role.UserPermissionId equals permission.Id
                              where role.UserRoleId == roleId
                              && permission.Id == permissionId 
                              && permission.Active == true 
                              select new AdminUserPermissionModel
                              {
                                  RoleId = role.Id,
                                  PermissionId = role.UserPermissionId,
                                  Allowed = role.Allowed,
                                  AllowActive = role.AllowActive,
                                  AllowDisplayOrder=role.AllowDisplayOrder,
                                  AllowList = role.AllowList,
                                  AllowAdd = role.AllowAdd,
                                  AllowEdit = role.AllowEdit,
                                  AllowDelete = role.AllowDelete,
                                  AllowView = role.AllowView, 
                              })
                              .AsNoTracking()
                              .FirstOrDefaultAsync();
            if (item is null)
            {
                item = new AdminUserPermissionModel();
            }
            return item;
        }
        #endregion

        #region Role Types

        public async Task<dynamic> GetRoleTypesForDropDownList()
        {
            var items = await _dbcontext.UserRoleTypes
                .Where(x => x.Deleted == false)
                .Select(x => new { x.Id, x.Name })
                .AsNoTracking().ToListAsync();

            return items;
        }
        #endregion Role Types

        #region Refresh Token
        
        public  async Task<bool> UpdateRefreshToken(string email, long userId, string refreshToken, DateTime tokenExpiry )
        {
            var data = await _dbcontext.Users.Where(x => x.EmailAddress == email ).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.RefreshToken = refreshToken;
                data.RefreshTokenExpiryTime = tokenExpiry;  
                data.ModifiedBy = userId;
               return await _dbcontext.SaveChangesAsync() >0;
                //return true;
            }

            return false;
        }
        
      
        #endregion

        #region User History
        public async Task<UserHistory> AddUserHistory(UserHistory model)
        {
            model.CreatedOn = DateTime.Now;
            await _dbcontext.UserHistories.AddAsync(model);
            await _dbcontext.SaveChangesAsync();
            return model;
        }
        #endregion

    }
}
