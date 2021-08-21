var validNavigation = false;
setInterval(function () { CheckUserSession(); }, 5000);
function endSession() {
    // Browser or broswer tab is closed
    // Do sth here ...
    $.ajax({
        type: "POST",
        url: rootDir + 'Login/LogOffUser',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        async: true,
        success: function (data, status) {
            window.location.href = rootDir + "/Login/UserLogIn";

        },
        error: function () {

        },
        complete: function () {

        }

    });
}
function CheckUserSession() {

    $.ajax({
        url: rootDir + 'Login/CheckUserSession',
        type: 'Post',
        data: {},
        global: false,
        success: function (data) {
            if (data != "") {
                $(location).attr('href', rootDir + "Login/UserLogin" + data);
            }
        },
        failure: function (data) {
            alertify.alert('<b>Error:</b> \n<br>  Oops! There seems to be a problem with your internet connection. Please check your settings. ');
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alertify.alert('<b>Error:</b> \n<br>  Oops! Your session has been expired. Please login again.');
        },
        complete: function (xhr, status) {
        }
    });


}

function wireUpEvents() {
    //location = window.location.href;
    /*
    * For a list of events that triggers onbeforeunload on IE
    * check http://msdn.microsoft.com/en-us/library/ms536907(VS.85).aspx
    */
   

    window.onbeforeunload = function () {
        if (!validNavigation) {

         //   endSession();
           
        }
        
    }
    

    // Attach the event keypress to exclude the F5 refresh
    $(document).bind('keypress', function (e) {
        if (e.keyCode == 116 || e.keyCode == 13) {
            validNavigation = true;
        }
    });

    // Attach the event click for all links in the page
    $("a").bind("click", function () {
        validNavigation = true;
    });

    // Attach the event submit for all forms in the page
    $("form").bind("submit", function () {
        validNavigation = true;
    });

    // Attach the event click for all inputs in the page
    $("input[type=submit],input[type=button]").bind("click", function () {
        validNavigation = true;
    });


    $(document).keydown(function (e) {
        return (e.which || e.keyCode) != 116;
    });

}

// Wire up the events as soon as the DOM tree is ready
$(document).ready(function () {
    wireUpEvents();
});