// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
var queryStringValues = {};
var appFolder = "/";
var appStoreLink = "https://apps.apple.com/kw/app/%D8%B3-%D8%B1%D8%A7/id6446940774";
var playStoreLink = "https://play.google.com/store/apps/details?id=com.sera.app";
//summernote for code paste

showLog = (text) => {
    if (isDebug) {
        console.log(text);
    }
}
showFormData = (submitData) => {
    if (isDebug) {
        for (var pair of submitData.entries()) {
            console.log(pair[0] + ':' + pair[1]);
        }
    }
}

setHiddenData = (r) => {
    setTextValue('createdBy', r.createdBy);
    setTextValue('createdOn', r.createdOn);
    setTextValue('modifiedBy', r.modifiedBy);
    setTextValue('modifiedOn', r.modifiedOn);
}

submitHiddenData = (submitData) => {

    if (getTextValue('guid') != '') {
        submitData.append('createdBy', getTextValue('createdBy'));
        submitData.append('createdOn', getTextValue('createdOn'));
        submitData.append('modifiedBy', getTextValue('modifiedBy'));
        submitData.append('modifiedOn', getTextValue('modifiedOn'));
    }

}

reloadPage = () => {
    window.location.reload();
}

disableAllElement = (element) => {
    $(element).attr('disabled', true);
}
enableAllElement = (element) => {
    $(element).attr('disabled', false);
}

clearDropdownData = (element) => {
    $(element).empty();
}
validItem = (inputId) => {
    var input = $("#" + inputId);
    if (input.hasClass("invalid")) { input.removeClass("invalid"); }
    if (!input.hasClass("valid")) { input.addClass("valid"); }
}

invalidItem = (inputId) => {
    var input = $("#" + inputId);
    if (input.hasClass("valid")) { input.removeClass("valid"); }
    if (!input.hasClass("invalid")) { input.addClass("invalid"); }
}

clearInputText = (inputId) => {
    $("#" + inputId).val('');
}

isValidURL = (inputId) => {
    var url = inputId.value;
    var pattern = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/;
    if (pattern.test(url)) {
        //alert("Url is valid");
        return true;
    } else {
        alert("Url is Invalid");
    }
    return false;

}

function DownloadFile(url, fileName) {

    $.ajax({
        url: getAPIUrl() + url,
        type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", 'Bearer ' + getToken());
            xhr.setRequestHeader("X-XSRF-TOKEN", getCookie("X-XSRF-TOKEN"));
            xhr.setRequestHeader("lang", $("html").attr("lang"));
        },

        data: '{fileName: "' + fileName + '"}',
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        success: function (r) {
            //Convert Base64 string to Byte Array.
            var bytes = Base64ToBytes(r);

            //Convert Byte Array to BLOB.
            var blob = new Blob([bytes], { type: "application/octetstream" });

            //Check the Browser type and download the File.
            var isIE = false || !!document.documentMode;
            if (isIE) {
                window.navigator.msSaveBlob(blob, fileName);
            } else {
                var url = window.URL || window.webkitURL;
                link = url.createObjectURL(blob);
                var a = $("<a />");
                a.attr("download", fileName);
                a.attr("href", link);
                $("body").append(a);
                a[0].click();
                $("body").remove(a);
            }
        }
    });
};
function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
};

ajaxDownloadFile = (url, callbackGetSuccess = undefined, callbackGetError = undefined) => {
    if (isDebug) {
        showLog('dev Url:' + getAPIUrl() + url);
    }
    $.ajax({
        url: getAPIUrl() + url,
        type: "POST", xhrFields: { withCredentials: true, responseType: 'blob' }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        dataType: "text",
        success: function (data) {

            let blob = new Blob([data], { type: "application/octetstream" });

            var a = document.createElement('a');
            var url = window.URL.createObjectURL(blob);
            a.href = url;
            a.download = "sitemaps.xml";
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(a.href);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showLog(jqXHR);
        }
    }).done(function (data, status, xhr) {
        showLog(data);
    });
}

