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
using System.Threading.Tasks;
using Utility.API;
using Utility.Models.Admin.DTO;
using System.Linq.Dynamic.Core;
using Utility.Models.Base;

namespace Services.Backend.InventoryManagement.Stocks
{
    public class StockService : BaseService, IStockService<Stock>
    {
        private readonly IMapper _mapper;
        private readonly string ServiceName = "StockService";

        public StockService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }


        public async Task<Stock> GetByGuid(Guid guid)
        {
            try
            {
                return await _dbcontext.Stocks
                    .AsNoTracking()
                    .Where(x => x.Guid == guid)
                    .FirstOrDefaultAsync() ?? new Stock();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetByGuid failed: " + ex.Message);
                return new Stock();
            }
        }

        public async Task<Stock> GetById(long id)
        {
            try
            {
                return await _dbcontext.Stocks
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync() ?? new Stock();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetById failed: " + ex.Message);
                return new Stock();
            }
        }

        public async Task<Stock> Create(Stock model)
        {
            try
            {
                model.Guid = Guid.NewGuid();
                model.CreatedOn = DateTime.Now;
                model.DisplayOrder = await GetNextDisplayOrder();
                await _dbcontext.Stocks.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> Create failed: " + ex.Message);
                return model;
            }
        }

        public async Task<bool> Update(Stock model)
        {
            try
            {
                var data = await _dbcontext.Stocks
                    .Where(x => x.Guid == model.Guid)
                    .FirstOrDefaultAsync();

                if (data is not null)
                {
                    data.InventoryItemId = model.InventoryItemId;
                    data.Unit = model.Unit;
                    data.NetUnitCost = model.NetUnitCost;
                    data.CompanyCostMargin = model.CompanyCostMargin;
                    data.OldQuantity = model.OldQuantity;
                    data.NewQuantity = model.NewQuantity;
                    data.TotalQuantity = model.TotalQuantity;
                    data.TotalUnitNetCost = model.TotalUnitNetCost;
                    data.TotalUnitCompanyPrice = model.TotalUnitCompanyPrice;
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

        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.Stocks
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
                var data = await _dbcontext.Stocks
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

        public async Task<DataTableResult<List<StockDto>>> GetStocksForDataTable(DataTableParam param, int? inventoryItemId)
        {
            var result = new DataTableResult<List<StockDto>> { Draw = param.Draw };
            try
            {
                var items = _dbcontext.Stocks.AsNoTracking().AsQueryable();

                if (inventoryItemId.HasValue)
                {
                    items = items.Where(i => i.InventoryItemId == inventoryItemId.Value);
                }

                var records = items.Select(x => new StockDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    InventoryItemId = x.InventoryItemId,
                    Unit = x.Unit,
                    NetUnitCost = x.NetUnitCost,
                    CompanyCostMargin = x.CompanyCostMargin,
                    OldQuantity = x.OldQuantity,
                    NewQuantity = x.NewQuantity,
                    TotalQuantity = x.TotalQuantity,
                    TotalUnitNetCost = x.TotalUnitNetCost,
                    TotalUnitCompanyPrice = x.TotalUnitCompanyPrice,
                    DisplayOrder = x.DisplayOrder,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    Active = x.Active,
                    Deleted = x.Deleted
                });

                // Stock Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var searchValue = param.SearchValue.ToLower();
                    records = records.Where(obj =>
                        obj.Unit.Contains(searchValue) ||
                        obj.TotalQuantity.ToString().Contains(searchValue));
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
                Log.Error(ServiceName + "--> GetStocksForDataTable failed: " + err.Message);
                result.Error = err;
            }
            return result;
        }

        public async Task<int> GetNextDisplayOrder()
        {
            try
            {
                return await _dbcontext.Stocks.CountAsync() + 1;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetNextDisplayOrder failed: " + ex.Message);
                return 1;
            }
        }

        public async Task<List<Stock>> GetStocksByInventoryItemId(int inventoryItemId)
        {
            try
            {
                return await _dbcontext.Stocks
                    .Where(s => s.InventoryItemId == inventoryItemId && !s.Deleted)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetStocksByInventoryItemId failed: " + ex.Message);
                return new List<Stock>();
            }
        }

        public async Task<bool> Exists(string name, Guid? guid = null)
        {
            try
            {
                var result = await _dbcontext.Stocks
                    .AsNoTracking()
                    .Select(x => new { x.Guid, x.InventoryItem.Name })
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

        public async Task<bool> UpdateDisplayOrder(Guid guid, int num = 0)
        {
            try
            {
                var data = await _dbcontext.Stocks
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

        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> items)
        {
            try
            {
                bool modified = false;
                var orderList = items.Select(x => x.Id).ToList();
                var stocks = await _dbcontext.Stocks
                    .Where(x => orderList.Contains(x.Id))
                    .ToListAsync();

                foreach (var stock in stocks)
                {
                    var row = items.FirstOrDefault(p => p.Id == stock.Id);
                    if (row is not null)
                    {
                        modified = true;
                        stock.ModifiedBy = row.UserId;
                        stock.ModifiedOn = DateTime.Now;
                        stock.DisplayOrder = (int)row.DisplayOrder;
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

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            try
            {
                var stocks = await _dbcontext.Stocks
                    .Where(x => !x.Deleted)
                    .Select(x => new
                    {
                        x.Id,
                        Name = isEnglish ? x.InventoryItem.Name : x.InventoryItem.Name
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return stocks;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "--> GetAllForDropDownList failed: " + ex.Message);
                return null;
            }
        }
    }
}