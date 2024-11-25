
using Utility.API;
using Utility.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.InventoryManagement;
using Services.Base;
using Utility.Models.Admin.DTO;

namespace Services.Backend.Inventory
{
    public interface ISupplierService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<SupplierDto>>> GetSupplierForDataTable(DataTableParam param);
    }
}
