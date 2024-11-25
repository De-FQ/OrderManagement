$(function () { // Handler for .ready() called. 
    clearDataTable(); searchDataTable(); 
});

searchDataTable = () => { 
    if ($.fn.dataTable.isDataTable("#datatable-default-")) {
        $("#datatable-default-").DataTable().state.clear();
        $("#datatable-default-").DataTable().destroy();
    }
   
    var  table = $("#datatable-default-").DataTable({
        searching: true,
        ajax: {
            url: getAPIUrl() + "User/GetForDataTable",
            type: "POST",  xhrFields: { withCredentials: true }, beforeSend: function (xhr) { getDataTableHeaders(xhr); }, 
            dataSrc: function (json) {
                checkAPIResponse(json);
                hideTableColumn(table, [{ col: 1, visible: allowActive }]); 
                return json.data;
            }, error: function (error) { fixDTError(error); }, 
             
        },
        columns: [
             {
                "data": "id", render: function (data, type, row, meta) {
                    return getRowNumber(meta);
                }
            }, 
            {
                "data": "active", render: function (data, type, row) {
                    return addCheckedActionGuid('User/ToggleActive', row);
                }
            },
            {
                "data": "imageUrl", render: function (data, type, row) {
                    return "<img src='" + row.imageUrl + "'  class='img-fluid text-center mx-auto' width='100px' />";
                }
            },
            { "data": "fullName" },
            { "data": "emailAddress" },
            { "data": "roleName" },
            { "data": "formattedRegisteredBy" },
            { "data": "formattedLastLogin" },
            {
                "data": null, "name": "Actions", render: function (data, type, row) {
                    return `${addDropDownMenuOptions("User", "/UserMgmt/User/", row, { edit: allowEdit, delete: allowDelete, password: allowDelete,  roleId: row.id })}`;
                }
            }, 
        ],
        createdRow: function (row, data, index) {
            //change display order value, to avoid page refresh
            $(row).attr('data-rowid', data.id);
            $(row).find('td:eq(0)').attr('data-displayorder', row.id);

            //    Reinitialize ios-switch
            $(row).find('[data-plugin-ios-switch]').themePluginIOS7Switch();
        },
        columnDefs: [
            {
                targets: [0],
                defaultContent: '',
                render: function (data, type, row, meta) {
                    var displayid = meta.row + meta.settings._iDisplayStart + 1;
                    return `<div data-i="${displayid}" style="cursor: pointer;">${displayid}</div>`;
                },
                orderable: false,
                searchable: false
            },
            /*{ "targets": -1, "orderable": false },*/
            { "className": "text-wrap", "targets": "_all" },
        ],
    });
    //.responsive.recalc().columns.adjust();
}


showDialogPassword = (guid, uname) => { 
    $("#guid").val(guid);
    $("#update_passwordModalLabel").text("Change Password - " +uname);
    $('#update_passwordModal').modal('toggle'); //show popup window for permissions
    setupFormValidation();
}


newPasswordOnFocus = () => {
    document.getElementById("message").style.display = "block";
}
newPasswordOnBlur = () => { 
   document.getElementById("message").style.display = "none"; 
}
newPasswordOnKeyUp = (input) => {
    isPasswordValid();
}
isPasswordValid = () => {
    var p1 = false, p2 = false, p3 = false, p4 = false;
    var input = document.getElementById("pwd");

    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (input.value.match(lowerCaseLetters)) { validItem("letter"); p1 = true;
    } else {invalidItem("letter");}

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (input.value.match(upperCaseLetters)) { validItem("capital"); p2 = true;
    } else { invalidItem("capital"); }

    // Validate numbers
    var numbers = /[0-9]/g;
    if (input.value.match(numbers)) {  validItem("number"); p3 = true;
    } else { invalidItem("number"); }

    // Validate length
        if (input.value.length >= 5) { validItem("minlength"); p4 = true;
        } else { invalidItem("minlength"); }

    if (p1 ==true && p2 ==true && p3==true && p4==true) { 
         return true;
    } else { 
        return false;
    }
}

setupFormValidation = () => {

    $("#dataForm").validate({
        rules: {
            //password: {
            //    required: true, minlength: 5,
            //    regex: /(?=[A-Za-z0-9@#$%&+!=]+$)^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+!=)])(?=.{6,12}).*$/,
            //},
            pwd: {
                required: true, minlength: 5, maxlength: 12,
                //regex: /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}/,
            },
            cpwd: {
                required: true, minlength: 5, maxlength: 12,
                //regex: /(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,12}/,
                //regex: /(?=[A-Za-z0-9@#$%&+!=]+$)^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+!=)])(?=.{6,12}).*$/,
                // For checking both passwords are same or not
                equalTo: "#pwd"
            },
        },
        messages: {
            //password: {
            //    required: 'Please enter password',
            //    minlength: 'password length minimum 6 and maximum 12 characters',
            //    regex: `Password must contain at least <br/>1 capital letter,<br/>1 small letter, <br/>1 number and <br/>1 special character.<br/>For special characters you can pick one of these !,@,#,$,%,&,+`,
            //},
            pwd: {
                required: 'Please enter password',
                //minlength: 'Length is short, minimum 5 required',
                //maxlength: 'Length is not valid, maximum 12 characters allowed',
                //regex: `Password must contain at least <br/>1 capital letter,<br/>1 small letter, <br/>1 number and <br/>1 special character.<br/>For special characters you can pick one of these !,@,#,$,%,&,+`,
            },
            cpwd: {
                required: 'Please enter confirm password',
                minlength: 'Length is short, minimum 5 required',
                maxlength: 'Length is not valid, maximum 12 characters allowed',
                equalTo: " Please enter the same password as above",
                // regex: `Password must contain at least <br/>1 capital letter,<br/>1 small letter, <br/>1 number and <br/>1 special character.<br/>For special characters you can pick one of these !,@,#,$,%,&,+`,
            },
        },
        submitHandler: function (form, event) {
            event.preventDefault();
            var valid = isPasswordValid();
            if (valid == false) {
                event.stopPropagation();
                var pwd = $("#pwd");
                pwd.trigger("focus");
                if (pwd.hasClass("is-valid")) { pwd.removeClass("is-valid"); }
            } else {
                $("button#btnSave").attr('disabled', 'disabled');
                saveData();
            }
        },
        errorPlacement: function ($error, $element) {
            if ($element.siblings(".invalid-feedback").length)
                $element.siblings(".invalid-feedback").html($error);
        }
    });
} 
 

saveData = () => {
      
    let submitData = new FormData(); 
    var guid = $("#guid").val();
    submitData.append("Guid", $("#guid").val()); 
    submitData.append("password", getTextValue('pwd'));  
    ajaxPost("User/UpdatePassword", submitData, cbSuccess );
 }


cbSuccess = (data) => {
    if (data.success) {
        ToastAlert('success', 'System User', 'Password changed successfully');
        setTimeout(() => location.href = "/UserMgmt/User/UserList", 1000);
    } else {
        ToastAlert('error', 'System User', data.message);
    }
}



