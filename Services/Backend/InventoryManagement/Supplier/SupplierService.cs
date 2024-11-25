using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Utility.API;
using Utility.Models.Base;
using Services.Base;
using AutoMapper;
using Serilog;
using Data.Model.InventoryManagement;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Utility.Models.Admin.DTO;

namespace Services.Backend.Inventory
{
    public class SupplierService : BaseService, ISupplierService<Supplier>
    {
        private readonly IMapper _mapper;
        private readonly string ServiceName = "SupplierService";

        public SupplierService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public async Task<bool> Exists(string name, Guid? guid = null)
        {
            var result = await _dbcontext.Suppliers
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

        public async Task<Supplier> GetByGuid(Guid guid)
        {
            var item = await _dbcontext.Suppliers.Where(x => x.Guid == guid).AsNoTracking().FirstOrDefaultAsync();
            if (item is null) { return new Supplier(); } else { return item; }
        }

        public async Task<Supplier> GetById(long id)
        {
            var item = await _dbcontext.Suppliers.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (item is null) { return new Supplier(); } else { return item; }
        }

        public async Task<Supplier> Create(Supplier model)
        {
            try
            {
                model.Guid = Guid.NewGuid();
                model.CreatedOn = DateTime.Now;
                await _dbcontext.Suppliers.AddAsync(model);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            return model;
        }

        public async Task<bool> Update(Supplier model)
        {
            try
            {
                var data = await _dbcontext.Suppliers.Where(x => x.Guid == model.Guid).FirstOrDefaultAsync();
                if (data is not null)
                {
                    data.Name = model.Name;
                    data.Address = model.Address;
                    data.Contact = model.Contact;
                    data.ModifiedBy = model.ModifiedBy;
                    data.ModifiedOn = DateTime.Now;

                    bool result = await _dbcontext.SaveChangesAsync() > 0;
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

        public async Task<bool> Delete(Guid guid)
        {
            try
            {
                var data = await _dbcontext.Suppliers.Where(x => x.Guid == guid).FirstOrDefaultAsync();
                if (data is not null)
                {
                    _dbcontext.Remove(data);
                    return await _dbcontext.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + "-->Delete action Failed Reason: " + ex.Message.ToString());
            }
            return false;
        }

        public async Task<DataTableResult<List<SupplierDto>>> GetSupplierForDataTable(DataTableParam param)
        {
            DataTableResult<List<SupplierDto>> result = new() { Draw = param.Draw };
            try
            {
                var items = _dbcontext.Suppliers.AsNoTracking().AsQueryable();
                var records = items.Select(x => new SupplierDto()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    Name = x.Name,
                    Address = x.Address,
                    Contact = x.Contact,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                    Active = x.Active,
                    Deleted = x.Deleted
                });

                // Supplier Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var searchValue = param.SearchValue.ToLower();
                    records = records.Where(obj =>
                        obj.Name.Contains(searchValue) ||
                        obj.Address.Contains(searchValue) ||
                        obj.Contact.Contains(searchValue));
                }

                // Sorting
                if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
                {
                    records = records.OrderBy(param.SortColumn + " " + param.SortColumnDirection);
                }
                else
                {
                    records = records.OrderBy(x => x.Name);
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

        public async Task<bool> ToggleActive(Guid guid)
        {
            var data = await _dbcontext.Suppliers.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.ModifiedOn = DateTime.Now;
                data.Active = !data.Active;
                bool result = await _dbcontext.SaveChangesAsync() > 0;

                return result;
            }
            return false;
        }

        public async Task<bool> UpdateDisplayOrder(Guid guid, int num = 0)
        {
            var data = await _dbcontext.Suppliers.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data is not null)
            {
                data.DisplayOrder = num;
                data.ModifiedOn = DateTime.Now;
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            bool modified = false;
            var orderList = rowOrders.Select(x => x.Id).ToList();
            var items = await _dbcontext.Suppliers.Where(x => orderList.Contains(x.Id)).ToListAsync();

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

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            var items = await _dbcontext.Suppliers.Where(x => x.Deleted == false).Select(x => new
            {
                x.Id,
                Name = isEnglish ? x.Name : x.Name
            }).AsNoTracking().ToListAsync();

            return items;
        }
    }
}
