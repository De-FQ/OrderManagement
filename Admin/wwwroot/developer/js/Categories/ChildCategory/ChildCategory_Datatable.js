$(function () {
    loadCategories();
    clearDataTable(); // Clear the DataTable on page load

    $('#categoryFilter').change(function () {
        var categoryId = $(this).val();
        loadSubCategories(categoryId);
        clearChildCategoryDropdown();
        clearDataTable(); // Clear the DataTable when category changes
    });

    $('#subCategoryFilter').change(function () {
        var subCategoryId = $(this).val();
        if (subCategoryId) {
            loadChildCategories(subCategoryId);
        }
        searchDataTable(); // Show DataTable data based on selected subcategory
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

function loadSubCategories(categoryId) {
    if (!categoryId) {
        clearSubCategoryDropdown();
        return;
    }

    $.ajax({
        url: getAPIUrl() + 'Common/ForSubCategoryDropDownList?categoryId=' + categoryId,
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let subCategoryFilter = $('#subCategoryFilter');
                subCategoryFilter.empty();
                subCategoryFilter.append('<option value="">Select SubCategory</option>');
                $.each(response.data, function (index, item) {
                    subCategoryFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            }
        },
        error: function (error) {
            console.error('Error loading subcategories:', error);
        }
    });
}

function loadChildCategories(subCategoryId) {
    if (!subCategoryId) {
        clearChildCategoryDropdown();
        return;
    }

    $.ajax({
        url: getAPIUrl() + 'Common/ForChildCategoryDropDownList?subCategoryId=' + subCategoryId,
        type: 'GET',
        xhrFields: { withCredentials: true },
        success: function (response) {
            if (response.success) {
                let childCategoryFilter = $('#childCategoryFilter');
                childCategoryFilter.empty();
                childCategoryFilter.append('<option value="">Select ChildCategory</option>');
                $.each(response.data, function (index, item) {
                    childCategoryFilter.append('<option value="' + item.id + '">' + item.name + '</option>');
                });
            }
        },
        error: function (error) {
            console.error('Error loading child categories:', error);
        }
    });
}

function clearSubCategoryDropdown() {
    let subCategoryFilter = $('#subCategoryFilter');
    subCategoryFilter.empty();
    subCategoryFilter.append('<option value="">Select SubCategory</option>');
}

function clearChildCategoryDropdown() {
    let childCategoryFilter = $('#childCategoryFilter');
    childCategoryFilter.empty();
    childCategoryFilter.append('<option value="">Select ChildCategory</option>');
}

function searchDataTable() {
    var selectedCategoryId = $('#categoryFilter').val();
    var selectedSubCategoryId = $('#subCategoryFilter').val();
    var selectedChildCategoryId = $('#childCategoryFilter').val();

    if (!selectedSubCategoryId) {
        clearDataTable(); // Clear DataTable if no subcategory is selected
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
            url: getAPIUrl() + "ChildCategory/GetChildCategoryForDataTable",
            type: "POST",
            xhrFields: { withCredentials: true },
            beforeSend: function (xhr) {
                getDataTableHeaders(xhr);
            },
            data: function (d) {
                d.categoryId = selectedCategoryId;
                d.subCategoryId = selectedSubCategoryId;
                d.childCategoryId = selectedChildCategoryId;
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
                    return addCheckedActionGuid('ChildCategory/ToggleActive', row);
                }
            },
            {
                "data": "imageUrl", render: function (data, type, row) {
                    return "<img src='" + row.imageUrl + "' class='img-fluid text-center mx-auto img-zoom' />";
                }
            },
            { "data": "name" },
            { "data": "description" },
            
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("ChildCategory", "/ChildCategory/", row, { edit: allowEdit, delete: allowDelete })}`;
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
