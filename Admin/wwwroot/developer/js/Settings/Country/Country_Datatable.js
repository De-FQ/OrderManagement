$(function () { // Handler for .ready() called.
    clearDataTable(); 
    searchDataTable();
}); 
searchDataTable = () => { 
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
        $("#datatable-default-").DataTable().clear().draw(); 
    }
      
    var table = $("#datatable-default-").DataTable({
        rowReorder: allowReOrder,
        ordering: true,
        ajax: {
            url: getAPIUrl() + "Country/GetForDataTable",
            type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); }, 
            dataSrc: function (json) {
                checkAPIResponse(json);
                hideTableColumn(table,[{ col: 1, name: 'allowActive'}]); 
                return json.data;
            }, error: function (error) { fixDTError(error); },
            //complete: function (data, textStatus) {
            //    showLog(data.responseJSON); 
            //},
            //error: function (error) {
            //    showLog('error' + error); 
            //},
        },
        columns: [
            {
                "data": "id",  render: function (data, type, row, meta) {
                    return getRowNumber(meta); 
                }
            }, 
            {
                "data": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('Country/ToggleActive', row);
                }
            },
            { "data": "displayOrder", "name": "DisplayOrder" },
            { "data": "isDefault",  render: function (data, type, row) { return row.isDefault ? "Yes" : "No"; } },
            { "data": "name", "name": "name" },
            { "data": "nameAr", "name": "nameAr" }, 
            { "data": "formattedCreatedOn", render: function (data, type, row) { return row.formattedCreatedOn; } },
            { "data": "formattedModifiedOn", render: function (data, type, row) { return row.formattedModifiedOn; } },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("Country", "/Settings/Country/", row, { edit: allowEdit, delete: allowDelete })}`;
                }
            }, 
        ],
        createdRow: function (row, data, index) {
            //change display order value, to avoid page refresh
            $(row).attr('data-rowid', data.id);
            $(row).find('td:eq(0)').attr('data-displayorder', row.id);

            //    Reinitialize ios-switch
            $(row).find('[data-plugin-ios-switch]').themePluginIOS7Switch();
        },
       columnDefs: [
            { sortable: false, targets: [0, 1, 5, 6, 7] }, /* hide sortable columns options*/
            { orderable: true, targets: [2, 3, 4] }, /*{ "orderable": false, "targets": -1  }, set ordering:true, only targets columns for orderable */
            { className : "text-wrap",  targets : "_all" }
        ]
    });
   
    if (allowReOrder) {
        table.on("row-reorder", function (e, diff, edit) {
            if (diff.length == 0) { return; }
            var result = "Reorder started on row: " + edit.triggerRow.data()["id"] + "\r\n";
            var items = [];
            for (var i = 0, ien = diff.length; i < ien; i++) {
                var rowData = table.row(diff[i].node).data();

                var name = rowData["name"];
                var id = rowData["id"];
                var displayOrder = diff[i].newPosition + 1;
                items.push({ "id": id, "name": name, "displayOrder": displayOrder });

                //in debug mode show ordering events list
                result += name + " updated to be in position " + diff[i].newPosition + " (was " + diff[i].oldPosition + ")\r\n";

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
    ajaxPost("Country/UpdateDisplayOrders", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'Country', data.message);
        setTimeout(() => location.href = "/Settings/Country/CountryList", 1000);
    } else {
        ToastAlert('error', 'Country', data.message); 
    }
}

cbPostError = (error) => {
    ToastAlert('error', 'Country', error.message);
}
 