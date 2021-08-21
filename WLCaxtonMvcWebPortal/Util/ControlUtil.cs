//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ControlUtil.cs
// Program Description  : This class is use to handle the control items.
// Programmed By        : Satyendra Gupta
// Programmed On        : 31-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WLCaxtonMvcWebPortal.Util
{
    internal static class ControlUtil
    {
        /// <summary>
        /// This function returns the operans into list to the control.
        /// </summary>
        /// <returns></returns>
        internal static ListItem[] GetOperands()
        {
            ListItem[] items = new ListItem[8];
            items[0] = GetSelectListItem();
            items[1] = new ListItem("=", "=");
            items[2] = new ListItem("! =", "! =");
            items[3] = new ListItem("<=", "<=");
            items[4] = new ListItem("<", "<");
            items[5] = new ListItem(">", ">");
            items[6] = new ListItem(">=", ">=");
            items[7] = new ListItem("><", " between ");
            return items;
        }
        /// <summary>
        /// This function returns the ListItems. 
        /// </summary>
        /// <returns></returns>
        internal static ListItem GetSelectListItem()
        {
            return new ListItem("Select", "");
        }

        /// <summary>
        /// created by : VRT
        /// created date :9/08/2018
        /// discription: add enum to know Login STATUS.
        /// </summary>
        public enum Login_STATUS
        {
            [Description("ChangePassword")]
            CPWD,
            [Description("ResetPassword")]
            RPWD,
            [Description("Login failed")]
            LFL,
            [Description("ForgotPassword")]
            FPWD
        }
    }
}