$(function () {
    $('#daterange').daterangepicker({
        locale: {
            format: 'YYYY-MM-DD'
        },
        startDate: moment().startOf('month'),
        endDate: moment().endOf('month'),
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    });

    $('#daterange').on('apply.daterangepicker', function (ev, picker) {
        var startDate = picker.startDate.format('YYYY-MM-DD');
        var endDate = picker.endDate.format('YYYY-MM-DD');

        if (startDate === endDate) {
            searchDataTable(startDate, null);
        } else {
            searchDataTable(startDate, endDate);
        }
    });


    fetchTotalRevenue();
    fetchTopSoldItemsForToday();
    fetchTopSoldItemsForWeek();
    fetchTopSoldItemsForMonth();
    fetchTopSoldItemsForYear();

    fetchSaleDaysForWeek();
    fetchSaleDaysForMonth();
    fetchSaleDaysForYear();

    fetchPeakHourForDay();
    fetchPeakHourForWeek();
    fetchPeakHourForMonth();
    fetchPeakHourForYear();
    fetchPeakHourForAllTime();

    clearDataTable();
    searchDataTable();
});

function fetchTotalRevenue() {
    fetchTotalRevenueForDay();
    fetchTotalRevenueForWeek();
    fetchTotalRevenueForMonth();
    fetchTotalRevenueForYear();
}
function searchDataTable(startDate, endDate) {
    if ($.fn.dataTable.isDataTable("#datatable-sales-report")) {
        $("#datatable-sales-report").DataTable().state.clear();
        $("#datatable-sales-report").DataTable().destroy();
    }

    var table = $("#datatable-sales-report").DataTable({
        searching: true,
        ajax: {
            url: getAPIUrl() + "SalesReport/GetSalesReportForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) { getDataTableHeaders(xhr); },
            data: {
                startDate: startDate,
                endDate: endDate
            },
            dataSrc: function (json) {
                checkAPIResponse(json);
                return json.data;
            },
            error: function (error) { fixDTError(error); },
        },
        columns: [
            {
                "data": "id", render: function (data, type, row, meta) {
                    return getRowNumber(meta);
                }
            },
            {
                "data": "createdOn", render: function (value, type, row) {
                    var date = new Date(value);
                    return date.toLocaleDateString('en-US'); 
                }
            },
            {
                "data": "createdOn", render: function (value, type, row) {
                    var date = new Date(value);
                    return date.toLocaleDateString('en-US', { weekday: 'long' }); 
                }
            },
            {
                "data": "priceTypeName"
            },
            {
                "data": "quantity"
            },
            {
                "data": "total", render: function (value, type, row) {
                    return value.toFixed(2);
                }
            },
        ],
        createdRow: function (row, data, index) {
            $(row).attr('data-rowid', data.id);
            $(row).find('td:eq(0)').attr('data-displayorder', row.id);
        },
        columnDefs: [
            {
                targets: [0],
                defaultContent: '',
                render: function (data, type, row, meta) {
                    var displayid = meta.row + meta.settings._iDisplayStart + 1;
                    return `<div data-i="${displayid}" style="cursor: pointer;">${displayid}</div>`;
                },
                orderable: false,
                searchable: false
            },
            { "className": "text-wrap", "targets": "_all" },
        ],
    });
}




function fetchTotalRevenueForDay() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForDay",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var day = moment().format('dddd, MMMM Do'); // Current day format
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#totalRevenueDay');
            list.empty();

            list.append('<li class="list-group-item active">' + day + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}


