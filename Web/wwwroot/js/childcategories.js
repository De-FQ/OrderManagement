let cart = []; // Initialize an empty array to hold cart items

async function fetchChildCategories(subCategoryId) {
    try {

        // Clean URL to remove parameters before making the API call
        history.pushState({}, '', window.location.pathname);
        let response = await fetch(getWebAPIUrl() + 'webapi/ChildCategories/GetActiveChildCategories?subCategoryId=' + subCategoryId);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let childCategories = await response.json();

        let container = document.getElementById('childcategoryContainer');
        container.innerHTML = ''; // Clear the container

        let row = document.createElement('div');
        row.className = 'row'; // Add row class for Bootstrap grid

        childCategories.forEach(childCategory => {
            let card = document.createElement('div');
            // Responsive column sizes: 6 columns on small devices, 4 on medium, and 2 on large devices
            card.className = 'col-6 col-md-4 col-lg-2 mb-3 ';

            card.innerHTML = `
                <div class="childcategory-card card h-100" onclick="openChildCategoryModal(${childCategory.id})">
                    <img src="${getChildCategoryImageUrl(childCategory.imageName)}" class="card-img-top childcategory-image" alt="${childCategory.name}">
                    <div class="card-body">
                        <h5 class="childcategory-name">${childCategory.name}</h5>
                    </div>
                </div>
            `;

            row.appendChild(card);
        });

        container.appendChild(row);
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

    // Clean URL to remove parameters
    history.pushState({}, '', window.location.pathname); // This will clear the query parameters

    try {
        let response = await fetch(getWebAPIUrl() + 'webapi/ChildCategoryDetails/GetChildCategoryDetails?childCategoryId=' + childCategoryId);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let childCategory = await response.json();
        console.log(childCategory);

        const imageUrl = getChildCategoryImageUrl(childCategory.imageName);
        const imgElement = document.getElementById('childCategoryImage');

        // Load the image with fallback
        imgElement.src = imageUrl;
        imgElement.onload = () => console.log('Image loaded successfully.');
        imgElement.onerror = () => {
            console.error('Image failed to load, using default image.');
            imgElement.src = getChildCategoryImageUrl('default.png'); // Fallback to default image
        };

        document.getElementById('childCategoryDetailsModalLabel').innerText = childCategory.name;
        document.getElementById('childCategoryName').innerText = childCategory.name;

        // Dynamically set the modal background
        let modalContent = document.querySelector('.custom-modal-content');
        modalContent.style.backgroundImage = `url(${imageUrl})`;
        modalContent.style.backgroundSize = 'cover';
        modalContent.style.backgroundPosition = 'center';
        modalContent.style.backgroundRepeat = 'no-repeat';

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

document.getElementById("hamburgerBtn").addEventListener("click", function () {
    var menu = document.getElementById("categoryMenu");
    menu.classList.toggle("menu-visible");
});

