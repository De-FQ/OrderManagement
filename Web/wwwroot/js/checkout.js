let heldOrders = JSON.parse(localStorage.getItem('heldOrders')) || [];

function checkout() {
    let checkoutItemsContainer = document.getElementById('checkoutItemsContainer');
    checkoutItemsContainer.innerHTML = '';

    let totalAmount = 0;

    cart.forEach((item, index) => {
        let checkoutItemDiv = document.createElement('div');
        checkoutItemDiv.className = 'checkout-item-row';

        checkoutItemDiv.innerHTML = `
            <div class="row mb-3">
                <div class="col-1">${index + 1}</div>
                <div class="col-2"><img src="${item.childCategoryImage}" alt="${item.priceTypeName}" class="img-fluid"></div>
                <div class="col-3">${item.priceTypeName}</div>
                <div class="col-2">${item.quantity}</div>
                <div class="col-2">${item.total}</div>
            </div>
        `;

        totalAmount += item.total;

        checkoutItemsContainer.appendChild(checkoutItemDiv);
    });

    let totalDiv = document.createElement('div');
    totalDiv.className = 'row mt-3';
    totalDiv.innerHTML = `<div class="col-md-12 text-right"><strong>Total Amount: </strong><span id="checkoutTotalAmount">${totalAmount}</span></div>`;
    checkoutItemsContainer.appendChild(totalDiv);

    // Clear amount received and change to return fields
    document.getElementById('amountReceived').value = '';
    document.getElementById('changeToReturn').value = '';

    // Show the checkout modal
    $('#checkoutModal').modal('show');

    // Set default payment method to cash and update fields
    document.getElementById('paymentMethod').value = 'cash';
    togglePaymentFields();
}

function calculateChange() {
    let totalAmount = parseFloat(document.getElementById('checkoutTotalAmount').innerText);
    let amountReceived = parseFloat(document.getElementById('amountReceived').value);
    let changeToReturn = amountReceived - totalAmount;
    document.getElementById('changeToReturn').value = changeToReturn.toFixed(2);
}

function confirmOrder() {
    let customerName = document.getElementById('customerName').value;
    let customerContact = document.getElementById('customerContact').value;
    let paymentMethod = document.getElementById('paymentMethod').value;
    let amountReceived = document.getElementById('amountReceived').value;

    if (!customerName) {
        alert('Please enter customer name.');
        return;
    }

    if (!customerContact) {
        alert('Please enter customer Contact.');
        return;
    }

    if (paymentMethod === 'cash' && !amountReceived) {
        alert('Please enter amount received.');
        return;
    }

    // Implement order confirmation logic here
    alert(`Order confirmed for ${customerName} with payment method ${paymentMethod}.`);

    // Clear the cart
    cart = [];
    localStorage.removeItem('cart');
    displayCartItems();
    updateCartTotal();

    // Hide the checkout modal
    $('#checkoutModal').modal('hide');

    // Show Toastr notification
    toastr.success('Order placed successfully!', 'Success', {
        timeOut: 2000,
        positionClass: 'toast-top-right',
        progressBar: true
    });

    // Remove held order button if exists
    heldOrders = heldOrders.filter(order => order.customerName !== customerName);
    displayHeldOrdersButtons();
}
function togglePaymentFields() {
    let paymentMethodElement = document.getElementById('paymentMethod');
    let paymentFields = document.getElementById('paymentFields');
    let amountReceivedField = document.getElementById('amountReceivedField');
    let changeToReturnField = document.getElementById('changeToReturnField');

    // Check if all required elements are found
    if (!paymentMethodElement || !paymentFields || !amountReceivedField || !changeToReturnField) {
        console.error('Required elements not found in the DOM');
        return;
    }

    let paymentMethod = paymentMethodElement.value;

    paymentFields.innerHTML = '';
    amountReceivedField.style.display = 'none';
    changeToReturnField.style.display = 'none';

    if (paymentMethod === 'cash') {
        amountReceivedField.style.display = 'block';
        changeToReturnField.style.display = 'block';
    } else if (paymentMethod === 'card') {
        paymentFields.innerHTML = `
            <div class="form-group">
                <label for="bankName">Bank Name</label>
                <select class="form-control" id="bankName">
                    <option value="hbl">HBL</option>
                    <option value="meezan">Meezan Bank</option>
                </select>
            </div>
        `;
    } else if (paymentMethod === 'online') {
        paymentFields.innerHTML = `
            <div class="form-group">
                <label for="onlinePaymentMethod">Online Payment Method</label>
                <select class="form-control" id="onlinePaymentMethod">
                    <option value="easypaisa">EasyPaisa</option>
                    <option value="jazzcash">JazzCash</option>
                </select>
            </div>
        `;
    }
}
let acknowledgedAlerts = JSON.parse(localStorage.getItem('acknowledgedAlerts')) || {}; // Load acknowledged alerts from localStorage

