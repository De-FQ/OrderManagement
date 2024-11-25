function calculateStrawberryPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleStrawberryScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleStrawberryScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleStrawberryScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleStrawberryScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalStrawberryPrice').innerText = totalPrice;
    document.getElementById('totalStrawberryQuantity').innerText = totalQuantity;
}

function addToStrawberryOrder() {
    const checkoutItem = {
        itemName: 'Strawberry Cup Ice Cream',
        imageUrl: '/assets/img/cup/strawberry.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleStrawberryScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleStrawberryScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleStrawberryScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleStrawberryScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalStrawberryQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalStrawberryPrice').innerText, 10),
    };

    fetch('https://localhost:7111/api/CheckoutItems', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(checkoutItem),
    })
        .then(response => {
            if (response.ok) {
                return response.json();
            }
            throw new Error('Network response was not ok.');
        })
        .then(data => {
            console.log('Success:', data);
            // Close the modal
            $('#strawberryModal').modal('hide');
            // Reset the form values
            resetStrawberryValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetStrawberryValues() {
    document.getElementById('singleStrawberryScoop').value = 0;
    document.getElementById('doubleStrawberryScoop').value = 0;
    document.getElementById('singleStrawberryScoopPrice').innerText = '00';
    document.getElementById('doubleStrawberryScoopPrice').innerText = '00';
    document.getElementById('totalStrawberryPrice').innerText = '00';
    document.getElementById('totalStrawberryQuantity').innerText = '0';
}