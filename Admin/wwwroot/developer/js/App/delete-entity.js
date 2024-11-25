addDeletePopupAction = (apibase, guid, callbackFunction) => {    
    var html = `<a href="javascript:;" onclick="javascript:doDeleteEntity('${apibase}','${guid}','${callbackFunction}')" class="mb-1 mt-1 me-1 btn btn-sm dropdown-item btn-danger" aria-label="Delete">
                 <i class="fas fa-trash-can"></i>
                 <span class="ms-2">Delete</span>
               </a>`;
    return html;
}

doDeleteEntity = (apiName, guid, callbackFunction) => {
    $("#confirmDeleteModel").attr("data-api", apiName);
    $("#confirmDeleteModel").attr("data-guid", guid);
    $("#confirmDeleteModel").attr("data-callback", callbackFunction);
    $('#confirmDeleteModel').modal('toggle');
}

deleteEntity = () => {
    var endpoint = getAPIUrl() + $("#confirmDeleteModel").attr("data-api") + "?guid=" + $("#confirmDeleteModel").attr("data-guid");
    $('#confirmDeleteModel').modal('toggle');
    showLoader();
    $.ajax({
        url: endpoint,
        method: "GET",
        headers: getAjaxHeader(),
        success: function (data) {
            hideLoader();
            if (data.success) {
                var callbackFunction = $("#confirmDeleteModel").attr("data-callback")
                if (callbackFunction != "") {
                    eval(callbackFunction);
                }

                ToastAlert('success', `Delete`, `Deleted Successfully`);
            } else {
                ToastAlert('danger', `Delete`, data.message);
            }
        },
        error: function (xhr) {
            hideLoader();
            ToastAlert('error', 'Display Order', xhr);
        }
    });
}