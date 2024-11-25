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



/// Login JS
// Function to send AJAX request
function loginUser() {
    var email = $('#email').val();
    var password = $('#password').val();
    var returnUrl = $('#returnUrl').val();

    var loginData = {
        EmailAddress: email,
        Password: password,
        ReturnUrl: returnUrl
    };

    ajaxWebPost(getWebAPIUrl() + 'webapi/WebUserAuth/Login', JSON.stringify(loginData), loginSuccess, loginError);
}

// Success handler
function loginSuccess(response) {
    if (response.isSuccess) {
        window.location.href = response.returnUrl;
    } else {
        alert('Invalid login credentials. Please try again.');
    }
}

// Error handler
function loginError(xhr, status, error) {
    console.log(xhr.responseText);
    alert('An error occurred during login. Please try again.');
}

// Bind click event to the login button
$('#loginButton').click(function () {
    loginUser();
});

// Utility function to send AJAX request
function ajaxWebPost(url, data, successCallback, errorCallback) {
    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json; charset=utf-8', // Set for JSON
        data: data,
        success: successCallback,
        error: errorCallback
    });
}