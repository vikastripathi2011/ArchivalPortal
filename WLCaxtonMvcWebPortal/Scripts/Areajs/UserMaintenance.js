/// <reference path="../DataTables/ColReorder-1.4.1/js/dataTables.colReorder.js" />

$(document).ready(function () {
    //RowGroup extension
    specialCharacter();
    selectDropdown();

    $('.close').click(function () {
        $(this).parents('.alert').fadeOut();
    });

    var pageNo = 0;
    $.fn.dataTableExt.oApi.fnStandingRedraw = function (settings) {
        /* Note the use of a DataTables 'private' function thought the 'oApi' object */
        var before = settings._iDisplayStart;

        settings.oApi._fnReDraw(settings, true);
        settings._iDisplayStart = pageNo;
        settings.oApi._fnCalculateEnd(settings);
        settings.oApi._fnDraw(settings);

    }
    $('#tblUser thead').on('click', 'th', function () {

        table.fnStandingRedraw();

        return false;
    });
    $('#tblUser').on('page.dt', function () {

        var api = table.api();
        var pageInfo = api.page.info();
        pageNo = parseInt($('Select[name=tblUser_length]').val()) * (parseInt(pageInfo.page));


    });

    var table = $('.tblUser').dataTable({
        responsive: true,
        colReorder: true,
        "bStateSave": true,
        // "bJQueryUI": true,
        "lengthMenu": [[10, 20, 50], [10, 20, 50]],
        "pagingType": "full_numbers",
        // "sPaginationType": "full_numbers",
        "order": [[0, "asc"]],
        "columnDefs": [{
            "targets": [4, 5], //first column / numbering column
            "orderable": true, //set not orderable
         
        },
        { "targets": [6, 7], "bSortable": false, "searchable": false }
        ],
        "bDestroy": true
    });

    //  var allData = table.columns().data();
    $("#tblUser_wrapper").attr("style", "height: 400px !important");


    // validation at input level 



    $('#FirstName').on('input', function () {
        var input = $(this);
        
        var is_name = input.val();

        if (is_name) { OmitError('FirstName', 'lblFirstNameError'); }
        else { raiseError('FirstName', 'Please enter the user`s first name.', 'lblFirstNameError'); }

        if (!IsAlphaNumericWithSpace(is_name)) {
            raiseError('FirstName', 'The first name field contains invalid characters.', 'lblFirstNameError');
        }
        else { OmitError('FirstName', 'lblFirstNameError'); }
    });

    $('#RoleDetails_RecordId').on('change', function () {
        var input = $(this);
        var is_name = input.val();
        if (is_name) { OmitError('.nice-select', 'lblRolesError'); }
        else { raiseError('.nice-select', 'Please select an access level for the user.', 'lblRolesError'); }
    });
    $('#UserEmailId').on('input', function () {
        var input = $(this);
        var is_email = input.val();
        if (is_email) {
            var is_emailvalid = validateEmail(input.val());
            OmitError('UserEmailId', 'lblUserEmailIdError');
            if (is_emailvalid) { OmitError('UserEmailId', 'lblUserEmailIdError'); }
            else { raiseError('UserEmailId', 'The Username field contains invalid characters.', 'lblUserEmailIdError'); }
        }
        else {
            raiseError('UserEmailId', 'Please enter a Username.', 'lblUserEmailIdError');

        }

    });

    $('#LastName').on('input', function () {
        var input = $(this);
        var is_name = input.val();
        if (is_name) { OmitError('LastName', 'lblLastNameError'); }
        else { raiseError('LastName', 'Please enter the user`s last name.', 'lblLastNameError'); }

        if (!IsAlphaNumericWithSpace(is_name)) {
            raiseError('LastName', 'The last name field contains invalid characters.', 'lblLastNameError');
        }
        else { OmitError('LastName', 'lblLastNameError'); }
    });

});

$(document).ready(function () {
    try {
        $("input[type='text']").each(function () {
            $(this).prop("autocomplete", "off");
        });
    }
    catch (e)
    { }
});

function ClearAll() {
    $("#RoleDetails_RecordId").prop('selectedIndex', 0);
    $("#FirstName").val('');
    $("#UserEmailId").val('');
    $("#LastName").val('');
    $('select').niceSelect('update');
    try {
        $("input[type='text']").each(function () {
            $(this).removeClass("simple-form__form-group--inputError");
            $('label[id*="Error"]').text('');
        });
        $(".nice-select").each(function () {
            $(this).removeClass("simple-form__form-group--inputError");
        });
        $('.alert').hide();
    }
    catch (e)
    { }

}


