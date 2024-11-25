let heldOrders = JSON.parse(localStorage.getItem('heldOrders')) || [];
let acknowledgedAlerts = {}; // Track acknowledged alerts

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
    if (remainingTime <= 900000 && remainingTime > 600000 && !acknowledgedAlerts[order.customerName + '10']) {
        showBlinkingAlert(order.customerName, '10');
    } else if (remainingTime <= 600000 && remainingTime > 300000 && acknowledgedAlerts[order.customerName + '10'] && !acknowledgedAlerts[order.customerName + '5']) {
        showBlinkingAlert(order.customerName, '5');
    }
}

function showBlinkingAlert(customerName, stage) {
    Swal.fire({
        title: `<span style="color: red; font-weight: bold; animation: blink 1s infinite;">Call ${customerName}, time is running out!</span>`,
        html: '<button id="okCallingButton" class="btn btn-danger">Ok Calling</button>',
        showConfirmButton: false,
        background: '#fff',
        didOpen: () => {
            // CSS for blinking effect
            const style = document.createElement('style');
            style.innerHTML = `
                @keyframes blink {
                    0% { opacity: 1; }
                    50% { opacity: 0; }
                    100% { opacity: 1; }
                }
            `;
            document.head.appendChild(style);

            document.getElementById('okCallingButton').onclick = () => {
                acknowledgedAlerts[customerName + stage] = true;
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
    displayHeldOrdersButtons();
};
