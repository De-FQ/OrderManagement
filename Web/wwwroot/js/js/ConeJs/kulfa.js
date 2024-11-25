function calculateKulfaPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleKulfaScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleKulfaScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleKulfaScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleKulfaScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalKulfaPrice').innerText = totalPrice;
    document.getElementById('totalKulfaQuantity').innerText = totalQuantity;
}

function addToKulfaOrder() {
    const checkoutItem = {
        itemName: 'Kulfa Cone Ice Cream',
        imageUrl: '/assets/img/cone/kulfa.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleKulfaScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleKulfaScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleKulfaScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleKulfaScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalKulfaQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalKulfaPrice').innerText, 10),
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
            $('#kulfaModal').modal('hide');
            // Reset the form values
            resetKulfaValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetKulfaValues() {
    document.getElementById('singleKulfaScoop').value = 0;
    document.getElementById('doubleKulfaScoop').value = 0;
    document.getElementById('singleKulfaScoopPrice').innerText = '00';
    document.getElementById('doubleKulfaScoopPrice').innerText = '00';
    document.getElementById('totalKulfaPrice').innerText = '00';
    document.getElementById('totalKulfaQuantity').innerText = '0';
}