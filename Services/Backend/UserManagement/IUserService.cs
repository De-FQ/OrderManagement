
using Utility.API;
using Utility.Enum;
using Utility.Models.Admin.UserManagement;
using Services.Base;
using Utility.Models.Admin.DTO;
using Data.UserManagement;

namespace Services.Backend.UserManagement
{
    public interface IUserService<T> : IBaseService<T>
    { 
        #region Users    
        Task<User> GetByEmail(string email);
        Task<string> GetForDisplay(long? id, DateTime? date, bool isEnglish);
        Task<string> GetLastLoginWithDate(long? id);
        Task<IEnumerable<User>> GetUsers();  
        Task<DataTableResult<List<UserDto>>> GetForDataTable(DataTableParam param, SystemUserSearchParamModel? SearchParam = null);
        Task<dynamic> GetForDropDownList(string selectedIds = "");
        Task<dynamic> GetRoleTypesForDropDownList();
        Task<bool> Allowed(long roleId, long permissionId);
        Task<bool> AccessPermission(long roleId, PermissionTypes permType, AllowPermission allowed);
        Task<AdminUserPermissionModel> GetViewPermissionBy(long roleId, long permissionId);
         

        #endregion

        #region Refresh Token
        Task<bool> UpdateRefreshToken(string email, long userId, string refreshToken, DateTime tokenExpiry);
        //Task<bool> UpdateRefreshToken(string email, string refreshToken, long userId);
        Task<bool> UpdatePassword(User model);
//        Task<bool> RevokeRefreshToken(User model);
        Task<bool> RevokeAllRefreshToken();
        #endregion

        #region User history
        Task<UserHistory> AddUserHistory(UserHistory model);
        #endregion 

    }
}
