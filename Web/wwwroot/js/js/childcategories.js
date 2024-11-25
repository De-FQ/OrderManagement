let cart = []; // Initialize an empty array to hold cart items

async function fetchChildCategories(subCategoryId) {
    try {
        let response = await fetch(getWebAPIUrl() + 'webapi/ChildCategories/GetActiveChildCategories?subCategoryId=' + subCategoryId);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let childCategories = await response.json();
        console.log(childCategories); // Debugging: Log the child categories fetched from API

        let container = document.getElementById('childcategoryContainer');
        container.innerHTML = ''; // Clear the container

        childCategories.forEach(childCategory => {
            let card = document.createElement('div');
            card.className = 'col-md-2';

            card.innerHTML = `
                <div class="childcategory-card card" onclick="openChildCategoryModal(${childCategory.id})">
                    <img src="${getChildCategoryImageUrl(childCategory.imageName)}" class="card-img-top childcategory-image" alt="${childCategory.name}">
                    <div class="card-body">
                        <h5 class="childcategory-name">${childCategory.name}</h5>
                    </div>
                </div>
            `;

                    
            container.appendChild(card);
        });
    } catch (error) {
        console.error('Error fetching child categories:', error);
    }
}

function getChildCategoryImageUrl(imageName) {
    if (!imageName) {
        imageName = "default.png"; // Ensure a default image is used if no image name is provided
    }
    return `https://localhost:7000/Contents/Images/ChildCategories/${imageName}`;
}

async function openChildCategoryModal(childCategoryId) {
    console.log("Modal opening for Child Category ID: " + childCategoryId);
    try {
        let response = await fetch(getWebAPIUrl() + 'webapi/ChildCategoryDetails/GetChildCategoryDetails?childCategoryId=' + childCategoryId);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let childCategory = await response.json();
        console.log(childCategory);

        document.getElementById('childCategoryDetailsModalLabel').innerText = childCategory.name;
        document.getElementById('childCategoryImage').src = getChildCategoryImageUrl(childCategory.imageName);
        document.getElementById('childCategoryName').innerText = childCategory.name;

        // Dynamically set the modal background
        let modalContent = document.querySelector('.custom-modal-content');
        modalContent.style.backgroundImage = `url(${getChildCategoryImageUrl(childCategory.imageName)})`;

        let priceTypeCategoriesContainer = document.getElementById('priceTypeCategoriesContainer');
        priceTypeCategoriesContainer.innerHTML = '';

        childCategory.priceTypeCategories.forEach(priceTypeCategory => {
            let priceTypeCategoryDiv = document.createElement('div');
            priceTypeCategoryDiv.innerHTML = `<h4>${priceTypeCategory.name}</h4>`;

            priceTypeCategory.priceTypes.forEach(priceType => {
                let priceTypeDiv = document.createElement('div');
                priceTypeDiv.className = 'price-type-item';

                let quantity = 0;
                let price = priceType.productPrices[0]?.price || 0;

                priceTypeDiv.innerHTML = `
                    <h5>${priceType.name}</h5>
                    <div class="quantity-controls">
                        <button class="btn btn-primary" onclick="decreaseQuantity(${priceType.id}, ${price})">-</button>
                        <span id="quantity-${priceType.id}">${quantity}</span>
                        <button class="btn btn-primary" onclick="increaseQuantity(${priceType.id}, ${price})">+</button>
                    </div>
                    <div>Total: <span id="total-${priceType.id}">0</span></div>
                `;

                priceTypeCategoryDiv.appendChild(priceTypeDiv);
            });

            priceTypeCategoriesContainer.appendChild(priceTypeCategoryDiv);
        });

        $('#childCategoryDetailsModal').modal('show');
    } catch (error) {
        console.error('Error fetching child category details:', error);
    }
}


function increaseQuantity(priceTypeId, price) {
    let quantityElement = document.getElementById('quantity-' + priceTypeId);
    let totalElement = document.getElementById('total-' + priceTypeId);
    let quantity = parseInt(quantityElement.innerText);
    quantity++;
    quantityElement.innerText = quantity;
    totalElement.innerText = quantity * price;
}

function decreaseQuantity(priceTypeId, price) {
    let quantityElement = document.getElementById('quantity-' + priceTypeId);
    let totalElement = document.getElementById('total-' + priceTypeId);
    let quantity = parseInt(quantityElement.innerText);
    if (quantity > 0) {
        quantity--;
        quantityElement.innerText = quantity;
        totalElement.innerText = quantity * price;
    }
}
