function calculateDairymilkPrice() {
    const singleScoopPrice = 200;
    const doubleScoopPrice = 300;

    const singleScoopCount = parseInt(document.getElementById('singleDairymilkScoop').value) || 0;
    const doubleScoopCount = parseInt(document.getElementById('doubleDairymilkScoop').value) || 0;

    const singleTotalPrice = singleScoopPrice * singleScoopCount;
    const doubleTotalPrice = doubleScoopPrice * doubleScoopCount;

    const totalPrice = singleTotalPrice + doubleTotalPrice;
    const totalQuantity = singleScoopCount + doubleScoopCount;

    document.getElementById('singleDairymilkScoopPrice').innerText = singleTotalPrice;
    document.getElementById('doubleDairymilkScoopPrice').innerText = doubleTotalPrice;
    document.getElementById('totalDairymilkPrice').innerText = totalPrice;
    document.getElementById('totalDairymilkQuantity').innerText = totalQuantity;
}

function addToDairymilkOrder() {
    const checkoutItem = {
        itemName: 'Dairy Milk Cone Ice Cream',
        imageUrl: '/assets/img/cone/dairymilk.png', // Provide the correct path to your image
        singleScoopPrice: parseInt(document.getElementById('singleDairymilkScoopPrice').innerText, 10),
        singleScoopQuantity: parseInt(document.getElementById('singleDairymilkScoop').value, 10),
        doubleScoopPrice: parseInt(document.getElementById('doubleDairymilkScoopPrice').innerText, 10),
        doubleScoopQuantity: parseInt(document.getElementById('doubleDairymilkScoop').value, 10),
        totalQuantity: parseInt(document.getElementById('totalDairymilkQuantity').innerText, 10),
        totalPrice: parseInt(document.getElementById('totalDairymilkPrice').innerText, 10),
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
            $('#dairymilkModal').modal('hide');
            // Reset the form values
            resetDairymilkValues();
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function resetDairymilkValues() {
    document.getElementById('singleDairymilkScoop').value = 0;
    document.getElementById('doubleDairymilkScoop').value = 0;
    document.getElementById('singleDairymilkScoopPrice').innerText = '00';
    document.getElementById('doubleDairymilkScoopPrice').innerText = '00';
    document.getElementById('totalDairymilkPrice').innerText = '00';
    document.getElementById('totalDairymilkQuantity').innerText = '0';
}