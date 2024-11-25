
// Initialize and set up the form and data
var supplierGuid = '';
var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadCategories();
    loadSubCategories();
});

// Set up form validation and submission
setup = () => {
    $("#dataForm").validate({
        rules: {
            name: { required: true },
            address: { required: true },
            contact: { required: true },
        },
        messages: {
            name: { required: 'Please enter the supplier name' },
            address: { required: 'Please enter the supplier address' },
            contact: { required: 'Please enter the supplier contact' },
        },
        submitHandler: function (form, event) {
            event.preventDefault();
            $("button#btnSave").attr('disabled', 'disabled');
            saveSupplierData();
        },
        errorPlacement: function ($error, $element) {
            if ($element.siblings(".invalid-feedback").length)
                $element.siblings(".invalid-feedback").html($error);
        }
    });
};

// Load supplier data if a GUID is provided
loadSupplierData = () => {
    if (supplierGuid == '') { return; }
    ajaxGet('Supplier/GetByGuid?guid=' + supplierGuid, cbGetSupplierSuccess);
};

// Callback function to handle successful data retrieval
cbGetSupplierSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;

    console.log(r);
    setTextValue("name", r.name);
    setTextValue("address", r.address);
    setTextValue("contact", r.contact);
    setHiddenData(r);
};

// Function to save supplier data
saveSupplierData = () => {
    let submitData = new FormData();
    submitData.append("Guid", supplierGuid);
    submitData.append("name", getTextValue('name'));
    submitData.append("address", getTextValue('address'));
    submitData.append("contact", getTextValue('contact'));

    submitHiddenData(submitData);
    ajaxPost("Supplier/AddEdit", submitData, cbPostSupplierSuccess, cbPostSupplierError);
};

// Callback function for successful data submission
cbPostSupplierSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'Supplier', 'Saved Successfully');
        setTimeout(() => location.href = "/Supplier/SupplierList", 2000);
    } else {
        ToastAlert('error', 'Supplier', 'Unable to save, please try again or contact the system admin');
    }
};

// Callback function for data submission error
cbPostSupplierError = (error) => {
    ToastAlert('error', 'Supplier', 'Unable to save, please try again or contact the system admin');
};
