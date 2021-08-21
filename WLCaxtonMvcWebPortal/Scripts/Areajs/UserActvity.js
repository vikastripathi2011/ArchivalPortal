/// <reference path="../DataTables/ColReorder-1.4.1/js/dataTables.colReorder.js" />
/// <reference path="../DataTables/Buttons-1.5.1/js/dataTables.buttons.min.js" />
/// <reference path="../DataTables/Buttons-1.5.1/js/buttons.print.js" />


$(function () {
    $('body').on('click', '.CX img', function () {
        if ($(this).attr("src") == "/Images/toggle-plus.png") {
            $(this).attr("src", "/Images/toggle-minus.png");
        } else {
            $(this).attr("src", "/Images/toggle-plus.png");
        }

        //When Click On + sign
        //if ($(this).text() == '+') {
        //    $(this).text('-');
        //   // $('.CX img').prop('src', '/Images/img/icons/toggle-minus.png');
        //}
        //else {
        //    $(this).text('+');
        //   // $('.img1 img').prop('src', '/Images/img/icons/toggle-plus.png');
        //}
        $(this).closest('tr') // row of + sign
        .next('tr') // next row of + sign
        .toggle(); // if show then hide else show

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


    document.onkeypress = enter;
    function enter(event) {
        if (event.which == 13 || event.keyCode == 13) {
            $("#btnSearch").click();
        }
    }




});

function docDate_oper() {
    if ($("#docDate_Date_operand option:selected").val().trim() == "between") {
        $('#docDate_ToDate').prop("disabled", "");
        $('#docDate_ToDate').prop("readonly", "");
    }
    else {
        $("#docDate_ToDate").val('');
        $('#docDate_ToDate').prop("disabled", "disabled");
        //$("#docDate_ToDate").prop('selectedIndex', 0);
    }
}

function pageCount_oper() {

    if ($("#pageCount_Page_operand option:selected").val().trim() == "between") {
        $("#pageCount_endPage").prop('disabled', false);
        $("#pageCount_endPage").val('');
    }
    else {
        $("#pageCount_endPage").prop('disabled', true);
        $("#pageCount_endPage").val('');
    }
}

function ClearAll() {
    $("#productCategory").prop('selectedIndex', 0);
   
    $("#docDate_Date_operand").prop('selectedIndex', 0);
    $("#pageCount_Page_operand").prop('selectedIndex', 0);
    $("#docType").prop('selectedIndex', 0);
    $("#sortExp1_FieldName").prop('selectedIndex', 0);
    $("#sortExp2_FieldName").prop('selectedIndex', 0);
    $("#sortExp3_FieldName").prop('selectedIndex', 0);
    // $("#docDate_ToDate").datepicker().datepicker('disable');

    $("#Postcode").val('');

    $("#zipCode").val('');
    $("#policyNo").val('');
    $("#spoolName").val('');
    $("#pageCount_startPage").val('');
    $("#pageCount_endPage").val('');

    $('#docDate_ToDate').val("");
    $("#docDate_FromDate").val("");
    $("#docDate_ToDate").prop('disabled', true);
    $("#pageCount_endPage").prop('disabled', true);

    $("#sortExp1_OrderBy").prop('checked', 'checked');
    $("#sortExp2_OrderBy").prop('checked', 'checked');
    $("#sortExp3_OrderBy").prop('checked', 'checked');

    $('.loaderBg').hide();
    $('#dvSearcDetails').hide();

    //$('select').niceSelect('update');
    // $("#docDate_Date_operand").val('default');
    $("#docDate_Date_operand").selectpicker("refresh");
    $("#pageCount_Page_operand").selectpicker("refresh");
    $("#docType").selectpicker("refresh");
    $("#sortExp1_FieldName").selectpicker("refresh");
    $("#sortExp2_FieldName").selectpicker("refresh");
    $("#sortExp3_FieldName").selectpicker("refresh");
    $("#productCategory").selectpicker("refresh");


    try {
        $("input[type='text']").each(function () {
            $(this).removeClass("simple-form__form-group--inputError");
            $('label[id*="Error"]').text('');
        });
        $('.alert').hide();


    }
    catch (e)
    { }
}

//Purpose:Search Details
//Author: VRT:18-01-2018

$(document).on('click', '#btnSearch', function (event) {

    var errorMsg = "";
    // var chk = ($('input[id=sortExp1_OrderBy]:checked').val() == "Asc") ? $('input[id=sortExp1_OrderBy]:checked').val() : "Desc";
    //var chk = ($("#docDate_Date_operand option:selected").val().trim() == "between") ? $("#docDate_ToDate").val().trim() : $("#docDate_ToDate").val().trim();
    var policyNo = $('#policyNo').val();
    var pageCount_startPage = $('#pageCount_startPage').val();
    var pageCount_endPage = $('#pageCount_endPage').val();

    if (!AlphaNumericOnly(policyNo)) {
        errorMsg += " - Special Charector are not allow.\n <br>";
    }

    var toDayDate = new Date();
    var fromDateArr = $('#docDate_FromDate').val().split('/');
    var toDateArr = $('#docDate_ToDate').val().split('/');

    if ($("#productCategory").val() == "") {
        alertify.alert("Please select a Product");
        $(this).focus();
        return false;
    }
    if ($("#policyNo").val() == "*" || $("#Postcode").val() == "*") {
        alertify.alert("A wildcard symbol cannot be the first character of the field.");
        $(this).focus();
        return false;
    }
    if ($("#productCategory").val() == "" && $("#policyNo").val() == "" && $("#docDate_Date_operand").val() == ""
         && $("#docType").val() == "" && $("#Postcode").val() == "" && $("#spoolName").val() == "") {
        errorMsg += " - Please enter any one search parameter.\n <br>"
    }
    if (($("#docDate_Date_operand option:selected").val().trim() == "between") && ($("#docDate_FromDate").val() == "" || $("#docDate_ToDate").val() == "")) {
        errorMsg += " - Please enter both From and To dates.\n <br>"
    }
    if ($('#docDate_FromDate').val() != '' && $('#docDate_ToDate').val() != '') {
        var fromDate = new Date('20' + fromDateArr[2], parseInt(fromDateArr[1]) - 1, fromDateArr[0]);
        var toDate = new Date('20' + toDateArr[2], parseInt(toDateArr[1]) - 1, toDateArr[0]);
        if (fromDate > toDayDate) {
            errorMsg += " - Please enter a valid From date. It should be less than or equal to todays date.\n <br>";
        }
        if (toDate > toDayDate) {
            errorMsg += " - Please enter a valid To date. It should be less than or equal to todays date.\n <br>";
        }
        if (fromDate > toDate) {
            errorMsg += " - The To date must be greater than the From date.\n <br>"
        }
    }

    else {
        if ($('#docDate_FromDate').val() != '') {
            var fromDate = new Date('20' + fromDateArr[2], parseInt(fromDateArr[1]) - 1, fromDateArr[0]);
            if (fromDate > toDayDate) {
                errorMsg += " - Please enter a valid From date. It should be less than or equal to todays date.\n <br>";
            }

        }
        if ($('#docDate_ToDate').val() != '') {

            var toDate = new Date('20' + toDateArr[2], parseInt(toDateArr[1]) - 1, toDateArr[0]);
            if (toDate > toDayDate) {
                errorMsg += " - Please enter a valid To date. It should be less than or equal to todays date.\n <br>";
            }

        }
    }

    if ($("#docDate_FromDate").val() != "" && $("#docDate_Date_operand option:selected").val().trim() == "") {
        errorMsg += " -  Please select a condition for the date you have entered.\n <br>"
        $("#docDate_FromDate").val("");
    }
    if ($("#docDate_FromDate").val() == "" && ($("#docDate_Date_operand option:selected").val().trim() != "between" && $("#docDate_Date_operand option:selected").val().trim() != "")) {
        errorMsg += " - Please select from date.\n <br>"
        $("#docDate_Date_operandS").selectpicker("refresh");

    }


    if ($("#pageCount_Page_operand option:selected").val().trim() == "between") {

        if ($('#pageCount_startPage').val() == "" && $('#pageCount_endPage').val() == "") {
            errorMsg += " - Please enter both From and To page count.\n <br>";
        }
        if ($('#pageCount_startPage').val() != "" && $('#pageCount_endPage').val() != "") {

            if (parseInt($('#pageCount_startPage').val()) > parseInt($('#pageCount_endPage').val())) {
                errorMsg += " - The To page count must be greater than From page count.\n <br>";
            }
        }
        if ($('#pageCount_startPage').val() == "") {
            errorMsg += " -  Please enter a From page count.\n <br>";
        }
        if ($('#pageCount_endPage').val() == "") {
            errorMsg += " -  Please enter a To page count.\n <br>";
        }

    }

    if ($('#pageCount_startPage').val() != "" && $("#pageCount_Page_operand option:selected").val().trim() == "") {
        errorMsg += " - Please select a condition for the page count you have entered.\n <br>"
        $("#pageCount_startPage").val("");
    }
    if ($('#pageCount_startPage').val() == "" && ($("#pageCount_Page_operand option:selected").val().trim() != "" && $("#pageCount_Page_operand option:selected").val().trim() != "between")) {
        errorMsg += " -  Please enter a From page count.\n <br>"
        $("#pageCount_Page_operand").selectpicker("refresh");
    }

    var sortFlag = false;

    if ($("#sortExp1_FieldName option:selected").val().trim() == $("#sortExp2_FieldName option:selected").val().trim()
        && $("#sortExp1_FieldName option:selected").val().trim() != "") {
        errorMsg += " - A sort field can only be selected once.\n <br>";
        sortFlag = true;
    }
    if (sortFlag == false) {
        if ($("#sortExp2_FieldName option:selected").val().trim() == $("#sortExp3_FieldName option:selected").val().trim() && $("#sortExp2_FieldName option:selected").val().trim() != "") {
            errorMsg += " - A sort field can only be selected once.\n <br>"
            sortFlag = true;
        }
    }
    if (sortFlag == false) {
        if ($("#sortExp1_FieldName option:selected").val().trim() == $("#sortExp3_FieldName option:selected").val().trim() && $("#sortExp3_FieldName option:selected").val().trim() != "") {
            errorMsg += " - A sort field can only be selected once.\n <br>"
        }
    }


    if (errorMsg != '') {
        alertify.alert("<b>Warnning</b> \n <br>" + errorMsg);
        return false;
    }
    else {
        $('.loaderBg').show();
        var Model = {
            policyNo: $("#policyNo").val(),
            productCategory: $("#productCategory").val(),
            docType: $("#docType").val(),
            Postcode: $("#Postcode").val(),
            spoolName: $("#spoolName").val(),
            docDate: {
                FromDate: $("#docDate_FromDate").val(),
                ToDate: ($("#docDate_Date_operand option:selected").val().trim() == "between") ? $("#docDate_ToDate").val().trim() : $("#docDate_ToDate").val().trim(),
                Date_operand: $("#docDate_Date_operand").val()
            },
            pageCount: {
                startPage: $("#pageCount_startPage").val(),
                endPage: ($("#pageCount_Page_operand option:selected").val().trim() == "between") ? $("#pageCount_endPage").val().trim() : $("#pageCount_endPage").val().trim(),
                Page_operand: $("#pageCount_Page_operand").val()
            },
            sortExp1: {
                FieldName: ($('#sortExp1_FieldName').val() == "") ? $('#sortExp1_FieldName').val().trim() : $('#sortExp1_FieldName').val().trim(),
                OrderBy: ($('input[id=sortExp1_OrderBy]:checked').val() == "Asc") ? $('input[id=sortExp1_OrderBy]:checked').val().trim() : "Desc"
            },
            sortExp2: {
                FieldName: ($('#sortExp2_FieldName').val() == "") ? $('#sortExp2_FieldName').val().trim() : $('#sortExp2_FieldName').val().trim(),
                OrderBy: ($('input[id=sortExp2_OrderBy]:checked').val() == "Asc") ? $('input[id=sortExp2_OrderBy]:checked').val().trim() : "Desc"
            },
            sortExp3: {
                FieldName: ($('#sortExp3_FieldName').val() == "") ? $('#sortExp3_FieldName').val().trim() : $('#sortExp3_FieldName').val().trim(),
                OrderBy: ($('input[id=sortExp3_OrderBy]:checked').val() == "Asc") ? $('input[id=sortExp3_OrderBy]:checked').val().trim() : "Desc"
            }
        };
        $.ajax({
            type: "POST",
            url: rootDir + 'UserActvity/Data/Search/',

            data: JSON.stringify(Model),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            //async: true,
            success: function (data, status) {
                var result = data;
                if (status == "success" && result.ResultMsg != null) {
                    data = "";
                    $(location).attr('href', rootDir + "UserActvity/Data/SearchResult?PageName=SearchResult");
                    $('.loaderBg').show();
                    //..Bind with Webgrid 
                    //$(location).attr('href', "/UserActvity/UserActvity/DynamicRwsColBind/");

                }
                else {

                    if (result != undefined && result.errorCode == '-2') {
                        alertify.alert("<b>Error Message </b>\n <br>" + result.errorMsg);
                        ClearAll();
                    }
                }

            },
            failure: function (data) {
                $('.loaderBg').hide();
            },
            error: function (data) {
                if (data.statusText != "OK") {
                    // $.wait(function () { $("#message").slideUp() }, 5);
                    alertify.alert("<b>Error Message </b>\n <br>" + data.statusText);
                    ClearAll();
                    $('.loaderBg').hide();
                }
            }
        });
    }
});

$(document).on('click', '#btnRefresh', function (event) {
    $('.loaderBg').show();
    window.location.href = rootDir + 'UserActvity/Data/SearchResult';
});

//Purpose:Show Documnet Detail 
//Author: VRT:1-02-2018
function fn_ShowDocumentDetails(_this, type) {

    //var _cmidVal = $('#txtSearchCMID').val().trim();
    $('.loaderBg').show();
    _DocDetail = {
        DocumentGuid: $(_this).attr('data-DocguId'),
        PageCount: $(_this).attr('data-cnt'),
        Spoolname: $(_this).attr('data-spn')

    };
    if (type == 1) {
        $.ajax({
            type: "POST",
            url: rootDir + "UserActvity/Data/ShowDocumentDetails",
            data: JSON.stringify(_DocDetail),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            async: true,
            cache: false,
            success: function (resData) {
                $('#dvDocumentDetails').html('');
                $('#dvDocumentDetails').html(resData);
                $("#dvDocument").dialog({
                    width: 850,
                    modal: true,
                    title: "View Document Detail",
                    buttons: [
                          {
                              text: "Back",
                              "class": 'popBtn',
                              click: function () {
                                  $(this).dialog("close");
                                  $('#dvDocumentDetails').html('');
                              }
                          }
                    ]
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alertify.alert('<b>Error:</b> \n<br> ' + errorThrown);
            },
            complete: function (xhr, status) {
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: rootDir + "UserActvity/Data/ShowDocumentDetails",
            data: JSON.stringify(_DocDetail),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            async: true,
            cache: false,
            success: function (resData) {
                $('#dvDocumentDetails').html('');
                $('#dvDocumentDetails').html(resData);
                $('.formdiv').hide();
                $("#dvDocument").dialog({
                    width: 850,
                    modal: true,
                    title: "View Document Detail",
                    buttons: [
                          {
                              text: "Back",
                              "class": 'popBtn',
                              click: function () {
                                  $(this).dialog("close");
                                  $('#dvDocumentDetails').html('');
                              }
                          }
                    ]
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alertify.alert('<b>Error:</b> \n<br> ' + errorThrown);
            },
            complete: function (xhr, status) {

            }
        });

    }

    $('.loaderBg').hide();
}

//Purpose:View PDF
//Author: VRT:1-02-2018
function fn_GetpdfDocument(_this) {
    $('.loaderBg').show();
    _PdfDetail = {
        DocumentGUID: $(_this).attr('data-DocguId'),
        PageCount: $(_this).attr('data-cnt'),
        StartPageNo: $(_this).attr('data-spn')
    };
    $.ajax({
        type: "POST",
        //url: rootDir + "UserActvity/Data/Screenreport",
        url: rootDir + "UserActvity/Data/ShowPDF",
        data: JSON.stringify(_PdfDetail),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        cache: false,
        success: function (data, status) {
            var result = data;
            if (result.errorCode == 0 && result.ResultMsg != null) {
                if (result.ResultMsg != "") {
                    window.open(rootDir + "UserActvity/Data/PDFExport?file=" + result.ResultMsg, '_blank');
                }
            }
            else {
                alertify.alert('<b>Info:</b> \n<br> ' + " - Document not available");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alertify.alert('<b>Error:</b> \n<br> ' + errorThrown);
        },
        complete: function (xhr, status) {
            $('.loaderBg').hide();
        }
    });

}

//Purpose:Show Configure Search  List
//Author: VRT:13-02-2018
function fn_ShowConfigureSearch() {
    $.ajax({
        type: "POST",
        url: rootDir + "UserActvity/UserActvity/ConfigureSearchList",
        //data: JSON.stringify(_DocDetail),
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        async: true,
        cache: false,
        success: function (resData) {
            $('#dvDocumentDetails').html('');
            $('#dvDocumentDetails').html(resData);
            $("#dvDocument").dialog({
                width: 750,
                modal: true,
                title: "View Configure Search",
                buttons: [
                      {
                          text: "Back",
                          "class": 'popBtn',
                          click: function () {
                              $(this).dialog("close");
                              $('#dvDocumentDetails').html('');
                          }
                      },
                       {
                           text: "Save",
                           "class": 'popBtn',
                           click: function () {
                               // $(this).dialog("close");
                               //$('#dvDocumentDetails').html('');
                               alert("dialog Save call");
                           }
                       }
                ]
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alertify.alert('<b>Error:</b> \n<br> ' + errorThrown);
        },
        complete: function (xhr, status) {
        }
    });

}


//Purpose:Print Documnet Detail 
//Author: VRT:1-02-2018
function printDiv(obj) {
    /**********************************************************/
    //var divContents = $("#dvDocumentDetails").html();
    if (obj == 'divAllPrint-dvOnlyPrint') {
        obj = 'divAllPrint';
        var divContents = $("#" + obj).html();
        var printWindow = window.open('', '', 'height=400,width=800');
        printWindow.document.write('<html><head> <style>.dataTables_length { display: none !important;} </style> <style>.dataTables_filter { display: none !important;} </style><style>.dt-buttons { display: none !important;}</style> <style>.dataTables_info { display: none !important;}</style> <style>.dataTables_paginate { display: none !important;}</style> <style>.no-print { display: none !important;}</style><style>.dvOnlyPrint{ display: none !important;}</style>');

        printWindow.document.write(' <style>.tblMasterdocList {background-color:#ffffff;border:4px !important;border-spacing:4px;-webkit-border-radius: 3px;-moz-border-radius: 3px;border-radius: 3px;}</style>');
        printWindow.document.write(' <style>.display thead tr th {height: 22px; text-align: center; color: #323232; background: #fafafa; /* Old browsers */ background: -moz-linear-gradient(top, #fafafa 0%, #eaeaea 100%) !important; /* FF3.6+ */</style>');

        printWindow.document.write(' <style>tbody tr td {height: 22px; text-align: center; color: #323232; background: #fafafa; /* Old browsers */ background: -moz-linear-gradient(top, #fafafa 0%, #eaeaea 100%) !important; /* FF3.6+ */</style>');

        printWindow.document.write(' <style>background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#fafafa), color-stop(100%,#eaeaea)) !important; /* Chrome,Safari4+ */ background: -webkit-linear-gradient(top, #fafafa 0%,#eaeaea 100%) !important; /* Chrome10+,Safari5.1+ */background: -o-linear-gradient(top, #fafafa 0%,#eaeaea 100%) !important; /* Opera 11.10+ */ background: linear-gradient(top, #fafafa 0%,#eaeaea 100%) !important; /* W3C */</style>');
        printWindow.document.write(' <style>filter: progid:DXImageTransform.Microsoft.gradient( startColorstr="#fafaf", endColorstr="#eaeaea",GradientType=0 ) !important; /* IE6-9 */ font-size: 11px; vertical-align: middle;    }<title>Document Details</title></style>');
    } else {
        var divContents = $("#" + obj).html();
        var printWindow = window.open('', '', 'height=400,width=800');
        printWindow.document.write('<html><head> <style>.no-print { display: none !important;}</style><style>.display thead tr th {height: 22px; text-align: center; color: #323232; background: #fafafa;  background: -moz-linear-gradient(top, #fafafa 0%, #eaeaea 100%) !important; </style><title>Document Details</title>');
        //var printWindow = window.open('', '', 'height=400,width=800');
        //printWindow.document.write('<html><head> <style>.no-print { display: none !important;}</style><style>.dvOnlyPrint{ display: none !important;}</style>');
        //printWindow.document.write(' <style>.display  {background-color:#ffffff;border:0px;border-spacing:0px;-webkit-border-radius: 3px;-moz-border-radius: 3px;border-radius: 3px;}');
        //printWindow.document.write(' <style>.display thead tr th {height: 22px; text-align: center; color: #323232; background: #fafafa; /* Old browsers */ background: -moz-linear-gradient(top, #fafafa 0%, #eaeaea 100%) !important; /* FF3.6+ */');
        //printWindow.document.write(' <style>background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#fafafa), color-stop(100%,#eaeaea)) !important; /* Chrome,Safari4+ */ background: -webkit-linear-gradient(top, #fafafa 0%,#eaeaea 100%) !important; /* Chrome10+,Safari5.1+ */background: -o-linear-gradient(top, #fafafa 0%,#eaeaea 100%) !important; /* Opera 11.10+ */ background: linear-gradient(top, #fafafa 0%,#eaeaea 100%) !important; /* W3C */');
        //printWindow.document.write(' <style>filter: progid:DXImageTransform.Microsoft.gradient( startColorstr="#fafaf", endColorstr="#eaeaea",GradientType=0 ) !important; /* IE6-9 */ font-size: 11px; vertical-align: middle;    }<title>Document Details</title>');

    }
    //$(".no-print").hide();

    printWindow.document.write('</head><body>');
    printWindow.document.write(divContents);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
    $('.dataTables_length').show();
}

function pritsAll() {
    $('.buttons-print').trigger("click");
}

function colvisibility() {
    $('.buttons-colvis').trigger("click");
}


function GenerateReport() {
    var selectRptName = "Export in CSV";
    //$("#Search_Div input:radio[name=ReportName]:checked").val();
    //if (selectRptName != undefined) {
    //var LogDetail = { StartTime: getSystemTime(), ReportName: selectRptName, EndTime: null, UserName: $("#Search_Div input:radio[name=ReportName]:checked").attr('user-name') };
    _errorFlag = false;

    //write here your Logics{...}
    $.ajax({
        type: "POST",
        url: rootDir + "UserActvity/Data/GenerateReportinCSV",
        //data: JSON.stringify(Model),
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        async: true,
        cache: false,
        success: function (reportData) {

            if (reportData != null && reportData != 'undefined' && reportData.length > 0) {
                for (var i = 0; i < reportData.length; i++) {
                    ExportCsvReport(reportData[i].ReportTable, reportData[i].ReportName);
                    if (_errorFlag == true) {
                        break;
                    }
                }
                if (_errorFlag == false) {
                    // AddLogDetail(LogDetail);
                }
                // ResetAll();
            }
            else {
                alertify.alert('<b>Warning:</b> \n<br>' + selectRptName + " Records is not found");
                $('#alertify-ok').click(function () {
                    // ResetAll();
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alertify.alert('<b>Warning:</b> \n<br><b>' + selectRptName + ' File Not Generated</b> - ' + errorThrown);
        },
        complete: function (xhr, status) {

        }
    });

}



//-----------------------------Start Generate Report------------------------------------//
function ExportCsvReport(rptTable, rptName) {
    $('.loaderBg').show();
    var x = new CSVExport(rptTable, rptName);
    $('.loaderBg').hide();
    return false;
}

/**
@namespace Converts JSON to CSV.
*/
(function (window) {
    //    "use strict";    /**    Default constructor    */
    var _CSV = function (JSONData, _fileName) {
        try {
            if (typeof JSONData === 'undefined')
                return;

            var csvData = typeof JSONData != 'object' ? JSON.parse(settings.JSONData) : JSONData,
            csvHeaders,
            csvEncoding = 'data:text/csv;charset=utf-8,',
            csvOutput = "",
            csvRows = [],
            BREAK = '\r\n',
            DELIMITER = ',',
			FILENAME = _fileName + ".csv";

            // Get and Write the headers
            csvHeaders = Object.keys(csvData[0]);
            //modified by VRT
            csvHeaders = jQuery.grep(csvHeaders, function (value) {
                return value != "PageCountHidden" && value != "DocumentGUID";
            });

            /*****************modified by VRT********************************/
            csvOutput += csvHeaders.join(',') + BREAK;

            for (var i = 0; i < csvData.length; i++) {
                var rowElements = [];
                for (var k = 0; k < csvHeaders.length; k++) {
                    rowElements.push(csvData[i][csvHeaders[k]]);
                } // Write the row array based on the headers
                csvRows.push(rowElements.join(DELIMITER));
            }
            csvOutput += csvRows.join(BREAK);

            // Initiate Download
            var a = document.createElement("a");
            //        var content = "some content";


            if (navigator.msSaveBlob) { // IE10
                navigator.msSaveBlob(new Blob([csvOutput], { type: "text/csv" }), FILENAME);
            } else if ('download' in a) { //html5 A[download]


                var blob = new Blob(["\ufeff", csvOutput]);
                // var blob=new Blob([csvOutput], { type: 'text/csv' }); 
                var url = URL.createObjectURL(blob);
                a.href = url;

                //a.setAttribute('download', 'file.csv');
                //a.click();
                //  a.href = csvEncoding + encodeURIComponent(csvOutput);

                a.download = FILENAME;
                document.body.appendChild(a);
                setTimeout(function () {
                    a.click();
                    document.body.removeChild(a);
                }, 66);
            } else if (document.execCommand) { // Other version of IE
                var oWin = window.open("about:blank", "_blank");
                oWin.document.write(csvOutput);
                oWin.document.close();
                oWin.document.execCommand('SaveAs', true, FILENAME);
                oWin.close();
            } else {
                alertify.alert("<b>Warning:</b> \n<br>Support for your specific browser hasn't been created yet, please check back later.");
            }
        }
        catch (err) {
            _errorFlag = true;
            alertify.alert('<b>Error:</b> \n<br>Report Generated Error: ' + err.message)
        }
    };
    window.CSVExport = _CSV;
})(window);
//-----------------------------End Generate Report------------------------------------//


function EnableDisableOrderGrouping(obj) {
    if (obj != 'undefined') {
        var objOrder = $(obj).parent('td').parent('tr').find('Select')[0];
        var objGrouping = $(obj).parent('td').parent('tr').find('input[Id^="chkGrouping"]')[0];
        if ($(obj)[0].checked) {
            $(objOrder).removeAttr('disabled');
            $(objGrouping).removeAttr('disabled');

        }
        else {
            $(objOrder).prop('disabled', 'disabled');
            $(objGrouping).prop('disabled', 'disabled');
            $(objGrouping).prop('checked', '');
            $(objOrder).prop('selectedIndex', 0);

        }
    }
}
function CheckGrouping(obj) {
    var grpCount = $('input[id^="chkGrouping"]:checked').length - 1;
    if ($(obj)[0].checked) {
        if (grpCount > 0) {
            $(obj)[0].checked = false;
        }
    }
}
$(document).ready(function () {
    $('input[id^="chkGrouping"]').click(function () {
        CheckGrouping(this);
    });
    $('input[id^="chkVisibility"]').click(function () {
        EnableDisableOrderGrouping(this);
    });
});

///"Purpose:validate to Configure Search"
///Author: VRT:12-02-2018
//$('#btnSubmit').click(function () {
function validateConfigureSearch() {
    var selectedVal = new Array();
    var noOfSelectedCol = $('input[id^="chkVisibility"]:checked').length;
    var pos, showMsg, count, flag = true;
    if (noOfSelectedCol == 0) {
        alert('No one columns Selected.. ');
        return false;
    }
    else {
        pos = 0;
        count = 0;
        $("select[id*='item_DisplayOrder']").each(function () {
            if ($(this).attr("disabled") != "disabled") {
                pos = this.options[this.selectedIndex].value
                if (pos > 0) {
                    selectedVal[count] = pos;
                    count++;
                }
                else {
                    showMsg = true;
                    flag = false;
                }
            }
        });
        if (showMsg) {
            flag = false;
            alert("Please select the position order");
            //alert('@Resources.RSAPortalResource.M00052');
            return false;
        }
        else {
            $.ajax({
                type: "Get",
                url: rootDir + "UserActvity/Data/SaveSearchConfig/",
                contentType: "application/json; charset=utf-8",
                dataType: 'html',
                async: true,
                //cache: false,
                success: function (d) {
                    if (d != null) {
                        // $(location).attr('href', "/UserActvity/Data/SearchResult/");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alertify.alert('<b>Error:</b> \n<br> ' + errorThrown);
                },
                complete: function (xhr, status) {
                }
            });
            $('.loaderBg').hide();
        }
        selectedVal = selectedVal.sort();
        for (var i = 0; i < selectedVal.length - 1; i++) {
            if (selectedVal[i + 1] == selectedVal[i]) {
                flag = false;
                alert('@Resources.RSAPortalResource.M00053');
                break;
            }
        }
        if (flag == false) {
            return false;
        }
        else {
            return true;
        }


    }
}//});

function AlphaNumericOnly(event) {

    var code = (event.keyCode ? event.keyCode : event.which);
    if ((code == undefined || code == "") || (code >= 65 && code <= 90) || (code == 9 || code == 46 || code == 8 || code == 32) || (code >= 48 && code <= 57) || (code >= 96 && code <= 123)) {
        return true;
    }
    return false;
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

function IsNumericWithSpace(val) {
    var reg = /^[0-9\s]+$/;
    if (val.match(reg)) {
        return true;
    }
    else {
        return false;
    }
}


/* jQuery Table document ready	    */
$(document).ready(function () {
    var colGroup = $("#ddlColm").val();
    var pageNo = 0;
    $.fn.dataTableExt.oApi.fnStandingRedraw = function (settings) {
        /* Note the use of a DataTables 'private' function thought the 'oApi' object */
        var before = settings._iDisplayStart;

        settings.oApi._fnReDraw(settings, true);
        settings._iDisplayStart = pageNo;
        settings.oApi._fnCalculateEnd(settings);
        settings.oApi._fnDraw(settings);

    }
    $('#tblMasterdocList thead').on('click', 'th', function () {

        table.fnStandingRedraw();

        return false;
    });
    $('#tblMasterdocList').on('page.dt', function () {

        var api = table.api();
        var pageInfo = api.page.info();
        pageNo = parseInt($('Select[name=tblMasterdocList_length]').val()) * (parseInt(pageInfo.page));


    });
    $.fn.dataTableExt.oSort['time-date-sort-pre'] = function (value) {
        
        var frDatea2 = value.split('/');
        frDatea2 = (frDatea2[2] + frDatea2[1] + frDatea2[0]) * 1;
        return frDatea2;
    };
    $.fn.dataTableExt.oSort['time-date-sort-asc'] = function (a, b) {

        var x = new Date(a),
      y = new Date(b);
        return ((x < y) ? -1 : ((x > y) ? 1 : 0));
    };
    $.fn.dataTableExt.oSort['time-date-sort-desc'] = function (a, b) {
        var x = new Date(a),
       y = new Date(b);
        return ((x < y) ? 1 : ((x > y) ? -1 : 0));
    };
    var table = $('#tblMasterdocList').dataTable({
        responsive: true,
        colReorder: true,
        pageLength: 20,
        "lengthMenu": [[10, 20, 50], [10, 20, 50]],
        "pagingType": "full_numbers",
        "order": [],
        "columnDefs": [
           { "orderable": false, "targets": 0 },
           { type: 'time-date-sort', targets: 3 },
           { "targets": [0, 1, 2], "bSortable": false, "searchable": false }
           
           
            //"type": 'time-date-sort',
            //"targets": 3
            /*"bSortable": false,
            "searchable": false*/
        ],
        "dom": 'lBfrtip',
        //"buttons": ['print', 'colvis'],
        buttons: [{
            extend: 'print',
            exportOptions: {
                columns: ':visible'
            },
            customize: function (win) {
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();

                // ----added by sandy---
                
                //$(win.document.body).find('table').css('border', '#000 solid 1px');
                //$(win.document.body).find('table thead tr th').css('border', '#000 solid 1px');
                //$(win.document.body).find('table tbody tr td').css('border', '#000 solid 1px');
               // table td { text-align: center; }
                $(win.document.body).find('table thead tr th').css({ textAlign: 'center' });
                $(win.document.body).find('table tbody tr td').css({ textAlign: 'center' });
            }
        }, 'colvis'],

        rowGroup: {
            dataSrc: colGroup

        },
        "bDestroy": true

    });
    $('#tblMasterdocList > thead > tr:eq(0) > th:eq(0)').removeClass('sorting_asc');
    $('.buttons-columnVisibility:lt(3)').hide();
    //var allData = table.columns().data();
    $("#tblMasterdocList_wrapper").removeClass("dataTables_wrapper no-footer");
    $('.buttons-print').prop("style", "display:none");
    $('.buttons-colvis').prop("style", "display:none");
    $('.buttons-colvis').hide();
    $('.buttons-print').hide();

});
//Then add a click event handler to your row Groups:

function fn_colGroupDataTable() {
    var colGroup = $("#ddlColm").val();
    $('#tblMasterdocList').dataTable({
        "order": [],
        "pageLength": 20,
        "lengthMenu": [[10, 20, 50], [10, 20, 50]],
        "pagingType": "full_numbers",
        "columnDefs": [
             { type: 'time-date-sort', targets: 3 },
             { "bSortable": false, "targets": [0] },
             {
                 "targets": [0, 1, 2],
                 "bSortable": false,
                 "searchable": false
             }],
        "dom": 'lBfrtip',
        //"buttons": ['print'],
        buttons: [
        {
            extend: 'print',
            exportOptions: {
                columns: ':visible'
            },
            customize: function (win) {
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();
                //$(win.document.body).find('table').find('td:eq(0),th:eq(0)').remove();
                //$(win.document.body).find('table').find('td:eq(0),th:eq(0)').remove();
            }
        }
        , 'colvis'
        ],
        rowGroup: {
            dataSrc: colGroup

        },
        colReorder: true,
        "bDestroy": true
    });
    $('.buttons-print').prop("style", "display:none");
    $('.buttons-colvis').prop("style", "display:none");
    $('.buttons-colvis').hide();
    $('.buttons-print').hide();
    //$('select').addClass('selectpicker');
    // $('select').removeClass('nice-select form-control input-sm');
    $('.dataTables_filter').show();
}

/* jQuery End Table document ready	    */



/******************** Start Nested Data Table ****************************/
/*
function fnFormatDetails(table_id, html) {
    var sOut = "<table id=\"tblMasterdocList_" + table_id + "\">";
    sOut += html;
    sOut += "</table>";
    return sOut;
}



var iTableCounter = 1;
var oTable;
var oInnerTable;
var TableHtml;

//Run On HTML Build
$(document).ready(function () {
    var colGroup = $("#ddlColm").val();
    TableHtml = $("#tblMasterdocList").html();

    // Insert a 'details' column to the table
    var nCloneTh = document.createElement('th');
    var nCloneTd = document.createElement('td');
    nCloneTd.innerHTML = '<img src="/Images/toggle-plus.png">';
    nCloneTd.className = "center";

    $('#tblMasterdocList thead tr').each(function () {
        this.insertBefore(nCloneTh, this.childNodes[0]);
    });

    $('#tblMasterdocList tbody tr').each(function () {
        this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
    });
    //$('.Innertbl tr').each(function () {
    //    this.insertBefore(nCloneTd.cloneNode(true), this.childNodes[0]);
    //});
    //   Initialse DataTables, with no sorting on the 'details' column
    var oTable = $('#tblMasterdocList').DataTable({
        responsive: true,
        colReorder: true,
        "aLengthMenu": [10, 25, 50, 100],
        "sPaginationType": "full_numbers",
        "columnDefs": [{
            "targets": [0, 1, 2],
            "bSortable": false,
            "searchable": false
        }],
        "dom": 'Bfrtip',
        buttons: [{
            extend: 'print',
            exportOptions: {
                columns: ':visible'
            },
            customize: function (win) {
                $(win.document.body).find('table').find('td:first-child, th:first-child').remove();
                $(win.document.body).find('table').find('td:eq(0),th:eq(0)').remove();
                $(win.document.body).find('table').find('td:eq(0),th:eq(0)').remove();
            }
        }],

        rowGroup: {
            dataSrc: colGroup
           },
       // "bDestroy": true,
        "sPaginationType": "full_numbers",
        "aoColumnDefs": [
    { "bSortable": false, "aTargets": [0] }
        ],
        "aaSorting": [[1, 'asc']]

    });
   // var allData = oTable.columns().data();
    //$("#tblMasterdocList_wrapper").removeClass("dataTables_wrapper no-footer");
    $('.buttons-print').prop("style", "display:none");

    /* Add event listener for opening and closing details
    * Note that the indicator for showing which row is open is not controlled by DataTables,
    * rather it is done here/

    $('#tblMasterdocList tbody td img').bind('click', function () {
        var rowIdx = 1;
        oTable.rows().every(function () {
            this
                .child(
                    $(
                        '<tr>' +
                            '<td>' + rowIdx + '.1</td>' +
                            '<td>' + rowIdx + '.2</td>' +
                            '<td>' + rowIdx + '.3</td>' +
                            '<td>' + rowIdx + '.4</td>' +
                        '</tr>'
                    )
                )
                .show();
        });

        //var nTr = $(this).parents('tr')[0];
        //if (oTable.fnIsOpen(nTr)) {
        //    /* This row is already open - close it /
        //    this.src = "/Images/toggle-plus.png";
        //    oTable.fnClose(nTr);
        //}
        //else {
        //    /* Open this row /
        //    this.src = "/Images/toggle-minus.png";
        //    oTable.fnOpen(nTr, fnFormatDetails(iTableCounter, TableHtml), 'details');
        //    oInnerTable = $("#tblMasterdocList_" + iTableCounter).dataTable({
        //        "bJQueryUI": true,
        //        "sPaginationType": "full_numbers"
        //    });
        //    iTableCounter = iTableCounter + 1;
        //}
        var nTr = this.parentNode.parentNode;
        if (this.src.match('details_close')) {
            /* This row is already open - close it /
            this.src = "/Images/toggle-minus.png";
            oTable.fnClose(nTr);
        }
        else {
            /* Open this row /
            this.src = "/Images/toggle-plus.png";
            //oTable.fnOpen(nTr, fnFormatDetails(oTable, nTr), 'details');
            oTable.fnOpen(nTr, fnFormatDetails(iTableCounter, TableHtml), 'details');
                oInnerTable = $("#tblMasterdocList_" + iTableCounter).dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers"
                });
                iTableCounter = iTableCounter + 1;
        }
    });
});

*/


/***************************** End  Nested Data Table **********************************************/