function holdOrder() {
    let customerName = document.getElementById('customerName').value;
    let customerContact = document.getElementById('customerContact').value;

    if (!customerName) {
        alert('Please enter customer name.');
        return;
    }

    if (!customerContact) {
        alert('Please enter customer contact.');
        return;
    }

    if (heldOrders.length >= 2) {
        alert('Maximum 2 orders can be held at a time.');
        return;
    }

    // Use SweetAlert to select the hold duration type
    Swal.fire({
        title: 'Select hold duration',
        input: 'select',
        inputOptions: {
            'minutes': 'Minutes',
            'hours': 'Hours',
            'days': 'Days'
        },
        inputPlaceholder: 'Select duration type',
        showCancelButton: true
    }).then((result) => {
        if (result.isConfirmed && result.value) {
            let durationType = result.value;

            // Prompt to enter the value for selected duration type
            Swal.fire({
                title: `Enter hold duration in ${durationType}:`,
                input: 'number',
                inputPlaceholder: `Enter number of ${durationType}`,
                showCancelButton: true
            }).then((durationResult) => {
                if (durationResult.isConfirmed && durationResult.value) {
                    let durationValue = parseInt(durationResult.value);

                    // Convert the duration to milliseconds based on the type
                    let holdDuration;
                    switch (durationType) {
                        case 'minutes':
                            holdDuration = durationValue * 60000; // Convert minutes to milliseconds
                            break;
                        case 'hours':
                            holdDuration = durationValue * 3600000; // Convert hours to milliseconds
                            break;
                        case 'days':
                            holdDuration = durationValue * 86400000; // Convert days to milliseconds
                            break;
                    }

                    let order = {
                        customerName: customerName,
                        customerContact: customerContact,
                        items: cart.slice(),
                        totalAmount: parseFloat(document.getElementById('checkoutTotalAmount').innerText),
                        holdDuration: holdDuration, // Store the duration
                        timestamp: new Date().getTime()
                    };

                    heldOrders.push(order);
                    localStorage.setItem('heldOrders', JSON.stringify(heldOrders)); // Store held orders in localStorage
                    displayHeldOrdersButtons();

                    // Clear the cart and hide the modal
                    cart = [];
                    localStorage.removeItem('cart');
                    displayCartItems();
                    updateCartTotal();
                    $('#checkoutModal').modal('hide');

                    Swal.fire({
                        icon: 'info',
                        title: 'Info',
                        text: `Order held for ${durationValue} ${durationType}.`,
                        timer: 2000,
                        position: 'top-right',
                        showConfirmButton: false,
                        toast: true
                    });

                    // Redirect to Home/Category page
                    window.location.href = '/Home/Category';
                }
            });
        }
    });
}

function displayHeldOrdersButtons() {
    let heldOrdersButtonsContainer = document.getElementById('heldOrdersButtonsContainer');
    heldOrdersButtonsContainer.innerHTML = '';

    if (heldOrders.length === 0) {
        let message = document.createElement('p');
        message.innerText = 'No held orders currently.';
        heldOrdersButtonsContainer.appendChild(message);
        return;
    }

    heldOrders.forEach((order, index) => {
        let button = document.createElement('button');
        button.className = 'btn btn-primary1 m-2';

        // Calculate remaining time
        let currentTime = new Date().getTime();
        let elapsedTime = currentTime - order.timestamp;
        let remainingTime = order.holdDuration - elapsedTime;

        // Display customer name and remaining time
        button.innerText = `${order.customerName} - ${formatTime(remainingTime)}`;

        // Update button text every second with a countdown timer
        let interval = setInterval(() => {
            elapsedTime = new Date().getTime() - order.timestamp;
            remainingTime = order.holdDuration - elapsedTime;
            button.innerText = `${order.customerName} - ${formatTime(remainingTime)}`;

            // Check for alert conditions
            handleAlerts(order, remainingTime);

            if (remainingTime <= 0) {
                clearInterval(interval);
                heldOrders = heldOrders.filter(o => o !== order);
                localStorage.setItem('heldOrders', JSON.stringify(heldOrders)); // Update localStorage
                displayHeldOrdersButtons();
            }
        }, 1000);

        button.onclick = () => {
            clearInterval(interval);
            displayHeldOrder(index);
        };

        heldOrdersButtonsContainer.appendChild(button);
    });
}

