
$(function () {
    $('input[type=tel]').on('keyup', function (event) {
        $(this).val($(this).val().replace(/[^0-9٠-٩]+/g, "").replace(/[^\u0030-\u0039\u0660-\u0669]+/g, ""));
    });

    $('.numbers-only').on('keyup', function (event) {
        $(this).val($(this).val().replace(/[^0-9٠-٩]+/g, "").replace(/[^\u0030-\u0039\u0660-\u0669]+/g, ""));
    });

    jQuery.validator.addMethod("phoneStartCheck", function (value, element) {
        if (value.startsWith("0") || value.startsWith("1") || value.startsWith("2") || value.startsWith("3") || value.startsWith("7") || value.startsWith("8")) {
            return false;
        }
        return true;
    }, Resources.please_enter_a_valid_phone_number);

    jQuery.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
    });

    jQuery.validator.addMethod("customEmail", function (value, element, param) {
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        return emailReg.test(value);
    });

    jQuery.validator.addMethod("isCivilId", function (value, element) {
        if (!value.startsWith('2') && !value.startsWith('3')) {
            return false;
        }

        if (value.length != 12) {
            return false;
        }

        return true;
    });

    jQuery.validator.addMethod("select2Required", function (value) {
        if (value == null || value == undefined || value == "" || value == "0") {
            return false;
        }

        return true;
    });
});


showLog = (text) => {
    if (isDebug) {
        console.log(text);
    }
}

//function validateMobileNumber(event) {
//    if (event.key == "1" || event.key == "2" || event.key == "3" || event.key == "4" || event.key == "5" || event.key == "6" ||
//        event.key == "7" || event.key == "8" || event.key == "9" || event.key == "0" || event.key == "٠" || event.key == "١" ||
//        event.key == "٢" || event.key == "٣" || event.key == "٤" || event.key == "٥" || event.key == "٦" || event.key == "٧" ||
//        event.key == "٨" || event.key == "٩") {
//        return true;
//    }
//    else {
//        return false;
//    }
//}

setButtonText = (buttonId, text) => {
    try {
        return $("#" + buttonId).html(text);
    }
    catch (error) {
        showLog('setButtonText:' + buttonId + ' is ' + error);
    }
};

setTextValue = (inputId, text) => {

    try {
        return $("#" + inputId).val(text);
    }
    catch (error) {
        showLog('setTextValue:' + inputId + ' is ' + error);
    }
};

getTextValue = (inputId) => {
    try {
        return $("#" + inputId).val();
    }
    catch (error) {
        showLog('getTextValue:' + inputId + ' is ' + error);
    }
};

getFloatValue = (inputId) => {
    try {
        return (isNaN(parseFloat($("#" + inputId).val())) ? 0 : parseFloat($("#" + inputId).val()));
    }
    catch (error) {
        showLog('getFloatValue:' + inputId + ' is ' + error);
    }

};

setFloatValue = (inputId, value) => {

    try {
        return $("#" + inputId).val(value);
    }
    catch (error) {
        showLog('setIntegerValue:' + inputId + ' is ' + error);
    }

};

getIntegerValue = (inputId) => {

    try {
        return (isNaN(parseInt($("#" + inputId).val())) ? 0 : parseInt($("#" + inputId).val()));
    }
    catch (error) {
        showLog('setIntegerValue:' + inputId + ' is ' + error);
    }
};

setIntegerValue = (inputId, value) => {

    try {
        return $("#" + inputId).val(value);
    }
    catch (error) {
        showLog('setIntegerValue:' + inputId + ' is ' + error);
    }
};

setCheckValue = (checkBoxId, active) => {
    try {

        var box = $('#' + checkBoxId);
        if (active) {

            box.prop('checked', "checked");
            box.prev().addClass('on');
            box.prev().removeClass('off');
        }
        else {
            box.prop('checked', false);
            box.prev().addClass('off');
            box.prev().removeClass('on');
        }

    }
    catch (error) {
        showLog('setTextValue:' + inputId + ' is ' + error);
    }
};

getCheckValue = (inputId) => {

    try {
        return $("#" + inputId)[0].checked
    }
    catch (error) {
        showLog('getCheckedValue:' + inputId + ' is ' + error);
    }
};

setLabelValue = (labelId, text) => {

    try {
        return $("#" + labelId).text(text);
    }
    catch (error) {
        showLog('setLabelValue:' + labelId + ' is ' + error);
    }
};

