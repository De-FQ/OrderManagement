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
    if (guid == '') { return; } 
    ajaxGet('Country/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;

    setTextValue("name", r.name);
    setTextValue("nameAr", r.nameAr);
    setTextValue('countryCode', r.countryCode);
    setTextValue('mobileCode', r.mobileCode);
    setTextValue('currencyCodeEn', r.currencyCodeEn);
    setTextValue('currencyCodeAr', r.cCurrencyCodeAr);
    setTextValue('currencyFormat', r.currencyFormat);
    setTextValue('permissionId', r.parentPermissionId);
    setCheckValue('IsDefault', r.isDefault); 
     
    setHiddenData(r);
}
  
saveData = () => { 
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append('name', getTextValue('name'));
    submitData.append('nameAr', getTextValue('nameAr'));
    submitData.append('countryCode', getTextValue('countryCode')); 
    submitData.append('mobileCode', getTextValue('mobileCode'));
    submitData.append('currencyCodeEn', getTextValue('currencyCodeEn'));
    submitData.append('currencyCodeAr', getTextValue('currencyCodeAr'));
    submitData.append('currencyFormat', getCheckValue('isDefault'));  
     
    submitHiddenData(submitData);
    ajaxPostAdmin("https://localhost:7151/Country/AddEdit_For_API", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => { 

    if (data.success) {
        ToastAlert('success', 'Country', data.message);
        setTimeout(() => location.href = "/Country/CountryList", 2000);
    } else {
        ToastAlert('error', 'Country', 'Unable to save, please try again or contact to system admin');
    }
}

cbPostError = (error) => { 
    ToastAlert('error', 'Country', 'Unable to save, please try again or contact to system admin');
}