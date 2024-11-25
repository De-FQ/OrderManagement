function calculateOreoPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleOreoScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleOreoScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleOreoScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleOreoScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalOreoPrice').innerText = totalPrice;
    document.getElementById('totalOreoQuantity').innerText = totalQuantity;
}

function addToOreoOrder() {
    const checkoutItem = {
        itemName: 'Oreo Cup Ice Cream',
        imageUrl: '/assets/img/cup/oreo.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleOreoScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleOreoScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleOreoScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleOreoScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalOreoQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalOreoPrice').innerText, 10),
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
            $('#oreoModal').modal('hide');
            // Reset the form values
            resetOreoValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetOreoValues() {
    document.getElementById('singleOreoScoop').value = 0;
    document.getElementById('doubleOreoScoop').value = 0;
    document.getElementById('singleOreoScoopPrice').innerText = '00';
    document.getElementById('doubleOreoScoopPrice').innerText = '00';
    document.getElementById('totalOreoPrice').innerText = '00';
    document.getElementById('totalOreoQuantity').innerText = '0';
}