function calculateCoconutPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleCoconutScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleCoconutScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleCoconutScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleCoconutScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalCoconutPrice').innerText = totalPrice;
    document.getElementById('totalCoconutQuantity').innerText = totalQuantity;
}

function addToCoconutOrder() {
    const checkoutItem = {
        itemName: 'Coconut Cone Ice Cream',
        imageUrl: '/assets/img/cone/coconut.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleCoconutScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleCoconutScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleCoconutScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleCoconutScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalCoconutQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalCoconutPrice').innerText, 10),
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
            $('#coconutModal').modal('hide');
            // Reset the form values
            resetCoconutValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetCoconutValues() {
    document.getElementById('singleCoconutScoop').value = 0;
    document.getElementById('doubleCoconutScoop').value = 0;
    document.getElementById('singleCoconutScoopPrice').innerText = '00';
    document.getElementById('doubleCoconutScoopPrice').innerText = '00';
    document.getElementById('totalCoconutPrice').innerText = '00';
    document.getElementById('totalCoconutQuantity').innerText = '0';
}