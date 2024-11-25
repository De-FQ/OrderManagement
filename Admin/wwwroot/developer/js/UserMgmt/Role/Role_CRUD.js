var guid = '';
$(function () { // Handler for .ready() called. 
    setup();
    guid = getTextValue("Guid"); 
    fillDropDownList('roleTypeList', 'Common/ForRoleTypeDropDownList', false, '', 'id', 'name', loadDataFor);
});

setup = () => {
    $("#dataForm").validate({
        rules: {
            name: { required: true },
            roleTypeList: { select2Required: true }
        },
        messages: {
            name: { required: 'Please enter a name' },
            roleTypeList: { select2Required: 'Please select a type' },
        },
        submitHandler: function (form, event) {
            event.preventDefault();
            $("button#btnSave").attr('disabled', 'disabled');
            saveData();
        },
        errorPlacement: function ($error, $element) {
            if ($element.siblings(".invalid-feedback").length)
                $element.siblings(".invalid-feedback").html($error);
        }
    });
};
 
loadDataFor = ()=> {
    if (guid == '') { return; } 
    ajaxGet('UserRole/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;

    setTextValue("name", r.name);
    setSelectedItemByValueAndTriggerChangeEvent('roleTypeList', r.userRoleTypeId);

    setHiddenData(r);
}

saveData = () => { 
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append('name', getTextValue('name'));
    submitData.append('userRoleTypeId', getSelectedItemValue('roleTypeList'));

    submitHiddenData(submitData);
    ajaxPost("UserRole/AddEdit", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => { 
    if (data.success) {
        ToastAlert('success', 'Role', 'Saved Successfully');
        setTimeout(() => location.href = "/UserMgmt/Role/RoleList", 2000);
    } else {
        ToastAlert('error', 'Role', 'Unable to save, please try again or contact to system admin');
    }
}

cbPostError = (error) => { 
    ToastAlert('error', 'Role', 'Unable to save, please try again or contact to system admin');
}