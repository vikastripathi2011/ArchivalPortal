
//Get short name of a month
//month = month number
function GetMonthName(month) {
    var m_names = [
        "January", "February", "March", "April",
        "May", "June", "July", "August",
        "September", "October", "November", "December"
    ];
    return m_names[month];
}

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

//allow only alphabates & Space
function ValidateAlphabets(event) {
    var code = (event.keyCode ? event.keyCode : event.which);
    if ((code >= 65 && code <= 90) || (code == 9 || code == 46 || code == 8 || code == 32)) {
        return true;
    }
    return false;
}

function NumericOnly(event) {
    //var code = (event.keyCode ? event.keyCode : event.which);
    //if ((code >= 48 && code <= 57) || (code >= 96 && code <= 105) || (code == 9 || code == 46 || code == 8 || code == 35 || code == 36 || code == 37 || code == 39 || code == 190)) {
    //    return true;
    //}
    //return false;


    event = (event) ? event : window.event;
    var charCode = (event.which) ? event.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

$(document).ready(function () {
    var keyDown = false, ctrl = 17, vKey = 86, Vkey = 118;

    $(document).keydown(function (e) {
        if (e.keyCode == ctrl) keyDown = true;
    }).keyup(function (e) {
        if (e.keyCode == ctrl) keyDown = false;
    });

    $('.paste-numbers-only').on('keypress', function (e) {
        if (!e) var e = window.event;
        if (e.keyCode > 0 && e.which == 0) return true;
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        var character = String.fromCharCode(code);
        if (character == '\b' || character == ' ' || character == '\t') return true;
        if (keyDown && (code == vKey || code == Vkey)) return (character);
        else return (/[0-9\.]$/.test(character));
    }).on('focusout', function (e) {
        var $this = $(this);
        $this.val($this.val().replace(/[^0-9\.]/g, ''));
    }).on('paste', function (e) {
        var $this = $(this);
        setTimeout(function () {
            $this.val($this.val().replace(/[^0-9\.]/g, ''));
        }, 5);
    });
});

//allow only alphanumeric characters
function AlphaNumericOnly(event) {
    //Updated By AMK - 25JUL13 -Old Code is (code >= 96 && code <= 105)
    var code = (event.keyCode ? event.keyCode : event.which);
    if ((code >= 65 && code <= 90) || (code == 9 || code == 46 || code == 8) || (code >= 48 && code <= 57) || (code >= 96 && code <= 123)) {
        return true;
    }
    return false;
}
function IsNumericWithSpace(val) {
    var reg = /^[0-9\s]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}

function IsAlphaNumericWithSpace(val) {
    var reg = /^[a-zA-Z0-9\s]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}

//To check for alpha numeric values with only hyphen 
function IsAlphaNumericWithHyphen(val) {
    var reg = /^[a-zA-Z0-9-]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}



function IsAlphaNumericWithoutSpace(val) {
    var reg = /^[a-zA-Z0-9]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}



function IsAlphabetWithoutSpace(val) {
    var reg = /^[a-zA-Z]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}



function IsNumericWithoutSpace(val) {
    var reg = /^[0-9]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}


function IsAlphabetWithSpace(val) {
    var reg = /^[a-zA-Z\s]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}


function IsAlphaNumericWithSpacehyphen(val) {
    var reg = /^[a-zA-Z0-9-\s]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}

function GetBaseUrl(appPath, controller) {
    if (controller == null) {
        controller = "SystemControl";
    }
    if (appPath == null || appPath == "/") {
        appPath = "";
    }
    return appPath + "/" + controller;
}

//To log message to browsers that support console api
function Log(message) {
    if (typeof console != "undefined") {
        console.log(message);
    }
}

//Added  to fetch validation messages
//objValidMsg & MsgUrl variables are to be declared in the JS file as a global variable where validation messages are to be displayed
function FetchValidationMessages() {
    $.ajax({
        type: "POST",
        url: MsgUrl,
        data: {},
        dataType: "json",
        async: false,
        timeout: 5000,
        success: function (d) {
            if (d) {
                objValidMsg = d;
            }
        }
    });
}

//Added byVRT to fetch a particular validation message from the validation object
//objValidMsg variable is to be declared in the JS file as a global variable wheter validation messages is to be displayed
function GetMessage(strKey) {
    if (objValidMsg == null || typeof objValidMsg.length == "undefined" || objValidMsg.length == 0) {
        FetchValidationMessages();
    }
    for (var i = 0; i < objValidMsg.length; i++) {
        if (objValidMsg[i].Key == strKey) {
            return objValidMsg[i].Value;
        }
    }
    return "";
}

//strName = name of the cookie
//strValue = value of the cookie
//expires [optional] = when cookie will expire, should be in hours/integer
function SetCookie(strName, strValue, expires) {
    expires = expires ? parseInt(expires) : 1;
    if (strValue) {
        ClearCookie(strName);
        document.cookie = strName + "=" + strValue + ";expires=" + new Date().addHours(expires * 24) + ";path=/";
    }
    else {
        ClearCookie(strName);
    }
}

//clear cookie with the passed name
//strName = name of the cookie
function ClearCookie(strName) {
    document.cookie = strName + "=deleted;expires=" + new Date(0).toUTCString() + ";path=/";
}


/*
Making this a global function since this will be called from an inline script
*/
function SetDefaultImage(image) {
    image.onerror = "";
    image.src = iconUrl + "DFLT.gif";
    return true;
}


//strName = name of the cookie, if no cookie exists, null is returned
function GetCookie(strName) {
    var i, x, y,
        allCookies = document.cookie.split(";"),
        cValue = null;
    for (i = 0; i < allCookies.length; i++) {
        x = allCookies[i].substr(0, allCookies[i].indexOf("="));
        y = allCookies[i].substr(allCookies[i].indexOf("=") + 1);
        x = x.replace(/^\s+|\s+$/g, "");
        if (x == strName) {
            cValue = unescape(y);
            break;
        }

    }
    return cValue;
}



function IsNullOrEmpty(strVal) {
    // alert(strVal);
    // alert(IsNull(strVal));
    if (IsNull(strVal)) {
        return true;
    }
    if (Trim(strVal) == "") {
        return true;
    }
    return false;
}

//trim a string
function Trim(strVal) {
    // alert("2");
    if (typeof strVal == "string") {
        return strVal.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g, '').replace(/\s+/g, ' ');
    }
    else {
        Log(strVal + " is not a string");
    }
}

