async function fetchCategories() {
    try {
        let response = await fetch(getWebAPIUrl() + 'webapi/Categories/GetActiveCategories');
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let categories = await response.json();
        console.log(categories); // Debugging: Log the categories fetched from API

        let container = document.getElementById('categoryContainer');
        container.innerHTML = ''; // Clear the container

        categories.forEach((category, index) => {
            let categoryButton = document.createElement('button');
            categoryButton.className = 'category-button btn btn-primary';
            categoryButton.onclick = () => toggleSubCategories(category.id);

            if (index === 0) {
                categoryButton.style.marginTop = '100px'; // Add top margin to the first category button
            }

            categoryButton.innerHTML = `
                <img src="${category.imageUrl || '/Contents/Images/Categories/default.webp'}" class="category-image" alt="${category.name}">
                <span class="category-name">${category.name}</span>
            `;

            let subcategoryContainer = document.createElement('div');
            subcategoryContainer.className = 'subcategory-container collapse';
            subcategoryContainer.id = `subcategoryContainer${category.id}`;

            container.appendChild(categoryButton);
            container.appendChild(subcategoryContainer);
        });
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}

async function fetchSubCategories(categoryId) {
    try {
        let response = await fetch(getWebAPIUrl() + 'webapi/SubCategories/GetActiveSubCategories?categoryId=' + categoryId);
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        let subCategories = await response.json();
        console.log(subCategories); // Debugging: Log the subcategories fetched from API

        let container = document.getElementById(`subcategoryContainer${categoryId}`);
        container.innerHTML = ''; // Clear the container

        subCategories.forEach(subCategory => {
            let subCategoryButton = document.createElement('button');
            subCategoryButton.className = 'subcategory-button btn btn-secondary';
            subCategoryButton.onclick = () => navigateToChildCategories(subCategory.id);

            subCategoryButton.innerHTML = `
                <img src="${subCategory.imageUrl || '/Contents/Images/SubCategories/default.webp'}" class="subcategory-image" alt="${subCategory.name}">
                <span class="subcategory-name">${subCategory.name}</span>
            `;

            container.appendChild(subCategoryButton);
        });
    } catch (error) {
        console.error('Error fetching subcategories:', error);
    }
}

function toggleSubCategories(categoryId) {
    let subcategoryContainer = document.getElementById(`subcategoryContainer${categoryId}`);
    if (subcategoryContainer.classList.contains('collapse')) {
        fetchSubCategories(categoryId);
        subcategoryContainer.classList.remove('collapse');
    } else {
        subcategoryContainer.classList.add('collapse');
    }
}

function navigateToChildCategories(subCategoryId) {
    window.location.href = `/Categories/ChildCategory?subCategoryId=${subCategoryId}`;
}

document.addEventListener('DOMContentLoaded', fetchCategories);
