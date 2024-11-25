function calculateDarkchocoPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleDarkchocoScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleDarkchocoScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleDarkchocoScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleDarkchocoScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalDarkchocoPrice').innerText = totalPrice;
    document.getElementById('totalDarkchocoQuantity').innerText = totalQuantity;
}

function addToDarkchocoOrder() {
    const checkoutItem = {
        itemName: 'Dark Choco Cup Ice Cream',
        imageUrl: '/assets/img/cup/darkchoco.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleDarkchocoScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleDarkchocoScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleDarkchocoScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleDarkchocoScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalDarkchocoQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalDarkchocoPrice').innerText, 10),
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
            $('#darkchocoModal').modal('hide');
            // Reset the form values
            resetDarkchocoValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetDarkchocoValues() {
    document.getElementById('singleDarkchocoScoop').value = 0;
    document.getElementById('doubleDarkchocoScoop').value = 0;
    document.getElementById('singleDarkchocoScoopPrice').innerText = '00';
    document.getElementById('doubleDarkchocoScoopPrice').innerText = '00';
    document.getElementById('totalDarkchocoPrice').innerText = '00';
    document.getElementById('totalDarkchocoQuantity').innerText = '0';
}