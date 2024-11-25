
Issue
PM> Add-Migration SeedingData001
Both Entity Framework 6 and Entity Framework Core are installed. The Entity Framework 6 tools are running. Use 'EntityFrameworkCore\Add-Migration' for Entity Framework Core.
Your target project 'Data' doesn't reference EntityFramework. This package is required for the Entity Framework Core Tools to work. Ensure your target project is correct, install the package, and try again.

Solution
PM>EntityFrameworkCore\Add-Migration "AnyName"


If you set Cookie timeout at Admin site
1. below TimeSpan.FromMinutes(1) is set to 1 minute
2. if you browse admin site after 1 minute, you will be automatically redirected to login page
3. if you browse before 1 minute your time will be increase 1 minute
4. you can see the browser-->Application-->Cookies-->Expires/Max-age time, 
   the time will be update automatically + 1 minute
5. you do not have to set expiry time anywhere in the application for cookie in Admin project
6. CookieAuthenticationDefaults.AuthenticationScheme name is replaced with 
   Constants.Cookie.AuthenticationScheme
==================================================================================================
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;

        //if you did not specify LOGOUT page, app will look this page inside "AccountController"
        // options.LogoutPath = "/Account/Logout";

        //if you did not specify AccessDenied page, app will look this page inside "AccountController"
        // options.AccessDeniedPath= "/Account/AccessDenied"; ///{ReturnUrl}

        //if you did not specify login page, app will look this page inside "AccountController"
        // options.LoginPath= "/Account/LogIn";


        options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
        options.Cookie.MaxAge = options.ExpireTimeSpan; // optional
        options.SlidingExpiration = true;
    });

    

  NOTE
  ---------------------------------------------------
  Admin Route should be set as below, whenever admin panel is open, 
  it will open home page, if user is logged-in, the home\index page will open
  if user authentication token is expired it will redirect to "Account/SessionExpired" view
  "Account/SessionExpired" view have a javascript which will call auto-login method of "Account" controller to
  check "RefreshToken" is exists, if yes it will try to login using "refreshToken", if its expired-->redirect to login page
   
    
    endpointRouteBuilder.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}",
        defaults: new { controller = "Home", action = "Index" });

         
    =================================================
    jQuery validation: change default error message
    
The complete API for validate(...) : http://jqueryvalidation.org/validate
    =================================================
Add this code in a separate file/script included after the validation plugin to override the messages, edit at will :)
///HTML form input validation extension
jQuery.extend(jQuery.validator.messages, {
    required: "This field is required.",
    remote: "Please fix this field.",
    email: "Please enter a valid email address.",
    url: "Please enter a valid URL.",
    date: "Please enter a valid date.",
    dateISO: "Please enter a valid date (ISO).",
    number: "Please enter a valid number.",
    digits: "Please enter only digits.",
    creditcard: "Please enter a valid credit card number.",
    equalTo: "Please enter the same value again.",
    accept: "Please enter a value with a valid extension.",
    maxlength: jQuery.validator.format("Please enter no more than {0} characters."),
    minlength: jQuery.validator.format("Please enter at least {0} characters."),
    rangelength: jQuery.validator.format("Please enter a value between {0} and {1} characters long."),
    range: jQuery.validator.format("Please enter a value between {0} and {1}."),
    max: jQuery.validator.format("Please enter a value less than or equal to {0}."),
    min: jQuery.validator.format("Please enter a value greater than or equal to {0}.")
});

===========================================================================================
<form id="myform">
    <input type="text" name="firstname.fieldOne" /><br/>
    <br/>
    <input type="submit" />
