var allowReOrder = true;
addDropDownMenuOptions = (apibase, navigatePath, row, actions = undefined) => {
    var actionCount = 0;
    for (let action in actions) {
        if (actions[action] == true)
            actionCount++;
    }
    if (actionCount > 1) {
        var menu = ` <div class="d-flex flex-nowrap justify-content-end">
            <a href="javascript:;" class="btn btn-info w-auto responsive-view-btn me-2">
                <i class="fa-solid fa-chevron-down"></i>
            </a>
        <div class="dropdown  actions-dropdown-toggle-btn" id="dv-actions">
        <a href="javascript:;" data-bs-toggle="dropdown" aria-expanded="false" class="btn btn-secondary actions-dropdown dropdown-toggle px-3">
            <i class="fa-solid fa-ellipsis-vertical" ></i>
            </a>
            <ul class="dropdown-menu">`;
        if (actions.edit == true) { //!= undefined && actions.edit == 1 && $("#allowEdit").val() == '1'
            menu += `<li> 
                        <a href='${navigatePath + "AddEdit?guid=" + row.guid}' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-primary' aria-label="Edit">
                            <i class="fas fa-edit"></i>
                            <span class="ms-2">Edit</span>
                        </a> 
                </li>`;
        }
        if (actions.delete == true) { //!= undefined && actions.delete == 1 && $("#allowDelete").val() == '1'
            menu += `<li> 
                        <a onclick="showDeleteDialog('${apibase}', '${row.guid}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-danger' aria-label="Delete">
                            <i class="fas fa-xmark"></i>
                            <span class="ms-2">Delete</span>
                        </a> 
                </li>`;

        }
        if (actions.password == true) { //its only defined in user_datatable
            menu += `<li> 
                        <a onclick="showDialogPassword('${row.guid}','${row.fullName}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-warnning' aria-label="Change Password">
                            <i class="fas fa-user-lock"></i>
                            <span class="ms-2">Change Password</span>
                        </a> 
                </li>`;

        }

        if (actions.editPermission == true) { //!= undefined && actions.editPermission == 1
            menu += `<li> 
                        <a onclick="dialogPermission('${row.id}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-danger' aria-label="Delete">
                            <i class="fas fa-cogs"></i>
                            <span class="ms-2">Edit Permission</span>
                        </a> 
                </li>`;


        }

        if (actions.edit == true || actions.delete == true || actions.password == true || actions.editPermission == true || actions.viewApplicants == true) {
            return menu + '</ul><span id="notification-badge" style="display:none;" class="badge bg-glowing position-absolute translate-middle start-10 top-0">3</span></div></div>';
        } else {
            return "";
        }
    }
    else {
        var menu = "";
        if (actions.edit == true) { //!= undefined && actions.edit == 1 && $("#allowEdit").val() == '1'
            menu += `<a href='${navigatePath + "AddEdit?guid=" + row.guid}' class='mb-1 mt-1 me-1 btn btn-sm float-end btn-primary' aria-label="Edit">
                            <i class="fas fa-edit"></i>
                            <span class="ms-2">Edit</span>
                        </a>`;
        }
        if (actions.delete == true) { //!= undefined && actions.delete == 1 && $("#allowDelete").val() == '1'
            menu += `<a onclick="showDeleteDialog('${apibase}', '${row.guid}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm float-end btn-danger' aria-label="Delete">
                            <i class="fas fa-xmark"></i>
                            <span class="ms-2">Delete</span>
                        </a>`;

        }
        if (actions.password == true) { //its only defined in user_datatable
            menu += `<a onclick="showDialogPassword('${row.guid}','${row.fullName}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm float-end btn-warnning' aria-label="Change Password">
                            <i class="fas fa-user-lock"></i>
                            <span class="ms-2">Change Password</span>
                        </a>`;

        }

        if (actions.editPermission == true) { //!= undefined && actions.editPermission == 1
            menu += `<a onclick="dialogPermission('${row.id}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm float-end btn-danger' aria-label="Delete">
                            <i class="fas fa-cogs"></i>
                            <span class="ms-2">Edit Permission</span>
                        </a>`;


        }

        if (actions.edit == true || actions.delete == true || actions.password == true || actions.editPermission == true || actions.viewApplicants == true) {
            return menu + '<span id="notification-badge" style="display:none;" class="badge bg-glowing position-absolute translate-middle start-10 top-0">3</span></div></div>';
        } else {
            return "";
        }
    }

}



