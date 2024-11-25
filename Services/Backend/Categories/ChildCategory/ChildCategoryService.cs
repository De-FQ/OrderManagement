using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Utility.API;
using Utility.Models.Base;
using Services.Base;
using AutoMapper;
using Serilog;
using Utility.Models.Admin.DTO;
using System.Linq.Dynamic.Core;
using Data.Model.SiteCategory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SkiaSharp;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Services.Backend.ChildCategories
{
    public class ChildCategoryService : BaseService, IChildCategoryService<ChildCategory>
    {
        private readonly IMapper _mapper;
        private readonly string ServiceName = "ChildCategoryService";

        public ChildCategoryService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public async Task<bool> Exists(string name, Guid? guid = null)
        {
            try
            {
                var result = await _dbcontext.ChildCategories
                    .AsNoTracking()
                    .Select(x => new { x.Guid, x.Name })
                    .Where(a => a.Name.ToLower() == name.ToLower())
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    if (guid.HasValue && result.Guid == guid)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> Exists check failed: " + ex.Message);
            }

            return false;
        }

        public async Task<ChildCategory> GetByGuid(Guid guid)
        {
            try
            {
                return await _dbcontext.ChildCategories
                    .AsNoTracking()
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync() ?? new ChildCategory();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetByGuid failed: " + ex.Message);
                return new ChildCategory();
            }
        }

        public async Task<ChildCategory> GetById(long id)
        {
            try
            {
                return await _dbcontext.ChildCategories
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync() ?? new ChildCategory();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetById failed: " + ex.Message);
                return new ChildCategory();
            }
        }

        public async Task<ChildCategory> Create(ChildCategory model)
        {
            try
            {
                model.Guid = Guid.NewGuid();
                model.CreatedOn = DateTime.Now;
                model.DisplayOrder = await GetNextDisplayOrder();
                await _dbcontext.ChildCategories.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
                // Apply discounts after saving changes
                ApplyDiscounts();
                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> Create failed: " + ex.Message);
                return model;
            }
        }

        public async Task<bool> Update(ChildCategory model)
        {
            try
            {
                var data = await _dbcontext.ChildCategories
                    .Where(x => x.Guid == model.Guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.Name = model.Name;
                    data.Description = model.Description;
                    data.ShowInMenu = model.ShowInMenu;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;
                    data.DiscountActive = model.DiscountActive;
                    data.DiscountPercentage = model.DiscountPercentage;

                    if (!string.IsNullOrEmpty(model.ImageName))
                        data.ImageName = model.ImageName;

                    bool result = await _dbcontext.SaveChangesAsync() > 0;
                    // Apply discounts after saving changes
                    ApplyDiscounts();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> Update failed: " + ex.Message);
            }
            return false;
        }

        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            try
            {
                bool modified = false;
                var orderList = rowOrders.Select(x => x.Id).ToList();
                var items = await _dbcontext.ChildCategories
                    .Where(x => orderList.Contains(x.Id))
                    .ToListAsync();

                foreach (var item in items)
                {
                    var row = rowOrders.FirstOrDefault(p => p.Id == item.Id);
                    if (row is not null)
                    {
                        modified = true;
                        item.ModifiedBy = row.UserId;
                        item.ModifiedOn = DateTime.Now;
                        item.DisplayOrder = (int)row.DisplayOrder;
                    }
                }

                if (modified)
                {
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> UpdateDisplayOrders failed: " + ex.Message);
            }

            return false;
        }

        public async Task<bool> UpdateDisplayOrder(Guid guid, int num = 0)
        {
            try
            {
                var data = await _dbcontext.ChildCategories
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.DisplayOrder = num;
                    data.ModifiedOn = DateTime.Now;
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> UpdateDisplayOrder failed: " + ex.Message);
            }

            return false;
        }

        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.ChildCategories
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.Deleted = true;
                    data.DeletedAt = DateTime.Now;
                    bool result = await _dbcontext.SaveChangesAsync() > 0;

                    // Apply discounts after deletion
                    ApplyDiscounts();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> Delete failed: " + ex.Message);
            }

            return false;
        }

        public async Task<bool> ToggleActive(Guid guid)
        {
            try
            {
                var data = await _dbcontext.ChildCategories
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.ModifiedOn = DateTime.Now;
                    data.Active = !data.Active;
                    bool result = await _dbcontext.SaveChangesAsync() > 0;

                    // Apply discounts after toggling active state
                    ApplyDiscounts();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> ToggleActive failed: " + ex.Message);
            }

            return false;
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryId(long categoryId)
        {
            try
            {
                return await _dbcontext.SubCategories
                    .Where(sc => sc.CategoryId == categoryId && !sc.Deleted)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetSubCategoriesByCategoryId failed: " + ex.Message);
                return new List<SubCategory>();
            }
        }

        public async Task<List<ChildCategory>> GetChildCategoriesBySubCategoryId(long subCategoryId)
        {
            try
            {
                return await _dbcontext.ChildCategories
                    .Where(cc => cc.SubCategoryId == subCategoryId && !cc.Deleted)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetChildCategoriesBySubCategoryId failed: " + ex.Message);
                return new List<ChildCategory>();
            }
        }

        public async Task<DataTableResult<List<ChildCategoryDto>>> GetChildCategoryForDataTable(DataTableParam param, int? categoryId, int? subCategoryId)
        {
            var result = new DataTableResult<List<ChildCategoryDto>> { Draw = param.Draw };
            try
            {
                var items = _dbcontext.ChildCategories.AsNoTracking().AsQueryable();

                if (categoryId.HasValue)
                {
                    items = items.Where(sc => sc.SubCategory.CategoryId == categoryId.Value);
                }

                if (subCategoryId.HasValue)
                {
                    items = items.Where(sc => sc.SubCategoryId == subCategoryId.Value);
                }

                var records = items.Select(x => new ChildCategoryDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    SubCategoryId = x.SubCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    ShowInMenu = x.ShowInMenu,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    Active = x.Active,
                    Deleted = x.Deleted,
                    ImageName = x.ImageName
                });

                // ChildCategory Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var searchValue = param.SearchValue.ToLower();
                    records = records.Where(obj =>
                        obj.Name.Contains(searchValue) ||
                        obj.Description.Contains(searchValue));
                }

                // Sorting
                if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
                {
                    records = records.OrderBy(param.SortColumn + " " + param.SortColumnDirection);
                }
                else
                {
                    records = records.OrderBy(x => x.DisplayOrder);
                }

                result.RecordsTotal = await records.CountAsync();
                result.RecordsFiltered = await records.CountAsync();
                result.Data = await records.Skip(param.Skip).Take(param.PageSize).ToListAsync();

                return result;
            }
            catch (Exception err)
            {
                Log.Error(ServiceName + "--> GetChildCategoryForDataTable failed: " + err.Message);
                result.Error = err;
            }
            return result;
        }

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            try
            {
                var items = await _dbcontext.ChildCategories
                    .Where(x => !x.Deleted)
                    .Select(x => new
                    {
                        x.Id,
                        Name = isEnglish ? x.Name : x.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetAllForDropDownList failed: " + ex.Message);
                return null;
            }
        }

        private async Task<long> GetNextDisplayOrder()
        {
            try
            {
                var item = await _dbcontext.ChildCategories
                    .OrderByDescending(x => x.DisplayOrder)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return item?.DisplayOrder + 1 ?? 1;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetNextDisplayOrder failed: " + ex.Message);
                return 1;
            }
        }

        public async Task<List<ChildCategory>> GetChildCategoriesByRoleId(int roleId, bool isEnglish, int userId)
        {
            try
            {
                var ChildCategories = await _dbcontext.ChildCategories
                    .Where(c => c.SubCategoryId == roleId && c.Active)
                    .ToListAsync();

                return ChildCategories;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetChildCategoriesByRoleId failed: " + ex.Message);
                return new List<ChildCategory>();
            }
        }

        public async Task<bool> UpdateChildCategoryDiscount(Guid guid, bool isDiscountActive, double  discountPercentage)
        {
            var category = await _dbcontext.ChildCategories.FirstOrDefaultAsync(x => x.Guid == guid);
            if (category != null)
            {
                category.DiscountActive = isDiscountActive;
                category.DiscountPercentage = discountPercentage;
                category.ModifiedOn = DateTime.Now;

                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public void ApplyDiscounts()
        {
            var categories = _dbcontext.Categories
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.ChildCategories)
                .ThenInclude(cc => cc.ProductPrices)
                .ToList();

            foreach (var category in categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    foreach (var childCategory in subCategory.ChildCategories)
                    {
                        double categoryDiscount = category.DiscountActive ? category.DiscountPercentage :0;
                        double subCategoryDiscount = subCategory.DiscountActive ? subCategory.DiscountPercentage : 0;
                        double childCategoryDiscount = childCategory.DiscountActive ? childCategory.DiscountPercentage : 0;

                        foreach (var productPrice in childCategory.ProductPrices)
                        {
                            double totalDiscount = 1 - ((1 - categoryDiscount / 100) * (1 - subCategoryDiscount / 100) * (1 - childCategoryDiscount / 100));

                            if (category.DiscountActive || subCategory.DiscountActive || childCategory.DiscountActive)
                            {
                                // Calculate cumulative discount percentage
                                productPrice.Price = Convert.ToDecimal(Convert.ToDouble(productPrice.Price) *(1 - totalDiscount));
                                //productPrice.Price = Convert.ToDecimal(t);
                            }
                            else
                            {
                                // If no discounts are active, restore original price
                                //double totalDiscount = 1 - ((1 - categoryDiscount / 100) * (1 - subCategoryDiscount / 100) * (1 - childCategoryDiscount / 100));
                                //productPrice.Price = Convert.ToDecimal(productPrice.Price / (1 - totalDiscount));
                                productPrice.Price = Convert.ToDecimal(Convert.ToDouble(productPrice.Price) / (1 - totalDiscount));
                            }
                        }
                    }
                }
            }

            _dbcontext.SaveChanges();
        }
        public async Task<bool> ImportChildCategoriesFromExcel(IFormFile file, int categoryId, int subCategoryId)
        {
            try
            {
                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                if (file == null || file.Length == 0)
                {
                    Log.Error("ImportChildCategoriesFromExcel failed: No file uploaded.");
                    return false;
                }

                // Load the Excel file from the stream
                using var package = new ExcelPackage(file.OpenReadStream());
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++)
                {
                    string name = worksheet.Cells[row, 3].Text?.Trim();
                    string description = worksheet.Cells[row, 4].Text?.Trim();
                    string imageName = worksheet.Cells[row, 5].Text?.Trim();
                    bool active = worksheet.Cells[row, 2].Text.ToLower() == "true";

                    if (!string.IsNullOrEmpty(name))
                    {
                        // Check if the child category already exists in the specified subcategory
                        var existingChildCategory = await _dbcontext.ChildCategories
                            .FirstOrDefaultAsync(cc => cc.Name == name && cc.SubCategoryId == subCategoryId);

                        if (existingChildCategory != null)
                        {
                            // Update existing child category
                            existingChildCategory.Name = name;
                            existingChildCategory.Description = description;
                            existingChildCategory.ImageName = imageName;
                            existingChildCategory.Active = active;
                        }
                        else
                        {
                            // Insert new child category
                            var newChildCategory = new ChildCategory
                            {
                                Guid = Guid.NewGuid(),
                                Name = name,
                                Description = description,
                                ImageName = imageName,
                                Active = active,
                                SubCategoryId = subCategoryId,
                                CreatedOn = DateTime.Now,
                                DisplayOrder = await GetNextDisplayOrder()
                            };

                            await _dbcontext.ChildCategories.AddAsync(newChildCategory);
                        }
                    }
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"ImportChildCategoriesFromExcel action failed: {ex.Message}");
                return false;
            }
        }

    }
}