//check if object is null
function IsNull(obj) {
    return (obj == null);
}

//Convert string True/False value to JS boolean
function toBoolean(val) {
    if (typeof val === "undefined" || val === null) {
        return false;
    }

    return val.toUpperCase() === "TRUE" ? true : false;
};

function customRadio(radioName) {

    var radioButton = $('input[name="' + radioName + '"]');
    $(radioButton).each(function () {
        $(this).wrap("<span class='custom-radio'></span>");
        if ($(this).is(':checked')) {
            $(this).parent().addClass("selected");
        }
    });
    $(radioButton).click(function () {
        if ($(this).is(':checked')) {
            $(this).parent().addClass("selected");
        }
        $(radioButton).not(this).each(function () {
            $(this).parent().removeClass("selected");
        });
    });
}

function SelectRedio(obj, IsCheckBox) {
    if (IsCheckBox == undefined || IsCheckBox == false) {
        var IsSelect = obj.hasClass("myradioSelected");
        $(".myradio").removeClass("myradioSelected");
        $(".myradio").find('input[type="radio"]').removeAttr("checked", "");
        if (IsSelect == false) {
            obj.addClass("myradioSelected");
            obj.find('input[type="radio"]').attr("checked", "checked");
        }
        else {
            obj.find('input[type="radio"]').removeAttr("checked", "");
        }
    }
    else {
        var IsSelect = obj.hasClass("mycheckSelected");
        // $(".myradio").removeClass("mycheckSelected");
        if (IsSelect == false) {
            obj.addClass("mycheckSelected");
            obj.find('input[type="checkbox"]').attr("checked", "checked");
        } else {
            obj.removeClass("mycheckSelected");
            obj.find('input[type="checkbox"]').removeAttr("checked");
        }
    }
};

//----------------------------------

// a global month names array
var gsMonthNames = new Array(
 'January',
 'February',
 'March',
 'April',
 'May',
 'June',
 'July',
 'August',
 'September',
 'October',
 'November',
 'December'
 );
// a global day names array
var gsDayNames = new Array(
 'Sunday',
 'Monday',
 'Tuesday',
 'Wednesday',
 'Thursday',
 'Friday',
 'Saturday'
 );
