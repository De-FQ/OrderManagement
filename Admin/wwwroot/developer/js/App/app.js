$(function () { // Handler for .ready() called. 
    document.title = $(".page-header").find("h2").html();
    //date format initialization
    $.extend(theme.PluginDatePicker.defaults, {
        format: "dd/mm/yyyy"
    });

    //initialized

    if ($('.summernote').length > 0) {

        $('.summernote').summernote({
            height: "180",
            disableDragAndDrop: true,
            styleTags: ['p', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6'],
            toolbar: [
                ['para', ['style']],
                ['style', ['bold', 'italic', 'underline']],
                //['insert', ['link']],
                ['table', ['table']],
                ['para', ['ul', 'ol']]
            ],
            callbacks: {
                onChange: function (contents, $editable) {
                    // Note that at this point, the value of the `textarea` is not the same as the one
                    // you entered into the summernote editor, so you have to set it yourself to make
                    // the validation consistent and in sync with the value.
                    $(this).val($(this).summernote('isEmpty') ? "" : contents);

                    // You should re-validate your element after change, because the plugin will have
                    // no way to know that the value of your `textarea` has been changed if the change
                    // was done programmatically.
                    $(this).closest("form").validate().element($(this));
                }
            }

        });
        $('.summernote[disabled]').summernote('disable');
        // Sanitization being done OnPaste
        $(".summernote").on("summernote.paste", function (e, ne) {
            var bufferText = ((ne.originalEvent || ne).clipboardData || window.clipboardData).getData('Text');
            ne.preventDefault();
            document.execCommand('insertText', false, bufferText);
            $(ne.currentTarget).find("*").removeAttributes();
        });
        // Sanitization being done OnChange
        $('.summernote').on('summernote.change', function (we, contents, $editable) {
            $editable.find("*").removeAttributes();
        });

        // Helper Function to Remove Attributes
        jQuery.fn.removeAttributes = function () {
            return this.each(function () {
                var attributes = $.map(this.attributes, function (item) {
                    return item.name;
                });
                var obj = $(this);
                $.each(attributes, function (i, item) {
                    obj.removeAttr(item);
                });
            });
        }

        sanitizeSummernoteAndGetContent = (summernoteID) => {
            // Sanitizing of All Attributes
            $("#" + summernoteID + " + .note-editor .note-editable *").removeAttributes();
            // Returning Clean Code
            return $("#" + summernoteID).summernote("code");
        }

        
    }

    $(".clear-datepicker").click(function () {
        $(this).siblings("[data-plugin-datepicker],.input-group").find("input").val("");
    });

    jQuery.validator.addMethod("phoneStartCheck", function (value) {
        if (value.startsWith("0") || value.startsWith("3") || value.startsWith("7")) {
            return false;
        }
        return true;
    });

    jQuery.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
    });

    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param)
    });

    jQuery.validator.addMethod("select2Required", function (value) {
        if (value == null || value == undefined || value == "" || value == "0") {
            return false;
        }
        return true;
    });

    jQuery.validator.addMethod("stringLength200", function (value) {
        if (value.length > 200) {
            return false;
        }
        return true;
    });

    jQuery.validator.addMethod("stringLength500", function (value) {
        if (value.length > 500) {
            return false;
        }
        return true;
    });

    jQuery.validator.addMethod("customEmail", function (value, element, param) {
        var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
        return emailReg.test(value);
    });

    jQuery.validator.addMethod('summerNoteRequired', function (value, element) {
        if ($(element).summernote('isEmpty')) {
            return false;
        }
        else {
            return true;
        }
    });

    jQuery.validator.addMethod("validateDimension", function (value, element) {
        // Get the uploaded file object using the FormData API
        var file = element.files[0];

        // Check if the file upload is an image
        if (!/(\.|\/)(gif|jpe?g|png)$/i.test(file.type)) {
            return false;
        }

        // Get the minimum width and height from the data attributes
        var min_height_required = parseInt($(element).data('required-height'));
        var min_width_required = parseInt($(element).data('required-width'));

        // Create a new image object to get the actual dimensions
        var img = new Image();
        img.src = URL.createObjectURL(file);

        //Check the image dimensions once it has loaded
        new Promise((resolve, reject) => {
            img.onload = function () {
                // Check if the image is at least the minimum width and height
                if (img.width < min_width_required || img.height < min_height_required) {
                    console.log("Bad");
                    resolve(false);
                }

                // Check if the aspect ratio matches
                var aspect_ratio = img.width / img.height;
                var required_ratio = min_width_required / min_height_required;
                if (aspect_ratio !== required_ratio) {
                    console.log("Bad");
                    resolve(false);
                }

                if (aspect_ratio == required_ratio && (img.width >= min_width_required || img.height >= min_height_required)) {
                    console.log("All Good!");
                    resolve(true);
                }

            };

            img.onerror = function () {
                reject("Error loading image");
            };
        }).then(function (result) {
            return result;
        });


    });

    $.each(document.location.search.substr(1).split('&'), function (c, q) {
        var i = q.split('=');
        if (i.length == 2) {
            queryStringValues[i[0].toString()] = i[1].toString();
        }
    });

    
});

