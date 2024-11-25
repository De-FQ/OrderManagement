using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Utility.API;
using Utility.Models.Base;
using Services.Base;
using AutoMapper;
using Serilog;
using Data.Model.General;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Services.Backend.SalesReport
{
    public class SalesReportService : BaseService, ISalesReportService<OrderItem>
    {
        private readonly IMapper _mapper;
        private readonly string ServiceName = "SalesReportService";

        public SalesReportService(ApplicationDbContext dbcontext, IMapper mapper) : base(dbcontext)
        {
            _mapper = mapper;
        }

        public Task<OrderItem> Create(OrderItem model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(string name, Guid? guid = null)
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetAllForDropDownList(bool isEnglish = true)
        {
            var items = await _dbcontext.OrderItems.Where(x => !x.Deleted).Select(x => new
            {
                x.Id,
                Name = isEnglish ? x.PriceTypeName : x.PriceTypeName
            }).AsNoTracking().ToListAsync();

            return items;
        }

        private async Task<long> GetNextDisplayOrder()
        {
            var item = await _dbcontext.OrderItems.OrderByDescending(x => x.DisplayOrder).AsNoTracking().FirstOrDefaultAsync();
            if (item != null)
            {
                return item.DisplayOrder + 1;
            }
            return 1;
        }

        public async Task<List<OrderItem>> GetOrderItemsByRoleId(int roleId, bool isEnglish, int userId)
        {
            // Example logic: Fetch order items based on roleId, isEnglish, and userId
            var orderItems = await _dbcontext.OrderItems
                .Where(o => o.Id == roleId && o.Active)
                .ToListAsync();

            return orderItems;
        }


        public async Task<OrderItem> GetByGuid(Guid guid)
        {
            var item = await _dbcontext.OrderItems
            .Where(x => x.Guid == guid)
            .AsNoTracking()
            .FirstOrDefaultAsync();

            if (item is null)
            {
                return new OrderItem();
            }
            else
            {
                return item;
            }
        }

        public async Task<OrderItem> GetById(long id)
        {
            var item = await _dbcontext.OrderItems
                .Where(x => x.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (item is null)
            {
                return new OrderItem();
            }
            else
            {
                return item;
            }
        }

        public async Task<DataTableResult<List<OrderItem>>> GetSalesReportForDataTable(DateTime? startDate, DateTime? endDate, DataTableParam param)
        {
            DataTableResult<List<OrderItem>> result = new() { Draw = param.Draw };
            try
            {
                var items = _dbcontext.OrderItems.AsNoTracking().AsQueryable();

                // Apply date range or single date filtering
                if (startDate.HasValue)
                {
                    if (endDate.HasValue)
                    {
                        items = items.Where(x => x.CreatedOn >= startDate.Value && x.CreatedOn <= endDate.Value);
                    }
                    else
                    {
                        items = items.Where(x => x.CreatedOn.Date == startDate.Value.Date);
                    }
                }

                var records = items.Select(x => new OrderItem()
                {
                    Id = x.Id,
                    Guid = x.Guid,
                    OrderId = x.OrderId,
                    PriceTypeName = x.PriceTypeName,
                    Quantity = x.Quantity,
                    Total = x.Total,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedOn = x.ModifiedOn,
                });

                // Sales Report Search
                if (!string.IsNullOrEmpty(param.SearchValue))
                {
                    var searchValue = param.SearchValue.ToLower();
                    records = records.Where(obj =>
                        obj.PriceTypeName.ToLower().Contains(searchValue));
                }

                if (!string.IsNullOrEmpty(param.SortColumn) && !string.IsNullOrEmpty(param.SortColumnDirection))
                {
                    records = records.OrderBy(param.SortColumn + " " + param.SortColumnDirection);
                }
                else
                {
                    records = records.OrderBy(x => x.CreatedOn);
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

        public Task<bool> ToggleActive(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(OrderItem model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDisplayOrders(List<BaseRowOrder> rowOrders)
        {
            bool modified = false;
            var orderList = rowOrders.Select(x => x.Id).ToList();
            var items = await _dbcontext.OrderItems.Where(x => orderList.Contains(x.Id)).ToListAsync();

            foreach (var item in items)
            {
                var row = rowOrders.FirstOrDefault(p => p.Id == item.Id);
                if (row != null)
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
            var data = await _dbcontext.OrderItems.Where(x => x.Guid == guid).FirstOrDefaultAsync();
            if (data != null)
            {
                data.DisplayOrder = num;
                data.ModifiedOn = DateTime.Now;
                return await _dbcontext.SaveChangesAsync() > 0;
            }

            return false;
        }


        #region Total Revenue d,w,m,y

        #region Current Revenue
        public async Task<decimal> GetTotalRevenueForDay()
        {
            var today = DateTime.UtcNow.Date;
            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= today && x.CreatedOn < today.AddDays(1))
                .SumAsync(x => x.Total);

            return totalRevenue;
        }
        public async Task<decimal> GetTotalRevenueForWeek()
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }
        public async Task<decimal> GetTotalRevenueForMonth()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfMonth && x.CreatedOn < endOfMonth)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }

        public async Task<decimal> GetTotalRevenueForYear()
        {
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfYear && x.CreatedOn < endOfYear)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }
        #endregion

        #region Specific Revenue
        public async Task<decimal> GetTotalRevenueForSpecificDay(DateTime day)
        {
            var startOfDay = day.Date;
            var endOfDay = startOfDay.AddDays(1);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfDay && x.CreatedOn < endOfDay)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }

        public async Task<decimal> GetTotalRevenueForSpecificWeek(int year, int weekNumber)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var startOfWeek = firstDayOfYear.AddDays((weekNumber - 1) * 7);
            var endOfWeek = startOfWeek.AddDays(7);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }

        public async Task<decimal> GetTotalRevenueForSpecificMonth(int year, int month)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfMonth && x.CreatedOn < endOfMonth)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }

        public async Task<decimal> GetTotalRevenueForSpecificYear(int year)
        {
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var totalRevenue = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfYear && x.CreatedOn < endOfYear)
                .SumAsync(x => x.Total);

            return totalRevenue;
        }
        #endregion

        #endregion

        #region Top 3 Sold Items d,w,m,y,all time.
        #region Current Top 3 Sold Items
        public async Task<List<OrderItem>> GetTopSoldItemsForDay(int top = 3)
        {
            var today = DateTime.UtcNow.Date;
            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= today && x.CreatedOn < today.AddDays(1))
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }

        public async Task<List<OrderItem>> GetTopSoldItemsForWeek(int top = 3)
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek)
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }

        public async Task<List<OrderItem>> GetTopSoldItemsForMonth(int top = 3)
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfMonth && x.CreatedOn < endOfMonth)
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }
        public async Task<List<OrderItem>> GetTopSoldItemsForYear(int top = 3)
        {
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfYear && x.CreatedOn < endOfYear)
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }
        public async Task<List<OrderItem>> GetTopSoldItemsForAllTime(int top = 3)
        {
            var topSoldItems = await _dbcontext.OrderItems
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }
        #endregion

        #region Specific Top 3 Items

        public async Task<List<OrderItem>> GetSalesDataForSpecificDay(DateTime selectedDay, int top = 3)
        {
            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= selectedDay.Date && x.CreatedOn < selectedDay.Date.AddDays(1))
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }

        public async Task<List<OrderItem>> GetSalesDataForSpecificWeek(DateTime selectedWeek, int top = 3)
        {
            var startOfWeek = selectedWeek.Date.AddDays(-(int)selectedWeek.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek)
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }

        public async Task<List<OrderItem>> GetSalesDataForSpecificMonth(int year, int month, int top = 3)
        {
            var startOfMonth = new DateTime(year, month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfMonth && x.CreatedOn < endOfMonth)
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }

        public async Task<List<OrderItem>> GetSalesDataForSpecificYear(int year, int top = 3)
        {
            var startOfYear = new DateTime(year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var topSoldItems = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfYear && x.CreatedOn < endOfYear)
                .GroupBy(x => x.PriceTypeName)
                .Select(g => new OrderItem
                {
                    PriceTypeName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(top)
                .ToListAsync();

            return topSoldItems;
        }

        #endregion

        #endregion


        #region 2 Sale Days For Week, Month, Year

        public async Task<List<OrderItem>> GetSaleDaysForWeek()
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var saleDays = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek)
                .GroupBy(x => x.CreatedOn.Date)
                .Select(g => new OrderItem
                {
                    CreatedOn = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Total)
                .Take(2)
                .ToListAsync();

            return saleDays;
        }

        public async Task<List<OrderItem>> GetSaleDaysForMonth()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var saleDays = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfMonth && x.CreatedOn < endOfMonth)
                .GroupBy(x => x.CreatedOn.Date)
                .Select(g => new OrderItem
                {
                    CreatedOn = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Total)
                .Take(2)
                .ToListAsync();

            return saleDays;
        }

        public async Task<List<OrderItem>> GetSaleDaysForYear()
        {
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var saleDays = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfYear && x.CreatedOn < endOfYear)
                .GroupBy(x => x.CreatedOn.Date)
                .Select(g => new OrderItem
                {
                    CreatedOn = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Total = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.Total)
                .Take(2)
                .ToListAsync();

            return saleDays;
        }

        #endregion

        #region Peak Hours d,w,m,y,alltime
        public async Task<int> GetPeakHourForDay()
        {
            var today = DateTime.UtcNow.Date;
            var peakHour = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= today && x.CreatedOn < today.AddDays(1))
                .GroupBy(x => x.CreatedOn.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalSales = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalSales)
                .FirstOrDefaultAsync();

            return peakHour?.Hour ?? 0;
        }

        public async Task<int> GetPeakHourForWeek()
        {
            var startOfWeek = DateTime.UtcNow.Date.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var peakHour = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfWeek && x.CreatedOn < endOfWeek)
                .GroupBy(x => x.CreatedOn.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalSales = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalSales)
                .FirstOrDefaultAsync();

            return peakHour?.Hour ?? 0;
        }

        public async Task<int> GetPeakHourForMonth()
        {
            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);

            var peakHour = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfMonth && x.CreatedOn < endOfMonth)
                .GroupBy(x => x.CreatedOn.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalSales = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalSales)
                .FirstOrDefaultAsync();

            return peakHour?.Hour ?? 0;
        }

        public async Task<int> GetPeakHourForYear()
        {
            var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);
            var endOfYear = startOfYear.AddYears(1);

            var peakHour = await _dbcontext.OrderItems
                .Where(x => x.CreatedOn >= startOfYear && x.CreatedOn < endOfYear)
                .GroupBy(x => x.CreatedOn.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalSales = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalSales)
                .FirstOrDefaultAsync();

            return peakHour?.Hour ?? 0;
        }
        public async Task<int> GetPeakHourForAllTime()
        {
            var peakHour = await _dbcontext.OrderItems
                .GroupBy(x => x.CreatedOn.Hour)
                .Select(g => new
                {
                    Hour = g.Key,
                    TotalSales = g.Sum(x => x.Total)
                })
                .OrderByDescending(x => x.TotalSales)
                .FirstOrDefaultAsync();

            return peakHour?.Hour ?? 0;
        }

        #endregion

    }
}