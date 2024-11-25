//let heldOrders = [];

//function checkout() {
//    let checkoutItemsContainer = document.getElementById('checkoutItemsContainer');
//    checkoutItemsContainer.innerHTML = '';

//    let totalAmount = 0;

//    cart.forEach((item, index) => {
//        let checkoutItemDiv = document.createElement('div');
//        checkoutItemDiv.className = 'checkout-item-row';

//        checkoutItemDiv.innerHTML = `
//            <div class="row mb-3">
//                <div class="col-1">${index + 1}</div>
//                <div class="col-2"><img src="${item.childCategoryImage}" alt="${item.priceTypeName}" class="img-fluid"></div>
//                <div class="col-3">${item.priceTypeName}</div>
//                <div class="col-2">${item.quantity}</div>
//                <div class="col-2">${item.total}</div>
//            </div>
//        `;

//        totalAmount += item.total;

//        checkoutItemsContainer.appendChild(checkoutItemDiv);
//    });

//    let totalDiv = document.createElement('div');
//    totalDiv.className = 'row mt-3';
//    totalDiv.innerHTML = `<div class="col-md-12 text-right"><strong>Total Amount: </strong><span id="checkoutTotalAmount">${totalAmount}</span></div>`;
//    checkoutItemsContainer.appendChild(totalDiv);

//    // Clear amount received and change to return fields
//    document.getElementById('amountReceived').value = '';
//    document.getElementById('changeToReturn').value = '';

//    // Show the checkout modal
//    $('#checkoutModal').modal('show');

//    // Set default payment method to cash and update fields
//    document.getElementById('paymentMethod').value = 'cash';
//    togglePaymentFields();
//}

//function calculateChange() {
//    let totalAmount = parseFloat(document.getElementById('checkoutTotalAmount').innerText);
//    let amountReceived = parseFloat(document.getElementById('amountReceived').value);
//    let changeToReturn = amountReceived - totalAmount;
//    document.getElementById('changeToReturn').value = changeToReturn.toFixed(2);
//}

//function confirmOrder() {
//    let customerName = document.getElementById('customerName').value;
//    let paymentMethod = document.getElementById('paymentMethod').value;
//    let amountReceived = document.getElementById('amountReceived').value;

//    if (!customerName) {
//        alert('Please enter customer name.');
//        return;
//    }

//    if (paymentMethod === 'cash' && !amountReceived) {
//        alert('Please enter amount received.');
//        return;
//    }

//    // Implement order confirmation logic here
//    alert(`Order confirmed for ${customerName} with payment method ${paymentMethod}.`);

//    // Clear the cart
//    cart = [];
//    localStorage.removeItem('cart');
//    displayCartItems();
//    updateCartTotal();

//    // Hide the checkout modal
//    $('#checkoutModal').modal('hide');

//    // Show Toastr notification
//    toastr.success('Order placed successfully!', 'Success', {
//        timeOut: 2000,
//        positionClass: 'toast-top-right',
//        progressBar: true
//    });

//    // Remove held order button if exists
//    heldOrders = heldOrders.filter(order => order.customerName !== customerName);
//    displayHeldOrdersButtons();
//}

//function togglePaymentFields() {
//    let paymentMethod = document.getElementById('paymentMethod').value;
//    let paymentFields = document.getElementById('paymentFields');
//    let amountReceivedField = document.getElementById('amountReceivedField');
//    let changeToReturnField = document.getElementById('changeToReturnField');

//    paymentFields.innerHTML = '';
//    amountReceivedField.style.display = 'none';
//    changeToReturnField.style.display = 'none';

