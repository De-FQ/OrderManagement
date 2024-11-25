using Data.Model.General;
using Microsoft.AspNetCore.Http;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;

namespace Services.Backend.Price
{
    public interface IPriceTypeCategoryService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<PriceTypeCategoryDto>>> GetPriceTypeCategoryForDataTable(DataTableParam param, int? childCategoryId);
        Task<List<PriceTypeCategory>> GetPriceTypeCategoriesByChildCategoryId(int childCategoryId);
        Task<PriceTypeCategory> Create(PriceTypeCategory model);
        Task<bool> Update(PriceTypeCategory model);
        Task<bool> ImportPriceTypeCategoriesFromExcel(IFormFile file, int childCategoryId);
    }
}
