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

    // Attach the import function to the import button
    $("#btnImport").click(importSubCategories);
});

// Set up form validation and submission
setup = () => {
    $("#dataForm").validate({
        rules: {
            categoryList: { select2Required: true, required: true },
            subCategoryName: { required: true },
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
            subCategoryName: { required: 'Please enter the subcategory name' },
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

// Load categories into dropdown list
loadCategories = () => {
    fillDropDownList('categoryList', 'Common/ForCategoryDropDownList', false, '', 'id', 'name', loadDataFor);
};

// Load subcategory data if a GUID is provided
loadDataFor = () => {
    if (guid == '') { return; }
    ajaxGet('SubCategory/GetByGuid?guid=' + guid, cbGetSuccess);
};

// Callback function to handle successful data retrieval
cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('categoryList', r.categoryId);
    setTextValue("subCategoryName", r.name);
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

// Function to save subcategory data
saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("categoryId", getSelectedItemValue('categoryList'));
    submitData.append("name", getTextValue('subCategoryName'));
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
    ajaxPost("SubCategory/AddEdit", submitData, cbPostSuccess, cbPostError);
};

// Callback function for successful data submission
cbPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'SubCategory', 'Saved Successfully');
        setTimeout(() => location.href = "/SubCategory/SubCategoryList", 2000);
    } else {
        ToastAlert('error', 'SubCategory', 'Unable to save, please try again or contact to system admin');
    }
}

// Callback function for data submission error
cbPostError = (error) => {
    ToastAlert('error', 'SubCategory', 'Unable to save, please try again or contact to system admin');
}

// Function to handle the import of subcategories from an Excel file
importSubCategories = () => {
    let fileInput = document.getElementById("subCategoryExcelFile");
    let file = fileInput.files[0];
    let categoryId = getSelectedItemValue('categoryList');

    if (!file) {
        ToastAlert('error', 'SubCategory', 'Please select an Excel file to upload.');
        return;
    }

    if (!categoryId) {
        ToastAlert('error', 'SubCategory', 'Please select a category.');
        return;
    }

    let formData = new FormData();
    formData.append("file", file);
    formData.append("categoryId", categoryId);

    ajaxPost("SubCategory/ImportFromExcel", formData, cbImportSuccess, cbImportError);
};

// Callback function for successful import
cbImportSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'SubCategory', 'Subcategories imported successfully.');
        // Optionally reload the page or data
    } else {
        ToastAlert('error', 'SubCategory', data.message || 'Failed to import subcategories.');
    }
};

// Callback function for import error
cbImportError = (error) => {
    ToastAlert('error', 'SubCategory', 'An error occurred during the import. Please try again.');
}
