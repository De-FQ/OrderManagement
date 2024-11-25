using Data.Model.InventoryManagement;
using Services.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;

namespace Services.Backend.InventoryManagement
{

        public interface IInventoryItemService<T> : IBaseService<T>
        {
        Task<DataTableResult<List<InventoryItemDto>>> GetInventoryItemsForDataTable(DataTableParam param, int? supplierId, DateTime? date);
            Task<List<InventoryItem>> GetInventoryItemsBySupplierId(int supplierId);
            Task<InventoryItem> Create(InventoryItem model);
            Task<bool> Update(InventoryItem model);

        }

}