function fetchTotalRevenueForWeek() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForWeek",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var weekNumber = moment().week();
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#totalRevenueWeek');
            list.empty();
            
            list.append('<li class="list-group-item active">Week ' + weekNumber + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTotalRevenueForMonth() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForMonth",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var monthName = moment().format('MMMM');
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#totalRevenueMonth');
            list.empty();

            list.append('<li class="list-group-item active">Month ' + monthName + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}


function fetchTotalRevenueForYear() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForYear",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var year = moment().format('YYYY');
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#totalRevenueYear');
            list.empty();

            list.append('<li class="list-group-item active">Year ' + year + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}
function fetchTotalRevenueForSpecificDay(day) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForSpecificDay?day=" + day,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#specificRevenueDayData');
            list.empty();

            list.append('<li class="list-group-item active">Selected Day: ' + day + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTotalRevenueForSpecificWeek(year, weekNumber) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForSpecificWeek?year=" + year + "&weekNumber=" + weekNumber,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#specificRevenueWeekData');
            list.empty();

            list.append('<li class="list-group-item active">Selected Week: ' + weekNumber + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTotalRevenueForSpecificMonth(year, month) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForSpecificMonth?year=" + year + "&month=" + month,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#specificRevenueMonthData');
            list.empty();

            list.append('<li class="list-group-item active">Selected Month: ' + month + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTotalRevenueForSpecificYear(year) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTotalRevenueForSpecificYear?year=" + year,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var revenue = response.data.totalRevenue.toFixed(2);
            var list = $('#specificRevenueYearData');
            list.empty();

            list.append('<li class="list-group-item active">Selected Year: ' + year + '</li>');
            list.append('<li class="list-group-item">Total Revenue: ' + revenue + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTopSoldItemsForToday() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTopSoldItemsForDay",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var day = moment().format('dddd, MMMM Do'); // Current day format
            var items = response.data;
            var list = $('#topSoldItemsDay');
            list.empty();

            list.append('<li class="list-group-item active">' + day + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTopSoldItemsForWeek() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTopSoldItemsForWeek",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var weekNumber = moment().week();
            var items = response.data;
            var list = $('#topSoldItemsWeek');
            list.empty();

            list.append('<li class="list-group-item active">Week ' + weekNumber + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTopSoldItemsForMonth() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTopSoldItemsForMonth",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var monthName = moment().format('MMMM');
            var items = response.data;
            var list = $('#topSoldItemsMonth');
            list.empty();

            list.append('<li class="list-group-item active">Month ' + monthName + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchTopSoldItemsForYear() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetTopSoldItemsForYear",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var year = moment().format('YYYY');
            var items = response.data;
            var list = $('#topSoldItemsYear');
            list.empty();

            list.append('<li class="list-group-item active">Year ' + year + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchSalesDataForSpecificDay(day) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSalesDataForSpecificDay?day=" + day,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#specificDaySalesData');
            list.empty();

            list.append('<li class="list-group-item active">Day: ' + day + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchSalesDataForSpecificWeek(year, weekNumber) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSalesDataForSpecificWeek?year=" + year + "&weekNumber=" + weekNumber,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#specificWeekSalesData');
            list.empty();

            list.append('<li class="list-group-item active">Year: ' + year + ', Week: ' + weekNumber + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchSalesDataForSpecificMonth(year, month) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSalesDataForSpecificMonth?year=" + year + "&month=" + month,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#specificMonthSalesData');
            list.empty();

            list.append('<li class="list-group-item active">Year: ' + year + ', Month: ' + month + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchSalesDataForSpecificYear(year) {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSalesDataForSpecificYear?year=" + year,
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#specificYearSalesData');
            list.empty();

            list.append('<li class="list-group-item active">Year: ' + year + '</li>');
            items.forEach(function (item) {
                list.append('<li class="list-group-item">' + item.priceTypeName + ': ' + item.quantity + '</li>');
            });
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}



function fetchSaleDaysForWeek() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSaleDaysForWeek",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#saleDaysForWeek');
            list.empty();

            if (items.length === 0) {
                list.append('<li class="list-group-item">No sales data available for this week.</li>');
            } else {
                list.append('<li class="list-group-item active">Top 2 Sale Days of the Week</li>');
                items.forEach(function (item) {
                    var dayName = moment(item.createdOn).format('dddd'); // Get day name
                    var date = moment(item.createdOn).format('DD MMM YYYY'); // Get formatted date
                    list.append('<li class="list-group-item">' + dayName + ', ' + date + ': ' + item.total + ' sales</li>');
                });
            }
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchSaleDaysForMonth() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSaleDaysForMonth",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#saleDaysForMonth');
            list.empty();

            if (items.length === 0) {
                list.append('<li class="list-group-item">No sales data available for this month.</li>');
            } else {
                list.append('<li class="list-group-item active">Top 2 Sale Days of the Month</li>');
                items.forEach(function (item) {
                    var dayName = moment(item.createdOn).format('dddd'); // Get day name
                    var date = moment(item.createdOn).format('DD MMM YYYY'); // Get formatted date
                    list.append('<li class="list-group-item">' + dayName + ', ' + date + ': ' + item.total + ' sales</li>');
                });
            }
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchSaleDaysForYear() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetSaleDaysForYear",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var items = response.data;
            var list = $('#saleDaysForYear');
            list.empty();

            if (items.length === 0) {
                list.append('<li class="list-group-item">No sales data available for this year.</li>');
            } else {
                list.append('<li class="list-group-item active">Top 2 Sale Days of the Year</li>');
                items.forEach(function (item) {
                    var dayName = moment(item.createdOn).format('dddd'); // Get day name
                    var date = moment(item.createdOn).format('DD MMM YYYY'); // Get formatted date
                    list.append('<li class="list-group-item">' + dayName + ', ' + date + ': ' + item.total + ' sales</li>');
                });
            }
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchPeakHourForDay() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetPeakHourForDay",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var peakHour = response.data;
            var list = $('#peakHourForDay');
            list.empty();
            list.append('<li class="list-group-item active">Peak Hour for Today</li>');
            list.append('<li class="list-group-item">' + moment().format('HH:00') + ': ' + peakHour + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}


function fetchPeakHourForWeek() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetPeakHourForWeek",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var peakHour = response.data;
            var list = $('#peakHourForWeek');
            list.empty();
            list.append('<li class="list-group-item active">Peak Hour for This Week</li>');
            list.append('<li class="list-group-item">' + moment().format('HH:00') + ': ' + peakHour + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchPeakHourForMonth() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetPeakHourForMonth",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var peakHour = response.data;
            var list = $('#peakHourForMonth');
            list.empty();
            list.append('<li class="list-group-item active">Peak Hour for This Month</li>');
            list.append('<li class="list-group-item">' + moment().format('HH:00') + ': ' + peakHour + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchPeakHourForYear() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetPeakHourForYear",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var peakHour = response.data;
            var list = $('#peakHourForYear');
            list.empty();
            list.append('<li class="list-group-item active">Peak Hour for This Year</li>');
            list.append('<li class="list-group-item">' + moment().format('HH:00') + ': ' + peakHour + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}

function fetchPeakHourForAllTime() {
    $.ajax({
        url: getAPIUrl() + "SalesReport/GetPeakHourForAllTime",
        type: "GET",
        xhrFields: { withCredentials: true },
        beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (response) {
            checkAPIResponse(response);
            var peakHour = response.data;
            var list = $('#peakHourForAllTime');
            list.empty();
            list.append('<li class="list-group-item active">Peak Hour for All Time</li>');
            list.append('<li class="list-group-item">' + moment().format('HH:00') + ': ' + peakHour + '</li>');
        },
        error: function (error) {
            fixDTError(error);
        }
    });
}


document.addEventListener('DOMContentLoaded', function () {
    // Get the current date and day name
    var today = new Date();
    var dayOptions = { weekday: 'long' };
    var dateOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    var dayName = today.toLocaleDateString(undefined, dayOptions);
    var dateString = today.toLocaleDateString(undefined, dateOptions);

    // Set the day name and date in the small elements
    document.getElementById('currentDayRevenue').textContent = dayName + ', ' + dateString;
    document.getElementById('currentDayTopItems').textContent = dayName + ', ' + dateString;
});
