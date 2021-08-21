/// <reference path="../jquery.validate.js" />
/// <reference path="../jquery.validate.min.js" />
/// <reference path="Common2.js" />


$(document).ready(function () {
    $("#txtUserName").focus();
    $("input[type='text']").attr("autocomplete", "off");
    try {
        $("input[type='text']").each(function () {
            $(this).prop("autocomplete", "off");
        });
    }
    catch (e)
    { }
});

//validate email address
function ValidateEmail(test) {
    var reg = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

    if (test.match(reg)) {
        return true;
    }
    else {

        return false;
    }
}
//...Login Action
$(document).on('click', "#btnLogin", function () {
    $.post(rootDir + "Login/RedirectToLogin", function (data, status) {
        if (status != undefined && status == 'success') {
            $(location).attr('href', rootDir + "Login");
        }
    });
});

$(document).on('click', '#aSaveLogin', function (event) {
    
    if ($.support.msie && parseInt($.support.version, 10) === 8 || $.support.msie && parseInt($.support.version, 10) === 7) {
        //if ($.browser.msie && parseInt($.browser.version, 10) === 8 || $.browser.msie && parseInt($.browser.version, 10) === 7) {
        $.post(rootDir + "/Login/LogOff", function () {
            alertify.alert("<b>Warnning...! <br> <br></b>" + "We do not support to lower than IE9 browser. Please click <a href=\"http://www.microsoft.com/windows/downloads/ie/getitnow.mspx\"><b><i><span style='color:blue; '>here</span></i></b></a> to upgrade your current browser.");
            //alertify.alert(GetMessageByKey(allMessages, ['WarningHeaderMessage', 'BrowserNotSupportMessage']));
        })

    }
    else {


        var userName = $("#txtUserName").val().trim();
        if (AlphaNumericOnly(userName)) {
            $("#txtUserName").addClass('form-controlError');
            $('#lblLoginError').html('');
            $('#lblLoginError').html('Special charector are not allow.');
            $("#txtUserName").focus();
            return false;
        }
        if (userName == "") {
            $("#txtUserName").addClass('form-controlError');
            $('#lblLoginError').html('');
            $('#lblLoginError').html('Please Enter The User ID.');
            $("#txtUserName").focus();
            return false;
        }
        if ($("#txtPassword").val().trim() == "") {
            $("#txtPassword").addClass('form-controlError');
            $('#lblPassError').html('');
            $('#lblPassError').html('Please Enter The Password.');
            $("#txtPassword").focus();
            return false;
        }
        if (!ValidateEmail(userName)) {
            $("#txtUserName").addClass('form-controlError');
            $('#lblLoginError').html('');
            $('#lblLoginError').html('Please enter a valid Username.');
            $("#txtUserName").focus();
            return false;
        }
        else {
            $('.loaderBg').show();
            var Model = {
                EmailId: $("#txtUserName").val().trim(),
                Password: $("#txtPassword").val().trim()
            };
            $.ajax({
                type: "POST",
                url: rootDir + 'Login/IsUserLogin/',
                data: JSON.stringify(Model),
                contentType: "application/json; charset=utf-8",
                dataType: "html",
                async: true,
                success: function (data, status) {
                    var result = JSON.parse(data);
                    if (status == "success" && data === '3' && result.errorMsg == undefined) {
                         $(location).attr('href', rootDir + "Login/Home?PageName=Home");
                       // $(location).attr('href', rootDir + "UserMaintenance/UserMntnanc/UserListing?PageName=UserListing");

                    }
                    else {

                        if (result != undefined && result.errorCode == '-2') {
                            $('#lblGlobalErrMsg').html('');
                            $('#lblGlobalErrMsg').html(result.errorMsg);
                            ClearAll();
                        }
                        else if (result != undefined && result == '0' || result == '3') {
                            $(location).attr('href', rootDir + "UserMaintenance/UserMntnanc/UserListing?PageName=UserListing");
                        }
                        else if (result != undefined && result === '1' || result.SearchCode == '3') {
                            $(location).attr('href', rootDir + "UserActvity/UserActvity/Search?PageName=Search");
                        }
                        else if (result != undefined && result === true) {
                            $(location).attr('href', rootDir + "UserMaintenance/UserMntnanc/ChangePassword?PageName=ChangePassword");
                        }
                        else {
                            $('#lblGlobalErrMsg').html('');
                            $('#lblGlobalErrMsg').html(result.errorMsg);
                        }
                    }
                    $('.loaderBg').hide();
                    $("#txtUserName").focus();
                },
                failure: function (data) {
                    $('.loaderBg').hide();
                },
                error: function (data) {
                    if (data.statusText != "OK") {
                        $('#lblGlobalErrMsg').html('');
                        $('#lblGlobalErrMsg').html('Please enter valid login credentials.');
                        ClearAll();
                        $('.loaderBg').hide();
                        $("#txtUserName").focus();
                    }
                }
            });
        }
    }

});

