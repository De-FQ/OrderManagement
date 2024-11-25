function calculateMangoPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleMangoScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleMangoScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleMangoScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleMangoScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalMangoPrice').innerText = totalPrice;
    document.getElementById('totalMangoQuantity').innerText = totalQuantity;
}

function addToMangoOrder() {
    const checkoutItem = {
        itemName: 'Mango Cup Ice Cream',
        imageUrl: '/assets/img/cup/mango.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleMangoScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleMangoScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleMangoScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleMangoScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalMangoQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalMangoPrice').innerText, 10),
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
            $('#mangoModal').modal('hide');
            // Reset the form values
            resetMangoValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetMangoValues() {
    document.getElementById('singleMangoScoop').value = 0;
    document.getElementById('doubleMangoScoop').value = 0;
    document.getElementById('singleMangoScoopPrice').innerText = '00';
    document.getElementById('doubleMangoScoopPrice').innerText = '00';
    document.getElementById('totalMangoPrice').innerText = '00';
    document.getElementById('totalMangoQuantity').innerText = '0';
}