var guid = '';
$(function () { // Handler for .ready() called. 
    setup();
    guid = getTextValue("Guid");
    loadDataFor();
});

 
setup = () => {
    $("#dataForm").validate({
        rules: {
            name: { required: true },
            nameAr: { required: true }
        },
        messages: {
            name: { required: 'Please enter a name' },
            nameAr: { required: 'Please enter a name' },
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
 
loadDataFor = () => {
    if (guid == '') {
        return;
    } 
    ajaxGet('Governorate/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;

    setTextValue("name", r.name);
    setTextValue("nameAr", r.nameAr); 
    setHiddenData(r);
}

saveData = () => {
    showLoader();
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append('name', getTextValue('name'));
    submitData.append('nameAr', getTextValue('nameAr'));

    submitHiddenData(submitData);
    ajaxPost("Governorate/AddEdit", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => { 
    if (data.success) {
        ToastAlert('success', 'Governorate', data.message);
        setTimeout(() => location.href = "/Settings/Governorate/GovernorateList", 1000);
    } else {
        ToastAlert('error', 'Governorate', 'Unable to save, please try again or contact to system admin');
    }
}

cbPostError = (error) => { 
    ToastAlert('error', 'Governorate', 'Unable to save, please try again or contact to system admin');
}