//    if (paymentMethod === 'cash') {
//        amountReceivedField.style.display = 'block';
//        changeToReturnField.style.display = 'block';
//    } else if (paymentMethod === 'card') {
//        paymentFields.innerHTML = `
//            <div class="form-group">
//                <label for="bankName">Bank Name</label>
//                <select class="form-control" id="bankName">
//                    <option value="hbl">HBL</option>
//                    <option value="meezan">Meezan Bank</option>
//                </select>
//            </div>
//        `;
//    } else if (paymentMethod === 'online') {
//        paymentFields.innerHTML = `
//            <div class="form-group">
//                <label for="onlinePaymentMethod">Online Payment Method</label>
//                <select class="form-control" id="onlinePaymentMethod">
//                    <option value="easypaisa">EasyPaisa</option>
//                    <option value="jazzcash">JazzCash</option>
//                </select>
//            </div>
//        `;
//    }
//}

//function holdOrder() {
//    let customerName = document.getElementById('customerName').value;

//    if (!customerName) {
//        alert('Please enter customer name.');
//        return;
//    }

//    if (heldOrders.length >= 2) {
//        alert('Maximum 2 orders can be held at a time.');
//        return;
//    }

//    let order = {
//        customerName: customerName,
//        items: cart.slice(),
//        totalAmount: parseFloat(document.getElementById('checkoutTotalAmount').innerText),
//        timestamp: new Date().getTime()
//    };

//    heldOrders.push(order);
//    displayHeldOrdersButtons();

//    // Clear the cart and hide the modal
//    cart = [];
//    localStorage.removeItem('cart');
//    displayCartItems();
//    updateCartTotal();
//    $('#checkoutModal').modal('hide');

//    // Set a timer to remove the order after 30 minutes
//    setTimeout(() => {
//        heldOrders = heldOrders.filter(o => o !== order);
//        displayHeldOrdersButtons();
//    }, 1800000);

//    toastr.info('Order held for 30 minutes.', 'Info', {
//        timeOut: 2000,
//        positionClass: 'toast-top-right',
//        progressBar: true,
//        toastClass: 'toast-matte-yellow'
//    });
//}


//function displayHeldOrdersButtons() {
//    let heldOrdersButtonsContainer = document.getElementById('heldOrdersButtonsContainer');
//    heldOrdersButtonsContainer.innerHTML = '';

//    heldOrders.forEach((order, index) => {
//        let button = document.createElement('button');
//        button.className = 'btn btn-primary m-2';
//        button.innerText = order.customerName;
//        button.onclick = () => displayHeldOrder(index);
//        heldOrdersButtonsContainer.appendChild(button);
//    });
//}

//function displayHeldOrder(index) {
//    let order = heldOrders[index];
//    cart = order.items.slice();
//    displayCartItems();
//    updateCartTotal();
//    checkout();
//    document.getElementById('customerName').value = order.customerName;

//    // Remove the held order from the list
//    heldOrders.splice(index, 1);
//    displayHeldOrdersButtons();
//}

//function confirmCancelOrder() {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: 'Do you really want to cancel the order?',
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, cancel it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            cancelOrder();
//        }
//    });
//}

//function cancelOrder() {
//    // Clear the cart
//    cart = [];
//    localStorage.removeItem('cart');
//    displayCartItems();
//    updateCartTotal();

//    // Hide the checkout modal
//    $('#checkoutModal').modal('hide');

//    // Show Toastr notification
//    toastr.warning('Order canceled.', 'Warning', {
//        timeOut: 2000,
//        positionClass: 'toast-top-right',
//        progressBar: true,
//        toastClass: 'toast-matte-red'
//    });
//}
//// Function to generate a serial number
//function generateSerialNumber() {
//    // Fetch the last serial number from localStorage
//    let lastSerialNumber = localStorage.getItem('lastSerialNumber') || '#GTS-0000000';

//    // Extract the numeric part of the serial number
//    let numericPart = parseInt(lastSerialNumber.split('-')[1]);

//    // Increment the numeric part
//    numericPart++;

//    // Generate the new serial number
//    let newSerialNumber = `#GTS-${numericPart.toString().padStart(7, '0')}`;

//    // Store the new serial number in localStorage
//    localStorage.setItem('lastSerialNumber', newSerialNumber);

