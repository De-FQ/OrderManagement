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
using Services.Backend.InventoryManagement.SupplierItems;
using Data.Model.InventoryManagement;

public class SupplierItemService : BaseService, ISupplierItemService<SupplierItem>
{
    private readonly IMapper _mapper;
    private readonly string ServiceName = "SupplierItemService";

    public SupplierItemService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
    {
        _mapper = mapper;
    }

    public async Task<bool> Exists(string name, Guid? guid = null)
    {
        try
        {
            var result = await _dbcontext.SupplierItems
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

    public async Task<SupplierItem> GetByGuid(Guid guid)
    {
        try
        {
            return await _dbcontext.SupplierItems
                .AsNoTracking()
                .Where(x => x.Guid == guid)
                .FirstOrDefaultAsync() ?? new SupplierItem();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetByGuid failed: " + ex.Message);
            return new SupplierItem();
        }
    }

    public async Task<SupplierItem> GetById(long id)
    {
        try
        {
            return await _dbcontext.SupplierItems
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? new SupplierItem();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetById failed: " + ex.Message);
            return new SupplierItem();
        }
    }

    public async Task<SupplierItem> Create(SupplierItem model)
    {
        try
        {
            model.Guid = Guid.NewGuid();
            model.CreatedOn = DateTime.Now;
            model.DisplayOrder = await GetNextDisplayOrder();
            await _dbcontext.SupplierItems.AddAsync(model);
            await _dbcontext.SaveChangesAsync();
            return model;
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> Create failed: " + ex.Message);
            return model;
        }
    }

    public async Task<bool> Update(SupplierItem model)
    {
        try
        {
            var data = await _dbcontext.SupplierItems
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
            var items = await _dbcontext.SupplierItems
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
            var data = await _dbcontext.SupplierItems
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
            var data = await _dbcontext.SupplierItems
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
            var data = await _dbcontext.SupplierItems
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

    public async Task<List<SupplierItem>> GetSupplierItemsBySupplierId(int supplierId)
    {
        try
        {
            return await _dbcontext.SupplierItems
                .Where(ptc => ptc.SupplierId == supplierId && !ptc.Deleted)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetSupplierItemsByChildCategoryId failed: " + ex.Message);
            return new List<SupplierItem>();
        }
    }

    public async Task<DataTableResult<List<SupplierItemDto>>> GetSupplierItemForDataTable(DataTableParam param, int? supplierId)
    {
        var result = new DataTableResult<List<SupplierItemDto>> { Draw = param.Draw };
        try
        {
            var items = _dbcontext.SupplierItems.AsNoTracking().AsQueryable();

            if (supplierId.HasValue)
            {
                items = items.Where(si => si.SupplierId == supplierId.Value);
            }

            var records = items.Select(x => new SupplierItemDto()
            {
                Id = x.Id,
                Guid = x.Guid,
                SupplierId = x.SupplierId,
                Name = x.Name,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                Active = x.Active,
                Deleted = x.Deleted
            });

            // SupplierItem Search
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
            Log.Error(ServiceName + "--> GetSupplierItemForDataTable failed: " + err.Message);
            result.Error = err;
        }
        return result;
    }

    public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
    {
        try
        {
            var items = await _dbcontext.SupplierItems
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
            var item = await _dbcontext.SupplierItems
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

    public async Task<bool> ImportSupplierItemsFromExcel(IFormFile file, int supplierId)
    {
        try
        {
            // Set the license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            if (file == null || file.Length == 0)
            {
                Log.Error("ImportSupplierItemsFromExcel failed: No file uploaded.");
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
                    var existingSupplierItem = await _dbcontext.SupplierItems
                        .FirstOrDefaultAsync(si => si.Name == name && si.SupplierId == supplierId);

                    if (existingSupplierItem != null)
                    {
                        // Update existing price type category
                        existingSupplierItem.Name = name;
                        existingSupplierItem.Active = active;
                        existingSupplierItem.ModifiedOn = DateTime.Now;
                    }
                    else
                    {
                        // Insert new price type category
                        var newSupplierItem = new SupplierItem
                        {
                            Guid = Guid.NewGuid(),
                            Name = name,
                            Active = active,
                            SupplierId = supplierId,
                            CreatedOn = DateTime.Now,
                            DisplayOrder = await GetNextDisplayOrder()
                        };

                        await _dbcontext.SupplierItems.AddAsync(newSupplierItem);
                    }
                }
            }

            await _dbcontext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Log.Error($"ImportSupplierItemsFromExcel action failed: {ex.Message}");
            return false;
        }
    }


}