//Purpose:View User Registration
//Author: VRT:8-03-2018
function fn_ViewUserRegistration(_this) {
    $('.loaderBg').show();
    $('.dataTables_filter').show();
    UseId = { UserId: _this };
    $.ajax({
        type: "POST",
        data: JSON.stringify(UseId),
        url: rootDir + "UserMaintenance/Data/RegisterUser",
        // url: rootDir + "UserMaintenance/UserMntnanc/CreateUser",

        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        async: true,
        cache: false,
        success: function (resData) {
            $('#dvUserDetails').html('');
            $('#dvUserDetails').html(resData);
            $('#btnSaveUser').val('Update');
            $('#dvMainRegis').removeClass("middle-part__tab");
            $('.middle-part__tab--head-with-password-icon').html("Edit User Detail");
            $('.loaderBg').hide();//tempraury Called 
              
            $("#dvUserPop").dialog({
                width: 750,
                modal: true,
                title: "Edit User Detail",
                buttons: [
                      {
                          text: "Update",
                          text: "Back",
                          "class": 'popBtn',
                          click: function () {
                              $(this).dialog("close");
                              $('#dvUserDetails').html('');
                          }
                      }, {
                          text: "Update",
                          "class": 'popBtn',
                          click: function () {
                              fn_UserRegistration();

                              //bind Grid again to fetch updated record is here.
                          }
                      },
                      {
                          text: "Clear",
                          "class": 'popBtn',
                          click: function () {
                              ClearAll();
                          }
                      }
                ]
            });
            $('.loaderBg').hide();
            $('#btnSaveUser').hide();
            $('select').removeClass('form-control input-sm');
            $('select').addClass('selectpicker');
            $('.dataTables_filter').show();

        },
        failure: function (data) {
            $('.loaderBg').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alertify.alert('<b>Error:</b> \n<br> ' + errorThrown);
        },
        complete: function (xhr, status) {
            $('.loaderBg').hide();
          
        }
    });

  
}

//Purpose:Create User Registration
//Author: VRT:8-03-2018
function fn_UserRegistration() {

    if (formValidation()) {
        $('.loaderBg').show();
        Model = {
            RoleDetails: { RecordId: $("#RoleDetails_RecordId").val() },
            FirstName: $("#FirstName").val(),
            LastName: $("#LastName").val(),
            UserEmailId: $("#UserEmailId").val(),
            UserId: $("#hdnUserId").val()
        };
        $.ajax({
            type: "POST",
            url: rootDir + 'UserMaintenance/Data/CRUD_User/',
            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            //async: true,
            success: function (data, status) {
                var result = data;
                debugger;
                if (status == "success" && result.ResultMsg != null && (result.MessageCode == "M00060" || result.MessageCode == "M00007")) {

                    alertify.alert('<b>Success:</b> \n<br> ' + result.ResultMsg, function (e) {
                        $(location).attr('href', rootDir + "UserMaintenance/UserMntnanc/UserListing/");
                    });

                }
                else {
                    if (result.MessageCode == "M00080") {
                        $('.valid').parents('.alert').fadeOut();
                        $('.infoValid').parents('.alert').fadeOut();
                        $('#sectionError').find('p').html(result.ResultMsg)
                        $('.unvalid').parents('.alert').fadeIn();
                        $('.loaderBg').hide();
                    }
                    else {
                        $('.valid').parents('.alert').fadeOut();
                        $('.unvalid').parents('.alert').fadeOut();
                        $('#sectionInfo').find('p').html(result.ResultMsg)
                        $('.infoValid').parents('.alert').fadeIn();
                        $('.loaderBg').hide();
                        // ClearAll();
                    }
                }

                $('.loaderBg').hide();
            },
            failure: function (data) {
                $('.loaderBg').hide();
            },
            error: function (data) {
                if (data.statusText != "OK") {
                    alertify.alert("<b>Error Message </b>\n <br>" + data.statusText);
                    ClearAll();
                    $('.loaderBg').hide();
                }
            }
        });
    }
}

