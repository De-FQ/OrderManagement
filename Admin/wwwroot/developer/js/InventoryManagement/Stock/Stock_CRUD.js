var guid = '';
$(function () {
    setup();
    guid = getTextValue("Guid");
    loadInventoryItem();
    setupCostCalculations();
});

loadInventoryItem = () => {
    fillDropDownList('inventoryItemList', 'Common/ForInventoryItemDropDownList', false, '', 'id', 'name', loadDataFor);
};

setup = () => {
    $("#dataForm").validate({
        rules: {
            inventoryItemList: { select2Required: true, required: true },
            unit: { required: true },
            netUnitCost: { required: true, number: true },
            companyCostMargin: { required: true, number: true },
            oldQuantity: { required: true, digits: true },
            newQuantity: { required: true, digits: true },
            totalQuantity: { required: true, digits: true },
            totalUnitNetCost: { required: true, number: true },
            totalUnitCompanyPrice: { required: true, number: true },
        },
        messages: {
            inventoryItemList: { select2Required: 'Please select an inventory item' },
            unit: { required: 'Please enter the unit' },
            netUnitCost: { required: 'Please enter the net unit cost', number: 'Please enter a valid number' },
            companyCostMargin: { required: 'Please enter the company cost margin', number: 'Please enter a valid number' },
            oldQuantity: { required: 'Please enter the old quantity', digits: 'Please enter a valid number' },
            newQuantity: { required: 'Please enter the new quantity', digits: 'Please enter a valid number' },
            totalQuantity: { required: 'Please enter the total quantity', digits: 'Please enter a valid number' },
            totalUnitNetCost: { required: 'Please enter the total unit net cost', number: 'Please enter a valid number' },
            totalUnitCompanyPrice: { required: 'Please enter the total unit company price', number: 'Please enter a valid number' },
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

setupCostCalculations = () => {
    $('#netUnitCost, #newQuantity, #companyCostMargin, #oldQuantity').on('input', function () {
        calculateTotalQuantity();
        calculateTotalCosts();
    });
};

calculateTotalCosts = () => {
    let netUnitCost = parseFloat(getTextValue('netUnitCost')) || 0;
    let totalQuantity = parseInt(getTextValue('totalQuantity')) || 0;
    let companyCostMargin = parseFloat(getTextValue('companyCostMargin')) || 0;

    let totalUnitNetCost = netUnitCost * totalQuantity;
    let totalUnitCompanyPrice = totalUnitNetCost + (totalUnitNetCost * companyCostMargin / 100);

    setTextValue('totalUnitNetCost', totalUnitNetCost.toFixed(2));
    setTextValue('totalUnitCompanyPrice', totalUnitCompanyPrice.toFixed(2));
};

calculateTotalQuantity = () => {
    let oldQuantity = parseInt(getTextValue('oldQuantity')) || 0;
    let newQuantity = parseInt(getTextValue('newQuantity')) || 0;
    let totalQuantity = oldQuantity + newQuantity;

    setTextValue('totalQuantity', totalQuantity);
};

loadDataFor = () => {
    if (guid == '') { return; }
    ajaxGet('Stock/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;
    setSelectedItemByValueAndTriggerChangeEvent('inventoryItemList', r.inventoryItemId);
    setTextValue("unit", r.unit);
    setTextValue("netUnitCost", r.netUnitCost);
    setTextValue("companyCostMargin", r.companyCostMargin);
    setTextValue("oldQuantity", r.oldQuantity);
    setTextValue("newQuantity", r.newQuantity);
    setTextValue("totalQuantity", r.totalQuantity);
    setTextValue("totalUnitNetCost", r.totalUnitNetCost);
    setTextValue("totalUnitCompanyPrice", r.totalUnitCompanyPrice);
    setHiddenData(r);
};

saveData = () => {
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("inventoryItemId", getSelectedItemValue('inventoryItemList'));
    submitData.append("unit", getTextValue('unit'));
    submitData.append("netUnitCost", getTextValue('netUnitCost'));
    submitData.append("companyCostMargin", getTextValue('companyCostMargin'));
    submitData.append("oldQuantity", getTextValue('oldQuantity'));
    submitData.append("newQuantity", getTextValue('newQuantity'));
    submitData.append("totalQuantity", getTextValue('totalQuantity'));
    submitData.append("totalUnitNetCost", getTextValue('totalUnitNetCost'));
    submitData.append("totalUnitCompanyPrice", getTextValue('totalUnitCompanyPrice'));
    submitHiddenData(submitData);
    ajaxPost("Stock/AddEdit", submitData, cbPostSuccess, cbPostError);
};

cbPostSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'Stock', 'Saved Successfully');
        setTimeout(() => location.href = "/Stock/StockList", 2000);
    } else {
        ToastAlert('error', 'Stock', 'Unable to save, please try again or contact system admin');
    }
};

cbPostError = (error) => {
    ToastAlert('error', 'Stock', 'Unable to save, please try again or contact system admin');
};