function formatTime(milliseconds) {
    let totalSeconds = Math.floor(milliseconds / 1000);
    let days = Math.floor(totalSeconds / (3600 * 24));
    let hours = Math.floor((totalSeconds % (3600 * 24)) / 3600);
    let minutes = Math.floor((totalSeconds % 3600) / 60);
    let seconds = totalSeconds % 60;

    // Create formatted string
    let timeString = '';
    if (days > 0) timeString += `${days}d `;
    if (hours > 0 || days > 0) timeString += `${hours}h `;
    if (minutes > 0 || hours > 0 || days > 0) timeString += `${minutes}m `;
    timeString += `${seconds}s`;

    return timeString;
}

function handleAlerts(order, remainingTime) {
    // Check for last 15 minutes
    if (remainingTime <= 900000 && !acknowledgedAlerts[order.customerName + '15']) {
        showRedAlert(order.customerName);
    }
}

function showRedAlert(customerName) {
    if (acknowledgedAlerts[customerName + '15']) return; // Don't show if already acknowledged

    Swal.fire({
        title: `<span style="color: red; font-weight: bold;">Call ${customerName}, only 15 minutes left!</span>`,
        html: '<button id="okCallingButton" class="btn btn-danger">Ok Calling</button>',
        showConfirmButton: false,
        background: '#fff',
        didOpen: () => {
            document.getElementById('okCallingButton').onclick = () => {
                acknowledgedAlerts[customerName + '15'] = true;
                localStorage.setItem('acknowledgedAlerts', JSON.stringify(acknowledgedAlerts)); // Save to localStorage
                Swal.close();
            };
        }
    });
}

function displayHeldOrder(index) {
    let order = heldOrders[index];
    cart = order.items.slice();
    displayCartItems();
    updateCartTotal();
    checkout();
    document.getElementById('customerName').value = order.customerName;
    document.getElementById('customerContact').value = order.customerContact;

    // Remove the held order from the list
    heldOrders.splice(index, 1);
    localStorage.setItem('heldOrders', JSON.stringify(heldOrders));
    displayHeldOrdersButtons();
}

// Load held orders from localStorage when the page loads
window.onload = function () {
    heldOrders = JSON.parse(localStorage.getItem('heldOrders')) || [];
    acknowledgedAlerts = JSON.parse(localStorage.getItem('acknowledgedAlerts')) || {}; // Load acknowledged alerts
    displayHeldOrdersButtons();
};



function confirmCancelOrder() {
    Swal.fire({
        title: 'Are you sure?',
        text: 'Do you really want to cancel the order?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, cancel it!'
    }).then((result) => {
        if (result.isConfirmed) {
            cancelOrder();
        }
    });
}

function cancelOrder() {
    // Clear the cart
    cart = [];
    localStorage.removeItem('cart');
    displayCartItems();
    updateCartTotal();

    // Hide the checkout modal
    $('#checkoutModal').modal('hide');

    // Show Toastr notification
    toastr.warning('Order canceled.', 'Warning', {
        timeOut: 2000,
        positionClass: 'toast-top-right',
        progressBar: true,
        toastClass: 'toast-matte-red'
    });
}
// Function to generate a serial number
function generateSerialNumber() {
    let lastSerialNumber = localStorage.getItem('lastSerialNumber') || '#GTS-0000000';

    let numericPart = parseInt(lastSerialNumber.split('-')[1]);

    numericPart++;

    let newSerialNumber = `#GTS-${numericPart.toString().padStart(7, '0')}`;

    localStorage.setItem('lastSerialNumber', newSerialNumber);

    return newSerialNumber;
}
document.getElementById('checkoutForm').addEventListener('submit', function (e) {
    e.preventDefault();
    confirmOrder();
});

