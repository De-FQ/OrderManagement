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
using Services.Backend.Price;
using SkiaSharp;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace Services.Backend.Categorys
{
    public class CategoryService : BaseService, ICategoryService<Category>
    {
        private readonly IMapper _mapper;
        private readonly string ServiceName = "CategoryService";

        public CategoryService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public async Task<bool> Exists(string name, Guid? guid = null)
        {
            var result = await _dbcontext.Categories
                .Select(x => new { x.Guid, x.Name })
                .Where(a => a.Name.ToLower() == name.ToLower())
                .AsNoTracking()
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

            return false;
        }

        public async Task<Category> GetByGuid(Guid guid)
        {
            var item = await _dbcontext.Categories.Where(x => x.Guid == guid).AsNoTracking().FirstOrDefaultAsync();
            if (item is null) { return new Category(); } else { return item; }
        }

        public async Task<Category> GetById(long id)
        {
            var item = await _dbcontext.Categories.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (item is null) { return new Category(); } else { return item; }
        }

        public async Task<Category> Create(Category model)
        {
            try
            {
                model.Guid = Guid.NewGuid();
                model.CreatedOn = DateTime.Now;
                model.DisplayOrder = await GetNextDisplayOrder();
                await _dbcontext.Categories.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
                // Apply discounts after saving changes
                ApplyDiscounts();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return model;
        }

        public async Task<bool> Update(Category model)
        {
            try
            {
                var data = await _dbcontext.Categories.Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();
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
                return false;
            }
            catch (Exception ex)
            {
                Log.Information(ServiceName + ": " + ex.Message);
            }
            return false;
        }


        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            bool modified = false;
            var orderList = rowOrders.Select(x => x.Id).ToList();
            var items = await _dbcontext.Categories.Where(x => orderList.Contains(x.Id)).ToListAsync();

            foreach (var item in items)
            {
                var row = rowOrders.Where(p => p.Id == item.Id).FirstOrDefault();
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

            return false;
        }

        public async Task<bool> UpdateDisplayOrder(Guid guid, int num = 0)
        {
            var data = await _dbcontext.Categories.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.DisplayOrder = num;
                data.ModifiedOn = DateTime.Now;
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.Categories.Where(x => x.Guid == guid).FirstOrDefaultAsync();
                if (data is not null)
                {
                    _dbcontext.Remove(data);
                    bool result = await _dbcontext.SaveChangesAsync() > 0;

                    // Apply discounts after deletion
                    ApplyDiscounts();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "-->Delete action Failed Reason: " + ex.Message.ToString());
            }
            return false;
        }

        public async Task<bool> ToggleActive(Guid guid)
        {
            var data = await _dbcontext.Categories.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.ModifiedOn = DateTime.Now;
                data.Active = !data.Active;
                bool result = await _dbcontext.SaveChangesAsync() > 0;

                // Apply discounts after toggling active state
                ApplyDiscounts();

                return result;
            }
            return false;
        }

        public async Task<DataTableResult<List<CategoryDto>>> GetCategoryForDataTable(DataTableParam param)
        {
            DataTableResult<List<CategoryDto>> result = new() { Draw = param.Draw };
            try
            {
                var items = _dbcontext.Categories.AsNoTracking().AsQueryable();
                var records = items.Select(x => new CategoryDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
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
                    ImageName = x.ImageName // Add ImageUrl field
                });

                // Category Search
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
                result.Error = err;
            }
            return result;
        }

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            var items = await _dbcontext.Categories.Where(x => x.Deleted == false).Select(x => new
            {
                x.Id,
                Name = isEnglish ? x.Name : x.Name
            }).AsNoTracking().ToListAsync();

            return items;
        }

        private async Task<long> GetNextDisplayOrder()
        {
            var item = await _dbcontext.Categories.OrderByDescending(x => x.DisplayOrder).AsNoTracking().FirstOrDefaultAsync();
            if (item is not null)
            {
                return item.DisplayOrder + 1;
            }
            return 1;
        }


        public async Task<List<Category>> GetCategoriesByRoleId(int roleId, bool isEnglish, int userId)
        {
            // Implement the logic to fetch categories based on roleId, isEnglish, and userId
            // Example:
            var categories = await _dbcontext.Categories
                .Where(c => c.Id == roleId && c.Active)
                .ToListAsync();

            return categories;
        }

        public async Task<bool> UpdateCategoryDiscount(Guid guid, bool isDiscountActive, double discountPercentage)
        {
            var category = await _dbcontext.Categories.FirstOrDefaultAsync(x => x.Guid == guid);
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
                        double categoryDiscount = category.DiscountActive ? category.DiscountPercentage : 0;
                        double subCategoryDiscount = subCategory.DiscountActive ? subCategory.DiscountPercentage : 0;
                        double childCategoryDiscount = childCategory.DiscountActive ? childCategory.DiscountPercentage : 0;

                        foreach (var productPrice in childCategory.ProductPrices)
                        {
                            double totalDiscount = 1 - ((1 - categoryDiscount / 100) * (1 - subCategoryDiscount / 100) * (1 - childCategoryDiscount / 100));

                            if (category.DiscountActive || subCategory.DiscountActive || childCategory.DiscountActive)
                            {
                                // Calculate cumulative discount percentage
                                productPrice.Price = Convert.ToDecimal(Convert.ToDouble(productPrice.Price) * (1 - totalDiscount));
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

                        //foreach (var productPrice in childCategory.ProductPrices)
                        //{
                        //    if (category.DiscountActive || subCategory.DiscountActive || childCategory.DiscountActive)
                        //    {
                        //        // Calculate cumulative discount percentage
                        //        double totalDiscount = 1 - ((1 - categoryDiscount / 100) * (1 - subCategoryDiscount / 100) * (1 - childCategoryDiscount / 100));
                        //        productPrice.Price = Convert.ToInt32(productPrice.Price * (1 - totalDiscount));
                        //    }
                        //    else
                        //    {
                        //        // If no discounts are active, restore original price
                        //        double totalDiscount = 1 - ((1 - categoryDiscount / 100) * (1 - subCategoryDiscount / 100) * (1 - childCategoryDiscount / 100));
                        //        productPrice.Price = Convert.ToInt32(productPrice.Price / (1 - totalDiscount));
                        //    }
                        //}
                    }
                }
            }

            _dbcontext.SaveChanges();
        }
        public async Task<bool> ImportCategoriesFromExcel(IFormFile file)
        {
            try
            {
                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.Commercial; // Or LicenseContext.Public

                if (file == null || file.Length == 0)
                {
                    Log.Error("ImportCategoriesFromExcel failed: No file uploaded.");
                    return false;
                }

                // Load the Excel file from the stream
                using var package = new ExcelPackage(file.OpenReadStream());
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++)
                {
                    bool active = worksheet.Cells[row, 2].Text.ToLower() == "true";
                    string name = worksheet.Cells[row, 3].Text?.Trim();
                    string description = worksheet.Cells[row, 4].Text?.Trim();
                    string imageName = worksheet.Cells[row, 5].Text?.Trim();
                    

                    if (!string.IsNullOrEmpty(name))
                    {
                        // Check if the category already exists
                        var existingCategory = await _dbcontext.Categories.FirstOrDefaultAsync(c => c.Name == name);

                        if (existingCategory != null)
                        {
                            // Update existing category
                            existingCategory.Name = name;
                            existingCategory.Description = description;
                            existingCategory.ImageName = imageName;
                            existingCategory.Active = active;
                        }
                        else
                        {
                            // Insert new category
                            var newCategory = new Category
                            {
                                Guid = Guid.NewGuid(),
                                Name = name,
                                Description = description,
                                ImageName = imageName,
                                Active = active,
                                CreatedOn = DateTime.Now,
                                DisplayOrder = await GetNextDisplayOrder()
                            };

                            await _dbcontext.Categories.AddAsync(newCategory);
                        }
                    }
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"ImportCategoriesFromExcel action failed: {ex.Message}");
                return false;
            }
        }

    }
}
