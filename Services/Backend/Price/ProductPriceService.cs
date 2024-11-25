using AutoMapper;
using Data.EntityFramework;
using Data.Model.General;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Base;
using Utility.API;
using Utility.Models.Admin.DTO;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Utility.Models.Base;
using SkiaSharp;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Services.Backend.Price
{
    public class ProductPriceService : BaseService, IProductPriceService<ProductPrice>
    {
        private readonly IMapper _mapper;
        private readonly string ServiceName = "ProductPriceService";

        public ProductPriceService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public Task<bool> Exists(string name, Guid? guid = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductPrice> GetByGuid(Guid guid)
        {
            try
            {
                return await _dbcontext.ProductPrices
                    .AsNoTracking()
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync() ?? new ProductPrice();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetByGuid failed: " + ex.Message);
                return new ProductPrice();
            }
        }

        public async Task<ProductPrice> GetById(long id)
        {
            try
            {
                return await _dbcontext.ProductPrices
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync() ?? new ProductPrice();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetById failed: " + ex.Message);
                return new ProductPrice();
            }
        }

        public async Task<ProductPrice> Create(ProductPrice model)
        {
            try
            {
                model.Guid = Guid.NewGuid();
                model.CreatedOn = DateTime.Now;
                model.DisplayOrder = await GetNextDisplayOrder();
                await _dbcontext.ProductPrices.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> Create failed: " + ex.Message);
                return model;
            }
        }

        public async Task<bool> Update(ProductPrice model)
        {
            try
            {
                var data = await _dbcontext.ProductPrices
                    .Where(x => x.Guid == model.Guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.ChildCategoryId = model.ChildCategoryId;
                    data.PriceTypeCategoryId = model.PriceTypeCategoryId;
                    data.PriceTypeId = model.PriceTypeId;
                    data.Price = model.Price;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;

                    return await _dbcontext.SaveChangesAsync() > 0;
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
                var items = await _dbcontext.ProductPrices
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
                var data = await _dbcontext.ProductPrices
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
                var data = await _dbcontext.ProductPrices
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.Deleted = true;
                    data.DeletedAt = DateTime.Now;
                    return await _dbcontext.SaveChangesAsync() > 0;
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
                var data = await _dbcontext.ProductPrices
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.ModifiedOn = DateTime.Now;
                    data.Active = !data.Active;
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> ToggleActive failed: " + ex.Message);
            }

            return false;
        }

        public async Task<List<PriceTypeCategory>> GetPriceTypeCategoryByChildCategoryId(long childCategoryId)
        {
            try
            {
                return await _dbcontext.PriceTypeCategories
                    .Where(ptc => ptc.ChildCategoryId == childCategoryId && !ptc.Deleted)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetPriceTypeCategoryByCategoryId failed: " + ex.Message);
                return new List<PriceTypeCategory>();
            }
        }

        // Method to get PriceType by PriceTypeCategoryId
        public async Task<List<PriceType>> GetPriceTypeByPriceTypeCategoryId(long priceTypeCategoryId)
        {
            try
            {
                return await _dbcontext.PriceTypes
                    .Where(pt => pt.PriceTypeCategoryId == priceTypeCategoryId && !pt.Deleted)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetPriceTypeByPriceTypeCategoryId failed: " + ex.Message);
                return new List<PriceType>();
            }
        }

        public async Task<DataTableResult<List<ProductPriceDto>>> GetProductPriceForDataTable(DataTableParam param, int? childCategoryId)
        {
            var result = new DataTableResult<List<ProductPriceDto>> { Draw = param.Draw };
            try
            {
                var items = _dbcontext.ProductPrices.AsNoTracking().AsQueryable();

                if (childCategoryId.HasValue)
                {
                    items = items.Where(pp => pp.ChildCategoryId == childCategoryId.Value);
                }

                var records = items.Select(x => new ProductPriceDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    ChildCategoryId = x.ChildCategoryId,
                    PriceTypeCategoryId = x.PriceTypeCategoryId,
                    PriceTypeId = x.PriceTypeId,
                    Price = x.Price,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    Active = x.Active,
                    Deleted = x.Deleted,
                });

                // ProductPrice Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var searchValue = param.SearchValue.ToLower();
                    records = records.Where(obj =>
                        obj.Price.ToString().Contains(searchValue));
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
                Log.Error(ServiceName + "--> GetProductPriceForDataTable failed: " + err.Message);
                result.Error = err;
            }
            return result;
        }

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            try
            {
                var items = await _dbcontext.ProductPrices
                    .Where(x => !x.Deleted)
                    .Select(x => new
                    {
                        x.Id,
                        Name = isEnglish ? x.ChildCategories.Name + " " + x.PriceTypes.Name : x.ChildCategories.Name + " " + x.PriceTypes.Name // Assuming translation or alternative name logic can be added here
                    })
                    .OrderBy(x => x.Name)
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetAllForDropDownList failed: " + ex.Message);
                return new List<dynamic>();
            }
        }

        private async Task<long> GetNextDisplayOrder()
        {
            try
            {
                var item = await _dbcontext.ProductPrices
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

        public async Task<bool> ImportProductPriceFromExcel(IFormFile file, int childCategoryId, int priceTypeCategoryId, int priceTypeId)
        {
            try
            {
                // Set the license context for EPPlus
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                if (file == null || file.Length == 0)
                {
                    Log.Error("ImportProductPriceFromExcel failed: No file uploaded.");
                    return false;
                }

                // Load the Excel file from the stream
                using var package = new ExcelPackage(file.OpenReadStream());
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++)
                {
                    bool active = worksheet.Cells[row, 2].Text.ToLower() == "true";
                    decimal price = decimal.Parse(worksheet.Cells[row, 3].Text?.Trim() ?? "0");

                    if (price > 0 && priceTypeId > 0)
                    {
                        // Check if the product price already exists for the specified child category and price type
                        var existingProductPrice = await _dbcontext.ProductPrices
                            .FirstOrDefaultAsync(pp => pp.ChildCategoryId == childCategoryId &&
                                                       pp.PriceTypeCategoryId == priceTypeCategoryId &&
                                                       pp.PriceTypeId == priceTypeId);

                        if (existingProductPrice != null)
                        {
                            // Update existing product price
                            existingProductPrice.Price = price;
                            existingProductPrice.Active = active;
                            existingProductPrice.ModifiedOn = DateTime.Now;
                        }
                        else
                        {
                            // Insert new product price
                            var newProductPrice = new ProductPrice
                            {
                                Guid = Guid.NewGuid(),
                                ChildCategoryId = childCategoryId,
                                PriceTypeCategoryId = priceTypeCategoryId,
                                PriceTypeId = priceTypeId,
                                Price = price,
                                Active = active,
                                CreatedOn = DateTime.Now,
                                DisplayOrder = await GetNextDisplayOrder()
                            };

                            await _dbcontext.ProductPrices.AddAsync(newProductPrice);
                        }
                    }
                }

                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"ImportProductPriceFromExcel action failed: {ex.Message}");
                return false;
            }
        }


    }
}
