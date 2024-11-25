
using Utility.API;
using Services.Base;
using Utility.Models.Admin.DTO;
using Data.UserManagement;

namespace Services.Backend.UserManagement
{
    public interface IUserRoleService<T> : IBaseService<T>
    {
        // Task<dynamic> GetForDropDownList();

        //Task<UserRole?> GetByGuid(Guid guid);
        //Task<UserRole> Create(UserRole model);
        //Task Create(List<UserRole> models);
        // Task<bool> Update(UserRole model);
        // Task<bool> Delete(UserRole model);
        // Task<bool> ToggleActive(Guid guid);
        //  Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders); 
        Task<DataTableResult<List<UserRoleDto>>> GetForDataTable(DataTableParam param, long id = 0, long roleTypeId = 0);

        Task<List<UserPermission>> GetPermissions();
        //Task<UserRolePermission> GetViewPermission(long roleId, string navigationUrl);
        Task<List<UserRolePermission>> GetAllPermissionByRoleId(long roleId);
        Task<List<UserRolePermission>> UpdateRolePermission(UserRole Role);

        // Task<int> GetCount(long id); 
        // Task<UserRole?> GetById(long id);
        //Task<bool> UpdateDisplayOrder(long id, int num = 0);
        //Task<List<UserRole>> GetAllByRole(long roleTypeId);
        //Task<List<UserRole>> GetByTypeId(long systemRoleTypeId);
    }
}