setLink = (hrefId, text) => {

    try {
        return $("#" + hrefId).attr("href", text);
    }
    catch (error) {
        showLog('setLink:' + hrefId + ' is ' + error);
    }
};

setSelectedItem = (inputId, data) => {
    $("#" + inputId + " option:eq(" + data + ")").prop('selected', true);
};

setSelectedItemByValue = (inputId, id) => {
    $("#" + inputId + " option[value=" + id + "]").prop('selected', true);
};

setSelectedItemByValueAndTriggerChangeEvent = (inputId, id) => {
    if (id != null) {
        $("#" + inputId + " option[value=" + id + "]").prop('selected', true);
        $("#" + inputId).trigger('change'); //after set list item,fire change event to display the selected item in list
    }
};

getSelectedItemValue = (inputId) => {
    var selectedValue = $('#' + inputId + ' option:selected').val();

    if (selectedValue == undefined || selectedValue == null || selectedValue == "0")
        selectedValue = "";

    return selectedValue;
};

getSelectedStatusValue = (inputId) => {
    var selectedValue = $('#' + inputId + ' option:selected').val();

    if (selectedValue == undefined || selectedValue == null)
        selectedValue = "";

    return selectedValue;
};

getSelectedItemText = (inputId) => {
    var selectedText = $('#' + inputId + ' option:selected').text();
    return selectedText;
};

setImage = (inputId, imageUrl) => {
    try {
        $("#" + inputId).attr('src', imageUrl);
    }
    catch (error) {
        showLog('setImage:' + inputId + ' for ' + imageUrl + '-' + error);
    }
};

getImage = (inputId) => {
    try {
        return $("#" + inputId).file;
    }
    catch (error) {
        showLog('setImage:' + inputId + ' for ' + imageUrl + '-' + error);
    }
};

//for saving in database date column
getDatePickerValue = (inputId) => {
    var _day = $('#' + inputId).val();
    if (_day == null || _day === '') return null;
    //split date based on '-' or '/'
    var _split = (_day.indexOf('-') > 0 ? _day.split('-') : _day.split('/'));
    //return day format (year/month/day)
    var _date = _split[2] + '/' + _split[1] + '/' + _split[0];
    return _date;
};

//display date value in datatable row column
getFormatedDate = (_day) => {
    if (_day == null || _day === '') return null;
    //split date based on '-' or '/'
    var _split = (_day.indexOf('-') > 0 ? _day.split('-') : _day.split('/'));
    //return day format (year/month/day)
    if (_split.length == 3) {
        var _date = _split[2] + '/' + _split[1] + '/' + _split[0];
        return _date;
    }
    else {
        var _date = _split[1] + '/' + _split[0] + '/' + '01';
        return _date;
    }
};

ajaxPost = (url, submitData, callBackPostSuccess = undefined, callBackPostError = undefined, addApiUrl = false) => {
    if (addApiUrl) {
        url = getAPIUrl() + url;
    }

    $.ajax({
        url: url,
        type: "POST",
        processData: false,
        contentType: false,
        data: submitData,
        crossDomain: false,
        success: function (data) {
            if (callBackPostSuccess) { //so call callback action
                callBackPostSuccess(data);
            } else {
                showLog(data);
            }
        },
        error: function (jqXHR, status, errorThrown) {

            if (jqXHR.status == 400) {
                ToastAlert('danger', jqXHR.status, 'Bad Request, please check Method Name');
            } else {
                showLog(jqXHR);
                if (callBackPostError) {
                    callBackPostError(jqXHR);
                }
            }
        }
    }).done(function (data, status, xhr) {
        showLog(data);
    });

}


ajaxWebPost = (url, submitData, callBackPostSuccess = undefined, callBackPostError = undefined, addApiUrl = false) => {
    if (addApiUrl) {
        url = getWebAPIUrl() + url;
    }

    $.ajax({
        url: url,
        type: "POST",
        processData: false,
        contentType: false,
        data: submitData,
        crossDomain: false,
        success: function (data) {
            if (callBackPostSuccess) { //so call callback action
                callBackPostSuccess(data);
            } else {
                showLog(data);
            }
        },
        error: function (jqXHR, status, errorThrown) {

            if (jqXHR.status == 400) {
                ToastAlert('danger', jqXHR.status, 'Bad Request, please check Method Name');
            } else {
                showLog(jqXHR);
                if (callBackPostError) {
                    callBackPostError(jqXHR);
                }
            }
        }
    }).done(function (data, status, xhr) {
        showLog(data);
    });

}

