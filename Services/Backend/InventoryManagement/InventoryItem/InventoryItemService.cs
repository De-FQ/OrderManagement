using AutoMapper;
using Data.EntityFramework;
using Data.Model.InventoryManagement;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Backend.InventoryManagement;
using Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;

public class InventoryItemService : BaseService, IInventoryItemService<InventoryItem>
{
    private readonly IMapper _mapper;
    private readonly string ServiceName = "InventoryItemService";

    public InventoryItemService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
    {
        _mapper = mapper;
    }

    public async Task<bool> Exists(string name, Guid? guid = null)
    {
        try
        {
            var result = await _dbcontext.InventoryItems
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

    public async Task<InventoryItem> GetByGuid(Guid guid)
    {
        try
        {
            return await _dbcontext.InventoryItems
                .AsNoTracking()
                .Where(x => x.Guid == guid)
                .FirstOrDefaultAsync() ?? new InventoryItem();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetByGuid failed: " + ex.Message);
            return new InventoryItem();
        }
    }

    public async Task<InventoryItem> GetById(long id)
    {
        try
        {
            return await _dbcontext.InventoryItems
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? new InventoryItem();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetById failed: " + ex.Message);
            return new InventoryItem();
        }
    }

    public async Task<InventoryItem> Create(InventoryItem model)
    {
        try
        {
            model.Guid = Guid.NewGuid();
            model.CreatedOn = DateTime.Now;
            model.DisplayOrder = await GetNextDisplayOrder();
            await _dbcontext.InventoryItems.AddAsync(model);
            await _dbcontext.SaveChangesAsync();
            return model;
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> Create failed: " + ex.Message);
            return model;
        }
    }

    public async Task<bool> Update(InventoryItem model)
    {
        try
        {
            var data = await _dbcontext.InventoryItems
                .Where(x => x.Guid == model.Guid)
            .FirstOrDefaultAsync();

            if (data is not null)
            {
                data.Name = model.Name;
                data.Description = model.Description;
                data.SupplierId = model.SupplierId;
                data.Unit = model.Unit;
                data.UnitCost = model.UnitCost;
                data.Quantity = model.Quantity;
                data.CostPrice = model.CostPrice;
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
            var items = await _dbcontext.InventoryItems
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
            var data = await _dbcontext.InventoryItems
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
            var data = await _dbcontext.InventoryItems
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
            var data = await _dbcontext.InventoryItems
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

    public async Task<List<InventoryItem>> GetInventoryItemsBySupplierId(int supplierId)
    {
        try
        {
            return await _dbcontext.InventoryItems
                .Where(i => i.SupplierId == supplierId && !i.Deleted)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetInventoryItemsBySupplierId failed: " + ex.Message);
            return new List<InventoryItem>();
        }
    }

    public async Task<DataTableResult<List<InventoryItemDto>>> GetInventoryItemsForDataTable(DataTableParam param, int? supplierId, DateTime? date)
    {
        var result = new DataTableResult<List<InventoryItemDto>> { Draw = param.Draw };
        try
        {
            var items = _dbcontext.InventoryItems.AsNoTracking().AsQueryable();

            if (supplierId.HasValue)
            {
                items = items.Where(i => i.SupplierId == supplierId.Value);
            }

            // Date filtering
            if (date.HasValue)
            {
                items = items.Where(i => i.CreatedOn.Date == date.Value.Date);
            }

            // Get distinct dates that have inventory items
            var availableDates = await items.Select(i => i.CreatedOn.Date).Distinct().ToListAsync();

            var records = items.Select(x => new InventoryItemDto()
            {
                Id = x.Id,
                Guid = x.Guid,
                SupplierId = x.SupplierId,
                Name = x.Name,
                Description = x.Description,
                Unit = x.Unit,
                UnitCost = x.UnitCost,
                Quantity = x.Quantity,
                CostPrice = x.CostPrice,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreatedOn,
                ModifiedBy = x.ModifiedBy,
                ModifiedOn = x.ModifiedOn,
                Active = x.Active,
                Deleted = x.Deleted
            });

            // InventoryItem Search
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

            // Return the available dates as part of the result
            result.AdditionalData = new { AvailableDates = availableDates };

            return result;
        }
        catch (Exception err)
        {
            Log.Error(ServiceName + "--> GetInventoryItemsForDataTable failed: " + err.Message);
            result.Error = err;
        }
        return result;
    }




    public async Task<int> GetNextDisplayOrder()
    {
        try
        {
            return await _dbcontext.InventoryItems.CountAsync() + 1;
        }
        catch (Exception ex)
        {
            Log.Error(ServiceName + "--> GetNextDisplayOrder failed: " + ex.Message);
            return 1;
        }
    }

    public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
    {
        try
        {
            var items = await _dbcontext.InventoryItems
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

    //public async Task<SupplierBalance> CalculateAndSaveBalance(int supplierId, decimal paidAmount)
    //{
    //    try
    //    {
    //        var inventoryItems = await _dbcontext.InventoryItems
    //            .Where(i => i.SupplierId == supplierId && !i.Deleted)
    //            .ToListAsync();

    //        var totalUnitCost = inventoryItems.Sum(i => i.UnitCost * i.Quantity);

    //        var supplierBalance = new SupplierBalance
    //        {
    //            SupplierId = supplierId,
    //            TotalUnitCost = totalUnitCost,
    //            PaidAmount = paidAmount,
    //            RemainingBalance = totalUnitCost - paidAmount,
    //            CreatedOn = DateTime.Now
    //        };

    //        await _dbcontext.SupplierBalances.AddAsync(supplierBalance);
    //        await _dbcontext.SaveChangesAsync();

    //        return supplierBalance;
    //    }
    //    catch (Exception ex)
    //    {
    //        Log.Error("CalculateAndSaveBalance failed: " + ex.Message);
    //        return null;
    //    }
    //}

}
