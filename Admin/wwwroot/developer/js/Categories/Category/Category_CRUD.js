
// Initialize and set up the form and data
var categoryGuid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadCategoryData();
});

// Set up form validation and submission
function setup() {
    $("#dataForm").validate({
        rules: {
            name: { required: true },
            description: { required: true },
            discountPercentage: {
                number: true,
                min: 0,
                max: 100
            },
            uploadImage: {
                required: function () {
                    return $('#previewImage').attr('src') == '';
                }
            },
        },
        messages: {
            name: { required: 'Please enter the category name' },
            description: { required: 'Please enter the category description' },
            discountPercentage: {
                number: 'Please enter a valid percentage',
                min: 'Discount percentage cannot be negative',
                max: 'Discount percentage cannot exceed 100'
            },
            uploadImage: { required: 'Please upload an image' },
        },
        submitHandler: function (form, event) {
            event.preventDefault();
            $("button#btnSave").attr('disabled', 'disabled');
            saveCategoryData();
        },
        errorPlacement: function ($error, $element) {
            if ($element.siblings(".invalid-feedback").length)
                $element.siblings(".invalid-feedback").html($error);
        }
    });
}

// Load category data if a GUID is provided
function loadCategoryData() {
    if (guid == '') { return; }
    ajaxGet('Category/GetByGuid?guid=' + guid, cbGetCategorySuccess);
}

// Callback function to handle successful data retrieval
function cbGetCategorySuccess(data) {
    if (data.data == null) { return; }
    var r = data.data; 
    setTextValue("name", r.name);
    setTextValue("description", r.description);
    setTextValue("discountPercentage", r.discountPercentage || '');
    setChecked("discountActive", r.discountActive || false); // Set discountActive checkbox

    if (r.imageUrl != null) {
        setImage("previewImage", r.imageUrl);
        $("#uploadImage").data("oldImageName", r.imageName);
        $("#uploadImage").rules("remove", "required");
    }
    setHiddenData(r);
}

// Function to save category data
function saveCategoryData() {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("name", getTextValue('name'));
    submitData.append("description", getTextValue('description'));
    submitData.append("discountPercentage", getTextValue('discountPercentage'));
    submitData.append("discountActive", getChecked('discountActive')); // Get discountActive checkbox state

    var imageFile = document.getElementById("uploadImage");
    if (imageFile.files.length > 0) {
        var compressedFile = imageFile.files[0];
        submitData.append("image", compressedFile);
        submitData.append("imageName", compressedFile.name);
    }

    submitHiddenData(submitData);
    ajaxPost("Category/AddEdit", submitData, cbPostCategorySuccess, cbPostCategoryError);
}

// Callback function for successful data submission
function cbPostCategorySuccess(data) {
    if (data.success) {
        ToastAlert('success', 'Category', 'Saved Successfully');
        setTimeout(() => location.href = "/Category/CategoryList", 2000);
    } else {
        ToastAlert('error', 'Category', 'Unable to save, please try again or contact the system admin');
    }
}

// Callback function for data submission error
function cbPostCategoryError(error) {
    ToastAlert('error', 'Category', 'Unable to save, please try again or contact the system admin');
}

// Function to handle the import process from an Excel file
function importCategoriesFromExcel() {
    var fileInput = document.getElementById("importExcelFile");
    if (fileInput.files.length === 0) {
        ToastAlert('error', 'Category', 'Please select an Excel file to upload');
        return;
    }

    var formData = new FormData();
    formData.append("file", fileInput.files[0]);

    // Sending the file to the server
    ajaxPost("Category/ImportFromExcel", formData, cbImportSuccess, cbImportError);
}


// Callback function for successful import
function cbImportSuccess(data) {
    if (data.success) {
        ToastAlert('success', 'Category', data.message);
        setTimeout(() => location.href = "/Category/CategoryList", 2000);
    } else {
        ToastAlert('error', 'Category', data.message);
    }
}

// Callback function for import error
function cbImportError(error) {
    ToastAlert('error', 'Category', 'Unable to import categories, please try again or contact the system admin');
}
// Function to set the state of a checkbox or toggle switch
function setChecked(elementId, value) {
    document.getElementById(elementId).checked = value;
}

// Function to get the state of a checkbox or toggle switch
function getChecked(elementId) {
    return document.getElementById(elementId).checked;
}

// Function to set the value of a text input or textarea
function setTextValue(elementId, value) {
    document.getElementById(elementId).value = value;
}

// Function to get the value of a text input or textarea
function getTextValue(elementId) {
    return document.getElementById(elementId).value;
}

// Function to set the image source for an image element
function setImage(elementId, imageUrl) {
    document.getElementById(elementId).src = imageUrl;
}
