$(function () {
    loadSuppliers();
    clearDataTable(); // Clear the DataTable on page load

    $('#supplierFilter').change(function () {
        searchDataTable(); // Show DataTable data based on selected Supplier
    });
});

function loadSuppliers() {
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
            console.error('Error loading Supplier:', error);
        }
    });
}

function searchDataTable() {
    var selectedSupplierId = $('#supplierFilter').val();

    if (!selectedSupplierId) {
        clearDataTable(); // Clear DataTable if no Supplier is selected
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
            url: getAPIUrl() + "SupplierItem/GetSupplierItemForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.supplierId = selectedSupplierId;
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
                    return addCheckedActionGuid('SupplierItem/ToggleActive', row);
                }
            },
            { "data": "name" },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("SupplierItem", "/SupplierItem/", row, { edit: allowEdit, delete: allowDelete })}`;
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

function clearDataTable() {
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().clear().draw();
    }
}
