function calculateBlueBerryPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleBlueBerryScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleBlueBerryScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleBlueBerryScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleBlueBerryScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalBlueBerryPrice').innerText = totalPrice;
    document.getElementById('totalBlueBerryQuantity').innerText = totalQuantity;
}
document.getElementById('addToBlueBerryOrderBtn').addEventListener('click', function () {
    const checkoutItem = {
        itemName: 'Blue Berry Cup Ice Cream',
        imageUrl: '/assets/img/cup/blueberry.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleBlueBerryScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleBlueBerryScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleBlueBerryScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleBlueBerryScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalBlueBerryQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalBlueBerryPrice').innerText, 10),
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
            $('#blueberryModal').modal('hide');
            // Reset the form values
            resetBlueBerryValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
});

function resetBlueBerryValues() {
    document.getElementById('singleBlueBerryScoop').value = 0;
    document.getElementById('doubleBlueBerryScoop').value = 0;
    document.getElementById('singleBlueBerryScoopPrice').innerText = '00';
    document.getElementById('doubleBlueBerryScoopPrice').innerText = '00';
    document.getElementById('totalBlueBerryPrice').innerText = '00';
    document.getElementById('totalBlueBerryQuantity').innerText = '00';
}