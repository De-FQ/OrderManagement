using Data.Model.General;
using Data.Model.InventoryManagement;
using Microsoft.AspNetCore.Http;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;

namespace Services.Backend.InventoryManagement.SupplierItems
{
    public interface ISupplierItemService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<SupplierItemDto>>> GetSupplierItemForDataTable(DataTableParam param, int? supplierId);
        Task<List<SupplierItem>> GetSupplierItemsBySupplierId(int supplierId);
        Task<SupplierItem> Create(SupplierItem model);
        Task<bool> Update(SupplierItem model);
        Task<bool> ImportSupplierItemsFromExcel(IFormFile file, int supplierId);
    }
}
