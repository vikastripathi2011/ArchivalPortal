

jQuery.ptTimeSelect = {
    version: "0.7.3"
};

jQuery.ptTimeSelect.options = {
    containerClass: undefined,
    containerWidth: undefined,
    hoursLabel: 'Hour',
    minutesLabel: 'Minutes',
    setButtonLabel: 'Set',
    popupImage: undefined,
    onFocusDisplay: true,
    zIndex: 10,
    onBeforeShow: undefined,
    onClose: undefined
};

jQuery.ptTimeSelect._ptTimeSelectInit = function () {
    jQuery(document).ready(
		function () {
		    //if the html is not yet created in the document, then do it now
		    if (!jQuery('#ptTimeSelectCntr').length) {
		        jQuery("body").append(
						'<div id="ptTimeSelectCntr">'
					+ '		<div class="ui-widget ui-widget-content ui-corner-all">'
					+ '		<div class="ui-widget-header ui-corner-all">'
					+ '			<div id="ptTimeSelectCloseCntr" style="float: right;">'
					+ '				<a href="javascript: void(0);" onclick="jQuery.ptTimeSelect.closeCntr();" '
					+ '						onmouseover="jQuery(this).removeClass(\'ui-state-default\').addClass(\'ui-state-hover\');" '
					+ '						onmouseout="jQuery(this).removeClass(\'ui-state-hover\').addClass(\'ui-state-default\');"'
					+ '						class="ui-corner-all ui-state-default">'
					+ '					<span class="ui-icon ui-icon-circle-close">X</span>'
					+ '				</a>'
					+ '			</div>'
					+ '			<div id="ptTimeSelectUserTime" style="float: left;">'
					+ '				<span id="ptTimeSelectUserSelHr">9</span> : '
					+ '				<span id="ptTimeSelectUserSelMin">00</span> '
					+ '				<span id="ptTimeSelectUserSelAmPm">AM</span>'
					+ '			</div>'
					+ '			<br style="clear: both;" /><div></div>'
					+ '		</div>'
					+ '		<div class="ui-widget-content ui-corner-all">'
					+ '			<div>'
					+ '				<div class="ptTimeSelectTimeLabelsCntr">'
					+ '					<div class="ptTimeSelectLeftPane" style="width: 50%; text-align: center; float: left;" class="">Hour</div>'
					+ '					<div class="ptTimeSelectRightPane" style="width: 50%; text-align: center; float: left;">Minutes</div>'
					+ '				</div>'
					+ '				<div>'
					+ '					<div style="float: left; width: 50%;">'
					+ '						<div class="ui-widget-content ptTimeSelectLeftPane">'

					+ '							<div class="ptTimeSelectHrCntr">'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">1</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">2</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">3</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">4</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">5</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">6</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">7</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">8</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">9</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">10</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">11</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);">12</a>'
					+ '								<br style="clear: left;" /><div></div>'
					+ '							</div>'
					+ '						</div>'
					+ '					</div>'
					+ '					<div style="width: 50%; float: left;">'
					+ '						<div class="ui-widget-content ptTimeSelectRightPane">'
					+ '							<div class="ptTimeSelectMinCntr">'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">00</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">05</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">10</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">15</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">20</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">25</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">30</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">35</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">40</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">45</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">50</a>'
					+ '								<a class="ptTimeSelectMin ui-state-default" href="javascript: void(0);">55</a>'
					+ '								<br style="clear: left;" /><div></div>'
					+ '							</div>'
					+ '						</div>'
					+ '					</div>'
					+ '				</div>'
					+ '			</div>'
					+ '			<div style="clear: left;"></div>'
					+ '		</div>'

                    + '							<div class="ptTimeSelectHrAmPmCntr">'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);" '
					+ '										style="display: block; width: 17%; float: left;border-radius: 3px;height: 19px;margin: 7px 6px  0 15px;">AM</a>'
					+ '								<a class="ptTimeSelectHr ui-state-default" href="javascript: void(0);" '
					+ '										style="display: block; width: 17%; float: left;border-radius: 3px;height: 19px;margin: 7px 0 0 0;">PM</a>'
					+ '								<div></div>'
					+ '							</div>'
                    	+ '		<div id="ptTimeSelectSetButton">'
					+ '			<a href="javascript: void(0);" onclick="jQuery.ptTimeSelect.setTime1()"'
					+ '					onmouseover="jQuery(this).removeClass(\'ui-state-default\').addClass(\'ui-state-hover\');" '
					+ '						onmouseout="jQuery(this).removeClass(\'ui-state-hover\').addClass(\'ui-state-default\');"'
					+ '						class="ui-corner-all ui-state-default">'
					+ '				SET'
					+ '			</a>'
					+ '			<br style="clear: both;" /><div></div>'
					+ '		</div>'

					+ '		<!--[if lte IE 6.5]>'
					+ '			<iframe style="display:block; position:absolute;top: 0;left:0;z-index:-1;'
					+ '				filter:Alpha(Opacity=\'0\');width:3000px;height:3000px"></iframe>'
					+ '		<![endif]-->'
					+ '	</div></div>'
				);

		        var e = jQuery('#ptTimeSelectCntr');

		        // Add the events to the functions
		        e.find('.ptTimeSelectMin')
					.bind("click", function () {
					    jQuery.ptTimeSelect.setMin($(this).text());
					});

		        e.find('.ptTimeSelectHr')
					.bind("click", function () {
					    jQuery.ptTimeSelect.setHr($(this).text());
					});

		        $(document).mousedown(jQuery.ptTimeSelect._doCheckMouseClick);
		    } //end if
		}
	);
} ();
jQuery.ptTimeSelect.setHr = function (h) {

    if (h.toLowerCase() == "am" || h.toLowerCase() == "pm") 
    {
        jQuery('#ptTimeSelectUserSelAmPm').empty().append(h);
    }
    else 
    {
        jQuery('#ptTimeSelectUserSelHr').empty().append(h);
    }
}
jQuery.ptTimeSelect.setMin = function (m) {
    jQuery('#ptTimeSelectUserSelMin').empty().append(m);
}
jQuery.ptTimeSelect.setTime = function () {

    var tSel = jQuery('#ptTimeSelectUserSelHr').text()
				+ ":"
				+ jQuery('#ptTimeSelectUserSelMin').text()
				+ " "
				+ jQuery('#ptTimeSelectUserSelAmPm').text()

    jQuery(".isPtTimeSelectActive").val(tSel);
    this.closeCntr();

}
jQuery.ptTimeSelect.setTime1 = function () {

    var tSel = jQuery('#ptTimeSelectUserSelHr').text()
				+ ":"
				+ jQuery('#ptTimeSelectUserSelMin').text()
				+ " "
				+ jQuery('#ptTimeSelectUserSelAmPm').text();
    if (!(jQuery(".isPtTimeSelectActive").hasClass("noreq"))) {

        var hour = Number(jQuery('#ptTimeSelectUserSelHr').text());
        if (jQuery('#ptTimeSelectUserSelAmPm').text().toLowerCase() == "pm") {
            if (jQuery('#ptTimeSelectUserSelHr').text().toLowerCase() == "12") 
            {
                hour = 12;
            }
            else
            {            
                hour = hour + 12;              
            }

        }
        if (jQuery('#ptTimeSelectUserSelAmPm').text().toLowerCase() == "am") {
            if (jQuery('#ptTimeSelectUserSelHr').text().toLowerCase() == "12")
             {
                hour = 0;
            }
        }

        var seltedTime = HMStoSec1(hour + ":" + jQuery('#ptTimeSelectUserSelMin').text() + ":00");
        var frmTime = HMStoSec1("6:00:00");
        var toTime = HMStoSec1("22:00:00");
        var diff1 = seltedTime - frmTime;
        var diff2 = toTime - seltedTime;
        if (diff1 >= 0 && diff2 >= 0) {
            jQuery(".isPtTimeSelectActive").val(tSel);
            this.closeCntr();
        }
        else {
            alert('Time needs to be between 6.00am to 10.00pm');
        }
    }
    else {
        jQuery(".isPtTimeSelectActive").val(tSel);
        this.closeCntr();
    }

}
jQuery.ptTimeSelect.openCntr = function (ele) {
    jQuery.ptTimeSelect.closeCntr()
    jQuery(".isPtTimeSelectActive").removeClass("isPtTimeSelectActive");
    var cntr = jQuery("#ptTimeSelectCntr");
    var i = jQuery(ele).eq(0).addClass("isPtTimeSelectActive");
    var opt = i.data("ptTimeSelectOptions");
    var style = i.offset();
    style['z-index'] = opt.zIndex;
    style.top = (style.top + $('.hasPtTimeSelect').height()+10);
    if (opt.containerWidth) {
        style.width = opt.containerWidth;
    }
    if (opt.containerClass) {
        cntr.addClass(opt.containerClass);
    }
    cntr.css(style);
    var hr = 9;
    var min = '00';
    var tm = 'AM';
    if (i.val()) {
        var re = /([0-9]{1,2}).*:.*([0-9]{2}).*(PM|AM)/i;
        var match = re.exec(i.val());
        if (match) {
            hr = match[1] || 9;
            min = match[2] || '00';
            tm = match[3] || 'AM';
        }
    }
    cntr.find("#ptTimeSelectUserSelHr").empty().append(hr);
    cntr.find("#ptTimeSelectUserSelMin").empty().append(min);
    cntr.find("#ptTimeSelectUserSelAmPm").empty().append(tm);
    cntr.find(".ptTimeSelectTimeLabelsCntr .ptTimeSelectLeftPane")
		.empty().append(opt.hoursLabel);
    cntr.find(".ptTimeSelectTimeLabelsCntr .ptTimeSelectRightPane")
		.empty().append(opt.minutesLabel);
    cntr.find("#ptTimeSelectSetButton a").empty().append(opt.setButtonLabel);
    if (opt.onBeforeShow) {
        opt.onBeforeShow(i, cntr);
    }
    cntr.slideDown("fast");

}
jQuery.ptTimeSelect.closeCntr = function (i) {
    var e = $("#ptTimeSelectCntr");
    if (e.is(":visible") == true) {

        // If IE, then check to make sure it is realy visible
        if (jQuery.support.tbody == false) {
            if (!(e[0].offsetWidth > 0) && !(e[0].offsetHeight > 0)) {
                return;
            }
        }

        jQuery('#ptTimeSelectCntr')
			.css("display", "none")
			.removeClass()
			.css("width", "");
        if (!i) {
            i = $(".isPtTimeSelectActive");
        }
        if (i) {
            var opt = i.removeClass("isPtTimeSelectActive")
						.data("ptTimeSelectOptions");
            if (opt && opt.onClose) {
                opt.onClose(i);
            }
        }
    }
    return;
}
jQuery.ptTimeSelect._doCheckMouseClick = function (ev) {
    if (!$("#ptTimeSelectCntr:visible").length) {
        return;
    }
    if (!jQuery(ev.target).closest("#ptTimeSelectCntr").length
        && jQuery(ev.target).not("input.isPtTimeSelectActive").length) {
        jQuery.ptTimeSelect.closeCntr();
    }

}
jQuery.fn.ptTimeSelect = function (opt) {
    return this.each(function () {
        if (this.nodeName.toLowerCase() != 'input') return;
        var e = jQuery(this);
        if (e.hasClass('hasPtTimeSelect')) {
            return this;
        }
        var thisOpt = {};
        thisOpt = $.extend(thisOpt, jQuery.ptTimeSelect.options, opt);
        e.addClass('hasPtTimeSelect').data("ptTimeSelectOptions", thisOpt);

        //Wrap the input field in a <div> element with
        // a unique id for later referencing.
        if (thisOpt.popupImage || !thisOpt.onFocusDisplay) {
            var img = jQuery(
						'<span>&nbsp;</span><a href="javascript:" onclick="'
					+ 'jQuery.ptTimeSelect.openCntr(jQuery(this).data(\'ptTimeSelectEle\'));">'
					+ thisOpt.popupImage + '</a>'
				)
				.data("ptTimeSelectEle", e);
            e.after(img);
        }
        if (thisOpt.onFocusDisplay) {
            e.focus(function () {
                jQuery.ptTimeSelect.openCntr(this);
            });
        }
        return this;
    });
}
var secondsPerMinute = 60;
var minutesPerHour = 60;

function convertSecondsToHHMMSS(intSecondsToConvert) {
    var hours = convertHours(intSecondsToConvert);
    var minutes = getRemainingMinutes(intSecondsToConvert);
    minutes = (minutes == 60) ? "00" : minutes;
    var seconds = getRemainingSeconds(intSecondsToConvert);
    return hours + ":" + minutes;
}

function convertHours(intSeconds) {
    var minutes = convertMinutes(intSeconds);
    var hours = Math.floor(minutes / minutesPerHour);
    return hours;
}
function convertMinutes(intSeconds) {
    return Math.floor(intSeconds / secondsPerMinute);
}
function getRemainingSeconds(intTotalSeconds) {
    return (intTotalSeconds % secondsPerMinute);
}
function getRemainingMinutes(intSeconds) {
    var intTotalMinutes = convertMinutes(intSeconds);
    return (intTotalMinutes % minutesPerHour);
}

function HMStoSec1(T) { // h:m:s
    var A = T.split(/\D+/); return (A[0] * 60 + +A[1]) * 60 + +A[2]
}