using Utility.API;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Data.Model.General;
using Services.Base;
using Microsoft.AspNetCore.Http;

namespace Services.Backend.Price
{
    public interface IProductPriceService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<ProductPriceDto>>> GetProductPriceForDataTable(DataTableParam param, int? childCategoryId);
        Task<List<PriceTypeCategory>> GetPriceTypeCategoryByChildCategoryId(long childCategoryId);
        Task<List<PriceType>> GetPriceTypeByPriceTypeCategoryId(long priceTypeCategoryId);
        Task<bool> ImportProductPriceFromExcel(IFormFile file, int childCategoryId, int priceTypeCategoryId, int priceTypeId);
    }
}
