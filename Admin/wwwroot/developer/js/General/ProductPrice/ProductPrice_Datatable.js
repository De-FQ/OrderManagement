$(function () {
    loadChildCategories();
    clearDataTable();

    $('#childCategoryFilter').change(function () {
        loadPriceTypeCategories();
        clearDataTable();
    });

    $('#priceTypeCategoryFilter').change(function () {
        loadPriceTypes();
        clearDataTable();
    });

    $('#priceTypeFilter').change(function () {
        updateDataTable();
    });
});

function loadChildCategories() {
    $.ajax({
        url: getAPIUrl() + 'Common/ForChildCategoryDropDownList',
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let childCategoryFilter = $('#childCategoryFilter');
                childCategoryFilter.empty();
                childCategoryFilter.append('<option value="">Select Child Category</option>');
                $.each(response.data, function (index, item) {
                    childCategoryFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            } else {
                console.error('Failed to load child categories:', response.message);
            }
        },
        error: function (error) {
            console.error('Error loading child categories:', error);
        }
    });
}

function loadPriceTypeCategories() {
    var selectedChildCategoryId = $('#childCategoryFilter').val();

    if (!selectedChildCategoryId) {
        $('#priceTypeCategoryFilter').empty().append('<option value="">Select Price Type Category</option>');
        $('#priceTypeFilter').empty().append('<option value="">Select Price Type</option>');
        return;
    }

    $.ajax({
        url: getAPIUrl() + 'ProductPrice/GetPriceTypeCategoryByChildCategoryId',
        type: 'GET',
        data: { childCategoryId: selectedChildCategoryId },
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

function loadPriceTypes() {
    var selectedPriceTypeCategoryId = $('#priceTypeCategoryFilter').val();

    if (!selectedPriceTypeCategoryId) {
        $('#priceTypeFilter').empty().append('<option value="">Select Price Type</option>');
        return;
    }

    $.ajax({
        url: getAPIUrl() + 'ProductPrice/GetPriceTypeByPriceTypeCategoryId',
        type: 'GET',
        data: { priceTypeCategoryId: selectedPriceTypeCategoryId },
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let priceTypeFilter = $('#priceTypeFilter');
                priceTypeFilter.empty();
                priceTypeFilter.append('<option value="">Select Price Type</option>');
                $.each(response.data, function (index, item) {
                    priceTypeFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            } else {
                console.error('Failed to load price types:', response.message);
            }
        },
        error: function (error) {
            console.error('Error loading price types:', error);
        }
    });
}

function updateDataTable() {
    var selectedPriceTypeId = $('#priceTypeFilter').val();

    if (!selectedPriceTypeId) {
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
            url: getAPIUrl() + "ProductPrice/GetProductPriceForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.childCategoryId = $('#childCategoryFilter').val();
                d.priceTypeCategoryId = $('#priceTypeCategoryFilter').val();
                d.priceTypeId = selectedPriceTypeId;
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
                    return addCheckedActionGuid('ProductPrice/ToggleActive', row);
                }
            },
            { "data": "price" },
            {
                "data": null,
                "name": "Actions",
                "render": function (data, type, row) {
                    return `${addDropDownMenuOptions("ProductPrice", "/ProductPrice/", row, { edit: allowEdit, delete: allowDelete })}`;
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
