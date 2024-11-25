
var tokenKey = "AuthenticationToken";
var refreshKey = "refreshToken";
setToken = idToken => {
    // Saves user token to localStorage
    localStorage.setItem(tokenKey, idToken);
};

getToken = () => {
    // Retrieves the user token from localStorage
    var token = getCookie(tokenKey);
    //console.log('Admin: '+token);
    return token;
};
setCookie = (name, value, days) => {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + (value | '') + expires + "; path=/";
}

getCookie = (name) => {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
getAntiforgeryCookie = (name) => {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0)
            return c.substring(nameEQ.length, c.length);
    }
    return   getApiToken();
    //return null;

}

 getApiToken=()=> {
    $.ajax({
        url: getAPIUrl() + 'XsrfToken', method: "GET", headers: getAjaxHeader(),
        success: function (data, status, xhr) {
            console.log(data);
            return data.token;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR);
        }
    });

}
removeCookie = (name) => {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};

getAjaxHeaderImages = (xhr) => {
    xhr.setRequestHeader("Authorization", 'Bearer ' + getToken());
    xhr.setRequestHeader("X-XSRF-TOKEN", getCookie("X-XSRF-TOKEN"));
    xhr.setRequestHeader("lang", $("html").attr("lang"));
    xhr.setRequestHeader("Accept", 'application/json');
    //xhr.setRequestHeader("Accept", "*/*"); old

}

getDataTableHeaders = (xhr) => {
    xhr.setRequestHeader("Authorization", 'Bearer ' + getToken()); 
    xhr.setRequestHeader("X-XSRF-TOKEN", getCookie("X-XSRF-TOKEN"));  
    xhr.setRequestHeader("lang", $("html").attr("lang"));
    xhr.setRequestHeader("Accept", 'application/json'); 
    //xhr.setRequestHeader("Accept", "*/*"); old
    
}


//        "X-XSRF-TOKEN": getCookie("X-XSRF-TOKEN"),
getAjaxHeader = () => {
    return {
        "Content-Type": "application/json",
        "Authorization": 'Bearer ' + getToken(),
        "lang": getLang()
    };
};


removeToken = () => {
    // Clear user token and profile data from localStorage
    localStorage.removeItem(tokenKey);
};

getRefreshToken=() => {
    var _rt = getCookie('refreshToken');
    return _rt;

}
isLoggedIn = () => {
    // Checks if there is a saved token and it's still valid
    const token = this.getToken(); // Getting token from localstorage
    //const loggedIn = !!token && !this.isTokenExpired(); // handwaiving here
    //console.log(loggedIn);
    return !!token && !this.isTokenExpired(); // handwaiving here
};

isTokenExpired = () => {
    try {
        const decoded = jwt_decode(getToken());

        if (decoded.exp < Math.floor(Date.now() / 1000)) {
            // Checking if token is expired.
            showLog('token has expired');
            return true;
        } else return false;
    } catch (err) {
        showLog("expired check failed! " + err);
        return true;
    }
};

setUserCookie = (name, data, days) => {

    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }

    document.cookie = name + "=" + JSON.stringify(data) + expires + "; path=/";

}

getUserCookie = (name) => {
    var result = document.cookie.match(new RegExp(name + '=([^;]+)'));
    result && (result = JSON.parse(result[1]));
    return result;
}




var userEmail = getCookie("user_name");

setParam = (page, name, value) => {
    const key = page + '_' + name;
    localStorage.setItem(key, value);
}

getParam = (page, name) => {
    const key = page + '_' + name;
    const data = localStorage.getItem(key);
    return data;
}

removeParam = (page, name) => {
    localStorage.removeItem(page + '_' + name);
}
addStorage = (key, value) => {
    localStorage.setItem(key, JSON.stringify(value));
}
getStorage = (key) => {
    var data = JSON.parse(localStorage.getItem(key));
    return data;
}
removeStorage = (key) => {
    localStorage.removeItem(key);
}


