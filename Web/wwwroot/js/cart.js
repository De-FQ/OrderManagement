document.getElementById('showCartButton').addEventListener('click', function () {
    let cartMenu = document.getElementById('cartMenu');
    if (cartMenu.style.right === '0px') {
        displayCartItems();
    } else {
        displayCartItems();
        cartMenu.style.right = '0';
    }
});

document.getElementById('closeCartButton').addEventListener('click', function () {
    document.getElementById('cartMenu').style.right = '-600px';
});

function updateCartItemCount() {
    let itemCount = cart.reduce((count, item) => count + item.quantity, 0);
    document.getElementById('cartItemCount').innerText = itemCount;
}


function addToCart() {
    let priceTypeCategoriesContainer = document.getElementById('priceTypeCategoriesContainer');
    let priceTypeItems = priceTypeCategoriesContainer.querySelectorAll('.price-type-item');
    let hasItemsToAdd = false;

    priceTypeItems.forEach(item => {
        let priceTypeId = item.querySelector('.quantity-controls span').id.split('-')[1];
        let productPriceId = item.querySelector('.quantity-controls span').dataset.productPriceId;
        let quantity = parseInt(item.querySelector('.quantity-controls span').innerText);
        let total = parseFloat(item.querySelector('.quantity-controls + div span').innerText);

        if (quantity > 0) {
            hasItemsToAdd = true;
            let priceTypeName = item.querySelector('h5').innerText;
            let childCategoryImage = document.getElementById('childCategoryImage').src;

            let cartItemIndex = cart.findIndex(item => item.priceTypeId == priceTypeId);
            if (cartItemIndex !== -1) {
                cart[cartItemIndex].quantity += quantity;
                cart[cartItemIndex].total += total;
            } else {
                cart.push({
                    priceTypeId: priceTypeId,
                    productPriceId: productPriceId,
                    priceTypeName: priceTypeName,
                    childCategoryImage: childCategoryImage,
                    quantity: quantity,
                    total: total
                });
            }

            // Update the cart menu background with the latest added item's image
            let cartMenu = document.getElementById('cartMenu');
            cartMenu.style.backgroundImage = `url(${childCategoryImage})`;
            cartMenu.style.backgroundSize = 'cover';
            cartMenu.style.backgroundPosition = 'center';

            item.querySelector('.quantity-controls span').innerText = 0;
            item.querySelector('.quantity-controls + div span').innerText = 0;
        }
    });

    if (hasItemsToAdd) {
        // Save cart to local storage
        localStorage.setItem('cart', JSON.stringify(cart));

        // Show Toastr notification
        toastr.success('Items added to cart!', 'Success', {
            timeOut: 2000,
            positionClass: 'toast-top-right',
            progressBar: true
        });

        // Update the cart display
        displayCartItems();
        updateCartItemCount(); // Update item count
    } else {
        // Show Toastr danger alert
        toastr.error('No items selected', 'Error', {
            timeOut: 2000,
            positionClass: 'toast-top-right',
            progressBar: true
        });
    }

    console.log('Cart:', cart); // Debugging: Log the cart contents
}

document.addEventListener('DOMContentLoaded', function () {
    // Load cart from local storage
    let savedCart = localStorage.getItem('cart');
    if (savedCart) {
        cart = JSON.parse(savedCart);
        displayCartItems();
        updateCartItemCount(); // Update item count
    }

    const urlParams = new URLSearchParams(window.location.search);
    const subCategoryId = urlParams.get('subCategoryId');
    if (subCategoryId) {
        fetchChildCategories(subCategoryId);
    }
});
            //<span class="cart-column">Sno</span>

            //<span class="cart-column">${index + 1}</span>
