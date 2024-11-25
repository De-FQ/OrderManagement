
 
addPopupAction = (apibase, row) => {
    var html =
        `<span data-bs-toggle1="modal" data-bs-target1="#sort_modal">
                    <a href="javascript:;" onclick="javascript:doDisplayOrder('${apibase}','${row.guid}','${row.id}',${row.displayOrder})"
                        class="mb-1 mt-1 me-1 btn btn-sm btn-warning" data-bs-toggle="tooltip" data-bs-placement="bottom"
                        title="Display Order" data-bs-original-title="Display Order" aria-label="Display Order">
                        <i class="fas fa-sort"></i>
                    </a>
                </span>`;
    return html;
}

doDisplayOrder = (apiName, guid, id, num) => {
      
    $('#DisplayOrderModel').modal('toggle');
    $("#displayapiname").val(apiName);
    $("#displayorderguid").val(guid);
    $("#displayorderid").val(id);
    $("#displayordervalue").val(num);
    $("#displayordervalue").focus(); 
}

updateDisplayOrder = () => {
    var endpoint = getAPIUrl() + $('#displayapiname').val() + "/UpdateDisplayOrder?guid=" + $('#displayorderguid').val() + "&num=" + $('#displayordervalue').val();

    $('#DisplayOrderModel').modal('toggle');

    showLoader();
    $.ajax({
        url: endpoint,
        method: "POST",
        headers: getAjaxHeader(),
        success: function (data) {
            hideLoader();
            if (data.success) {
                //updateDataTableDisplayOrder(data); //// It's commented because first column is the serial number
                ToastAlert('success', 'Display Order', 'Successfully updated');
                setTimeout(searchDataTable(), 1000);
            } else {
                showLog(data);
                ToastAlert('danger', 'Display Order', data.message);
                checkAPIResponse(data);
            }

        },
        error: function (xhr) {
            hideLoader();
            ToastAlert('error', 'Display Order', xhr);
        }
    });
}

updateDataTableDisplayOrder = (data) => {
    showLog(data);
    if (data.data == null) { return; }
    var row = data.data;
    $("[data-rowid=" + row["id"] + "]").find('[data-displayorder]').html(row["displayOrder"]);
     
    //var item = $("[data-displayOrder=" + row["id"] + "]").html(row["displayOrder"])
    //$("[data-displayorder-id=" + row["id"] + "]").html(row["displayOrder"])
}


