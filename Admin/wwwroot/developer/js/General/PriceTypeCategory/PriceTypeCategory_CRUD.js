var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadChildCategories();
});

loadChildCategories = () => {
    fillDropDownList('childCategoryList', 'Common/ForChildCategoryDropDownList', false, '', 'id', 'name', loadDataFor);
};

setup = () => {
    $("#dataForm").validate({
        rules: {
            childCategoryList: { select2Required: true, required: true },
            priceTypeCategoryName: { required: true },
            fileUpload: { required: true }
        },
        messages: {
            childCategoryList: { select2Required: 'Please select a child category' },
            priceTypeCategoryName: { required: 'Please enter the price type category name' },
            fileUpload: { required: 'Please upload an Excel file' }
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
    ajaxGet('PriceTypeCategory/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('childCategoryList', r.childCategoryId);
    setTextValue("priceTypeCategoryName", r.name);
    setHiddenData(r);
};

saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("childCategoryId", getSelectedItemValue('childCategoryList'));
    submitData.append("name", getTextValue('priceTypeCategoryName'));

    // Handle file upload
    var fileInput = document.getElementById("priceTypeCategoryExcelFile");
    if (fileInput.files.length > 0) {
        submitData.append("file", fileInput.files[0]);
    }

    submitHiddenData(submitData);
    ajaxPost("PriceTypeCategory/AddEdit", submitData, cbImportSuccess, cbImportError);
};

// New function for importing categories from Excel
importPriceTypeCategories = () => {
    let fileInput = document.getElementById('priceTypeCategoryExcelFile');
    let file = fileInput.files[0];

    if (!file) {
        ToastAlert('error', 'Import Error', 'Please select a file to import.');
        return;
    }

    let formData = new FormData();
    formData.append("file", file);
    formData.append("childCategoryId", getSelectedItemValue('childCategoryList'));

    // Disable the import button to prevent multiple submissions
    document.getElementById("btnImport").disabled = true;

    ajaxPost("PriceTypeCategory/ImportPriceTypeCategoriesFromExcel", formData, cbImportSuccess, cbImportError);
}

// Callback function for successful import
cbImportSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'PriceTypeCategory', 'Price type categories imported successfully.');
        location.reload();
    } else {
        ToastAlert('error', 'Import Error', data.message || 'Failed to import price type categories.');
    }
    document.getElementById("btnImport").disabled = false;
}

// Callback function for import error
cbImportError = (error) => {
    ToastAlert('error', 'Import Error', 'Failed to import price type categories. Please try again or contact the system admin.');
    document.getElementById("btnImport").disabled = false;
}
