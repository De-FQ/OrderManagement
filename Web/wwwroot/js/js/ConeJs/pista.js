function calculatePistaPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singlePistaScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doublePistaScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singlePistaScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doublePistaScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalPistaPrice').innerText = totalPrice;
    document.getElementById('totalPistaQuantity').innerText = totalQuantity;
}

function addToPistaOrder() {
    const checkoutItem = {
        itemName: 'Pista Cone Ice Cream',
        imageUrl: '/assets/img/cone/pista.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singlePistaScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singlePistaScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doublePistaScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doublePistaScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalPistaQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalPistaPrice').innerText, 10),
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
            $('#pistaModal').modal('hide');
            // Reset the form values
            resetPistaValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetPistaValues() {
    document.getElementById('singlePistaScoop').value = 0;
    document.getElementById('doublePistaScoop').value = 0;
    document.getElementById('singlePistaScoopPrice').innerText = '00';
    document.getElementById('doublePistaScoopPrice').innerText = '00';
    document.getElementById('totalPistaPrice').innerText = '00';
    document.getElementById('totalPistaQuantity').innerText = '0';
}
