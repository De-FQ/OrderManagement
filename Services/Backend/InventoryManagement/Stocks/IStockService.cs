using Data.Model.InventoryManagement;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;

namespace Services.Backend.InventoryManagement.Stocks
{
    public interface IStockService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<StockDto>>> GetStocksForDataTable(DataTableParam param, int? inventoryItemId);
        Task<List<Stock>> GetStocksByInventoryItemId(int inventoryItemId);
        Task<Stock> Create(Stock model);
        Task<bool> Update(Stock model);


    }
}
