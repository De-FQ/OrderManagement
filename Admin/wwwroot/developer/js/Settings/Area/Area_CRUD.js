var guid = '';
$(function () { 
    setup();
    guid = getTextValue("Guid");
    fillDropDownList("governorateList", 'Governorate/ForDropDownList', false, null, "id", "name");
    fillDropDownList("countryList", 'Country/ForDropDownList', false, null, "id", "name", loadDataFor);
});

 
setup = () => {
    $("#dataForm").validate({
        rules: {
            nameEn: { required: true },
            nameAr: { required: true },
            deliveryFee: { required: true },
            minOrderAmount: { required: true }, 
            countryList: { select2Required: true },
            governorateList: { select2Required: true },
            expressdeliveryFee: { required: true },
        },
        messages: {
            nameEn: { required: '' },
            nameAr: { required: '' },
            deliveryFee: { required: '' },
            minOrderAmount: { required: '' },
            countryList: { select2Required: 'Please select a country' },
            governorateList: { select2Required: 'Please select a governorate' },
            expressdeliveryFee: { required: '' },
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
    if (guid == '') {return;} 
    ajaxGet('Area/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setTextValue("name", r.name);
    setTextValue("nameAr", r.nameAr);
    setTextValue("deliveryFee", r.deliveryFee);
    setTextValue("minOrderAmount", r.minOrderAmount);
    setSelectedItemByValueAndTriggerChangeEvent("governorateList", r.governorateId);
    setSelectedItemByValueAndTriggerChangeEvent("countryList", r.countryId);
    setTextValue("expressdeliveryFee", r.expressDeliveryFee);
    setHiddenData(r);
}

saveData = () => { 
    let submitData = new FormData();
    submitData.append("Guid", guid); 
    submitData.append('name', getTextValue('name'));
    submitData.append('nameAr', getTextValue('nameAr'));
    submitData.append('deliveryFee', getFloatValue('deliveryFee'));
    submitData.append('minOrderAmount', getFloatValue('minOrderAmount'));
    submitData.append('countryId', getSelectedItemValue('countryList'));
    submitData.append('governorateId', getSelectedItemValue('governorateList'));
    submitData.append('expressDeliveryFee', getFloatValue('expressdeliveryFee'));

    submitHiddenData(submitData);
    ajaxPost("Area/AddEdit", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => { 
    if (data.success) {
        ToastAlert('success', 'Area', data.message);
        setTimeout(() => location.href = "/Settings/Area/AreaList", 2000);
    } else {
        ToastAlert('error', 'Area', 'Unable to save, please try again or contact to system admin');
    }
}

cbPostError = (error) => { 
    ToastAlert('error', 'Area', 'Unable to save, please try again or contact to system admin');
}