function calculateChocolatecocoPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleChocolatecocoScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleChocolatecocoScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleChocolatecocoScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleChocolatecocoScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalChocolatecocoPrice').innerText = totalPrice;
    document.getElementById('totalChocolatecocoQuantity').innerText = totalQuantity;
}

function addToChocolatecocoOrder() {
    const checkoutItem = {
        itemName: 'Chocolatecoco Cone Ice Cream',
        imageUrl: '/assets/img/cone/chocolatecoco.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleChocolatecocoScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleChocolatecocoScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleChocolatecocoScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleChocolatecocoScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalChocolatecocoQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalChocolatecocoPrice').innerText, 10),
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
        $('#chocolatecocoModal').modal('hide');
        // Reset the form values
        resetChocolatecocoValues();
    })
    .catch((error) => {
        console.error('Error:', error);
    });
}
function resetChocolatecocoValues() {
    document.getElementById('singleChocolatecocoScoop').value = 0;
    document.getElementById('doubleChocolatecocoScoop').value = 0;
    document.getElementById('singleChocolatecocoScoopPrice').innerText = '00';
    document.getElementById('doubleChocolatecocoScoopPrice').innerText = '00';
    document.getElementById('totalChocolatecocoPrice').innerText = '00';
    document.getElementById('totalChocolatecocoQuantity').innerText = '00';
}
