using Utility.API;
using Utility.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.SiteCategory;
using Utility.Models.Admin.DTO;
using Services.Base;
using Microsoft.AspNetCore.Http;

namespace Services.Backend.ChildCategories
{
    public interface IChildCategoryService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<ChildCategoryDto>>> GetChildCategoryForDataTable(DataTableParam param, int? categoryId, int? subCategoryId);
        Task<List<ChildCategory>> GetChildCategoriesByRoleId(int roleId, bool isEnglish, int userId);
        Task<List<SubCategory>> GetSubCategoriesByCategoryId(long categoryId);
        Task<List<ChildCategory>> GetChildCategoriesBySubCategoryId(long subCategoryId);
        Task<bool> UpdateChildCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage);
        Task<bool> ImportChildCategoriesFromExcel(IFormFile file, int categoryId, int subCategoryId);
    }
}