//    return newSerialNumber;
//}

//document.getElementById('checkoutForm').addEventListener('submit', function (e) {
//    e.preventDefault();
//    confirmOrder();
//});

//function confirmOrder() {
//    let customerName = document.getElementById('customerName').value;
//    let paymentMethod = document.getElementById('paymentMethod').value;
//    let amountReceived = document.getElementById('amountReceived').value;

//    if (!customerName) {
//        alert('Please enter customer name.');
//        return;
//    }

//    if (paymentMethod === 'cash' && !amountReceived) {
//        alert('Please enter amount received.');
//        return;
//    }

//    // Generate the serial number for the receipt
//    let serialNumber = generateSerialNumber();

//    // Generate QR code for the website
//    let qrCodeUrl = 'https://www.gtechnosoft.com';
//    let qrCodeImg = `https://chart.googleapis.com/chart?cht=qr&chl=${encodeURIComponent(qrCodeUrl)}&chs=180x180&choe=UTF-8&chld=L|2`;

//    // Create a print-friendly receipt
//    let printContents = `
//        <div class="receipt" style="padding: 0; text-align: left; font-size: 18px;">
//            <div style="border-top: 1px solid black; margin-top: 5px;"></div>
//            <h2 style="margin-bottom: 0;">Global Techno Order System</h2>
//            <h3 style="margin-top: 5px;">Receipt</h3>
//            <div style="border-bottom: 1px solid black; margin-bottom: 10px;"></div>
//            <p><strong>Serial Number:</strong> ${serialNumber}</p>
//            <p><strong>Customer:</strong> ${customerName}</p>
//            <p><strong>Payment Method:</strong> ${paymentMethod}</p>
//            <table style="width: 100%;">
//                <thead>
//                    <tr>
//                        <th style="text-align: left;">#</th>
//                        <th style="text-align: left;">Item</th>
//                        <th style="text-align: left;">Qty</th>
//                        <th style="text-align: left;">Total</th>
//                    </tr>
//                </thead>
//                <tbody>
//                    ${cart.map((item, index) => `
//                        <tr>
//                            <td>${index + 1}</td>
//                            <td>${item.priceTypeName}</td>
//                            <td>${item.quantity}</td>
//                            <td>${item.total.toFixed(2)}</td>
//                        </tr>`).join('')}
//                </tbody>
//            </table>
//            <p><strong>Total Amount:</strong> ${parseFloat(document.getElementById('checkoutTotalAmount').innerText).toFixed(2)}</p>
//            <p><strong>Amount Received:</strong> ${amountReceived}</p>
//            <p><strong>Change to Return:</strong> ${document.getElementById('changeToReturn').value}</p>
//            <p><strong>Thank you for your purchase!</strong></p>
//            <p style="margin-top: 20px;"><strong>Powered By:</strong></p>
//            <div style="border-top: 1px solid black; margin-top: 5px; margin-bottom: 5px;"></div>
//            <p>Global Techno Soft</p>
//            <div style="border-top: 1px solid black; margin-top: 5px; margin-bottom: 5px;"></div>
//            <p>Contact: 03063888546</p>
//            <p>Visit: <img src="${qrCodeImg}" alt="QR Code" style="width: 50px; height: 50px; vertical-align: middle;"/> <a href="${qrCodeUrl}" target="_blank">${qrCodeUrl}</a></p>
//        </div>
//    `;

//    // Show SweetAlert before printing
//    Swal.fire({
//        title: 'Print receipt?',
//        text: 'Do you want to print the receipt?',
//        icon: 'question',
//        showCancelButton: true,
//        confirmButtonText: 'Yes',
//        cancelButtonText: 'No'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            // Create an iframe for printing
//            let printIframe = document.createElement('iframe');
//            printIframe.style.position = 'absolute';
//            printIframe.style.width = '0px';
//            printIframe.style.height = '0px';
//            document.body.appendChild(printIframe);