ajaxGet = (url, callbackGetSuccess = undefined, callbackGetError = undefined) => {
    if (isDebug) {
        showLog('dev Url:' + getAPIUrl() + url);
    }

    $.ajax({
        url: getAPIUrl() + url,
        method: "GET", headers: getAjaxHeader(), xhrFields: { withCredentials: true },
        success: function (data, status, xhr) {
            validateAPIResponse(data.statusCode, data.success, data.message, url);
            if (callbackGetSuccess) {
                callbackGetSuccess(data);
            } else {
                if (callbackGetError !== undefined) {
                    callbackGetError(data);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showLog(jqXHR);
            if (callbackGetError) {
                callbackGetError(jqXHR);
            };
            setTimeout(hideLoader(), 1000);
        }
    }).done(function (data, status, xhr) {
        showLog(data);
    });
}

ajaxPost = (url, submitData, callBackPostSuccess = undefined, callBackPostError = undefined) => {
    showLoader();
    if (isDebug) {
        showLog('dev Url:' + getAPIUrl() + url);
        if (submitData != null) { showFormData(submitData) };
    }
    $.ajax({
        url: getAPIUrl() + url,
        type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        processData: false,
        contentType: false,
        data: submitData,
        crossDomain: false,
        success: function (data) {
            hideLoader();
            var action = validateAPIResponse(data.statusCode, data.success, data.message, url);
            if (action == false) { //no action has been taken
                if (callBackPostSuccess) { //so call callback action
                    callBackPostSuccess(data);
                };
            }
        },
        error: function (jqXHR, status, errorThrown) {
            hideLoader();
            if (jqXHR.status == 404) {
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
ajaxPostNew = (url, submitData, callBackPostSuccess = undefined, callBackPostError = undefined) => {

    if (isDebug) {
        showLog('dev Url:' + getAPIUrl() + url);
        if (submitData != null) { showFormData(submitData) };
    }
    $.ajax({
        url: getAPIUrl() + url,
        type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        //},
        cache: false,
        processData: false,
        contentType: false,
        data: submitData,
        crossDomain: false,
        success: function (data) {
            var action = validateAPIResponse(data.statusCode, data.success, data.message, url);
            if (action == false) { //no action has been taken
                if (callBackPostSuccess) { //so call callback action
                    callBackPostSuccess(data);
                };
            }
        },
        error: function (jqXHR, status, errorThrown) {
            if (jqXHR.status == 404) {
                ToastAlert('danger', jqXHR.status, 'Bad Request, please check Method Name');
            } else {
                showLog(jqXHR);
                if (callBackPostError) {
                    callBackPostError(jqXHR);
                }
            }
            // setTimeout(hideLoader(), 1000);
        }
    }).done(function (data, status, xhr) {
        showLog(data);
        //setTimeout(hideLoader(), 1000);
        //console.log(xhr.getResponseHeader('Link'));
    });

}

beep = () => {
    if (getCookie("X-XSRF-TOKEN") == null) {
        ajaxGet('heartbit');
    }
}
//1--
addCheckedActionGuid = (apibase, row) => {
    var html = `<div class="switch switch-sm switch-primary my-0 " id="dv-active-${row.guid}" 
        onclick="doToggleActivateGuid('${apibase}','${row.guid}',${row.id});">
        <input ${(row.active ? "checked=true" : "")} type="checkbox"   data-plugin-ios-switch />
    </div>`;
    return html;
};
//follow-up of addCheckActionGuid
doToggleActivateGuid = (apibase, guid, id) => {

    $.ajax({
        url: getAPIUrl() + apibase + "?guid=" + guid,
        type: "POST", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (data) {

            if (data.success) {
                ToastAlert('success', 'Active', data.message);
                if (data.data != null) {
                    $(document).find("[data-rowid='" + id + "'] td.text-wrap span.status-column").removeClass("bg-success").removeClass("bg-danger").removeClass("bg-warning").addClass(data.data.statusColor).html(data.data.status);
                }
            } else {
                ToastAlert('danger', 'Active', data.message);
                if (!$("#dv-active-" + guid).find(".ios-switch").hasClass("on"))
                    $(document).find('#dv-active-' + guid + ' .ios-switch').removeClass('off').addClass("on");
                else
                    $(document).find('#dv-active-' + guid + ' .ios-switch').removeClass('on').addClass("off");
            }
            return false;
        },
    });
};

doDeleteGuid = (apibase, guid) => {
    $.ajax({
        url: getAPIUrl() + apibase + "/Delete?guid=" + guid,
        method: "DELETE", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (data) {
            if (data.success) {
                ToastAlert('success', 'Active', data.message);
                reloadPage();
            } else {
                showLog(data);
                ToastAlert('danger', 'Delete', data.message);
            }
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
//check for logout calling DataTable records
checkAPIResponse = (json) => {
    if (json.success == false && json.statusCode == 250) {
        showLog(json);
        removeToken();
        window.location.href = '/Account/SessionExpired';
    }
    return true;
};

//add change event on imageFile, if the file is selected, show it in the preview 
addEventForImageFileAndPreview = (imageFileId, imagePreviewId) => {

    $("#" + imageFileId).on('change', function (event) {
        var ext = this.value.match(/\.([^\.]+)$/)[1];
        switch (ext) {
            case 'jpg':
            case 'png':
                // alert('Allowed');
                const [file] = this.files
                if (file) {
                    $("#" + imagePreviewId).attr('src', window.URL.createObjectURL(this.files[0]));

                }
                break;
            default:
                alert('Not allowed');
                this.value = '';
        }
    });


};

doDeleteCommonGuid = (apibase, guid) => {
    $.ajax({
        url: getAPIUrl() + apibase + "?guid=" + guid,
        method: "GET", xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); },
        success: function (data) {
            if (data.success) {
                ToastAlert('success', 'Active', data.message);
                reloadPage();
            } else {
                showLog(data);
                ToastAlert('danger', 'Delete', data.message);
            }
        }
    });

};

//selecte dropdown item with value
//dataType: "json",'headers': { "Content-Type": "application/json" },
fillDropDownList = (divId, apiName, keyToMatch, KeyId, optionId, optionName, callbackFunction = undefined) => {
    var myDropDownList = $('#' + divId).empty();
    $.ajax({
        url: getAPIUrl() + apiName,
        type: "GET",
        headers: getAjaxHeader(),
        success: function (data) {
            showLog(data);
            validateAPIResponse(data.statusCode, data.success, data.message);
            myDropDownList.append(`<option value="0">${Resources.dropdown_select}</option>`);
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

//selecte dropdown item with value
//dataType: "json",'headers': { "Content-Type": "application/json" },
fillDropDownListWithAll = (divId, apiName, keyToMatch, KeyId, optionId, optionName, callbackFunction = undefined) => {
    var myDropDownList = $('#' + divId).empty();
    $.ajax({
        url: getAPIUrl() + apiName,
        type: "GET",
        headers: getAjaxHeader(),
        success: function (data) {
            showLog(data);
            validateAPIResponse(data.statusCode, data.success, data.message);
            myDropDownList.append(`<option value="0">${Resources.dropdown_all}</option>`);
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

fillDropDownListByControl = (control, apiName, keyToMatch, KeyId, optionId, optionName, callbackFunction = undefined) => {
    var myDropDownList = control;
    $.ajax({
        url: getAPIUrl() + apiName,
        type: "GET",
        headers: getAjaxHeader(),
        success: function (data) {
            showLog(data);
            validateAPIResponse(data.statusCode, data.success, data.message);
            myDropDownList.empty();
            myDropDownList.append(`<option value="0">${Resources.dropdown_select}</option>`);
            myDropDownList.attr("data-optionCount", data.data.length);
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

//parameters
//1-element Id
//2-list data
//3-KeyToMatch for item.id == with user provided Id for selected Item
//4-Key Value to be Matched
//5-Item Field Value
//6-Item Field Name for display
//7-Callback function
fillDropDownListData = (divId, data, keyToMatch, KeyId, optionId, optionName, callbackFunction = undefined) => {
    var myDropDownList = $('#' + divId).empty();

    myDropDownList.append(`<option value="0">${Resources.dropdown_select}</option>`);

    $.each(data, function (a, b) {
        if (keyToMatch) {
            if (b.id == KeyId) {
                myDropDownList.append($("<option selected></option>").val(b[optionId]).html(b[optionName]));
            } else {
                myDropDownList.append($("<option value='" + b[optionId] + "'></option>").val(b[optionId]).html(b[optionName]));
            }
        } else {
            myDropDownList.append($("<option value='" + b[optionId] + "'></option>").val(b[optionId]).html(b[optionName]));
        }

    });
    //after execution, call callbackfunction
    if (callbackFunction) {
        callbackFunction();
    };


};

setButtonText = (buttonId, text) => {
    try {
        return $("#" + buttonId).html(text);
    }
    catch (error) {
        showLog('setButtonText:' + buttonId + ' is ' + error);
    }
};

setInputAttribute = (inputId, attribute, boolean) => {
    try {
        //return $("#" + inputId).setAttribute(attribute, boolean);
        var element = document.getElementById(inputId);
        element.setAttribute(attribute, boolean);
        //element.onmouseover = element.style.cursor = 'pointer';
        //element.mouseover (function (e) {
        //    $(e.target).style.cursor = 'pointer';
        //    //console.log($(e.target).attr('id')); // i just retrieved the id for a demo
        //});
    }
    catch (error) {
        showLog('setTextValue:' + inputId + ' is ' + error);
    }
    // document.getElementById("emailAddress").setAttribute('readonly', true);
}

setTextAreaValue = (inputId, text) => {

    try {
        var item = $("#" + inputId);
        item.val(text);
        item.trigger('keyup');
        //        return $("#" + inputId).val(text);

        //return $("#" + inputId).val(text);
    }
    catch (error) {
        showLog('setTextValue:' + inputId + ' is ' + error);
    }
};

setTextValue = (inputId, text) => {

    try {
        var item = $("#" + inputId);
        item.val(text);
        item.change();
        return $("#" + inputId).val(text);

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
setCheckValueWithChangeEvent = (checkBoxId, active) => {
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
        box.change();
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

getCheckValueByName = (inputId) => {

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
    $("#" + inputId + " option[value=" + id + "]").prop('selected', true);
    $("#" + inputId).trigger('change'); //after set list item,fire change event to display the selected item in list
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

//Dynamic Toast
ToastAlert = (type, title, message, imgUrl, color = undefined) => {
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


//set text field date value
setDatePickerValue = (inputId, dateFromDb) => {
    //1.    datepicker should be added in layout.cshtml because 
    //      it is initialized with date display format 'theme.init.js'
    //2. below script file should be added in the View because ?
    //<script src="~/plugin/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
    $('#' + inputId).bootstrapDP('setDates', [getFormatedDate(dateFromDb)]);

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
getFormatedDate = (_date) => {
    if (_date == null) return null;
    //remove time from the date if it contains
    var _day = (_date.indexOf('T') > 0 ? _date.split('T')[0] : _date);
    //split date based on '-' or '/'
    var _split = (_day.indexOf('-') ? _day.split('-') : _day.split('/'));
    var _date = _split[2] + '/' + _split[1] + '/' + _split[0];
    return _date;
};

//for saving in database date column
getDatePickerValueN1 = (inputId) => {
    var _day = $('#' + inputId).val();
    if (_day == null || _day === '') return null;
    //split date based on '-' or '/'
    var _split = (_day.indexOf('-') > 0 ? _day.split('-') : _day.split('/'));
    //return day format (year/month/day)
    var _date = _split[2] + '/' + _split[1] + '/' + _split[0] + 'T00:00:00';
    return _date;
};

//display date format for datatable column (month/day/year) (6/6/2022, 1:01:47 PM)
getFormatedDateTime = (_date) => {
    if (_date == null) return null;
    var datetime = new Date(_date).toLocaleString();
    return datetime;
};

validateMobileNumber = (evt) => {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
};

maxlength = (event) => {
    const ele = event.target;
    const maxlength = ele.maxLength;
    const value = ele.value;
    if (event.type == 'keypress') {
        if (value.length >= maxlength) {
            event.preventDefault();
        }
    }
    else if (event.type == 'keyup') {
        if (value.length > maxlength) {
            ele.value = value.substring(0, maxlength);
        }
    }
};


getTwentyFourHourTime = (timeValue) => {
    var d = new Date("1/1/2013 " + timeValue);
    return d.getHours() + ':' + d.getMinutes();
};

compressImageFile = async (file) => {
    return new Promise((resolve, reject) => {
        new Compressor(file, {
            quality: 0.6,
            // The compression process is asynchronous,
            // which means you have to access the `result` in the `success` hook function.
            success(result) {
                console.log('compress success');
                //console.log(result);
                //console.log(result.name);
                return resolve(result);
            },
            error(err) {
                console.log(err.message);
                return reject(err);
            },
        })
    });
};


$(function () {
    $("#googleLocationLink").on('change paste onpaste keyup blur', function () {
        if ($(this).val()) {
            var data = extractCoordinatesFromGoogleMapsLink($(this).val());
            if (data) {
                $("#latitude").val(data.lat);
                $("#longitude").val(data.long);
            }
        }
    });
});

function extractCoordinatesFromGoogleMapsLink(link) {
    var regex = new RegExp('@(.*),(.*),');
    var lat_long_match = link.match(regex);
    var lat = lat_long_match[1];
    var long = lat_long_match[2];
    return {
        lat,
        long
    };
}

function hasDuplicates(arr) {
    var counts = [];

    for (var i = 0; i <= arr.length; i++) {
        if (counts[arr[i]] === undefined) {
            counts[arr[i]] = 1;
        } else {
            return true;
        }
    }

    return false;
}

$(function () {
    $("input,textarea,select").on("change", function () {
        if ($(this).val() != null || $(this).val() != "") $(this).attr("data-changed", "true");
    });
});
function getChangedFields() {
    var ChangedFields = [];
    $("[data-changed=true]").each(function () {
        var _Name = $(this).attr("name");
        var _ID = $(this).attr("id");
        var _value = $(this).val();
        var _type = $(this).attr("type") ? $(this).attr("type") : $(this).prop("tagName");
        ChangedFields.push({
            "name": _Name,
            "id": _ID,
            "value": _value,
            "type": _type
        });
    });
    return ChangedFields;
}

var SitePageType = {
    Home: 1,
    AboutUs: 2,
    Attraction: 3,
    Activity: 4,
    Projects: 5,
    News: 6,
    Events: 7,
    MediaGallery: 8,
    Careers: 9,
    GetInTouch: 10,
    QuickLinks: 11,
    InformationRequest: 12,
    GrievanceForm: 13,
    RightToAccessInformation: 14,
    TermsAndConditions: 15,
    PrivacyPolicy: 16,
    KuwaitTowers: 17,
    Clubs: 18,
    Resorts: 19,
    Beaches: 20,
    ParksGardens: 21,
    Entertainment: 22,
    Swimming: 23,
    HealthFitness: 24,
    KidsPlayAreas: 25,
    ThemeParks: 26,
    RentChalets: 27,
    ActivityBeaches: 28,
    BoatsYachts: 29,
    PicnicSpots: 30,
    Dining: 31,
    Festivals: 32,
    ActivityEntertainment: 33,
    PanoramicView: 34,
    TermsCondition: 35,
    ContactUs: 36,
    AttractionType: 37,
    SiteMenu: 38,
    ActivityType: 39,
};