function validateCustomerName() {
    let customerNameElement = document.getElementById('customerName');
    let customerNameErrorElement = document.getElementById('customerNameError');

    if (!customerNameElement.value.trim()) {
        customerNameElement.style.borderColor = 'red';
        customerNameErrorElement.style.display = 'block';
        return false;
    } else {
        customerNameElement.style.borderColor = 'green';
        customerNameErrorElement.style.display = 'none';
        return true;
    }
}

function validateCustomerContact() {
    let customerContactElement = document.getElementById('customerContact');
    let contactErrorElement = document.getElementById('contactError');

    if (!customerContactElement.value.trim()) {
        customerContactElement.style.borderColor = 'red';
        contactErrorElement.style.display = 'block';
        contactErrorElement.innerText = 'Customer Contact Required';
        return false;
    } else if (customerContactElement.value.length !== 11) {
        customerContactElement.style.borderColor = 'red';
        contactErrorElement.style.display = 'block';
        contactErrorElement.innerText = 'Customer contact number must be exactly 11 digits.';
        return false;
    } else {
        customerContactElement.style.borderColor = 'green';
        contactErrorElement.style.display = 'none';
        return true;
    }
}

function validateAmountReceived() {
    let amountReceivedElement = document.getElementById('amountReceived');
    let amountReceivedErrorElement = document.getElementById('amountReceivedError');

    if (!amountReceivedElement.value.trim()) {
        amountReceivedElement.style.borderColor = 'red';
        amountReceivedErrorElement.style.display = 'block';
        amountReceivedErrorElement.innerText = 'Amount Received Required';
        return false;
    } else if (parseFloat(amountReceivedElement.value) <= 0) {
        amountReceivedElement.style.borderColor = 'red';
        amountReceivedErrorElement.style.display = 'block';
        amountReceivedErrorElement.innerText = 'Amount received must be greater than zero.';
        return false;
    } else {
        amountReceivedElement.style.borderColor = 'green';
        amountReceivedErrorElement.style.display = 'none';
        return true;
    }
}


