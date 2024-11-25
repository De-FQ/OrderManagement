$(function () {
    if (pdfMake != undefined) {
        pdfMake.fonts = {
            ReadexPro: {
                normal: 'ReadexPro-Regular.ttf',
                bold: 'ReadexPro-Bold.ttf',
                italics: 'ReadexPro-Regular.ttf',
                bolditalics: 'ReadexPro-Bold.ttf'
            }
        };
    }
}); 

// Disable search and ordering by default
$.extend($.fn.dataTable.defaults, {

    responsive: {
        details: {
            type: 'none',
            target: ".responsive-view-btn"
        }
    },
    searching: true,
    paging: true,
    info: true,
    select: true,
    serverSide: true, 
    cache: false,
    filter: false, //Search Box
    orderMulti: false,
    stateSave: true,
    ordering: false, //disable ordering
    order: [[0, "desc"]], 
    datatype: "json",
    processing: false,
    language: {
        'loadingRecords': '&nbsp;',
        'processing': '<div class="spinner"></div>'
    },
    dom: '<"row"<"col-6"B><"col-6"f><"col-lg-12 ResponsiveTable"t><"col-lg-4 mt-lg-3 mt-0 text-lg-start text-center"l><"col-lg-4 mt-lg-3 mt-0 text-center"i><"col-lg-4 text-lg-end text-center"p>>',
    buttons:
        [
            {
                extend: 'excelHtml5',
                text: '<i class="fa-regular fa-file-excel text-light"></i> <span class="text-light">Excel</span>',
                titleAttr: 'Excel',
                action: newexportaction,
                exportOptions: {
                    columns: ':not(:last-child)',
                },
                filename: function () {
                    var d = new Date();
                    var l = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
                    var n = d.getHours() + "-" + d.getMinutes() + "-" + d.getSeconds();
                    return document.title + ' List_' + l + ' ' + n;
                }
            },
            //{
            //    extend: 'pdfHtml5',
            //    text: '<i class="fa-regular fa-file-pdf text-light"></i> <span class="text-light">PDF</span>',
            //    titleAttr: 'PDF',
            //    pageSize: 'A3',
            //    orientation: 'landscape',
            //    action: newexportaction,
            //    exportOptions: {
            //        columns: ':not(:last-child)',
            //    },
            //    customize: function (doc) {
            //        doc.defaultStyle.font = 'ReadexPro';
            //    },
            //    filename: function () {
            //        var d = new Date();
            //        var l = d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDate();
            //        var n = d.getHours() + "-" + d.getMinutes() + "-" + d.getSeconds();
            //        return 'List_' + l + ' ' + n;
            //    }
            //},

        ],
    lengthMenu: [[5, 10, 15, 25, 50, 100, 200, 250], [5, 10, 15, 25, 50, 100, 200, 250]],
   
    complete: function (data, textStatus) {
        showLog(data.responseJSON);
    },
    error: function (error) {
        showLog('error' + error);
    },
});


function newexportaction(e, dt, button, config) {
    var self = this;
    var oldStart = dt.settings()[0]._iDisplayStart;
    dt.one('preXhr', function (e, s, data) {
        // Just this once, load all data from the server...
        data.start = 0;
        data.length = 2147483647;
        dt.one('preDraw', function (e, settings) {
            // Call the original action function
            if (button[0].className.indexOf('buttons-copy') >= 0) {
                $.fn.dataTable.ext.buttons.copyHtml5.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-excel') >= 0) {
                $.fn.dataTable.ext.buttons.excelHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.excelHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.excelFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-csv') >= 0) {
                $.fn.dataTable.ext.buttons.csvHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.csvHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.csvFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-pdf') >= 0) {
                $.fn.dataTable.ext.buttons.pdfHtml5.available(dt, config) ?
                    $.fn.dataTable.ext.buttons.pdfHtml5.action.call(self, e, dt, button, config) :
                    $.fn.dataTable.ext.buttons.pdfFlash.action.call(self, e, dt, button, config);
            } else if (button[0].className.indexOf('buttons-print') >= 0) {
                $.fn.dataTable.ext.buttons.print.action(e, dt, button, config);
            }
            dt.one('preXhr', function (e, s, data) {
                // DataTables thinks the first item displayed is index 0, but we're not drawing that.
                // Set the property to what it was before exporting.
                settings._iDisplayStart = oldStart;
                data.start = oldStart;
            });
            // Reload the grid with the original page. Otherwise, API functions like table.cell(this) don't work properly.
            setTimeout(dt.ajax.reload, 0);
            // Prevent rendering of the full data to the DOM
            return false;
        });
    });
    // Requery the server with the new one-time export settings
    dt.ajax.reload();
}; 

 