showDateTime =()=> {
    var DivDateTime = document.getElementById("DivDateTime");
    if (DivDateTime == null) return;
    var date = new Date();
    var dayList = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    var monthNames = [ "January","February","March","April","May","June", "July","August","September","October","November","December" ];
    var dayName = dayList[date.getDay()];
    var monthName = monthNames[date.getMonth()];
    var today = `${dayName}, ${monthName} ${date.getDate()}, ${date.getFullYear()}`;
    var session = "AM";

    var hour = date.getHours();
    var min = date.getMinutes();
    var sec = date.getSeconds();
    if (hour == 0) {
        hour = 12;
    }

    if (hour > 12) {
        hour = hour - 12;
        session = "PM";
    }

    hour = (hour < 10) ? "0" + hour : hour;
    min = (min < 10) ? "0" + min : min;
    sec = (sec < 10) ? "0" + sec : sec;

    var time = hour + ":" + min + ":" + sec + " " + session;
    DivDateTime.innerText = `${today} : ${time}`;
}

getLang = () => {
    var lang = $("html").attr("lang"); // '@requestCultureFeature'
    return lang;
}

EnableImageCompressionHook = () => {
    $(".enable-compression").on("change", function (e) {
        var file = e.target.files[0];
        var originalSize = 0;
        var newSize = 0;
        var _URL = window.URL || window.webkitURL;
        if (!file) {
            return;
        }
        if (file) {
            var min_height_required = parseInt($(this).attr('data-required-height'));
            var min_width_required = parseInt($(this).attr('data-required-width'));
            var img = new Image();
            img.onload = function () { 
                if (this.height >= min_height_required && this.width >= min_width_required && min_height_required / min_width_required == this.height / this.width) {
                    console.log("All Good!");
                } else {
                    console.log("Invalid File res");
                }
            };
            img.onerror = function () {
                console.log("not a valid file: " + file.type);
            };
            img.src = _URL.createObjectURL(file);
        }
        originalSize = file.size;
        new Compressor(file, {
            quality: 0.6,
            // The compression process is asynchronous,
            // which means you have to access the `result` in the `success` hook function.
            success: function success(result) {
                var formData = new FormData();

                // The third parameter is required for server
                formData.append('file', result, result.name);

                // This is new Image File
                var outputFile = new File([result], result.name);
                newSize = outputFile.size;
                
                // For Displaying it in Preview
                var image = $(e.target).closest(".row").find(".preview-image");
                image.attr("src", URL.createObjectURL(outputFile));

                var reader = new FileReader();
                reader.readAsDataURL(result);
                reader.onloadend = function () {
                    var base64data = reader.result;
                    image.attr("data-base64", base64data);
                }

                // Compare Sizes for Demo
                var msg = `Compare Sizes for Demo\n****************
                      \nOriginal File Size:    ${originalSize} 
                      \nNew File Size: ${newSize}
                      \nSaved Size Percentage:  ${(newSize / originalSize * 100 - 100)} % 
                      \n****************`;
                //showLog(msg);
                // Temporary Download for Demo
                // var url  = URL.createObjectURL(result);
                // var newWindow = window.open(url, "popupWindow", "width=600,height=600,scrollbars=yes");

                
            },
            error: function error(err) {
                console.log(err.message);
            }
        });
    });
}

//convert dataUrl to File for backend
function dataURLtoFile(previewImageClassName, filename) {
    var base64data = $(previewImageClassName).attr("data-base64");
    var arr = base64data.split(','),
        mime = arr[0].match(/:(.*?);/)[1],
        bstr = atob(arr[arr.length - 1]),
        n = bstr.length,
        u8arr = new Uint8Array(n);
    while (n--) {
        u8arr[n] = bstr.charCodeAt(n);
    }
    return new File([u8arr], filename, { type: mime });
}