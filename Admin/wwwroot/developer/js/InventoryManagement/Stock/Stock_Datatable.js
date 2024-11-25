$(function () {
    loadInventoryItems();
    clearStockDataTable();

    $('#inventoryItemFilter').change(function () {
        searchStockDataTable();
    });
});

function loadInventoryItems() {
    $.ajax({
        url: getAPIUrl() + 'Common/ForInventoryItemDropDownList',
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let inventoryItemFilter = $('#inventoryItemFilter');
                inventoryItemFilter.empty();
                inventoryItemFilter.append('<option value="">Select Inventory Item</option>');
                $.each(response.data, function (index, item) {
                    inventoryItemFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            }
        },
        error: function (error) {
            console.error('Error loading inventory items:', error);
        }
    });
}

function searchStockDataTable() {
    var selectedInventoryItemId = $('#inventoryItemFilter').val();

    if (!selectedInventoryItemId) {
        clearStockDataTable();
        return;
    }

    if ($.fn.dataTable.isDataTable("#datatable-stock-")) {
        $("#datatable-stock-").DataTable().state.clear();
        $("#datatable-stock-").DataTable().destroy();
    }

    var table = $("#datatable-stock-").DataTable({
        searching: true,
        ordering: true,
        ajax: {
            url: getAPIUrl() + "Stock/GetStocksForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.inventoryItemId = selectedInventoryItemId;
            },
            dataSrc: function (json) {
                console.log(json);
                checkAPIResponse(json);
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
                    return addCheckedActionGuid('Stock/ToggleActive', row);
                }
            },
            { "data": "unit" },
            { "data": "netUnitCost" },
            { "data": "companyCostMargin" },
            { "data": "totalQuantity" },
            { "data": "totalUnitNetCost" },
            { "data": "totalUnitCompanyPrice" },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("Stock", "/Stock/", row, { edit: allowEdit, delete: allowDelete })}`;
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

function clearStockDataTable() {
    if ($.fn.dataTable.isDataTable("#datatable-stock-")) {
        $("#datatable-stock-").DataTable().clear().draw();
    }
}