addDropDownMenuOptions_old = (apibase, navigatePath, row, actions = undefined) => {


    var menu = ` <div class="d-flex flex-nowrap justify-content-end">
        <a href="javascript:;" class="btn btn-info w-auto responsive-view-btn me-2">
            <i class="fa-solid fa-chevron-down"></i>
        </a>
    <div class="dropdown  actions-dropdown-toggle-btn" id="dv-actions">
    <a href="javascript:;" data-bs-toggle="dropdown" aria-expanded="false" class="btn btn-secondary actions-dropdown dropdown-toggle px-3">
        <i class="fa-solid fa-ellipsis-vertical" ></i>
        </a>
        <ul class="dropdown-menu">`;
    if (actions.edit == true) { //!= undefined && actions.edit == 1 && $("#allowEdit").val() == '1'
        menu += `<li> 
                    <a href='${navigatePath + "AddEdit?guid=" + row.guid}' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-primary' aria-label="Edit">
                        <i class="fas fa-edit"></i>
                        <span class="ms-2">Edit</span>
                    </a> 
            </li>`;
    }
    if (actions.delete == true) { //!= undefined && actions.delete == 1 && $("#allowDelete").val() == '1'
        menu += `<li> 
                    <a onclick="showDeleteDialog('${apibase}', '${row.guid}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-danger' aria-label="Delete">
                        <i class="fas fa-xmark"></i>
                        <span class="ms-2">Delete</span>
                    </a> 
            </li>`;

    }
    if (actions.password == true) { //its only defined in user_datatable
        menu += `<li> 
                    <a onclick="showDialogPassword('${row.guid}','${row.fullName}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-warnning' aria-label="Change Password">
                        <i class="fas fa-user-lock"></i>
                        <span class="ms-2">Change Password</span>
                    </a> 
            </li>`;

    }

    if (actions.editPermission == true) { //!= undefined && actions.editPermission == 1
        menu += `<li> 
                    <a onclick="dialogPermission('${row.id}');" href='javascript:;' class='mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-danger' aria-label="Delete">
                        <i class="fas fa-cogs"></i>
                        <span class="ms-2">Edit Permission</span>
                    </a> 
            </li>`;


    }

    if (actions.edit == true || actions.delete == true || actions.password == true || actions.editPermission == true || actions.viewApplicants == true) {
        return menu + '</ul><span id="notification-badge" style="display:none;" class="badge bg-glowing position-absolute translate-middle start-10 top-0">3</span></div></div>';
    } else {
        return "";
    }

}

showDeleteDialog = (apibase, guid) => {
    var yesNoHtml = `<div class="modal fade" id='displayYesNoModel' data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="confirm-deleteLabel" aria-hidden="true">
        <div class="modal-dialog  modal-dialog-scrollable modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header text-bg-danger">
                    <h5 class="modal-title m-0" id="confirm-deleteLabel">Delete Confirmation</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p class="text-center mb-0">Are you sure you want to delete?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-light" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger" onclick="doDeleteGuid('${apibase}','${guid}')">Delete</button>
                </div>
            </div>
        </div>
    </div>`;
    $("body").append(yesNoHtml);
    $('#displayYesNoModel').modal('toggle');
}

