function calculateMughliaPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleMughliaScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleMughliaScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleMughliaScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleMughliaScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalMughliaPrice').innerText = totalPrice;
    document.getElementById('totalMughliaQuantity').innerText = totalQuantity;
}

function addToMughliaOrder() {
    const checkoutItem = {
        itemName: 'Mughlia Cone Ice Cream',
        imageUrl: '/assets/img/cone/mughlia.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleMughliaScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleMughliaScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleMughliaScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleMughliaScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalMughliaQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalMughliaPrice').innerText, 10),
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
            $('#mughliaModal').modal('hide');
            // Reset the form values
            resetMughliaValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetMughliaValues() {
    document.getElementById('singleMughliaScoop').value = 0;
    document.getElementById('doubleMughliaScoop').value = 0;
    document.getElementById('singleMughliaScoopPrice').innerText = '00';
    document.getElementById('doubleMughliaScoopPrice').innerText = '00';
    document.getElementById('totalMughliaPrice').innerText = '00';
    document.getElementById('totalMughliaQuantity').innerText = '0';
}