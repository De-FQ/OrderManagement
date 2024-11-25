$(function () { // Handler for .ready() called. 
    if (getCookie("X-XSRF-TOKEN") == null)  {
     //   getApiToken();
    }
});

getApiToken = () => {
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