//Purpose:Change Password 
//Author: VRT:18-01-2018

$(document).on('click', '#btnChangePwd', function (event) {

    if (ChangePWDformValidation()) {
        $('.loaderBg').show();
        var Model = {
            EmailId: $("#hdnChkUser").val().trim(),
            Password: $("#txtOldPassword").val().trim(),
            NewPassword: $("#txtINewPasswordNew").val().trim()
        };
        $.ajax({
            type: "POST",
            url: rootDir + 'UserMaintenance/Data/ChangePassword/',
            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,
            success: function (data, status) {
                
                var result = data;
                if (status == "success" && result.ResultMsg === true) {
                    $('.infoValid').parents('.alert').fadeOut();
                    $('.unvalid').parents('.alert').fadeOut();
                    $('#sectionSuccess').find('p').html('Password changed successfully, please login again.');
                    setTimeout($(location).attr('href', rootDir + "Login?msg=1"), 2000);
                }
                else {

                    if (result != undefined && result.errorCode == '-2') {
                        
                        if (result.MessageCode == 'M00014') {
                            $('.infoValid').parents('.alert').fadeOut();
                            $('.unvalid').parents('.alert').fadeOut();
                            $('#sectionSuccess').find('p').html(result.errorMsg)
                            $('.valid').parents('.alert').fadeIn();
                            $('.loaderBg').hide();
                        }
                        else {
                            $('.valid').parents('.alert').fadeOut();
                            $('.unvalid').parents('.alert').fadeOut();
                            $('#sectionInfo').find('p').html(result.errorMsg)
                            $('.infoValid').parents('.alert').fadeIn();
                            $('.loaderBg').hide();
                        }
                        //alertify.alert("<b>Login Message </b>\n <br>" + result.errorMsg);

                        ClearAll();
                    }
                }
                $('.loaderBg').hide();
            },
            failure: function (data) {
                $('.loaderBg').hide();
            },
            error: function (data) {
                if (data.statusText != "OK") {
                    $('.infoValid').parents('.alert').fadeOut();
                    $('.valid').parents('.alert').fadeOut();
                    $('#sectionError').find('p').html(data.statusText)
                    $('.unvalid').parents('.alert').fadeIn();
                    ClearAll();
                    $('.loaderBg').hide();
                }
            }
        });
    }
});


//Purpose:Forgot Password 
//Author: VRT:18-01-2018

