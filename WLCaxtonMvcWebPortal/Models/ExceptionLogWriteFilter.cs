using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WLCaxtonMvcWebPortal.Util;

namespace WLCaxtonMvcWebPortal.Models
{
    /// <summary>Author: VRT:12/6/2018
    /// Exception Log Write Filter
    /// </summary>
    public class ExceptionLogWriteFilter : FilterAttribute, IExceptionFilter
    {
        void IExceptionFilter.OnException(ExceptionContext filterContext)
        {
            string logFilePath = HttpContext.Current.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt");
            Exception e = filterContext.Exception;
            CommonMethods.LogFileWrite(e.Message, e.InnerException == null ? "" : e.InnerException.Message, logFilePath);
            filterContext.ExceptionHandled = true;

        }
    }
}