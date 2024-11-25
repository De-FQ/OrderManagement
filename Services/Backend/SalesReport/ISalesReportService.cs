using Utility.API;
using Utility.Models.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.General;
using Services.Base;

namespace Services.Backend.SalesReport
{
    public interface ISalesReportService<T> : IBaseService<T>
    {
        Task<DataTableResult<List<OrderItem>>> GetSalesReportForDataTable(DateTime? startDate, DateTime? endDate, DataTableParam param);
        

        #region Total Revenue d,w,m,y
        Task<decimal> GetTotalRevenueForDay();
        Task<decimal> GetTotalRevenueForWeek();
        Task<decimal> GetTotalRevenueForMonth();
        Task<decimal> GetTotalRevenueForYear();
        Task<decimal> GetTotalRevenueForSpecificDay(DateTime day);
        Task<decimal> GetTotalRevenueForSpecificWeek(int year, int weekNumber);
        Task<decimal> GetTotalRevenueForSpecificMonth(int year, int month);
        Task<decimal> GetTotalRevenueForSpecificYear(int year);
        #endregion

        #region Top 3 Items Sold d,w,m,y,all time
        Task<List<OrderItem>> GetTopSoldItemsForDay(int top = 3);
        Task<List<OrderItem>> GetTopSoldItemsForWeek(int top = 3);
        Task<List<OrderItem>> GetTopSoldItemsForMonth(int top = 3);
        Task<List<OrderItem>> GetTopSoldItemsForYear(int top = 3);
        Task<List<OrderItem>> GetTopSoldItemsForAllTime(int top = 3);
        Task<List<OrderItem>> GetSalesDataForSpecificDay(DateTime selectedDay, int top = 3);
        Task<List<OrderItem>> GetSalesDataForSpecificWeek(DateTime selectedWeek, int top = 3);
        Task<List<OrderItem>> GetSalesDataForSpecificMonth(int year, int month, int top = 3);
        Task<List<OrderItem>> GetSalesDataForSpecificYear(int year, int top = 3);

        #endregion

        #region Top 2 Sale Days

        Task<List<OrderItem>> GetSaleDaysForWeek();
        Task<List<OrderItem>> GetSaleDaysForMonth();
        Task<List<OrderItem>> GetSaleDaysForYear();

        #endregion

        #region Peak Hours d,w,m,y,alltime

        Task<int> GetPeakHourForDay();
        Task<int> GetPeakHourForWeek();
        Task<int> GetPeakHourForMonth();
        Task<int> GetPeakHourForYear();
        Task<int> GetPeakHourForAllTime();

        #endregion

    }
}
