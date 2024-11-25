$(function () {
    clearDataTable();
    searchDataTable();
});

function searchDataTable() {
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
    }

    var table = $("#datatable-default-").DataTable({
        searching: true,
        ajax: {
            url: getAPIUrl() + "Supplier/GetSupplierForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) { getDataTableHeaders(xhr); },
            dataSrc: function (json) {
                // checkAPIResponse(json); // Uncomment this if you have a checkAPIResponse function
                hideTableColumn(table, [{ col: 1, visible: allowActive }]);
                console.log(json.data);
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
                "data": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('Supplier/ToggleActive', row);
                }
            },
            {
                "data": "name"
            },
            {
                "data": "address"
            },
            {
                "data": "contact"
            },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("Supplier", "/Supplier/", row, { edit: allowEdit, delete: allowDelete })}`;
                }
            },
        ],
        createdRow: function (row, data, index) {
            $(row).attr('data-rowid', data.id);
            $(row).find('td:eq(0)').attr('data-displayorder', row.id);

            // Reinitialize ios-switch
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
            { "className": "text-wrap", "targets": "_all" },
        ],
    });
}