//            let printDocument = printIframe.contentWindow.document;
//            printDocument.open();
//            printDocument.write(`
//                <html>
//                <head>
//                    <style>
//                        body {
//                            margin: 0;
//                            padding: 10px;
//                        }
//                        ${document.querySelector('style').innerText}
//                    </style>
//                </head>
//                <body>
//                    ${printContents}
//                </body>
//                </html>
//            `);
//            printDocument.close();

//            printIframe.contentWindow.focus();
//            printIframe.contentWindow.print();

//            // Remove the iframe after printing
//            document.body.removeChild(printIframe);

//            // Implement order confirmation logic here
//            // Prepare order data for the API call
//            let orderData = {
//                customerName: customerName,
//                paymentMethod: paymentMethod,
//                amountReceived: parseFloat(amountReceived),
//                totalAmount: parseFloat(document.getElementById('checkoutTotalAmount').innerText),
//                changeToReturn: parseFloat(document.getElementById('changeToReturn').value),
//                orderDate: new Date().toISOString(),
//                serialNumber: serialNumber
//            };

//            // Send order data to the server
//            $.ajax({
//                url: getWebAPIUrl() + 'api/Order/AddEdit', // Replace with your actual API endpoint
//                type: 'POST',
//                contentType: 'application/json',
//                data: JSON.stringify(orderData),
//                success: function (response) {
//                    console.log('Order saved successfully:', response);

//                    let orderId = response.orderId; // Assuming the response contains the orderId

//                    // Create order items data
//                    let orderItemsData = cart.map(item => ({
//                        orderId: orderId,
//                        priceTypeName: item.priceTypeName,
//                        quantity: item.quantity,
//                        total: item.total
//                    }));

//                    // Send order items data to the order items API
//                    $.ajax({
//                        url: getWebAPIUrl() + 'api/OrderItem/AddEdit', // Replace with your actual API endpoint
//                        type: 'POST',
//                        contentType: 'application/json',
//                        data: JSON.stringify(orderItemsData),
//                        success: function (response) {
//                            console.log('Order items saved successfully:', response);

//                            // Clear the cart
//                            cart = [];
//                            localStorage.removeItem('cart');
//                            displayCartItems();
//                            updateCartTotal();

//                            // Hide the checkout modal
//                            $('#checkoutModal').modal('hide');

//                            // Show Toastr notification
//                            toastr.success('Order placed successfully!', 'Success', {
//                                timeOut: 2000,
//                                positionClass: 'toast-top-right',
//                                progressBar: true
//                            });

//                            // Remove held order button if exists
//                            heldOrders = heldOrders.filter(order => order.customerName !== customerName);
//                            displayHeldOrdersButtons();
//                        },
//                        error: function (xhr, status, error) {
//                            console.error('Error saving order items:', error);
//                            toastr.error('Failed to save the order items. Please try again.', 'Error', {
//                                timeOut: 2000,
//                                positionClass: 'toast-top-right',
//                                progressBar: true
//                            });
//                        }
//                    });
//                },
//                error: function (xhr, status, error) {
//                    console.error('Error saving order:', error);
//                    toastr.error('Failed to save the order. Please try again.', 'Error', {
//                        timeOut: 2000,
//                        positionClass: 'toast-top-right',
//                        progressBar: true
//                    });
//                }
//            });
//        } else {
//            // If not confirmed, do nothing
//            Swal.fire({
//                title: 'Order not confirmed',
//                text: 'You can add more items or modify the order.',
//                icon: 'info',
//                confirmButtonText: 'OK'
//            });
//        }
//    });
//}

//function generateSerialNumber() {
//    return 'SN' + Date.now();
//}

//function clearCart() {
//    cart = [];
//    document.getElementById('checkoutItemsContainer').innerHTML = '';
//    document.getElementById('checkoutTotalAmount').innerText = '0.00';
//}

////function checkout() {
////    window.location.href = '/Order/PlaceOrder';
////}