// the date format prototype
format = function (f) {
    if (!this.valueOf())
        return '&nbsp;';

    var d = this;

    return f.replace(/(yyyy|mmmm|mmm|mm|dddd|ddd|dd|hh|nn|ss|a\/p)/gi,
         function ($1) {
             switch ($1.toLowerCase()) {
                 case 'yyyy': return d.getFullYear();
                 case 'mmmm': return gsMonthNames[d.getMonth()];
                 case 'mmm': return gsMonthNames[d.getMonth()].substr(0, 3);
                 case 'mm': return (d.getMonth() + 1).zf(2);
                 case 'dddd': return gsDayNames[d.getDay()];
                 case 'ddd': return gsDayNames[d.getDay()].substr(0, 3);
                 case 'dd': return d.getDate().zf(2);
                 case 'hh': return ((h = d.getHours() % 12) ? h : 12).zf(2);
                 case 'nn': return d.getMinutes().zf(2);
                 case 'ss': return d.getSeconds().zf(2);
                 case 'a/p': return d.getHours() < 12 ? 'a' : 'p';
             }
         }
     );
}

//VRT:24/07/2015..Disable Browser Back Button Functionality 
function preventBack() {
   // window.history.forward();
}

setTimeout("preventBack()", 0);

//window.onunload = function () { null };


//VRT:24/07/2015.....Disable right click script
var message = "";
///////////////////////////////////
function clickIE() {
    if (document.all) {
        (message);
        return false;
    }
}

function clickNS(e) {
    if (document.layers || (document.getElementById && !document.all)) {
        if (e.which == 2 || e.which == 3) {
            (message);
            return false;
        }
    }
}
if (document.layers) {
    document.captureEvents(Event.MOUSEDOWN);
    document.onmousedown = clickNS;
} else {
    document.onmouseup = clickNS;
    document.oncontextmenu = clickIE;
}

document.oncontextmenu = new Function("return false")

//VRT:24/07/2015......F12 disable code////////////////////////
document.onkeypress = function (event) {
    event = (event || window.event);
    if (event.keyCode == 123) {
        //alert('No F-12');
        return false;
    }
}
document.onmousedown = function (event) {
    event = (event || window.event);
    if (event.keyCode == 123) {
        //alert('No F-keys');
        return false;
    }
}
document.onkeydown = function (event) {
    event = (event || window.event);
    if (event.keyCode == 123) {
        //alert('No F-keys');
        return false;
    }
    else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
        return false;  //Prevent from ctrl+shift+i
    }
}
/////////////////////end///////////////////////


//Note The Keycode value for F1-F12 is 112 to 123
//VRT:18/08/2015
$(window).keydown(function (e) {
    switch (e.keyCode) {
        case 112:
            // doSomething();
            alertify.alert('Disabled F1 Key');
            return false;
        case 113:
            alertify.alert('Disabled F2 Key');
            return false;
        case 114:
            alertify.alert('Disabled F3 Key');
            return false;
        case 115:
            alertify.alert('Disabled F4 Key');
            return false;
        case 116:
            alertify.alert('Disabled F5 Key');
            return false;
        case 117:
            alertify.alert('Disabled F6 Key');
            return false;
        case 118:
            alertify.alert('Disabled F7 Key');
            return false;
        case 119:
            alertify.alert('Disabled F8 Key');
            return false;
        case 120:
            alertify.alert('Disabled F9 Key');
            return false;
        case 121:
            alertify.alert('Disabled F10 Key');
            return false;
        case 122:
            alertify.alert('Disabled F11 Key');
            return false;
        case 123:
            alertify.alert('Disabled F12 Key');
            return false;

        default:
            return true;
    }
});

//*************************************************/
//Handling Keyboard Shortcuts 
//Shift+Alt+1
//Ref: http://www.openjs.com/scripts/events/keyboard_shortcuts/
//shortcut.add("Ctrl+Shift+X", function () {
//    alert("Hi there!");
//});

//shortcut.add("Ctrl+Alt+1", function () {
//    alert("Hi Alt+1!");
//});

//shortcut.add("Ctrl+Shift+Alt+1", function () {
//    alert("Hi Ctrl+Shift+Al+1!");
//});

//*************************************************/


function menuActive(mainMenu, childMenu) {
    $('.panel-heading a').each(function () {
        if ($(this).text() == mainMenu.trim()) { // main menu name
            $(this).addClass('active');
            $(this).removeClass('collapsed');
            $(this).attr("aria-expanded", "true");
            $(this).parents('.panel-heading').siblings('.collapse').addClass('in');
            $(this).parents('.panel-heading').siblings('.collapse').children('.panel-body').children('a').each(function () {
                if ($(this).text() == childMenu.trim()) { // child menu name
                    $(this).addClass('active');
                }
            })
        }
    });
}

