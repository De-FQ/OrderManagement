var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadSupplier();
    setupCostPriceCalculation();  // Setup the cost price calculation on input change
});

loadSupplier = () => {
    fillDropDownList('supplierList', 'Common/ForSupplierDropDownList', false, '', 'id', 'name', loadDataFor);
};

setup = () => {
    $("#dataForm").validate({
        rules: {
            supplierList: { select2Required: true, required: true },
            inventoryItemName: { required: true },
            description: { required: true },
            unit: { required: true },
            unitCost: { required: true, number: true },
            quantity: { required: true, digits: true },
            costPrice: { required: true, number: true },
        },
        messages: {
            supplierList: { select2Required: 'Please select a supplier' },
            inventoryItemName: { required: 'Please enter the inventory item name' },
            description: { required: 'Please enter the inventory item description' },
            unit: { required: 'Please enter the unit' },
            unitCost: { required: 'Please enter the unit cost', number: 'Please enter a valid number' },
            quantity: { required: 'Please enter the quantity', digits: 'Please enter a valid number' },
            costPrice: { required: 'Please enter the cost price', number: 'Please enter a valid number' },
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

setupCostPriceCalculation = () => {
    $('#unitCost, #quantity').on('input', function () {
        calculateCostPrice();
    });
};

calculateCostPrice = () => {
    let unitCost = parseFloat(getTextValue('unitCost')) || 0;
    let quantity = parseInt(getTextValue('quantity')) || 0;
    let costPrice = unitCost * quantity;
    setTextValue('costPrice', costPrice.toFixed(2));
};

loadDataFor = () => {
    if (guid == '') { return; }
    ajaxGet('InventoryItem/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('supplierList', r.supplierId);
    setTextValue("inventoryItemName", r.name);
    setTextValue("description", r.description);
    setTextValue("unit", r.unit);
    setTextValue("unitCost", r.unitCost);
    setTextValue("quantity", r.quantity);
    setTextValue("costPrice", r.costPrice);
    setHiddenData(r);
};

saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("supplierId", getSelectedItemValue('supplierList'));
    submitData.append("name", getTextValue('inventoryItemName'));
    submitData.append("description", getTextValue('description'));
    submitData.append("unit", getTextValue('unit'));
    submitData.append("unitCost", getTextValue('unitCost'));
    submitData.append("quantity", getTextValue('quantity'));
    submitData.append("costPrice", getTextValue('costPrice'));
    submitHiddenData(submitData);
    ajaxPost("InventoryItem/AddEdit", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'InventoryItem', 'Saved Successfully');
        setTimeout(() => location.href = "/InventoryItem/InventoryItemList", 2000);
    } else {
        ToastAlert('error', 'InventoryItem', 'Unable to save, please try again or contact to system admin');
    }
};

cbPostError = (error) => {
    ToastAlert('error', 'InventoryItem', 'Unable to save, please try again or contact to system admin');
};
