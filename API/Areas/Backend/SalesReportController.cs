using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using Data.Model.General;
using Data.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Services.Backend.SalesReport;
using Services.Backend.UserManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace API.Areas.Backend.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class SalesReportController : BaseController
    {
        private readonly ISalesReportService<OrderItem> _salesReportService;
        private readonly IMapper _mapper;

        public SalesReportController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            ISalesReportService<OrderItem> salesReportService,
            IMapper mapper,
            IMemoryCache memoryCache) :
             base(options, apiHelper, systemUserService, PermissionTypes.SalesReports)
        {
            base.ControllerName = typeof(SalesReportController).Name;
            _salesReportService = salesReportService;
            _mapper = mapper;
            base._memoryCache = memoryCache;
        }

        #region Sales Report Management

        [HttpGet, Route("api/SalesReport/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid? guid)
        {
            ResponseMapper<OrderItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (guid.HasValue)
                {
                    var item = await _salesReportService.GetByGuid(guid.Value);
                    response.GetById(item);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/SalesReport/GetSalesReportForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetSalesReportForDataTable([FromForm] DateTime? startDate, [FromForm] DateTime? endDate)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var items = await _salesReportService.GetSalesReportForDataTable(startDate, endDate, base.GetDataTableParameters);

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }


        [HttpPost, Route("api/SalesReport/UpdateDisplayOrders")]
        public async Task<IActionResult> UpdateDisplayOrders([FromForm] List<BaseRowOrder> items)
        {
            ResponseMapper<OrderItem> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.DisplayOrder)) { return Ok(accessResponse); }

                if (items.Count > 0)
                {
                    items.ForEach(i => i.UserId = this.UserId);
                    await _salesReportService.UpdateDisplayOrders(items);
                    response.DisplayOrder(true);
                }

                ClearCache();
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        
        [HttpPost, Route("api/SalesReport/GetAllForDropDownList")]
        public async Task<IActionResult> GetAllForDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                var items = await _salesReportService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        private void ClearCache()
        {
            _memoryCache.Remove("SalesReports");
        }

        #endregion

        #region Total Revenue for d,w,m,y

        #region Current Revenue

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForDay")]
        public async Task<IActionResult> GetTotalRevenueForDay()
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForDay();
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForWeek")]
        public async Task<IActionResult> GetTotalRevenueForWeek()
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForWeek();
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForMonth")]
        public async Task<IActionResult> GetTotalRevenueForMonth()
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForMonth();
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForYear")]
        public async Task<IActionResult> GetTotalRevenueForYear()
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForYear();
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        #endregion

        #region Specific Revenue
        [HttpGet, Route("api/SalesReport/GetTotalRevenueForSpecificDay")]
        public async Task<IActionResult> GetTotalRevenueForSpecificDay(DateTime day)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForSpecificDay(day);
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForSpecificWeek")]
        public async Task<IActionResult> GetTotalRevenueForSpecificWeek(int year, int weekNumber)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForSpecificWeek(year, weekNumber);
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForSpecificMonth")]
        public async Task<IActionResult> GetTotalRevenueForSpecificMonth(int year, int month)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForSpecificMonth(year, month);
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTotalRevenueForSpecificYear")]
        public async Task<IActionResult> GetTotalRevenueForSpecificYear(int year)
        {
            ResponseMapper<dynamic> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var totalRevenue = await _salesReportService.GetTotalRevenueForSpecificYear(year);
                response.GetAll(new { totalRevenue });

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        #endregion

        #endregion

        #region Top 3 Item Sold d,w,m,y

        #region Sales Data for Current Day, Week, Month, Year

        [HttpGet, Route("api/SalesReport/GetTopSoldItemsForDay")]
        public async Task<IActionResult> GetTopSoldItemsForDay()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var topSoldItems = await _salesReportService.GetTopSoldItemsForDay();
                response.GetAll(topSoldItems);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetTopSoldItemsForWeek")]
        public async Task<IActionResult> GetTopSoldItemsForWeek()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var topSoldItems = await _salesReportService.GetTopSoldItemsForWeek();
                response.GetAll(topSoldItems);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpGet, Route("api/SalesReport/GetTopSoldItemsForMonth")]
        public async Task<IActionResult> GetTopSoldItemsForMonth()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var topSoldItems = await _salesReportService.GetTopSoldItemsForMonth();
                response.GetAll(topSoldItems);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpGet, Route("api/SalesReport/GetTopSoldItemsForYear")]
        public async Task<IActionResult> GetTopSoldItemsForYear()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var topSoldItems = await _salesReportService.GetTopSoldItemsForYear();
                response.GetAll(topSoldItems);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpGet, Route("api/SalesReport/GetTopSoldItemsForAllTime")]
        public async Task<IActionResult> GetTopSoldItemsForAllTime()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var topSoldItems = await _salesReportService.GetTopSoldItemsForAllTime();
                response.GetAll(topSoldItems);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        #endregion
        
        #region Sales Data for Specific Day, Week, Month, Year

        [HttpGet, Route("api/SalesReport/GetSalesDataForSpecificDay")]
        public async Task<IActionResult> GetSalesDataForSpecificDay(DateTime day)
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var salesData = await _salesReportService.GetSalesDataForSpecificDay(day);
                response.GetAll(salesData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetSalesDataForSpecificWeek")]
        public async Task<IActionResult> GetSalesDataForSpecificWeek(int year, int weekNumber)
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                // ISO 8601 week number calculation
                DateTime jan4 = new DateTime(year, 1, 4); // 4th Jan is always in week 1
                DateTime startOfYear = jan4.AddDays(-((int)jan4.DayOfWeek + 6) % 7); // Get the first Monday of the year
                DateTime weekStart = startOfYear.AddDays((weekNumber - 1) * 7);

                var salesData = await _salesReportService.GetSalesDataForSpecificWeek(weekStart);
                response.GetAll(salesData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }



        [HttpGet, Route("api/SalesReport/GetSalesDataForSpecificMonth")]
        public async Task<IActionResult> GetSalesDataForSpecificMonth(int year, int month)
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var salesData = await _salesReportService.GetSalesDataForSpecificMonth(year, month);
                response.GetAll(salesData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetSalesDataForSpecificYear")]
        public async Task<IActionResult> GetSalesDataForSpecificYear(int year)
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var salesData = await _salesReportService.GetSalesDataForSpecificYear(year);
                response.GetAll(salesData);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        #endregion
        
        #endregion

        #region 2 Sale Days For Week, Month, Year

        [HttpGet, Route("api/SalesReport/GetSaleDaysForWeek")]
        public async Task<IActionResult> GetSaleDaysForWeek()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var saleDays = await _salesReportService.GetSaleDaysForWeek();
                response.GetAll(saleDays);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetSaleDaysForMonth")]
        public async Task<IActionResult> GetSaleDaysForMonth()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var saleDays = await _salesReportService.GetSaleDaysForMonth();
                response.GetAll(saleDays);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetSaleDaysForYear")]
        public async Task<IActionResult> GetSaleDaysForYear()
        {
            ResponseMapper<List<OrderItem>> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var saleDays = await _salesReportService.GetSaleDaysForYear();
                response.GetAll(saleDays);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        #endregion

        #region Peak Hours Controller Methods

        [HttpGet, Route("api/SalesReport/GetPeakHourForDay")]
        public async Task<IActionResult> GetPeakHourForDay()
        {
            ResponseMapper<int> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var peakHour = await _salesReportService.GetPeakHourForDay();
                response.GetAll(peakHour);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetPeakHourForWeek")]
        public async Task<IActionResult> GetPeakHourForWeek()
        {
            ResponseMapper<int> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var peakHour = await _salesReportService.GetPeakHourForWeek();
                response.GetAll(peakHour);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetPeakHourForMonth")]
        public async Task<IActionResult> GetPeakHourForMonth()
        {
            ResponseMapper<int> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var peakHour = await _salesReportService.GetPeakHourForMonth();
                response.GetAll(peakHour);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetPeakHourForYear")]
        public async Task<IActionResult> GetPeakHourForYear()
        {
            ResponseMapper<int> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var peakHour = await _salesReportService.GetPeakHourForYear();
                response.GetAll(peakHour);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/SalesReport/GetPeakHourForAllTime")]
        public async Task<IActionResult> GetPeakHourForAllTime()
        {
            ResponseMapper<int> response = new();

            try
            {
                if (!await base.AccessPermission(PermissionTypes.SalesReports, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var peakHour = await _salesReportService.GetPeakHourForAllTime();
                response.GetAll(peakHour);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        #endregion


    }
}
