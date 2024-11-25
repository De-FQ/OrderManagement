var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadPriceTypeCategories();
});

loadPriceTypeCategories = () => {
    fillDropDownList('priceTypeCategoryList', 'Common/ForPriceTypeCategoryDropDownList', false, '', 'id', 'name', loadDataFor);
};

setup = () => {
    $("#dataForm").validate({
        rules: {
            priceTypeCategoryList: { select2Required: true, required: true },
            priceTypeName: { required: true },
        },
        messages: {
            priceTypeCategoryList: { select2Required: 'Please select a price type category' },
            priceTypeName: { required: 'Please enter the price type name' },
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
    ajaxGet('PriceType/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('priceTypeCategoryList', r.priceTypeCategoryId);
    setTextValue("priceTypeName", r.name);
    setHiddenData(r);
}

saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("priceTypeCategoryId", getSelectedItemValue('priceTypeCategoryList'));
    submitData.append("name", getTextValue('priceTypeName'));
    submitHiddenData(submitData);
    ajaxPost("PriceType/AddEdit", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'PriceType', 'Saved Successfully');
        setTimeout(() => location.href = "/PriceType/PriceTypeList", 2000);
    } else {
        ToastAlert('error', 'PriceType', 'Unable to save, please try again or contact to system admin');
    }
}

cbPostError = (error) => {
    ToastAlert('error', 'PriceType', 'Unable to save, please try again or contact to system admin');
}


// New function for importing from Excel
importPriceTypes = () => {
    let fileInput = document.getElementById('priceTypeExcelFile');
    let file = fileInput.files[0];

    if (!file) {
        ToastAlert('error', 'Import Error', 'Please select a file to import.');
        return;
    }

    let formData = new FormData();
    formData.append("file", file);
    formData.append("priceTypeCategoryId", getSelectedItemValue('priceTypeCategoryList'));

    // Disable the import button to prevent multiple submissions
    document.getElementById("btnImport").disabled = true;

    ajaxPost("PriceType/ImportPriceTypeFromExcel", formData, cbImportSuccess, cbImportError);
}

// Callback function for successful import
cbImportSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'PriceType', 'Price type categories imported successfully.');
        location.reload();
    } else {
        ToastAlert('error', 'Import Error', data.message || 'Failed to import price type.');
    }
    document.getElementById("btnImport").disabled = false;
}

// Callback function for import error
cbImportError = (error) => {
    ToastAlert('error', 'Import Error', 'Failed to import price type. Please try again or contact the system admin.');
    document.getElementById("btnImport").disabled = false;
}
