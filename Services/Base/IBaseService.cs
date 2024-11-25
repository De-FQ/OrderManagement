
using Utility.Models.Base;

namespace Services.Base
{

    public interface IBaseService<T>
    {
        Task<bool> Exists(string name, Guid? guid = null);
        Task<T> GetByGuid(Guid guid);
        Task<T> GetById(long id);
        Task<T> Create(T model);
        Task<bool> Update(T model);  
        Task<bool> Delete(Guid guid);
        Task<bool> ToggleActive(Guid guid);

      
        Task<bool> UpdateDisplayOrder(Guid guid, int num = 0);
        Task<bool> UpdateDisplayOrders(List<BaseRowOrder> items);
        Task<dynamic> GetAllForDropDownList(bool isEnglish = true);

    }
}