function confirmOrder() {
    let customerNameElement = document.getElementById('customerName');
    let customerContactElement = document.getElementById('customerContact');
    let paymentMethodElement = document.getElementById('paymentMethod');
    let amountReceivedElement = document.getElementById('amountReceived');
    let checkoutTotalAmountElement = document.getElementById('checkoutTotalAmount');
    let changeToReturnElement = document.getElementById('changeToReturn');
    let contactErrorElement = document.getElementById('contactError');

    if (!customerNameElement ||
        !customerContactElement ||
        !paymentMethodElement ||
        !amountReceivedElement ||
        !checkoutTotalAmountElement ||
        !changeToReturnElement) {
        alert('Required elements not found in the DOM.');
        return;
    }

    if (!validateCustomerName() || !validateCustomerContact() || !validateAmountReceived()) {
        return;
    }

    let customerName = customerNameElement.value;
    let customerContact = customerContactElement.value;
    let paymentMethod = paymentMethodElement.value;
    let amountReceived = amountReceivedElement.value;
    let checkoutTotalAmount = parseFloat(checkoutTotalAmountElement.innerText).toFixed(2);
    let changeToReturn = changeToReturnElement.value;


    if (paymentMethod === 'cash' && !amountReceived) {
        alert('Please enter amount received.');
        return;
    }


    // Generate the serial number for the receipt
    let serialNumber = generateSerialNumber();

    // Generate QR code for the website
    let qrCodeUrl = 'https://www.gtechnosoft.com';
    let qrCodeImg = `https://chart.googleapis.com/chart?cht=qr&chl=${encodeURIComponent(qrCodeUrl)}&chs=180x180&choe=UTF-8&chld=L|2`;

    // Create a print-friendly receipt

    // Create a print-friendly receipt with further enhanced styling
    let printContents = `
    <div class="receipt" style="padding: 10px; text-align: left; font-size: 16px; width: 2.75in; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;">
        <div style="border-top: 2px solid black; margin: 10px 0;"></div>
        <h2 style="margin: 0; text-align: center; color: #000; font-size: 20px;">Ice Cream Parlour</h2>
        <h3 style="margin: 5px 0; text-align: center; color: #000; font-size: 18px;">Receipt</h3>
        <div style="border-bottom: 2px solid black; margin: 10px 0;"></div>
        <p style="color: #000; font-size: 16px;"><strong>Serial Number:</strong> ${serialNumber}</p>
        <p style="color: #000; font-size: 16px;"><strong>Customer Name:</strong> ${customerName}</p>
        <p style="color: #000; font-size: 16px;"><strong>Customer Contact:</strong> ${customerContact}</p>
        <p style="color: #000; font-size: 16px;"><strong>Payment Method:</strong> ${paymentMethod}</p>
        <div style="border-bottom: 1px dashed #000; margin: 10px 0;"></div>
        <table style="width: 100%; border-collapse: collapse; font-size: 14px;">
            <thead>
                <tr>
                    <th style="text-align: left; border-bottom: 1px solid black; padding: 5px; color: #000; font-size: 14px;">#</th>
                    <th style="text-align: left; border-bottom: 1px solid black; padding: 5px; color: #000; font-size: 14px;">Item</th>
                    <th style="text-align: center; border-bottom: 1px solid black; padding: 5px; color: #000; font-size: 14px;">Qty</th>
                    <th style="text-align: right; border-bottom: 1px solid black; padding: 5px; color: #000; font-size: 14px;">Total</th>
                </tr>
            </thead>
            <tbody>
                ${cart.map((item, index) => `
                    <tr>
                        <td style="padding: 5px; color: #000; font-size: 14px;">${index + 1}</td>
                        <td style="padding: 5px; color: #000; font-size: 14px;">${item.priceTypeName}</td>
                        <td style="text-align: center; padding: 5px; color: #000; font-size: 14px;">${item.quantity}</td>
                        <td style="text-align: right; padding: 5px; color: #000; font-size: 14px;">${item.total.toFixed(2)}</td>
                    </tr>`).join('')}
            </tbody>
        </table>
        <div style="border-top: 1px dashed #000; margin: 10px 0;"></div>
        <p style="text-align: right; color: #000; font-size: 16px;"><strong>Total Amount:</strong> ${checkoutTotalAmount}</p>
        <p style="text-align: right; color: #000; font-size: 16px;"><strong>Amount Received:</strong> ${amountReceived}.00</p>
        <p style="text-align: right; color: #000; font-size: 16px;"><strong>Change to Return:</strong> ${changeToReturn}</p>
        <div style="border-bottom: 1px dashed #000; margin: 10px 0;"></div>
        <p style="text-align: center; font-style: italic; color: #000; font-size: 16px;"><strong>Thank you for your purchase!</strong></p>
        <p style="text-align: center; margin-top: 20px; font-size: 12px; color: #000;"><strong>Powered By:</strong> Global Techno Soft</p>
        <p style="text-align: center; font-size: 12px; color: #000;">Contact: 03063888546</p>
    </div>
`.replace(/\n\s*/g, '');

    // Show SweetAlert before printing
    Swal.fire({
        title: 'Print receipt?',
        text: 'Do you want to print the receipt?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonText: 'Yes',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed) {
            // Create an iframe for printing
            let printIframe = document.createElement('iframe');
            printIframe.style.position = 'absolute';
            printIframe.style.width = '0px';
            printIframe.style.height = '0px';
            document.body.appendChild(printIframe);

            let printDocument = printIframe.contentWindow.document;
            printDocument.open();
            printDocument.write(`
                <html>
                <head>
                    <style>
                        body {
                            margin: 0;
                            padding: 0;
                        }
                        ${document.querySelector('style').innerText}
                    </style>
                </head>
                <body>
                    ${printContents}
                </body>
                </html>
            `);
            printDocument.close();

            printIframe.contentWindow.focus();
            printIframe.contentWindow.print();

            // Remove the iframe after printing
            document.body.removeChild(printIframe);

            // Prepare order data for the API call
            let orderData = new FormData();
            orderData.append('customerName', customerName);
            orderData.append('customerContact', customerContact);
            orderData.append('paymentMethod', paymentMethod);
            orderData.append('amountReceived', amountReceived);
            orderData.append('totalAmount', checkoutTotalAmount);
            orderData.append('changeToReturn', changeToReturn);
            orderData.append('orderDate', new Date().toISOString());
            orderData.append('__RequestVerificationToken', $('input[name="__RequestVerificationToken"]').val());

            cart.forEach((item, index) => {
                orderData.append(`orderItems[${index}].priceTypeName`, item.priceTypeName);
                orderData.append(`orderItems[${index}].quantity`, item.quantity);
                orderData.append(`orderItems[${index}].total`, item.total);
            });

            // Send order data to the server
            ajaxWebPost(getWebAPIUrl() + 'webapi/Order/AddOrder', orderData, orderSuccess, orderError);
            cart = [];
            localStorage.removeItem('cart');
            window.location.href = '/Home/Category';
        } else {
            // If not confirmed, do nothing
            Swal.fire({
                title: 'Order not confirmed',
                text: 'You can add more items or modify the order.',
                icon: 'info',
                confirmButtonText: 'OK'
            });
        }
    });
}
document.getElementById('customerName').addEventListener('input', validateCustomerName);
function generateSerialNumber() {
    return 'SN' + Date.now();
}

