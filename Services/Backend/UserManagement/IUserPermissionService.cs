
using Utility.API;
using Services.Base;
using Utility.Models.Admin.DTO;
using Data.UserManagement;

namespace Services.Backend.UserManagement
{
    public interface IUserPermissionService<T> : IBaseService<T>
    {
        #region User Permissions
        //  Task<UserPermission?> GetById(long id);
        //  Task<UserPermission?> GetByGuid(Guid guid);
        // Task<UserPermission> Create(UserPermission model);
        // Task<bool> Update(UserPermission model);
        //  Task<bool> Delete(UserPermission model);
        // Task<bool> ToggleActive(long id);
        //  Task<bool> ToggleActiveByGuid(Guid guid);
        //   Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders);
        //   Task<bool> UpdateDisplayOrder(long id, int num = 0);
        //Task<bool> UpdateDisplayOrder(Guid guid, int num = 0);
        Task<List<UserPermission>> GetMenuByRoleId(long roleId, bool isEnglish, long? systemUserId = null);
        Task<List<UserPermission>> GetAllPermissions();
        Task<DataTableResult<List<UserPermissionDto>>> GetForDataTable(DataTableParam param);
        Task<long> GetNextDisplayOrder();
        #endregion
    }
}
