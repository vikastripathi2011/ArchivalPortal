var specialKeys = new Array();
specialKeys.push(8); //Backspace
specialKeys.push(46); //for decimal
function isNumberwithdecimal(e) {
    var isactive = false;
    var keyCode = e.which ? e.which : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
    if (ret) {
        isactive = true;
    }
    return isactive;
}

var splKeys = new Array();
splKeys.push(8); //Backspace
function isNumber(e) {
    var isactive = false;
    var keyCode = e.which ? e.which : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || splKeys.indexOf(keyCode) != -1);
    if (ret) {
        isactive = true;
    }
    return isactive;
}
function isPercentage(obj) {
    var isactive = false;
    var num = parseFloat(obj.value);
    if (num <= 100 && num >= 0) {
        isactive = true;
    }
    else {
        obj.value = obj.defaultValue;
        alertify.error('Value cannot be greater than 100.');
    }
    return isactive;
}



function isPaymentplanPercentage(obj) {
    var isactive = false;
    var num = parseInt(obj.value);
    if (num <= 100 && num > 0) {
        isactive = true;
    }
    else {
        obj.value = obj.defaultValue;
        alertify.error('Value cannot be 0 or greater than 100 .');
    }
    return isactive;
}


function isPaymentplanReminder(obj) {
    var isactive = false;
    var num = parseInt(obj.value);
    if ((num <= 365 && num > 0) || $(obj).val() == '') {
        isactive = true;
    }
    else {
        obj.value = obj.defaultValue;
        alertify.error('Value cannot be 0 or greater than 365.');
    }
    return isactive;
}


/*******************************************************/
// VRT:31/05/2017
//Purpose:Numeric values only allowed (With Decimal Point)

$(".clsonlynumber").die("keypress keyup blur").live("keypress keyup blur", function (event) {
    //this.value = this.value.replace(/[^0-9\.]/g,'');
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

// BY VRT:30/05/2017   //replace the special characters to '' except decmal  

function clsonlynumber(_thisObj) {
    setTimeout(function () {
        //get the value of the input text
        var data = $(_thisObj).val();
        //replace the special characters to '' 
        var dataFull = data.replace(/[^\w\s\.\d+)]/gi, '');
              
        //set the new value of the input text without special characters
        $(_thisObj).val(dataFull);
    });
    return isNumber(event, this);
    // });
}      
