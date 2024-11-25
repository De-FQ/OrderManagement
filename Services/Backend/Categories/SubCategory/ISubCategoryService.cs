using Utility.API;
using Utility.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.SiteCategory;
using Utility.Models.Admin.DTO;
using Services.Base;
using Microsoft.AspNetCore.Http;

namespace Services.Backend.SubCategories
{
    public interface ISubCategoryService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<SubCategoryDto>>> GetSubCategoryForDataTable(DataTableParam param, int? categoryId);
        Task<List<SubCategory>> GetSubCategoriesByRoleId(int roleId, bool isEnglish, int userId);
        Task<bool> UpdateSubCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage);
        Task<bool> ImportSubCategoriesFromExcel(IFormFile file, int categoryId);
    }
}