fixDTError = (xhr, textStatus, errorThrown) => {
    if (xhr.status == 0) {
        showLog(xhr, xhr.status);
        ajaxGet('heartbit', ajaxHeartBit);

    }

}
ajaxHeartBit = (data) => {
    //if cross-orgin failed, after getting antiforgery token
    //reload the page
    reloadPage();
}
getRowNumber = (meta) => {
    return meta.row + meta.settings._iDisplayStart + 1;
}

hideTableColumn = (table, items) => {
    for (var i = 0; i < items.length; i++) {
        table.column(items[i].col).visible(items[i].visible);
        //table.column(items[i].col).visible($("#" + items[i].name).val() == '1' ? true : false);
    }
}

doCancelPopup = (guid) => {
    $('#notification-cancel').modal('toggle');
    $("#hdnGuid").val(guid);
}

addRowAnimation = (node) => {
    $(node).addClass("highlight");
}

removeRowAnimation = (node) => {
    $(node).removeClass("highlight");
}

clearDataTable = () => {
    localStorage.removeItem("DataTables_datatable-default-_" + document.location.pathname);
}

createDataTableRowAttribute = (row, data, index) => {
    //change display order value, to avoid page refresh
    $(row).attr('data-rowid', data.id);
    $(row).find('td:eq(0)').attr('data-displayorder', row.id);

    //    Reinitialize ios-switch
    $(row).find('[data-plugin-ios-switch]').themePluginIOS7Switch();
}

///==============================================
/// Create row-reorder event with backend update
///==============================================
createDataTableRowReOrder = (table, endpoint, title) => {
    localStorage.setItem("__title__", title);
    table.on("row-reorder", function (e, diff, edit) {
        if (diff.length == 0) { return; }

        for (var i = 0; i < diff.length; i++) {
            $(diff[i].node).addClass("highlight");
            setTimeout(removeRowAnimation, 1000, diff[i].node);
        }

        setTimeout(updateReOrderInBackend(table, diff, endpoint), 5000);
    });


}

updateReOrderInBackend = (table, diff, endpoint) => {
    //var items = [];
    let submitData = new FormData();
    for (var i = 0, ien = diff.length; i < ien; i++) {
        var rowData = $("#datatable-default-").DataTable().row(diff[i].node).data();
        //var rowData = table.row(diff[i].node).data();
        //items.push({ "id": rowData["id"], "name": rowData["name"], "displayOrder": diff[i].newPosition + 1 });

        submitData.append(`items[][${i}][id]`, rowData["id"]);
        submitData.append(`items[][${i}][name]`, rowData["name"]);
        submitData.append(`items[][${i}][displayOrder]`, diff[i].newPosition + 1);
    }


    //for (var index = 0; index < items.length; index++) {
    //    var p = items[index];
    //    submitData.append(`items[][${index}][id]`, p.id);
    //    submitData.append(`items[][${index}][name]`, p.name);
    //    submitData.append(`items[][${index}][displayOrder]`, p.displayOrder);
    //};

    ajaxPost(endpoint, submitData, cbReOrderSuccess, cbReOrderError);
};

cbReOrderSuccess = (data) => {
    var title = localStorage.getItem('__title__');
    if (data.success) {
        reorderDataTableAfterAnimation();
        $("#dv-media-gallery").removeClass("disabled").tooltip("dispose");
        ToastAlert('success', title, data.message);
    } else {
        ToastAlert('error', title, data.message);
    }
}

cbReOrderError = (data) => {
    ToastAlert('error', title, data.message);
}
///==============================================
/// end of row-reorder event
///==============================================

getDataTableDisplayOrdering = () => {
    //allowReOrder variable is set in _DataTablePartial and value is reterived by calling API in Controller
    return [{ "targets": [0], "orderable": allowReOrder, className: allowReOrder ? "reorder" : "" }, { className: "text-wrap", targets: "_all", "orderable": false }];
}

reorderDataTableAfterAnimation = () => {
    setTimeout(function () { $("#datatable-default-").DataTable().draw(); }, 1000);
}
