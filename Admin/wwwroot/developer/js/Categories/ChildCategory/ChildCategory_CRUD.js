// Function to set the state of a checkbox or toggle switch
function setChecked(elementId, value) {
    document.getElementById(elementId).checked = value;
}

// Function to get the state of a checkbox or toggle switch
function getChecked(elementId) {
    return document.getElementById(elementId).checked;
}

// Initialize and set up the form and data
var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadCategories();
    loadSubCategories();
});

// Load categories and subcategories
loadCategories = () => {
    fillDropDownList('categoryList', 'Common/ForCategoryDropDownList', false, '', 'id', 'name', loadDataFor);
};

loadSubCategories = () => {
    fillDropDownList('subCategoryList', 'Common/ForSubCategoryDropDownList', false, '', 'id', 'name', loadDataFor);
};

// Set up form validation and submission
setup = () => {
    $("#dataForm").validate({
        rules: {
            categoryList: { select2Required: true, required: true },
            subCategoryList: { select2Required: true, required: true },
            childCategoryName: { required: true },
            description: { required: true },
            discountPercentage: {
                number: true,
                min: 0,
                max: 100
            },
            imageFile: {
                required: function () {
                    return $('#imagePreview').attr('src') == '';
                }
            },
        },
        messages: {
            categoryList: { select2Required: 'Please select a category' },
            subCategoryList: { select2Required: 'Please select a subcategory' },
            childCategoryName: { required: 'Please enter the child category name' },
            description: { required: 'Please enter the description' },
            discountPercentage: {
                number: 'Please enter a valid percentage',
                min: 'Discount percentage cannot be negative',
                max: 'Discount percentage cannot exceed 100'
            },
            imageFile: { required: 'Please upload an image' },
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

// Load data for a specific child category if a GUID is provided
loadDataFor = () => {
    if (guid == '') { return; }
    ajaxGet('ChildCategory/GetByGuid?guid=' + guid, cbGetSuccess);
};

// Callback function to handle successful data retrieval
cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('categoryList', r.categoryId);
    setSelectedItemByValueAndTriggerChangeEvent('subCategoryList', r.subCategoryId);
    setTextValue("childCategoryName", r.name);
    setTextValue("description", r.description);
    setTextValue("discountPercentage", r.discountPercentage || '');
    setChecked("discountActive", r.discountActive || false); // Set discountActive checkbox

    if (r.imageUrl != null) {
        setImage("imagePreview", r.imageUrl);
        $("#imageFile").data("oldImageName", r.imageName);
        $("#imageFile").rules("remove", "required");
    }
    setHiddenData(r);
}

// Function to save child category data
saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("categoryId", getSelectedItemValue('categoryList'));
    submitData.append("subCategoryId", getSelectedItemValue('subCategoryList'));
    submitData.append("name", getTextValue('childCategoryName'));
    submitData.append("description", getTextValue('description'));
    submitData.append("discountPercentage", getTextValue('discountPercentage'));
    submitData.append("discountActive", getChecked('discountActive')); // Get discountActive checkbox state

    var imageFile = document.getElementById("imageFile");
    if (imageFile.files.length > 0) {
        var compressedFile = imageFile.files[0];
        submitData.append("image", compressedFile);
        submitData.append("imageName", compressedFile.name);
    }

    submitHiddenData(submitData);
    ajaxPost("ChildCategory/AddEdit", submitData, cbPostSuccess, cbPostError);
};

// Callback function for successful data submission
cbPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'ChildCategory', 'Saved Successfully');
        setTimeout(() => location.href = "/ChildCategory/ChildCategoryList", 2000);
    } else {
        ToastAlert('error', 'ChildCategory', 'Unable to save, please try again or contact the system admin');
    }
}

// Callback function for data submission error
cbPostError = (error) => {
    ToastAlert('error', 'ChildCategory', 'Unable to save, please try again or contact the system admin');
}

// Function to handle importing child categories from an Excel file
function importChildCategoriesFromExcel() {
    let fileInput = document.getElementById('childCategoryExcelFile');
    let file = fileInput.files[0];

    if (!file) {
        ToastAlert('error', 'Import Error', 'Please select a file to import.');
        return;
    }

    let formData = new FormData();
    formData.append("file", file);
    formData.append("categoryId", getSelectedItemValue('categoryList'));
    formData.append("subCategoryId", getSelectedItemValue('subCategoryList'));

    // Disable the import button to prevent multiple submissions
    document.getElementById("btnImport").disabled = true;

    ajaxPost("ChildCategory/ImportFromExcel", formData, cbImportSuccess, cbImportError);
}

// Callback function for successful import
function cbImportSuccess(data) {
    if (data.success) {
        ToastAlert('success', 'ChildCategory', 'Child categories imported successfully');
        // Reload the page or update the UI as needed
        location.reload();
    } else {
        ToastAlert('error', 'Import Error', data.message || 'Failed to import child categories.');
    }
    // Re-enable the import button
    document.getElementById("btnImport").disabled = false;
}

// Callback function for import error
function cbImportError(error) {
    ToastAlert('error', 'Import Error', 'Failed to import child categories. Please try again or contact the system admin.');
    // Re-enable the import button
    document.getElementById("btnImport").disabled = false;
}
