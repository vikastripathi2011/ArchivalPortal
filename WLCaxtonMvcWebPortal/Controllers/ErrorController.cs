using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WLCaxtonMvcWebPortal.Controllers
{
    public class ErrorController : Controller
    {
       /// <summary>
       /// Error handler
       /// </summary>
       /// <param name="statusCode"></param>
       /// <param name="exception"></param>
       /// <returns></returns>
        public ActionResult Error(int statusCode, Exception exception)
        {
            Response.StatusCode = statusCode;
            return View();
        }
    }
}