$(document).on('click', '#btnForgotPwd', function (event) {
    //var errorMsg = "";
    var EmailID = $("#txtForgotUserName").val().trim();
    if (EmailID == "") {
        //errorMsg = " - Please Enter Username.\n <br>"
        $("#txtForgotUserName").addClass('simple-form__form-group--inputError');
        $('#lblForgotUserNameError').html('');
        $('#lblForgotUserNameError').html('Please enter the Username.');
        $("#txtForgotUserName").focus();
        return false;
    }
    if (!ValidateEmail(EmailID)) {
        $("#txtForgotUserName").addClass('simple-form__form-group--inputError');
        $('#lblForgotUserNameError').html('');
        $('#lblForgotUserNameError').html('Please enter a valid Username.');
        $("#txtForgotUserName").focus();
        return false;
    }
    else {
        
        $('.loaderBg').show();
        var Model = {
            EmailId: $("#txtForgotUserName").val().trim()
        };
        $.ajax({
            type: "POST",
            url: rootDir + 'Login/ForgotPassword/',
            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            async: true,

            success: function (data, status) {
                debugger;
                var result = data;

                if (status == "success" && result.errorCode != '-2' && result != undefined) {
                    if (result.MessageCode == 'M00029') {
                        $('.infoValid').parents('.alert').fadeOut();
                        $('.unvalid').parents('.alert').fadeOut();
                        $('#sectionSuccess').find('p').html(result.ResultMsg)
                        $('.valid').parents('.alert').fadeIn();
                        $('.loaderBg').hide();
                    }
                    else {
                        $('.valid').parents('.alert').fadeOut();
                        $('.unvalid').parents('.alert').fadeOut();
                        $('#sectionInfo').find('p').html(result.ResultMsg)
                        $('.infoValid').parents('.alert').fadeIn();
                        $('.loaderBg').hide();
                    }
                }
                else {
                    if (result != undefined && result.errorCode == '-2') {
                        $('.infoValid').parents('.alert').fadeOut();
                        $('.valid').parents('.alert').fadeOut();
                        $('#sectionError').find('p').html(result.ResultMsg)
                        $('.unvalid').parents('.alert').fadeIn();
                        $('.loaderBg').hide();
                    }
                }

            },
            failure: function (data) {
                $('.loaderBg').hide();
            },
            error: function (data) {
                if (data.statusText != "OK") {
                    $('.infoValid').parents('.alert').fadeOut();
                    $('.valid').parents('.alert').fadeOut();
                    $('#sectionError').find('p').html(data.statusText)
                    $('.unvalid').parents('.alert').fadeIn();
                    $('.loaderBg').hide();
                }
            }
        });
        ClearAll();
    }
});

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}


//Write the following script for set the default enter key:
$(document).ready(function () {
    var url = getUrlVars();
    if (url.PageName != 'Change_Password') {
        $('.panel-group').html('');// hide left menu on change password page
    }


    $("input").bind("keydown", function (event) {
        var keycode = (event.keyCode ? event.keyCode : (event.which ? event.which : event.charCode));
        if (keycode == 13) {
            document.getElementById('aSaveLogin').click();
            return false;
        } else {
            return true;
        }
    });

    $('#txtForgotUserName').on('input', function () {
        var input = $(this);
        var is_name = input.val();
        if (is_name) { OmitError('txtForgotUserName', 'lblForgotUserNameError'); }
        else { raiseError('txtForgotUserName', 'Please enter the Username.', 'lblForgotUserNameError'); }
    });


    $("#txtUserName").focusout(function () {
        if ($(this).val() == "") {
            $("#txtUserName").addClass('form-controlError');
            $('#lblLoginError').html('');
            $('#lblLoginError').html('Please enter the Username.');
           // $("#txtUserName").focus();
            $("#txtPassword").removeClass('form-controlError');
            $('#lblPassError').html('');
            $('#lblGlobalError').html('');
        }
        else {
            $("#txtUserName").removeClass('form-controlError');
            $('#lblLoginError').html('');
            $('#lblGlobalErrMsg').html('');
        }

    });


    $("#txtPassword").focusout(function () {
        if ($(this).val() == "") {
            $("#txtPassword").addClass('form-controlError');
            $('#lblPassError').html('');
            $('#lblPassError').html('Please enter the Password.');
           // $("#txtPassword").focus();
            $("#txtUserName").removeClass('form-controlError');
            $('#lblLoginError').html('');
            $('#lblGlobalError').html('');
        }
        else {
            $("#txtPassword").removeClass('form-controlError');
            $('#lblPassError').html('');
            $('#lblGlobalErrMsg').html('');
        }
    });
    $('.close').click(function () {
        $(this).parents('.alert').fadeOut();
    });
});


function ClearAll() {
    $("#txtUserName").val('');
    $("#txtPassword").val('');

    $("#txtOldPassword").val('');
    $("#txtINewPasswordNew").val('');
    $("#txtConfirmPassword").val('');

    $("#txtForgotUserName").val('');

    try {
        $("input[type='text']").each(function () {
            $(this).removeClass("simple-form__form-group--inputError");
            $('label[id*="Error"]').text('');
           
        });
        
        $("input[type='password']").each(function () {
            $(this).removeClass("simple-form__form-group--inputError");
            $('label[id*="Error"]').text('');
        });
       // $('.alert').hide();
    }
    catch (e)
    { }

    // $('.loaderBg').hide();
}

