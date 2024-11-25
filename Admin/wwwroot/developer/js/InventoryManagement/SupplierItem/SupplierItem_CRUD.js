var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadSuppliers();
});

loadSuppliers = () => {
    fillDropDownList('supplierList', 'Common/ForSupplierDropDownList', false, '', 'id', 'name', loadDataFor);
};

setup = () => {
    $("#dataForm").validate({
        rules: {
            supplierList: { select2Required: true, required: true },
            supplierItemName: { required: true },
            fileUpload: { required: true }
        },
        messages: {
            supplierList: { select2Required: 'Please select a Supplier' },
            supplierItemName: { required: 'Please enter the Supplier Item name' },
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
    ajaxGet('SupplierItem/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('supplierList', r.supplierId);
    setTextValue("supplierItemName", r.name);
    setHiddenData(r);
};

saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("supplierId", getSelectedItemValue('supplierList'));
    submitData.append("name", getTextValue('supplierItemName'));

    // Handle file upload
    var fileInput = document.getElementById("supplierItemExcelFile");
    if (fileInput.files.length > 0) {
        submitData.append("file", fileInput.files[0]);
    }

    submitHiddenData(submitData);
    ajaxPost("SupplierItem/AddEdit", submitData, cbImportSuccess, cbImportError);
};

// New function for importing categories from Excel
importSupplierItem = () => {
    let fileInput = document.getElementById('supplierItemExcelFile');
    let file = fileInput.files[0];

    if (!file) {
        ToastAlert('error', 'Import Error', 'Please select a file to import.');
        return;
    }

    let formData = new FormData();
    formData.append("file", file);
    formData.append("supplierId", getSelectedItemValue('supplierList'));

    // Disable the import button to prevent multiple submissions
    document.getElementById("btnImport").disabled = true;

    ajaxPost("SupplierItem/ImportPriceTypeCategoriesFromExcel", formData, cbImportSuccess, cbImportError);
}

// Callback function for successful import
cbImportSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'SupplierItem', 'Supplier Item imported successfully.');
        location.reload();
    } else {
        ToastAlert('error', 'Import Error', data.message || 'Failed to import Supplier Item.');
    }
    document.getElementById("btnImport").disabled = false;
}

// Callback function for import error
cbImportError = (error) => {
    ToastAlert('error', 'Import Error', 'Failed to import Supplier Item. Please try again or contact the system admin.');
    document.getElementById("btnImport").disabled = false;
}
