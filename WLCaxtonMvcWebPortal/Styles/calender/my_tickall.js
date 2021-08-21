$(document).ready(function () {
    CheckRadio();
    $('.innerbox').click(function () {

        $('.myradio').css('background-position', '-1px 0px');
        $(".like_red").removeAttr("checked");
        $(".like_red").removeClass('rpval');

        if (GetCheckedStatus($(this).find('.myradio .like_red input[type="radio"]').attr("id"))) {
            // alert('in RemooveChecked');

            $(this).find('.myradio').css('background-position', '-1px 0px');
            RemooveChecked($(this).find('.myradio .like_red input[type="radio"]').attr("id"));
            return false;
           
        }
        else {
            // $(".like_red").removeAttr("checked");
            // alert('in SetUniqueRadioButton');
            $(this).find('.myradio').css('background-position', '2px -29px');
            SetUniqueRadioButton($(this).find('.myradio .like_red input[type="radio"]').attr("id"));
            return false;
        }
    });

    $('.Grid_strip_white1').click(function () {
     
       var array = $(this).find('.myradio input[type="radio"]');
       $(this).find('.myradio').css('background-position', '2px -29px');
       $(this).find('.myradio').addClass("like_red");
            SetUniqueRadioButtons($(this).find('.myradio  input[type="radio"]').attr("Id"));
            return false;
        //}
    });

    $('.Grid_strip_white').click(function () {
        
        if (GetCheckedStatus($(this).find('.myradio .like_red input[type="radio"]').attr("Id")) == true) {
            $(this).find('.myradio').css('background-position', '-1px 0px');
            RemooveChecked($(this).find('.myradio .like_red input[type="radio"]').attr("Id"));
            return false;
        }
        else {
            $(this).find('.myradio').css('background-position', '2px -29px');
            SetUniqueRadioButtons($(this).find('.myradio .like_red input[type="radio"]').attr("Id"));
            return false;
        }
    });
});
function GetCheckedStatus(current) {
    var result = false;
    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i];
        if (elm.type == 'radio') {
            if (elm.id == current) {
                if (elm.checked == true)
                    result = elm.checked = true;
                else
                    result = elm.checked = false;
                break;
            }
            else {
                if (elm.checked == true)
                    result = elm.checked = false;
                else
                    result = elm.checked = true;
                break;
            }
        }
    }
    return result;
}
function RemooveChecked(current) {

    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i];
        if (elm.type == 'radio') {
            if (elm.id == current)
                elm.checked = false;
        }
    }
}
function SetUniqueRadioButton(current) {
    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i];
        if (elm.type == 'radio') {
            if (elm.id == current) {
                elm.checked = true;
                //$('#' + elm.id).parent().parent().css({ "background-position": "2px -29px" });
            }
            else {
                elm.checked = false;
                // $('#' + elm.id).parent().parent().css({ "background-position": "-1px 0px" });
            }
        
        }
    }
    // current.checked = true;
}
function CheckRadio() {
    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i];
        if (elm.type == 'radio') {
            if (elm.checked == true)
                $('#' + elm.id).parent().parent().css({ "background-position": "2px -29px" });
        }
    }
}



// multiple checkbox

function SetUniqueRadioButtons(current) {

    for (i = 0; i < document.forms[0].elements.length; i++) {
        elm = document.forms[0].elements[i];
        if (elm.type == 'radio') {
            if (elm.id == current)
                elm.checked = true;
            //                    else
            //                        elm.checked = false;
            //$('#' + elm.id).css({});
        }
    } // current.checked = true;
}



function SetAllCheckBox() {

    if ($('#hdnMC').val() == '0') {
        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i];
            if (elm.type == 'radio') {
                elm.checked = true;
                $(elm).parent('.myradio').css('background-position', '2px -29px');
            }
        }
        $('#hdnMC').val('1');
    }
    else {
        for (i = 0; i < document.forms[0].elements.length; i++) {
            elm = document.forms[0].elements[i];
            if (elm.type == 'radio') {
                elm.checked = false;
                $(elm).parent('.myradio').css('background-position', '-1px -0px');
            }
        }
        $('#hdnMC').val('0');
    }


}