// Define the success callback function
function orderSuccess(response) {
    if (response.success) {
        console.log('Order saved successfully:', response);

        cart = [];
        localStorage.removeItem('cart');
        displayCartItems();
        updateCartTotal();

        $('#checkoutModal').modal('hide');

        toastr.success('Order placed successfully!', 'Success', {
            timeOut: 2000,
            positionClass: 'toast-top-right',
            progressBar: true
        });

        heldOrders = heldOrders.filter(order => order.customerName !== customerName);
        displayHeldOrdersButtons();
    } else {
        toastr.error(response.message || 'Failed to save the order. Please try again.', 'Error', {
            timeOut: 2000,
            positionClass: 'toast-top-right',
            progressBar: true
        });
    }
}

function orderError(jqXHR, textStatus, errorThrown) {
    console.error('Error adding order:', textStatus, errorThrown);
    toastr.error('Failed to save the order. Please try again.', 'Error', {
        timeOut: 2000,
        positionClass: 'toast-top-right',
        progressBar: true
    });
}
function generateSerialNumber() {
    return 'SN' + Date.now();
}

function clearCart() {
    cart = [];
    document.getElementById('checkoutItemsContainer').innerHTML = '';
    document.getElementById('checkoutTotalAmount').innerText = '0.00';
}

function togglePaymentFields() {
    let paymentMethod = document.getElementById('paymentMethod').value;
    let paymentFieldsContainer = document.getElementById('paymentFields');

    paymentFieldsContainer.innerHTML = '';

    if (paymentMethod === 'card') {
        paymentFieldsContainer.innerHTML = `
            <div class="row mb-3">
                <div class="col-12">
                    <label for="cardNumber" class="form-label">Card Number</label>
                    <input type="text" class="form-control" id="cardNumber" name="cardNumber" placeholder="Enter card number">
                </div>
            </div>
        `;
    } else if (paymentMethod === 'online') {
        paymentFieldsContainer.innerHTML = `
            <div class="row mb-3">
                <div class="col-12">
                    <label for="transactionId" class="form-label">Transaction ID</label>
                    <input type="text" class="form-control" id="transactionId" name="transactionId" placeholder="Enter transaction ID">
                </div>
            </div>
        `;
    }
}
function calculateChange() {
    let amountReceived = parseFloat(document.getElementById('amountReceived').value) || 0;
    let totalAmount = parseFloat(document.getElementById('checkoutTotalAmount').innerText);
    let discount = parseFloat(document.getElementById('discountAmount').value) || 0;

    // Calculate the final amount after discount
    let finalAmount = totalAmount - discount;

    // Calculate the change to return
    let changeToReturn = amountReceived - finalAmount;

    // Set the change to return value
    document.getElementById('changeToReturn').value = changeToReturn < 0 ? '' : changeToReturn.toFixed(2);

    // Toggle confirm button
    document.getElementById('confirmOrderButton').disabled = amountReceived < finalAmount;
}



document.addEventListener('DOMContentLoaded', function () {
    var checkoutModal = document.getElementById('checkoutModal');
    checkoutModal.addEventListener('shown.bs.modal', function () {
        document.getElementById('amountReceived').focus();
    });
});
