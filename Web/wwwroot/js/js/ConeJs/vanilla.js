
function calculateVanillaPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleVanillaScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleVanillaScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleVanillaScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleVanillaScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalVanillaPrice').innerText = totalPrice;
    document.getElementById('totalVanillaQuantity').innerText = totalQuantity;
}

function addToVanillaOrder() {
    const checkoutItem = {
        itemName: 'Vanilla Cone Ice Cream',
        imageUrl: '/assets/img/cone/vanilla.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleVanillaScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleVanillaScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleVanillaScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleVanillaScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalVanillaQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalVanillaPrice').innerText, 10),
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
            $('#vanillaModal').modal('hide');
            // Reset the form values
            resetVanillaValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetVanillaValues() {
    document.getElementById('singleVanillaScoop').value = 0;
    document.getElementById('doubleVanillaScoop').value = 0;
    document.getElementById('singleVanillaScoopPrice').innerText = '00';
    document.getElementById('doubleVanillaScoopPrice').innerText = '00';
    document.getElementById('totalVanillaPrice').innerText = '00';
    document.getElementById('totalVanillaQuantity').innerText = '0';
}