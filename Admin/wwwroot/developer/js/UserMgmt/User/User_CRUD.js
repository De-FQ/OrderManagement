var guid = '';
$(function () { // Handler for .ready() called.  
    setup();
    guid = getTextValue("Guid");
    loadRoleFor();
});

loadRoleFor = () => {
    fillDropDownList('roleList', 'Common/ForRoleDropDownList', false, '', 'id', 'name', loadDataFor);
};

 
setup = () => {
     
    $("#dataForm").validate({
        rules: {
            emailAddress: { required: true, email: true },
            fullName: { required:true, },
            roleList: { select2Required: true, required: true, },
            pwd: { required: true },
            cpwd: {required: true, minlength: 5, maxlength: 12,  equalTo: "#pwd" },
        },
        messages: {
            emailAddress: { required: 'Please enter a valid email address' },
            fullName: { required: 'Please enter your full name' },
            pwd: { 
                required: 'Please enter password',
                //minlength: 'Length is short, minimum 5 required',
                //maxlength: 'Length is not valid, minimum 12 characters allowed',
            },
            cpwd: {
                required: 'Please enter confirm password',
                minlength: 'Length is short, minimum 5 required',
                maxlength: 'Length is not valid, maximum 12 characters allowed',
                equalTo: " Please enter the same password as above",
            },
            roleList: { select2Required: 'Please select a role' },

        },
        submitHandler: function (form, event) {
            event.preventDefault(); 
            var valid = isPasswordValid(); 
            if (valid==false) { 
                event.stopPropagation();
                var pwd = $("#pwd"); pwd.trigger("focus");
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
     

    newPasswordOnFocus = () => {
        document.getElementById("message").style.display = "block";
    }
    newPasswordOnBlur = () => {
        document.getElementById("message").style.display = "none";
    }
    newPasswordOnKeyUp = (input) => {
        isPasswordValid();
    }



};

isPasswordValid = () => {
    var p1 = false, p2 = false, p3 = false, p4 = false;
    var input = document.getElementById("pwd");   

    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (input.value.match(lowerCaseLetters)) { validItem("letter"); p1 = true;
    } else { invalidItem("letter");  }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (input.value.match(upperCaseLetters)) { validItem("capital");  p2 = true;
    } else { invalidItem("capital"); }

    // Validate numbers
    var numbers = /[0-9]/g;
    if (input.value.match(numbers)) { validItem("number"); p3 = true;
    } else { invalidItem("number")  }

    // Validate length
    if (input.value.length >= 5) { validItem("minlength"); p4 = true;
    } else { invalidItem("minlength"); }
     
    if (p1 == true && p2 == true && p3 == true && p4 == true) { 
        return true;
    } else { 
        return false;
    }
}


loadDataFor = () => {
    if (guid == '') { return; } 
    ajaxGet('User/GetByGuid?guid=' + guid, cbGetSuccess);
};

cbGetSuccess = (data) => {
    if (data.data == null) { return; }
    var r = data.data;


    console.log(r);
    setTextValue("emailAddress", r.emailAddress);
    setInputAttribute("emailAddress", "readonly", true);
    setTextValue("fullName", r.fullName);
    setTextValue("pwd", r.password);
    setTextValue("cpwd", r.password);
    setSelectedItemByValueAndTriggerChangeEvent('roleList', r.userRoleId);
    if (r.imageUrl != null) {
        setImage("imagePreview", r.imageUrl);
        $("#imageFile").data("oldImageName", r.imageName);
        $("#imageFile").rules("remove", "required");
    }
    setHiddenData(r);
    isPasswordValid();
}

saveData = () => { 
    let submitData = new FormData();
    submitData.append("Guid", guid);
    submitData.append("emailAddress", getTextValue('emailAddress'));
    submitData.append("fullName", getTextValue('fullName'));
    submitData.append("password", getTextValue('pwd'));
    submitData.append('userRoleId', getSelectedItemValue('roleList')); 
    
    if (imageFile.files[0] != undefined) { 
        var compressedFile = dataURLtoFile(".preview-image", imageFile.files[0].name);
        submitData.append("image", compressedFile);
        submitData.append("imageName", $("#imageFile").data("oldImageName"));
    }
    submitHiddenData(submitData);
     ajaxPost("User/AddEdit", submitData, cbPostSuccess, cbPostError);
};



cbPostSuccess = (data) => { 
    
    if (data.success) {
        ToastAlert('success', 'System User', 'Saved Successfully');
        setTimeout(() => location.href = "/UserMgmt/User/UserList", 2000);
    } else {
        ToastAlert('error', 'System User', 'Unable to save, please try again or contact to system admin');
    }
}

cbPostError = (error) => { 
    ToastAlert('error', 'System User', 'Unable to save, please try again or contact to system admin');
}