
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

        for (let index = 0; index < categories.length; index++) {
            let category = categories[index];

            let categoryButton = document.createElement('button');
            categoryButton.className = 'category-button btn btn-secondary';
            categoryButton.style.marginTop = index === 0 ? '0px' : '0'; // Add top margin to the first category button

            categoryButton.innerHTML = `
                <img src="${category.imageUrl || '/Contents/Images/Categories/default.webp'}" class="category-image" alt="${category.name}">
                <span class="category-name">${category.name}</span>
            `;

            let subcategoryContainer = document.createElement('div');
            subcategoryContainer.className = 'subcategory-container';
            subcategoryContainer.id = `subcategoryContainer${category.id}`;

            container.appendChild(categoryButton);
            container.appendChild(subcategoryContainer);

            // Fetch and display subcategories immediately
            fetchSubCategories(category.id);
        }
    } catch (error) {
        console.error('Error fetching categories:', error);
    }
}

async function fetchSubCategories(categoryId) {
    try {
        history.pushState({}, '', window.location.pathname);
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

function navigateToChildCategories(subCategoryId) {
    window.location.href = `/Categories/ChildCategory?subCategoryId=${subCategoryId}`;
}

document.addEventListener('DOMContentLoaded', fetchCategories);


function filterCategories() {
    let input = document.getElementById('categorySearch').value.toLowerCase();
    let categoryButtons = document.getElementsByClassName('category-button');
    let subCategoryContainers = document.getElementsByClassName('subcategory-container');

    Array.from(categoryButtons).forEach(button => {
        let categoryName = button.querySelector('.category-name').innerText.toLowerCase();
        let subCategoryContainer = document.getElementById(button.nextElementSibling.id);

        if (categoryName.includes(input)) {
            button.style.display = 'block'; // Show the category button
            subCategoryContainer.style.display = 'block'; // Show related subcategories
        } else {
            // Check subcategories if category name doesn't match
            let subCategoryButtons = subCategoryContainer.getElementsByClassName('subcategory-button');
            let showCategory = false;

            Array.from(subCategoryButtons).forEach(subButton => {
                let subCategoryName = subButton.querySelector('.subcategory-name').innerText.toLowerCase();
                if (subCategoryName.includes(input)) {
                    subButton.style.display = 'block'; // Show matching subcategory
                    showCategory = true; // At least one subcategory matches, show the category
                } else {
                    subButton.style.display = 'none'; // Hide non-matching subcategory
                }
            });

            if (showCategory) {
                button.style.display = 'block'; // Show category if any subcategory matches
                subCategoryContainer.style.display = 'block'; // Show related subcategories
            } else {
                button.style.display = 'none'; // Hide category if no subcategory matches
                subCategoryContainer.style.display = 'none'; // Hide subcategories
            }
        }
    });
}