//Purpose: User Change Status
//Author: VRT:20-03-2018
function fn_UserChangeStatus(_thisUid, _thisStatusId) {
    var errorMsg = "";
    $('.loaderBg').show();
    Model = {
        StatusId: _thisStatusId,
        UserId: _thisUid
    };
    $.ajax({
        type: "POST",
        url: rootDir + 'UserMaintenance/Data/ChangeStatus/',
        data: JSON.stringify(Model),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        //async: true,
        success: function (data, status) {
            var result = data;
            if (status == "success" && result.ResultMsg != null && result.MessageCode == "M00007") {
                alertify.alert('<b>Success:</b> \n<br> ' + result.ResultMsg, function (e) {
                    $(location).attr('href', rootDir + "UserMaintenance/UserMntnanc/UserListing/");
                });
            }
            else {
                if (result.MessageCode == "M00080") {
                    $('.infoValid').parents('.alert').fadeOut();
                    $('.valid').parents('.alert').fadeOut();
                    $('#sectionError').find('p').html(result.ResultMsg)
                    $('.unvalid').parents('.alert').fadeIn();
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
                $('.loaderBg').hide();
                ClearAll();
                $('.loaderBg').hide();
            }
        }
    });
}


//Purpose: User Unlock Account Status
//Author: VRT:20-03-2018
function fn_UserUnlockAcc(_thisUid) {
    var errorMsg = "";
    $('.loaderBg').show();
    Model = {
        UserId: _thisUid
    };
    $.ajax({
        type: "POST",
        url: rootDir + 'UserMaintenance/Data/UnlockAcc/',
        data: JSON.stringify(Model),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        //async: true,
        success: function (data, status) {
            var result = data;
            if (status == "success" && result.ResultMsg != null && result.MessageCode == "M00012") {
                alertify.alert('<b>Success:</b> \n<br> ' + result.ResultMsg, function (e) {
                    $(location).attr('href', rootDir + "UserMaintenance/UserMntnanc/UserListing/");
                });
            }
            else {

                if (result.MessageCode == "M00080") {
                    $('.infoValid').parents('.alert').fadeOut();
                    $('.valid').parents('.alert').fadeOut();
                    $('#sectionError').find('p').html(result.ResultMsg)
                    $('.unvalid').parents('.alert').fadeIn();
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
                $('.loaderBg').hide();
                ClearAll();
                $('.loaderBg').hide();
            }
        }
    });
}


//Purpose: User Unlock Account Status
//Author: VRT:20-03-2018
function fn_ResetPassword(_thisUid, _thisEmailId) {
    var errorMsg = "";
    $('.loaderBg').show();
    Model = {
        UserId: _thisUid,
        UserEmailId: _thisEmailId
    };
    $('.alert').hide();
    $.ajax({
        type: "POST",
        url: rootDir + 'UserMaintenance/Data/ResetPassword/',
        data: JSON.stringify(Model),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        //async: true,
        success: function (data, status) {
           
            var result = data;
            if (status == "success" && result.ResultMsg != null && result.MessageCode == "M00005") {
                ClearAll();
                $('#sectionSuccess,.alert-success').show();
                $('#sectionSuccess').find('p').html(result.ResultMsg)
                
                $('.loaderBg').hide();
               

            }
            else {
                if (result.MessageCode == "M00080") {
                    $('#sectionError,.alert-danger').show();
                    
                    $('#sectionError').find('p').html(result.ResultMsg)
                   
                    $('.loaderBg').hide();
                }
                else {
                   
                    $('#sectionInfo,.alert-warning').show();
                    $('#sectionInfo').find('p').html(result.ResultMsg)
                    
                    $('.loaderBg').hide();
                }
            }
            $('.loaderBg').hide();
        },
        failure: function (data) {
            $('.loaderBg').hide();
        },
        error: function (data) {
            if (data.statusText != "OK") {
                ClearAll();
                $('#sectionError,.alert-danger').show();
                $('#sectionError').find('p').html(data.statusText)
                $('.loaderBg').hide();
            }
        }
    });
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
//email validation for 
function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/
    if (filter.test(sEmail)) {
        return true;
    }
    else {
        return false;
    }
}
function IsAlphaNumericWithSpace(oVal) {
    var filter = /^[a-zA-Z0-9\s]+$/
    if (filter.test(oVal)) {
        return true;
    }
    else {
        return false;
    }
}


function formValidation() {
    var errorMsg = true;
    var is_name = $('#FirstName').val().trim();
   
    if ($('#FirstName').val().trim() == '') {
        raiseError('FirstName', 'Please enter the user`s first name.', 'lblFirstNameError');
        errorMsg = false;
    }
    else {
        OmitError('FirstName', 'lblFirstNameError');
        if (!IsAlphaNumericWithSpace(is_name)) {
            raiseError('FirstName', 'The first name field contains invalid characters.', 'lblFirstNameError');
        }
        else { OmitError('FirstName', 'lblFirstNameError'); }
    }
   

    if ($('#RoleDetails_RecordId').val() == '') {
        raiseError('.nice-select', 'Please select an access level for the user.', 'lblRolesError');
        errorMsg = false;
    }
    else {
        OmitError('.nice-select', 'lblRolesError');
    }

    if ($('#UserEmailId').val().trim() == '') {
        raiseError('UserEmailId', 'Please enter a Username.', 'lblUserEmailIdError');
        errorMsg = false;
    }
    else {
        if (!validateEmail($('#UserEmailId').val().trim())) {
            raiseError('UserEmailId', 'Please enter a valid Username.', 'lblUserEmailIdError');
            errorMsg = false;
        }
        else {
            OmitError('UserEmailId', 'lblUserEmailIdError');
        }
    }

    if ($('#LastName').val().trim() == '') {
        raiseError('LastName', 'Please enter the user`s last name.', 'lblLastNameError');
        errorMsg = false;
    }
    else {
        OmitError('LastName', 'lblLastNameError');
        if (!IsAlphaNumericWithSpace($('#LastName').val().trim())) {
            raiseError('LastName', 'The last name field contains invalid characters.', 'lblLastNameError');
            errorMsg = false;
        }
        else { OmitError('LastName', 'lblLastNameError'); }
    }

   

    return errorMsg;
}