function AlphaNumericOnly(event) {
    //Vrt:03/07/2018
    var code = (event.keyCode ? event.keyCode : event.which);
    if ((code >= 65 && code <= 90) || (code == 9 || code == 46 || code == 8) || (code >= 48 && code <= 57) || (code >= 96 && code <= 123)) {
        return true;
    }
    return false;
}

// for RAISE validation error;
function raiseError(mainElement, ErrorMsg, errorElementID) {
    if (~mainElement.indexOf(".")) {
        $(mainElement).addClass('simple-form__form-group--inputError');
    }
    else {
        $('#' + mainElement).addClass('simple-form__form-group--inputError');
    }
    $('#' + errorElementID).html('');
    $('#' + errorElementID).html(ErrorMsg);
    // $('#' + mainElement).focus();
}
// for omit validation error;
function OmitError(mainElement, errorElementID) {
    if (~mainElement.indexOf(".")) {
        $(mainElement).removeClass('simple-form__form-group--inputError');
    }
    else {
        $('#' + mainElement).removeClass('simple-form__form-group--inputError');
    }
    $('#' + errorElementID).html('');
}

function validatepassword(password) {
    var filter = /^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])([a-zA-Z0-9!@#$%^&*]{8,})$/
    if (filter.test(password)) {
        return true;
    }
    else {
        return false;
    }
}

function ChangePWDformValidation() {
    var errorMsg = true;
    if ($("#txtOldPassword").val().trim() == "") {
        raiseError('txtOldPassword', 'Please enter your old password.', 'lblOldPwdError');
        errorMsg = false;
    }
    else {
        OmitError('txtOldPassword', 'lblOldPwdError');
    }
    if ($("#txtINewPasswordNew").val().trim() == "") {
        raiseError('txtINewPasswordNew', 'Please enter a new password.', 'lblNewPwdError');
        errorMsg = false;
    }
    else {
        OmitError('txtINewPasswordNew', 'lblNewPwdError');
    }
    if ($("#txtConfirmPassword").val().trim() == "") {
        raiseError('txtConfirmPassword', 'Please re-type your password.', 'lblConfirmPwdError');
        errorMsg = false;
    }
    else {
        OmitError('txtConfirmPassword', 'lblConfirmPwdError');
    }
    if ($("#txtINewPasswordNew").val().trim() != "" && $("#txtConfirmPassword").val().trim()!="")
    {
        if ($("#txtINewPasswordNew").val().trim() != $("#txtConfirmPassword").val().trim()) {
            raiseError('txtConfirmPassword', 'Passwords do not match.', 'lblConfirmPwdError');
            errorMsg = false;
        }
    }
    //checking  new password 
    if (errorMsg) {
        if (!validatepassword($('#txtINewPasswordNew').val().trim())) {
            raiseError('txtINewPasswordNew', 'Password must be at least 8 characters contain numbers, letters and special characters.', 'lblGlobalError');
            errorMsg = false;
        }
        else {
            OmitError('txtINewPasswordNew', 'lblGlobalError');
        }
    }
    //checking  old password 
    if (errorMsg) {
        if (!validatepassword($('#txtConfirmPassword').val().trim())) {
            raiseError('txtConfirmPassword', 'Password must be at least 8 characters contain numbers, upper & lower case and special characters.', 'lblGlobalError');
            errorMsg = false;
        }
        else {
            OmitError('txtConfirmPassword', 'lblGlobalError');
        }
    }

    //compare old and new pwd
    if (errorMsg) {
        if ($('#txtINewPasswordNew').val().trim() != $('#txtConfirmPassword').val().trim()) {
            raiseError('txtINewPasswordNew', 'Password and Confirm password are not same.', 'lblGlobalError');
            $('#txtConfirmPassword').addClass('simple-form__form-group--inputError');
            errorMsg = false;
        }
        else {
            OmitError('txtINewPasswordNew', 'lblGlobalError');
            $('#txtConfirmPassword').removeClass('simple-form__form-group--inputError');
        }
    }

    return errorMsg;
}