async function fetchSubCategories(categoryId) {
    try {
        let response = await fetch(getWebAPIUrl() + 'webapi/SubCategories/GetActiveSubCategories?categoryId=' + categoryId);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let subCategories = await response.json();
        console.log(subCategories); // Debugging: Log the subcategories fetched from API

        let container = document.getElementById('subcategoryContainer');
        container.innerHTML = ''; // Clear the container

        let row;
        subCategories.forEach((subCategory, index) => {
            if (index % 6 === 0) {
                row = document.createElement('div');
                row.className = 'row';
                container.appendChild(row);
            }

            let card = document.createElement('div');
            card.className = 'col-md-3'; // 12 columns / 6 cards per row = 2 columns per card

            card.innerHTML = `
                <div class="subcategory-card card" onclick="navigateToChildCategories(${subCategory.id})">
                    <img src="${subCategory.imageUrl || '/Contents/Images/SubCategories/default.webp'}" class="card-img-top subcategory-image" alt="${subCategory.name}">
                    <div class="card-body">
                        <h5 class="subcategory-name">${subCategory.name}</h5>
                    </div>
                </div>
            `;

            row.appendChild(card);
        });
    } catch (error) {
        console.error('Error fetching subcategories:', error);
    }
}

function navigateToChildCategories(subCategoryId) {
    window.location.href = `Childcategory?subCategoryId=${subCategoryId}`;
}

document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const categoryId = urlParams.get('categoryId');
    if (categoryId) {
        fetchSubCategories(categoryId);
    }
});
