using AutoMapper;
using Data.EntityFramework;
using Data.Model.General;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Backend.Price;
using Services.Base;
using Utility.API;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

public class PriceTypeCategoryService : BaseService, IPriceTypeCategoryService<PriceTypeCategory>
{
    private readonly IMapper _mapper;
    private readonly string ServiceName = "PriceTypeCategoryService";

    public PriceTypeCategoryService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
    {
        _mapper = mapper;
    }

    public async Task<bool> Exists(string name, Guid? guid = null)
    {
        try
        {
            var result = await _dbcontext.PriceTypeCategories
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

    public async Task<PriceTypeCategory> GetByGuid(Guid guid)
    {
        try
        {
            return await _dbcontext.PriceTypeCategories
                .AsNoTracking()
                .Where(x => x.Guid == guid)
                .FirstOrDefaultAsync() ?? new PriceTypeCategory();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetByGuid failed: " + ex.Message);
            return new PriceTypeCategory();
        }
    }

    public async Task<PriceTypeCategory> GetById(long id)
    {
        try
        {
            return await _dbcontext.PriceTypeCategories
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? new PriceTypeCategory();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetById failed: " + ex.Message);
            return new PriceTypeCategory();
        }
    }

    public async Task<PriceTypeCategory> Create(PriceTypeCategory model)
    {
        try
        {
            model.Guid = Guid.NewGuid();
            model.CreatedOn = DateTime.Now;
            model.DisplayOrder = await GetNextDisplayOrder();
            await _dbcontext.PriceTypeCategories.AddAsync(model);
            await _dbcontext.SaveChangesAsync();
            return model;
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> Create failed: " + ex.Message);
            return model;
        }
    }

    public async Task<bool> Update(PriceTypeCategory model)
    {
        try
        {
            var data = await _dbcontext.PriceTypeCategories
                .Where(x => x.Guid == model.Guid)
            .FirstOrDefaultAsync();

            if (data is not null)
            {
                data.Name = model.Name;
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
            var items = await _dbcontext.PriceTypeCategories
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
            var data = await _dbcontext.PriceTypeCategories
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
            var data = await _dbcontext.PriceTypeCategories
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
            var data = await _dbcontext.PriceTypeCategories
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

    public async Task<List<PriceTypeCategory>> GetPriceTypeCategoriesByChildCategoryId(int childCategoryId)
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
            Log.Error(ServiceName + "--> GetPriceTypeCategoriesByChildCategoryId failed: " + ex.Message);
            return new List<PriceTypeCategory>();
        }
    }

    public async Task<DataTableResult<List<PriceTypeCategoryDto>>> GetPriceTypeCategoryForDataTable(DataTableParam param, int? childCategoryId)
    {
        var result = new DataTableResult<List<PriceTypeCategoryDto>> { Draw = param.Draw };
        try
        {
            var items = _dbcontext.PriceTypeCategories.AsNoTracking().AsQueryable();

            if (childCategoryId.HasValue)
            {
                items = items.Where(ptc => ptc.ChildCategoryId == childCategoryId.Value);
            }

            var records = items.Select(x => new PriceTypeCategoryDto()
            {
                Id = x.Id,
                Guid = x.Guid,
                ChildCategoryId = x.ChildCategoryId,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                Active = x.Active,
                Deleted = x.Deleted
            });

            // PriceTypeCategory Search
            if (!string.IsNullOrEmpty(param.SearchValue))
            {
                var searchValue = param.SearchValue.ToLower();
                records = records.Where(obj =>
                    obj.Name.Contains(searchValue));
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
            Log.Error(ServiceName + "--> GetPriceTypeCategoryForDataTable failed: " + err.Message);
            result.Error = err;
        }
        return result;
    }

    public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
    {
        try
        {
            var items = await _dbcontext.PriceTypeCategories
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
            var item = await _dbcontext.PriceTypeCategories
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

    public async Task<bool> ImportPriceTypeCategoriesFromExcel(IFormFile file, int childCategoryId)
    {
        try
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            if (file == null || file.Length == 0)
            {
                Log.Error("ImportPriceTypeCategoriesFromExcel failed: No file uploaded.");
                return false;
            }

            // Load the Excel file from the stream
            using var package = new ExcelPackage(file.OpenReadStream());
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 3; row <= rowCount; row++)
            {
                string name = worksheet.Cells[row, 3].Text?.Trim();
                bool active = worksheet.Cells[row, 2].Text.ToLower() == "true";

                if (!string.IsNullOrEmpty(name))
                {
                    // Check if the price type category already exists in the specified child category
                    var existingPriceTypeCategory = await _dbcontext.PriceTypeCategories
                        .FirstOrDefaultAsync(ptc => ptc.Name == name && ptc.ChildCategoryId == childCategoryId);

                    if (existingPriceTypeCategory != null)
                    {
                        // Update existing price type category
                        existingPriceTypeCategory.Name = name;
                        existingPriceTypeCategory.Active = active;
                        existingPriceTypeCategory.ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        // Insert new price type category
                        var newPriceTypeCategory = new PriceTypeCategory
                        {
                            Guid = Guid.NewGuid(),
                            Name = name,
                            Active = active,
                            ChildCategoryId = childCategoryId,
                            CreatedOn = DateTime.Now,
                            DisplayOrder = await GetNextDisplayOrder()
                        };

                        await _dbcontext.PriceTypeCategories.AddAsync(newPriceTypeCategory);
                    }
                }
            }

            await _dbcontext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"ImportPriceTypeCategoriesFromExcel action failed: {ex.Message}");
            return false;
        }
    }


}