function displayCartItems() {
    let cartItemsContainer = document.getElementById('cartItemsContainer');
    cartItemsContainer.innerHTML = '';

    let totalAmount = 0;

    cartItemsContainer.innerHTML = `
        <div class="cart-header-row">
            <span class="cart-column" style="margin-right: 20px;">Image</span>
            <span class="cart-column product-column">Product</span>
            <span class="cart-column" style="margin-left: 15px;">Quantity</span>
            <span class="cart-column">Amount</span>
            <span class="cart-column">Action</span>
        </div>
    `;

    cart.forEach((item, index) => {
        let cartItemDiv = document.createElement('div');
        cartItemDiv.className = 'cart-item-row';

        cartItemDiv.innerHTML = `
            <img class="cart-column" src="${item.childCategoryImage}" alt="${item.priceTypeName}" style="margin-right: 10px;">
            <span class="cart-column product-column">${item.priceTypeName}</span>
            <div class="quantity-controls cart-column">
                <button class="btn-minus" onclick="decreaseCartQuantity(${item.priceTypeId}, ${item.total / item.quantity})">-</button>
                <span id="cart-quantity-${item.priceTypeId}" data-product-price-id="${item.productPriceId}">${item.quantity}</span> <!-- Add data attribute -->
                <button class="btn-plus" onclick="increaseCartQuantity(${item.priceTypeId}, ${item.total / item.quantity})">+</button>
            </div>
            <span class="cart-column" id="cart-total-${item.priceTypeId}">${item.total}</span>
            <button class="remove-button" onclick="removeCartItem(${item.priceTypeId})">
                <i class="fas fa-trash-alt"></i>
            </button>
        `;

        totalAmount += item.total;

        cartItemsContainer.appendChild(cartItemDiv);
    });

    let totalDiv = document.createElement('div');
    totalDiv.className = 'cart-total-row';
    totalDiv.innerHTML = `<span class="cart-column">Total Amount: </span><span class="cart-column" id="cartTotalAmount">${totalAmount}</span>`;
    cartItemsContainer.appendChild(totalDiv);
}


function increaseCartQuantity(priceTypeId, price) {
    let quantityElement = document.getElementById('cart-quantity-' + priceTypeId);
    let totalElement = document.getElementById('cart-total-' + priceTypeId);
    let quantity = parseInt(quantityElement.innerText);
    quantity++;
    quantityElement.innerText = quantity;
    totalElement.innerText = quantity * price;

    // Update cart array
    let cartItem = cart.find(item => item.priceTypeId == priceTypeId);
    cartItem.quantity = quantity;
    cartItem.total = quantity * price;

    // Save cart to local storage
    localStorage.setItem('cart', JSON.stringify(cart));

    updateCartTotal();
}

function decreaseCartQuantity(priceTypeId, price) {
    let quantityElement = document.getElementById('cart-quantity-' + priceTypeId);
    let totalElement = document.getElementById('cart-total-' + priceTypeId);
    let quantity = parseInt(quantityElement.innerText);
    if (quantity > 0) {
        quantity--;
        quantityElement.innerText = quantity;
        totalElement.innerText = quantity * price;

        // Update cart array
        let cartItem = cart.find(item => item.priceTypeId == priceTypeId);
        cartItem.quantity = quantity;
        cartItem.total = quantity * price;

        // Save cart to local storage
        localStorage.setItem('cart', JSON.stringify(cart));

        updateCartTotal();
    }
}

function updateCartTotal() {
    let totalAmount = cart.reduce((total, item) => total + item.total, 0);
    document.getElementById('cartTotalAmount').innerText = totalAmount;
}

function checkout() {
    // Implement checkout logic here
    alert('Proceeding to checkout.');
}

function removeCartItem(priceTypeId) {
    // Find the index of the item in the cart
    let cartItemIndex = cart.findIndex(item => item.priceTypeId == priceTypeId);

    // Remove the item from the cart array
    if (cartItemIndex !== -1) {
        cart.splice(cartItemIndex, 1);
    }
    // Save cart to local storage
    localStorage.setItem('cart', JSON.stringify(cart));
    // Update the cart display
    displayCartItems();
    updateCartItemCount(); // Update item count
    // Show Toastr notification
    toastr.success('Item removed from cart!', 'Success', {
        timeOut: 2000,
        positionClass: 'toast-top-right',
        progressBar: true
    });
    // Update the total amount
    updateCartTotal();
}