</form>
<script>
$(document).ready(function () {

    $.validator.addMethod("pwcheckspechars", function (value) {
        
        return /[!@#$%^&*()_=\[\]{};':"\\|,.<>\/?+-]/.test(value)
    }, "The password must contain at least one special character");
    
	$.validator.addMethod("pwcheckconsecchars", function (value) {
        return ! (/(.)\1\1/.test(value)) // does not contain 3 consecutive identical chars
    }, "The password must not contain 3 consecutive identical characters");

    $.validator.addMethod("pwchecklowercase", function (value) {
        return /[a-z]/.test(value) // has a lowercase letter
    }, "The password must contain at least one lowercase letter");
    
    $.validator.addMethod("pwcheckrepeatnum", function (value) {
        return /\d{2}/.test(value) // has a lowercase letter
    }, "The password must contain at least one lowercase letter");
    
    $.validator.addMethod("pwcheckuppercase", function (value) {
        return /[A-Z]/.test(value) // has an uppercase letter
    }, "The password must contain at least one uppercase letter");
    
    $.validator.addMethod("pwchecknumber", function (value) {
        return /\d/.test(value) // has a digit
    }, "The password must contain at least one number");
    
    
    
    
    
    $('#myform').validate({
        // other options,
        rules: {
            "firstname.fieldOne": {
                required: true,
                pwchecklowercase: true,
                pwcheckuppercase: true,
                pwchecknumber: true,
                pwcheckconsecchars: true,
                pwcheckspechars: true,
                minlength: 8,
                maxlength: 20
            }
        }
    });

});
</script>
===========================================================================================








Use the following Regex to satisfy the below conditions:

Conditions:

Min 1 uppercase letter.
Min 1 lowercase letter.
Min 1 special character.
Min 1 number.
Min 8 characters.
Max 30 characters.
Regex:

/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$@!%&*?])[A-Za-z\d#$@!%&*?]{8,30}$/
===========================================================================================

https://www.regextester.com/110035

https://www.w3resource.com/javascript/form/decimal-numbers.php

 You can expand your solution with translated Message from your resource bundle

<script type="text/javascript">
    $.validator.messages.number = '@Html.Raw(@Resources.General.ErrorMessageNotANumber)';
</script>
============================================================================================
var validator = $("#signupform").validate({
    rules: {
        firstname: "required",
        lastname: "required",
        username: {
            required: true,
            minlength: 2,
            remote: "users.php"
        }
    },
    messages: {
        firstname: "Enter your firstname",
        lastname: "Enter your lastname",
        username: {
            required: "Enter a username",
            minlength: jQuery.format("Enter at least {0} characters"),
            remote: jQuery.format("{0} is already in use")
        }
    }
});

// Change default JQuery validation Messages.
$("#addnewcadidateform").validate({
        rules: {
            firstname: "required",
            lastname: "required",
            email: "required email",
        },
        messages: {
            firstname: "Enter your First Name",
            lastname: "Enter your Last Name",
            email: {
                required: "Enter your Email",
                email: "Please enter a valid email address.",
            }
        }
    })



Since we're already using JQuery, we can let page designers add custom messages to the markup rather than the code:

<input ... data-msg-required="my message" ...
Or, a more general solution using a single short data-msg attribute on all fields:

<form id="form1">
    <input type="text" id="firstName" name="firstName" 
        data-msg="Please enter your first name" />
    <br />
    <input type="text" id="lastName" name="lastName" 
        data-msg="Please enter your last name" />
    <br />
    <input type="submit" />
</form>
And the code then contains something like this:

function getMsg(selector) {
    return $(selector).attr('data-msg');
}

$('#form1').validate({
    // ...
    messages: {
        firstName: getMsg('#firstName'),
        lastName: getMsg('#lastName')
    }
    // ...
});

    List of rules
required — a field should be filled obligatory (true or false);
maxlength — maximum number of characters (a number);
minlength — minimum number of characters (a number);
email — verifying of the email address correctness (true or false);
url — verifying of the url address correctness (true or false);
remote — specifying a file for checking the field (for example: check.php);
date — verifying of the date correctness (true or false);
dateISO — verifying of the ISO date correctness (true or false);
number — the number verifying (true or false);
digits — only numbers (true or false);
creditcard — a credit card number correctness (true or false);
equalTo — equal to something (for example, to another field equalTo:»#pswd»);
accept — verifying of correct extension (accept: «xls|csv»);
rangelength — range of character lengths, minimum and maximum (rangelength: [2, 6]);
range — a number should be within the range from to (range: [13, 23]);
max — maximum number (22);
min — minimum number (11).

C# DateTime Format
-----------------
https://www.c-sharpcorner.com/blogs/date-and-time-format-in-c-sharp-programming1

WebP.Net 
-----------------------
https://github.com/frankodoom/libwebp.net