//Dynamic Toast
function ToastAlert(type, title, message, imgUrl, color = undefined) {
    var imgTag = "";
    if (imgUrl != null) {
        imgTag = `<img src="${imgUrl}" class="rounded me-2" alt="..." >`;
    }

    if (type == 'error') {
        type = 'danger';
        // color = 'light'; 
    }

    var color = color ?? 'white';
    const check_time = (i) => {
        return (i < 10) ? "0" + i : i;
    }

    var today = new Date(),
        h = check_time(today.getHours()),
        m = check_time(today.getMinutes()),
        s = check_time(today.getSeconds());
    var time = h + ':' + m + ':' + s;

    var toast_template_html =
        `<div aria-atomic="true" aria-live="assertive" class="toast position-fixed top-0 end-0 m-3" style="z-index:9999999;" role="alert" id="toast_message-${today}">
            <div class="toast-header bg-${type}"> 
                ${imgTag}
                <strong class="me-auto text-${color}">${title}</strong>
                <small class="text-${color}">${time}</small>
                <button aria-label="Close" class="btn-close" data-bs-dismiss="toast" type="button"></button>
            </div>
            <div class="toast-body">${message}</div>
        </div>`;

    var toast_wrapper = document.createElement('template');
    toast_wrapper.innerHTML = toast_template_html.trim();
    var awesome_toast = toast_wrapper.content.firstChild;
    document.querySelector('.toast-container').appendChild(awesome_toast);

    new bootstrap.Toast(
        awesome_toast,
        {
            autohide: true, /* set false for demonstration */
            delay: 5000
        }
    ).show();
};

fillDropDownList = (divId, apiName, keyToMatch, KeyId, optionId, optionName, callbackFunction = undefined, customSelect = undefined) => {
    var myDropDownList = $('#' + divId).empty();
    $.ajax({
        url: getAPIUrl() + apiName,
        type: "GET",
        headers: getAjaxHeader(),
        success: function (data) {
            showLog(data);
            validateAPIResponse(data.statusCode, data.success, data.message);
            if (customSelect && data.data.length == 0) {
                myDropDownList.append(`<option value="0">${customSelect}</option>`);
            }
            else {
                myDropDownList.append(`<option value="0">${Resources.please_select}</option>`);
            }
            $.each(data.data, function (a, b) {
                if (keyToMatch) {
                    if (b.id == KeyId) {
                        myDropDownList.append($("<option selected></option>").val(b[optionId]).html(b[optionName]));
                    } else if (b.idWithType == KeyId) {
                        myDropDownList.append($("<option selected></option>").val(b[optionId]).html(b[optionName]));
                    }
                    else {
                        myDropDownList.append($("<option></option>").val(b[optionId]).html(b[optionName]));
                    }
                } else {
                    myDropDownList.append($("<option></option>").val(b[optionId]).html(b[optionName]));
                }
            });
            //after execution, call callbackfunction
            if (callbackFunction) {
                callbackFunction(data);
            };
        },
        failure: function (response) {
            alert(response.d);
        }
    });
};

validateAPIResponse = (statusCode, success, message, url = undefined) => {

    if (statusCode == undefined) { return true; }

    if (statusCode == 0) { // custom message from backend
        ToastAlert('danger', 'API 0', message + "<br> Please check the admin for below url<br>" + url);
        return true;
    } else if (statusCode == 250) { // custom message from backend, session timeout
        //removeToken();
        ToastAlert('danger', 'Session Timeout', message);
        window.location.href = '/Account/SessionExpired';
        return true;
    } else if (statusCode == 300) { // custom message from backend
        ToastAlert('danger', 'API 300', message);
        return true;
    } else if (statusCode == 401) { // might be token is expired
        ToastAlert('danger', 'API Permission', 'Sorry!, You are not authorized, please contact your System Administrator');
        setTimeout(window.location.href = "/Home/Index", 2000);
        return true;
    } else if (statusCode == 400) {
        ToastAlert('danger', 'API 400', 'Bad Request, please check \r\nMethod type GET/POST or Method Name');
        return true;
    } else if (statusCode == 404) {
        ToastAlert('danger', 'API 404', 'The requested page is not available');
        return true;
    } else if (statusCode == 405) {
        ToastAlert('danger', 'New User', message);
        return true;
    }
    if (success == false && statusCode == 401) {
        removeToken();
        ToastAlert('danger', 'API 401', message);
        window.location.href = '/Account/Login';
        return;
    } else {
        return false;
    }
};