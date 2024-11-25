$(function () {
    loadCategories();
    clearDataTable();
    searchDataTable();

    $('#categoryFilter').change(function () {
        searchDataTable();
    });
});

function loadCategories() {
    $.ajax({
        url: getAPIUrl() + 'Common/ForCategoryDropDownList',
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let categoryFilter = $('#categoryFilter');
                categoryFilter.empty();
                categoryFilter.append('<option value="">Select Category</option>');
                $.each(response.data, function (index, item) {
                    categoryFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            }
        },
        error: function (error) {
            console.error('Error loading categories:', error);
        }
    });
}

function searchDataTable() {
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
    }

    var selectedCategoryId = $('#categoryFilter').val();

    var table = $("#datatable-default-").DataTable({
        searching: true,
        ordering: true,
        ajax: {
            url: getAPIUrl() + "SubCategory/GetSubCategoryForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.categoryId = selectedCategoryId; // Pass selected category ID to filter subcategories
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
            {
                "data": "displayOrder", "name": "DisplayOrder"
            },
            {
                "data": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('SubCategory/ToggleActive', row);
                }
            },
            {
                "data": "imageUrl", render: function (data, type, row) {
                    return "<img src='" + row.imageUrl + "' class='img-fluid text-center mx-auto img-zoom' />";
                }
            },
            {
                "data": "name"
            },
            {
                "data": "description"
            },
            
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("SubCategory", "/SubCategory/", row, { edit: allowEdit, delete: allowDelete })}`;
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
