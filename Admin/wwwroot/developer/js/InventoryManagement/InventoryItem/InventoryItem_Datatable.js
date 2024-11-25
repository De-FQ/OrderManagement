

//$(function () {
//    loadSupplier();
//    clearDataTable(); 

//    $('#supplierFilter').change(function () {
//        searchDataTable(); 
//    });
//});

//function loadSupplier() {
//    $.ajax({
//        url: getAPIUrl() + 'Common/ForSupplierDropDownList',
//        type: 'GET',
//        xhrFields: { withCredentials: true },
//        success: function (response) {
//            if (response.success) {
//                let supplierFilter = $('#supplierFilter');
//                supplierFilter.empty();
//                supplierFilter.append('<option value="">Select Supplier</option>');
//                $.each(response.data, function (index, item) {
//                    supplierFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
//                });
//            }
//        },
//        error: function (error) {
//            console.error('Error loading suppliers:', error);
//        }
//    });
//}

//function searchDataTable() {
//    var selectedSupplierId = $('#supplierFilter').val();

//    if (!selectedSupplierId) {
//        clearDataTable(); 
//        return;
//    }

//    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
//        $("#datatable-default-").DataTable().state.clear();
//        $("#datatable-default-").DataTable().destroy();
//    }

//    var table = $("#datatable-default-").DataTable({
//        searching: true,
//        ordering: true,
//        ajax: {
//            url: getAPIUrl() + "InventoryItem/GetInventoryItemsForDataTable",
//            type: "POST",
//            xhrFields: { withCredentials: true },
//            beforeSend: function (xhr) {
//                getDataTableHeaders(xhr);
//            },
//            data: function (d) {
//                d.supplierId = selectedSupplierId;
//            },
//            dataSrc: function (json) {
//                console.log(json);
//                checkAPIResponse(json);
//                hideTableColumn(table, [{ col: 1, visible: allowActive }]);
//                return json.data;
//            },
//            error: function (error) {
//                fixDTError(error);
//            }
//        },
//        columns: [
//            { "data": "displayOrder", "name": "DisplayOrder" },
//            {
//                "data": "active", render: function (data, type, row) {
//                    return addCheckedActionGuid('InventoryItem/ToggleActive', row);
//                }
//            },
//            { "data": "name" },
//            { "data": "description" },
//            { "data": "unit" },
//            { "data": "unitCost" },
//            { "data": "quantity" },
//            { "data": "costPrice" },
//            {
//                "data": null, "name": "Actions", render: function (data, type, row) {
//                    return `${addDropDownMenuOptions("InventoryItem", "/InventoryItem/", row, { edit: allowEdit, delete: allowDelete })}`;
//                }
//            }
//        ],
//        createdRow: function (row, data, index) {
//            $(row).attr('data-rowid', data.id);
//            $(row).find('td:eq(0)').attr('data-displayorder', data.displayOrder);
//            $(row).find('[data-plugin-ios-switch]').themePluginIOS7Switch();
//        },
//        columnDefs: [
//            {
//                targets: [0],
//                defaultContent: '',
//                render: function (data, type, row, meta) {
//                    var displayid = meta.row + meta.settings._iDisplayStart + 1;
//                    return `<div data-i="${displayid}" style="cursor: pointer;">${displayid}</div>`;
//                },
//                orderable: false,
//                searchable: false
//            },
//            { "className": "text-wrap", "targets": "_all" }
//        ]
//    });
//}

//function clearDataTable() {
//    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
//        $("#datatable-default-").DataTable().clear().draw();
//    }
//}







//Static Supplier Balance Calculation
$(function () {
    loadSupplier();
    clearDataTable();

    // Trigger searchDataTable when supplierFilter or dateFilter is changed
    $('#supplierFilter, #dateFilter').change(function () {
        searchDataTable();
    });

    // Trigger calculateBalance when Paid Amount is input
    $('#paidAmount').on('input', function () {
        calculateBalance();
    });
});

function loadSupplier() {
    $.ajax({    
        url: getAPIUrl() + 'Common/ForSupplierDropDownList',
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let supplierFilter = $('#supplierFilter');
                supplierFilter.empty();
                supplierFilter.append('<option value="">Select Supplier</option>');
                $.each(response.data, function (index, item) {
                    supplierFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            }
        },
        error: function (error) {
            console.error('Error loading suppliers:', error);
        }
    });
}

function searchDataTable() {
    var selectedSupplierId = $('#supplierFilter').val();
    var selectedDate = $('#dateFilter').val(); // Fetch the selected date from dateFilter

    if (!selectedSupplierId && !selectedDate) {
        clearDataTable();
        return;
    }

    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
    }

    var table = $("#datatable-default-").DataTable({
        searching: true,
        ordering: true,
        ajax: {
            url: getAPIUrl() + "InventoryItem/GetInventoryItemsForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.supplierId = selectedSupplierId;
                d.date = selectedDate; // Send the selected date to the server
            },
            dataSrc: function (json) {
                console.log(json);
                checkAPIResponse(json);

                // Calculate total cost price
                let totalCostPrice = 0;
                $.each(json.data, function (index, item) {
                    totalCostPrice += item.costPrice || 0;
                });

                // Display total cost price
                $('#totalCostPriceInput').val(totalCostPrice.toFixed(2));

                // Recalculate balance
                calculateBalance();

                hideTableColumn(table, [{ col: 1, visible: allowActive }]);
                return json.data;
            },
            error: function (error) {
                fixDTError(error);
            }
        },
        columns: [
            { "data": "displayOrder", "name": "DisplayOrder" },
            {
                "data": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('InventoryItem/ToggleActive', row);
                }
            },
            { "data": "name" },
            { "data": "description" },
            { "data": "unit" },
            { "data": "unitCost" },
            { "data": "quantity" },
            { "data": "costPrice" },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("InventoryItem", "/InventoryItem/", row, { edit: allowEdit, delete: allowDelete })}`;
                }
            }
        ],
        createdRow: function (row, data, index) {
            $(row).attr('data-rowid', data.id);
            $(row).find('td:eq(0)').attr('data-displayorder', data.displayOrder);
            $(row).find('[data-plugin-ios-switch]').themePluginIOS7Switch();
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
            { "className": "text-wrap", "targets": "_all" }
        ]
    });
}

function calculateBalance() {
    let totalCostPrice = parseFloat($('#totalCostPriceInput').val()) || 0;
    let paidAmount = parseFloat($('#paidAmount').val()) || 0;
    let balance = totalCostPrice - paidAmount;

    $('#balanceAmountInput').val(balance.toFixed(2));
}

function clearDataTable() {
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().clear().draw();
    }
    $('#totalCostPriceInput').val('0.00');
    $('#balanceAmountInput').val('0.00');
}

