$(function () {
    clearDataTable(); searchDataTable();
});

searchDataTable = () => {

    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
    }

    var table = $("#datatable-default-").DataTable({
        rowReorder: (allowReOrder ? { update: false } : false),
        searching: true,
        ajax: {
            url: getAPIUrl() + "UserPermission/GetForDataTable",
            type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
            dataSrc: function (json) {
                console.log(json);
                checkAPIResponse(json);
                hideTableColumn(table, [{ col: 1, visible: allowActive }]);
                return json.data;
            }, error: function (error) { fixDTError(error); },
        },
        columns: [
            {
                "data": "id", render: function (data, type, row, meta) {
                    return getRowNumber(meta);
                }
            },
            {
                "data": "active", "name": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('UserPermission/ToggleActive', row);
                }
            },
            { "data": "displayOrder", "name": "DisplayOrder" },
            { "data": "title", "name": "Title" },
            { "data": "titleAr", "name": "Title AR" },
            { "data": "navigationUrl", "name": "Navigation Url" },
            { "data": "icon", render: function (data, type, row) { return (row.icon != null ? row.icon : ''); } },
            { "data": "showInMenu", render: function (data, type, row) { return (row.showInMenu ? 'Yes' : 'No'); } },
            { "data": "formattedCreatedOn", render: function (data, type, row) { return row.formattedCreatedOn; } },
            { "data": "formattedModifiedOn", render: function (data, type, row) { return row.formattedModifiedOn; } },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("UserPermission", "/UserMgmt/Permission/", row, { edit: allowEdit, delete: allowDelete })}`;
                }
            },
        ],

        createdRow: function (row, data, index) {
            $(row).attr('data-rowid', data.id);
            $(row).find('td:eq(0)').attr('data-displayorder', row.id);

            $(row).find('[data-plugin-ios-switch]').themePluginIOS7Switch();
        },
        columnDefs: getDataTableDisplayOrdering()
    });
    if (allowReOrder) {
        table.on("row-reorder", function (e, diff, edit) {
            if (diff.length == 0) { return; }
            var result = "Reorder started on row: " + edit.triggerRow.data()["id"] + "\r\n";
            var items = [];
            for (var i = 0, ien = diff.length; i < ien; i++) {
                var rowData = table.row(diff[i].node).data();

                result += rowData["title"] + " updated to be in position " + diff[i].newPosition + " (was " + diff[i].oldPosition + ")\r\n";
                var id = rowData["id"];
                var name = rowData["title"];
                var displayOrder = diff[i].newPosition + 1;
                items.push({ "id": id, "name": name, "displayOrder": displayOrder });
            }
            if (isDebug) {
                alert("Event result:\r\n" + result);
            }
            saveData(items);
        });
    }
}
saveData = (orderItems) => {

    let submitData = new FormData();
    for (var index = 0; index < orderItems.length; index++) {
        var p = orderItems[index];
        submitData.append(`items[][${index}][id]`, p.id);
        submitData.append(`items[][${index}][name]`, p.name);
        submitData.append(`items[][${index}][displayOrder]`, p.displayOrder);
    };
    ajaxPost("UserPermission/UpdateDisplayOrders", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => {
    if (data.success) {
        reorderDataTableAfterAnimation();
        ToastAlert('success', 'Permissions', data.message);
        //setTimeout(() => location.href = "/UserMgmt/Permission/PermissionList", 1000);
    } else {
        ToastAlert('error', 'Permissions', data.message);
    }
}

cbPostError = (error) => {
    ToastAlert('error', 'Permissions', data.message);
}

getPermissionHtml = (row, checked) => {
    var li = `<li class='col-12 col-md-8 list-group-item1 py-1 px-2 d-flex justify-content-between align-items-center mx-auto border-bottom'> 
                <p class='mb-0 me-3'>${row.title}</p>
                <div class='switch switch-sm switch-primary'>
                    <input type='checkbox' name='switch_${row.id}' data-id='${row.id}' data-plugin-ios-switch  (${checked} ? 'checked' :'')/> 
                </div> 
            </li>`;
    return li;
}