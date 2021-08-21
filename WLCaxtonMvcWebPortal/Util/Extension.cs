//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Extension.cs
// Program Description  : This class is use to convert the date into different format.
// Programmed By        : Satyendra Gupta
// Programmed On        : 31-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLCaxtonMvcWebPortal.Util
{
    internal static class Extension
    {
        /// <summary>
        /// Convert the datetime format to yyyy-MM-dd format. This is used to in sql stements.
        /// </summary>
        /// <param name="date">Source date to be formatted in db format</param>
        /// <returns></returns>
        internal static string DateToDBFormat(this DateTime? date)
        {
            //return date.Value.ToString("yyyy-MM-dd");
            return date.Value.ToString("yyyyMMdd");
        }



        internal static string DateToDBFormat(this DateTime date)
        {
            //return date.Value.ToString("yyyy-MM-dd");
            return date.ToString("yyyyMMdd");
        }
    }
}