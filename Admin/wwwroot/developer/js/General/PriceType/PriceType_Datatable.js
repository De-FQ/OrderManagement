$(function () {
    loadPriceTypeCategories();
    clearDataTable(); // Clear the DataTable on page load

    $('#priceTypeCategoryFilter').change(function () {
        searchDataTable(); // Show DataTable data based on selected price type category
    });
});

function loadPriceTypeCategories() {
    $.ajax({
        url: getAPIUrl() + 'Common/ForPriceTypeCategoryDropDownList',
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let priceTypeCategoryFilter = $('#priceTypeCategoryFilter');
                priceTypeCategoryFilter.empty();
                priceTypeCategoryFilter.append('<option value="">Select Price Type Category</option>');
                $.each(response.data, function (index, item) {
                    priceTypeCategoryFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            } else {
                console.error('Failed to load price type categories:', response.message);
            }
        },
        error: function (error) {
            console.error('Error loading price type categories:', error);
        }
    });
}

function searchDataTable() {
    var selectedPriceTypeCategoryId = $('#priceTypeCategoryFilter').val();

    if (!selectedPriceTypeCategoryId) {
        clearDataTable(); // Clear DataTable if no price type category is selected
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
            url: getAPIUrl() + "PriceType/GetPriceTypeForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.priceTypeCategoryId = selectedPriceTypeCategoryId;
                console.log("Data sent to server:", d);
            },
            dataSrc: function (json) {
                console.log("Data received from server:", json);
                checkAPIResponse(json);
                hideTableColumn(table, [{ col: 1, visible: allowActive }]);
                return json.data;
            },
            error: function (error) {
                console.error('Error fetching data:', error);
                fixDTError(error);
            }
        },
        columns: [
            { "data": "displayOrder", "name": "DisplayOrder" },
            {
                "data": "active",
                "render": function (data, type, row) {
                    return addCheckedActionGuid('PriceType/ToggleActive', row);
                }
            },
            { "data": "name" },
            {
                "data": null,
                "name": "Actions",
                "render": function (data, type, row) {
                    return `${addDropDownMenuOptions("PriceType", "/PriceType/", row, { edit: allowEdit, delete: allowDelete })}`;
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
                "targets": [0],
                "defaultContent": '',
                "render": function (data, type, row, meta) {
                    var displayid = meta.row + meta.settings._iDisplayStart + 1;
                    return `<div data-i="${displayid}" style="cursor: pointer;">${displayid}</div>`;
                },
                "orderable": false,
                "searchable": false
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
