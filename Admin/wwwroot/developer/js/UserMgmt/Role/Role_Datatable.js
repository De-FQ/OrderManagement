$(function () { // Handler for .ready() called. 
    clearDataTable(); searchDataTable();
    ajaxGet('UserRole/GetAllPermission', callbackGetAllPermissionSuccess);
});

searchDataTable = () => { 
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
    }

    var table = $("#datatable-default-").DataTable({
        rowReorder: (allowReOrder ? { update: false } : false),
        searching: true,
        ordering: allowReOrder,
        ajax: {
            url: getAPIUrl() + "UserRole/GetForDataTable",
            type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); }, 
            crossDomain:true,
            dataSrc: function (json) {
                console.log(json);
                checkAPIResponse(json);
                hideTableColumn(table, [{ col: 1, visible: allowActive }]);
                 return json.data;
            }, error: function (error) { fixDTError(error); },
            
        },
        columns: [
            { "data": "displayOrder", "name": "DisplayOrder" },
             //{
            //    "data": "id", render: function (data, type, row, meta) {
            //        return getRowNumber(meta);
            //    }
            //},
            {
                "data": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('UserRole/ToggleActive', row);
                }
            },
           
            { "data": "name" }, 
            { "data": "formattedCreatedOn", render: function (data, type, row) { return row.formattedCreatedOn; } },
            { "data": "formattedModifiedOn", render: function (data, type, row) { return row.formattedModifiedOn; } }, 
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("UserRole", "/UserMgmt/Role/", row, { edit: allowEdit, delete: allowDelete, editPermission: true, roleId: row.id}  )}`;
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
        columnDefs: getDataTableDisplayOrdering() 
    });
    if (allowReOrder) {
        table.on("row-reorder", function (e, diff, edit) {
            if (diff.length == 0) { return; }

            for (var i = 0; i < diff.length; i++) {
                $(diff[i].node).addClass("highlight");
                // setTimeout(addRowAnimation,i*500,diff[i].node);
                setTimeout(removeRowAnimation, 1000, diff[i].node);
            }



          //  var result = "Reorder started on row: " + edit.triggerRow.data()["id"] + "\r\n";
            var items = [];
            for (var i = 0, ien = diff.length; i < ien; i++) {
                var rowData = table.row(diff[i].node).data();

                var name = rowData["name"];
                var id = rowData["id"];
                var displayOrder = diff[i].newPosition + 1;
                items.push({ "id": id, "name": name, "displayOrder": displayOrder });

                //in debug mode show ordering events list
               // result += name + " updated to be in position " + diff[i].newPosition + " (was " + diff[i].oldPosition + ")\r\n";

            }
            //if (isDebug) {
            //    alert("Event result:\r\n" + result);
            //}
            saveReOrderData(items);
        });
    }
}
saveReOrderData = (orderItems) => {
    let submitData = new FormData();
    for (var index = 0; index < orderItems.length; index++) {
        var p = orderItems[index];
        submitData.append(`items[][${index}][id]`, p.id);
        submitData.append(`items[][${index}][name]`, p.name);
        submitData.append(`items[][${index}][displayOrder]`, p.displayOrder);
    };
    ajaxPost("UserRole/UpdateDisplayOrders", submitData, cbReOrderSuccess, cbReOrderError);
};

cbReOrderSuccess = (data) => {
    if (data.success) {
        reorderDataTableAfterAnimation();
        ToastAlert('success', 'Role', data.message);
       // setTimeout(() => location.href = "/UserMgmt/Role/RoleList", 1000);
    } else {
        ToastAlert('error', 'Role', data.message);
    }
}

cbReOrderError = (data) => {
    ToastAlert('error', 'Role', data.message);
}


callbackGetAllPermissionSuccess = (data) => {
    addStorage("permissions", data.data); //for reuse 
}

addEditPermissionAction = (row) => {
    return "<a  href='#' onclick=dialogPermission(" + row.id + ") class='mb-1 mt-1 me-1 btn btn-success btn-sm' data-bs-placement='bottom' title='Edit Permission'><i class='fas fa-cogs'></i></a>";
}

dialogPermission = (userRoleId) => {
    //$(".permissions-list").data("roleId", userRoleId);
    $("#datatable-permission-list").data("roleId", userRoleId);
    ajaxGet('UserRole/GetAllPermissionByRoleId?id=' + userRoleId, callbackGetRolePermissionSuccess);
}

callbackGetRolePermissionSuccess = (data) => {

    $('#edit_permissions').modal('toggle'); //show popup window for permissions
    var permissions = getStorage("permissions"); //get all permissions 
     
    var table = $("#datatable-permission-list tbody").empty();
    
    $.each(permissions, function (a, row) {
        if (data.data.length > 0) {
            const results = data.data.filter(obj => { return obj.userPermissionId === row.id; });
            if (results.length > 0) { 
                table.append(addPermissionRow(row, results[0], results[0].allowed));
            } else { 
                table.append(addPermissionRow(row, row, false));
            }
        } else { 
            table.append(addPermissionRow(row, row, false));
        }
    });

    $('#edit_permissions').find('[data-plugin-ios-switch]').themePluginIOS7Switch();
    
}

saveRolePermission = () => {
    var index = 0; 
    let submitData = new FormData();
    submitData.append('id', $("#datatable-permission-list").data("roleId"));
    $("#datatable-permission-list tbody tr").each(function (r, row) {
         
        var data = {
            "id": 0,
            "allowed": false,
            "allowList": false,
            "allowDisplayOrder": false,
            "allowActive": false,
            "allowAdd": false,
            "allowEdit": false,
            "allowDelete": false,
            "allowView": false
        };
         
        // get the current row
        var $row = $(row); 
        //every row contains id + allowed checkbox
        data.id = $(this).attr("rowid")
        data.allowed = getChecked($row, "allowed");
        data.allowList = getChecked($row, "allowList");
        data.allowDisplayOrder = getChecked($row, "allowDisplayOrder");
        data.allowActive = getChecked($row, "allowActive");  
        data.allowAdd = getChecked($row, "allowAdd");
        data.allowEdit = getChecked($row, "allowEdit");
        data.allowDelete = getChecked($row, "allowDelete");
        data.allowView = getChecked($row, "allowView");
        

        submitData.append(`RolePermissions[][${index}][id]`, data.id);
        submitData.append(`RolePermissions[][${index}][allowed]`, data.allowed);
        submitData.append(`RolePermissions[][${index}][allowList]`, data.allowList);
        submitData.append(`RolePermissions[][${index}][allowDisplayOrder]`, data.allowDisplayOrder);
        submitData.append(`RolePermissions[][${index}][allowActive]`, data.allowActive);
        submitData.append(`RolePermissions[][${index}][allowAdd]`, data.allowAdd);
        submitData.append(`RolePermissions[][${index}][allowEdit]`, data.allowEdit);
        submitData.append(`RolePermissions[][${index}][allowDelete]`, data.allowDelete);
        submitData.append(`RolePermissions[][${index}][allowView]`, data.allowView);

        index += 1; 
       
    });

     ajaxPost("UserRole/UpdatePermission", submitData, cbUpdateRole, cbUpdateRoleError);
}

getChecked = ($row, name) => {
    if ($row.find(`input[name*=${name}]`)[0] != undefined) {
        return $row.find(`input[name*=${name}]`)[0].checked;
    }
    return false;
}

cbUpdateRole = (data) => {
    ToastAlert('success', 'Role Permission', 'Role Permissions are updated successfully'); 
     $('#edit_permissions').modal('toggle');
     reloadPage();
}
cbUpdateRoleError = (data) => {
    ToastAlert('error', 'Role Permission', 'unable to save, please try again or contact to system admin');
}

addPermissionRow = (row, userRow, checked) => {
    var tableRow = `<tr rowid="${row.id}">
                         <td><p class='mb-0 me-3'>${row.title}</p></td>`;
  
    if ( row.accessList == "" || row.accessList == null) {
        tableRow += `   <td colspan="8"> </td>  `;
    } else {
        if (row.accessList.search("Allowed") > -1) { tableRow += createPerm({ 'name': 'allowed', active: userRow.allowed }) } else { tableRow += createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("List") > -1) { tableRow += createPerm({ 'name': 'allowList', active: userRow.allowList }) } else { tableRow +=createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("DisplayOrder") > 0) { tableRow += createPerm({ 'name': 'allowDisplayOrder', active: userRow.allowDisplayOrder }) } else { tableRow +=createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("Active") > -1) { tableRow += createPerm({ 'name': 'allowActive', active: userRow.allowActive }) } else { tableRow += createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("Add") > -1) { tableRow += createPerm({ 'name': 'allowAdd', active: userRow.allowAdd }) } else { tableRow +=createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("Edit") > -1) { tableRow += createPerm({ 'name': 'allowEdit', active: userRow.allowEdit }) } else { tableRow +=createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("Delete") > -1) { tableRow += createPerm({ 'name': 'allowDelete', active: userRow.allowDelete }) } else { tableRow +=createPerm({ 'name': 'empty', active: false }) }
        if (row.accessList.search("View") > -1) { tableRow += createPerm({ 'name': 'allowView', active: userRow.allowView }) } else { tableRow +=createPerm({ 'name': 'empty', active: false }) }
            
        
    }
    return tableRow + "</tr>";
}
 //Allowed,List,DisplayOrder,Active,Add,Edit,Delete,View
    createPerm = (item) => { 
        var html = '';
        if (item.name == 'empty') {
                html += '<td></td>';
            } else {
                html += `<td><div class="switch switch-sm switch-primary">
                             <input type="checkbox" id="${item.name}" name="${item.name}" data-plugin-ios-switch ${item.active ? "checked" : ""} />
                            </div>
                        </td>`;
            }
         
        return html;
    }
     