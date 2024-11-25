using Utility.API;
using Utility.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.SiteCategory;
using Utility.Models.Admin.DTO;
using Services.Base;
using Microsoft.AspNetCore.Http;

namespace Services.Backend.Categorys
{
    public interface ICategoryService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<CategoryDto>>> GetCategoryForDataTable(DataTableParam param);
        Task<List<Category>> GetCategoriesByRoleId(int roleId, bool isEnglish, int userId);
        Task<bool> UpdateCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage);
        Task<bool> ImportCategoriesFromExcel(IFormFile file);
    }
}
