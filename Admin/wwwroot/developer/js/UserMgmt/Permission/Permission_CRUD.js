var guid = '';
$(function () { // Handler for .ready() called. 
    guid = getTextValue("Guid");
    loadDataFor();
    setUpFormValidation();
    $.getJSON("/assets/json/Icons.json", function (obj) {
        var html = "<option>Please select an icon...</option>";
        obj.Icons.forEach(function (item, index) {
            html += (`<optgroup label="${item.Type}"">`);
            item.Content.forEach(function (gitem, index) {
                html += (`<option data-icon="${gitem.class}" value="${gitem.class}">${gitem.name}</option>`);
            });
        });
        $("#iconsList").html(html);
    });

    $("#iconsList").select2({
        theme: "bootstrap",
        templateSelection: formatText,
        templateResult: formatText
    });
});
formatText = (state) => {
    if ($(state.element).data('icon') != null)
        return $('<span><i class="pe-2 ' + $(state.element).data('icon') + '"></i> ' + state.text + '</span>');
    else
        return $('<span>' + state.text + '</span>');

};
setUpFormValidation = () => {
    $("#dataForm").validate({
        rules: {
            title: { required: true, minlength: 4, maxlength: 50, },
            navigationUrl: { required: true },
        },
        messages: {
            title: { required: 'Required' },
            navigationUrl: { required: 'Required' }
        },

        submitHandler: function (form, event) {
            event.preventDefault();
            $("button#btnSave").attr('disabled', 'disabled');
            saveData();
        }
    });

};


loadDataFor = () => {
    if (guid == '') { return; }
    ajaxGet('UserPermission/GetByGuid?guid=' + guid, cbSuccess);
};

cbSuccess = (data) => {
    if (data.data == null) return;
    var r = data.data;
    setTextValue('title', r.title);
    setTextValue('titleAr', r.titleAr);
    setTextValue('navigationUrl', r.navigationUrl);
    /*setTextValue('icon', r.icon);*/
    setTextValue('permissionId', r.userPermissionId);
    setTextValue("displayOrder").val(r.displayOrder);
    setCheckValue('showInMenu', r.showInMenu);

    ///icons update from select list
    if (r.icon != "" && r.icon != null) {
        $("#iconsList").val(r.icon).trigger('change');
    }
    if (r.accessList != "" && r.accessList != null) {
        var pm = r.accessList.split(',');
        if (pm.length > 0) {
            $('#accessList').val(pm).change();
        }
    }
    setHiddenData(r);
}


saveData = () => {
    let submitData = new FormData();
    submitData.append("guid", guid);
    submitData.append('title', getTextValue('title'));
    submitData.append('titleAr', getTextValue('titleAr'));
    submitData.append('navigationUrl', getTextValue('navigationUrl'));
    //submitData.append('icon', getTextValue('icon'));
    submitData.append('icon', $("#iconsList").val());
    submitData.append('userPermissionId', getTextValue('permissionId'));
    submitData.append('displayOrder', getIntegerValue('displayOrder'));
    submitData.append('accessList', $("#accessList").val().toString());
    submitData.append('showInMenu', getCheckValue('showInMenu'));
    submitHiddenData(submitData);
    ajaxPost("UserPermission/AddEdit", submitData, callbackPostSuccess, callbackPostError);

};


callbackPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'User Permission', 'Saved Successfully');
        setTimeout(() => location.href = "/UserMgmt/Permission/PermissionList", 2000);
    } else {
        showLog(data);
        ToastAlert('error', 'User Permission', 'unable to save, please try again or contact to system admin');
    }
}

callbackPostError = (error) => {
    ToastAlert('error', 'User Permission', 'unable to save, please try again or contact to system admin');
}

