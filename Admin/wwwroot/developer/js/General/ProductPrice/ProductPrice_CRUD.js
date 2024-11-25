var productPriceGuid = '';

$(function () {
    setupProductPrice();
    productPriceGuid = getTextValue("Guid");
    loadChildCategories();
});

loadChildCategories = () => {
    fillDropDownList('childCategoryList', 'Common/ForChildCategoryDropDownList', false, '', 'id', 'name');
    loadPriceTypeCategories();
};

loadPriceTypeCategories = () => {
    fillDropDownList('priceTypeCategoryList', 'Common/ForPriceTypeCategoryDropDownList', false, '', 'id', 'name');
    loadPriceTypes();
};

loadPriceTypes = () => {
    fillDropDownList('priceTypeList', 'Common/ForPriceTypeDropDownList', false, '', 'id', 'name');
    loadDataForProductPrice();
};

setupProductPrice = () => {
    $("#dataForm").validate({
        rules: {
            childCategoryList: { select2Required: true, required: true },
            priceTypeCategoryList: { select2Required: true, required: true },
            priceTypeList: { select2Required: true, required: true },
        },
        messages: {
            childCategoryList: { select2Required: 'Please select a child category' },
            priceTypeCategoryList: { select2Required: 'Please select a price type category' },
            priceTypeList: { select2Required: 'Please select a price type' },
        },
        submitHandler: function (form, event) {
            event.preventDefault();
            $("button#btnSave").attr('disabled', 'disabled');
            saveProductPriceData();
        },
        errorPlacement: function ($error, $element) {
            if ($element.siblings(".invalid-feedback").length)
                $element.siblings(".invalid-feedback").html($error);
        }
    });
};

loadDataForProductPrice = () => {
    if (productPriceGuid == '') { return; }
    ajaxGet('ProductPrice/GetByGuid?guid=' + productPriceGuid, cbGetProductPriceSuccess);
};

cbGetProductPriceSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('childCategoryList', r.childCategoryId);
    setSelectedItemByValueAndTriggerChangeEvent('priceTypeCategoryList', r.priceTypeCategoryId);
    setSelectedItemByValueAndTriggerChangeEvent('priceTypeList', r.priceTypeId);
    setTextValue("productPrice", r.price);
    setHiddenData(r);
};

saveProductPriceData = () => {
    let submitData = new FormData();
    submitData.append("Guid", productPriceGuid);
    submitData.append("childCategoryId", getSelectedItemValue('childCategoryList'));
    submitData.append("priceTypeCategoryId", getSelectedItemValue('priceTypeCategoryList'));
    submitData.append("priceTypeId", getSelectedItemValue('priceTypeList'));
    submitData.append("price", getTextValue('productPrice'));
    submitHiddenData(submitData);
    ajaxPost("ProductPrice/AddEdit", submitData, cbPostProductPriceSuccess, cbPostProductPriceError);
};

cbPostProductPriceSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'Product Price', 'Saved Successfully');
        setTimeout(() => location.href = "/ProductPrice/ProductPriceList", 2000);
    } else {
        ToastAlert('error', 'Product Price', 'Unable to save, please try again or contact to system admin');
    }
};

cbPostProductPriceError = (error) => {
    ToastAlert('error', 'Product Price', 'Unable to save, please try again or contact to system admin');
};

importProductPrices = () => {
    let fileInput = document.getElementById('productPriceExcelFile');
    let file = fileInput.files[0];

    if (!file) {
        ToastAlert('error', 'Import Error', 'Please select a file to import.');
        return;
    }

    let formData = new FormData();
    formData.append("file", file);
    formData.append("childCategoryId", getSelectedItemValue('childCategoryList'));
    formData.append("priceTypeCategoryId", getSelectedItemValue('priceTypeCategoryList'));
    formData.append("priceTypeId", getSelectedItemValue('priceTypeList'));

    // Disable the import button to prevent multiple submissions
    document.getElementById("btnImport").disabled = true;

    ajaxPost("ProductPrice/ImportFromExcel", formData, cbImportProductPriceSuccess, cbImportProductPriceError);
}

// Callback function for successful import
cbImportProductPriceSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'Product Price', 'Product prices imported successfully.');
        location.reload();
    } else {
        ToastAlert('error', 'Import Error', data.message || 'Failed to import product prices.');
    }
    document.getElementById("btnImport").disabled = false;
}

// Callback function for import error
cbImportProductPriceError = (error) => {
    ToastAlert('error', 'Import Error', 'Failed to import product prices. Please try again or contact the system admin.');
    document.getElementById("btnImport").disabled = false;
}
