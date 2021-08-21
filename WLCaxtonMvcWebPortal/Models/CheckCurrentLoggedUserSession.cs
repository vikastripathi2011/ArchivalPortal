using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WLCaxtonMvcWebPortal.Util;

namespace WLCaxtonMvcWebPortal.Models
{

    /// <summary>Author: VRT:12/6/2018
    /// Check to Current Logged UserSession
    /// </summary>
    public class CheckCurrentLoggedUserSession : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string CollectionActionNames = "userlogin,logoff,redirecttolocal,checkusersession,_contactus,confirmuserlogin,removepagelock";
            string CollectionActionNames = "userlogin,logoff,_contactus,confirmuserlogin";
            string CurrentActionName = filterContext.ActionDescriptor.ActionName.ToLower();
            if (!CollectionActionNames.Contains(CurrentActionName))
            {
                if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] == null)
                {
                    SessionClass.SessionClear();
                    HttpContext.Current.Session.Abandon();
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new RedirectResult("~/Login/RedirectToLogin");
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Login/LogOff");
                    }
                }
                //else
                //{
                //    filterContext.Result = new RedirectResult("~/Login/LogOff");
                //}
                
                //if (SessionClass.loggedUser.LoginDetails != null)
                //{
                //    if (SessionClass.loggedUser.LoginDetails.EmailId == "" || SessionClass.loggedUser.LoginDetails.RecordId == 0)
                //    {
                //        SessionClass.SessionClear();
                //        HttpContext.Current.Session.Abandon();
                //        if (filterContext.HttpContext.Request.IsAjaxRequest())
                //        {
                //            filterContext.Result = new RedirectResult("~/Login/RedirectToLogin");
                //        }
                //        else
                //        {
                //            filterContext.Result = new RedirectResult("~/Login/LogOff");
                //        }
                //    }
                //}
                //else
                //{
                //    filterContext.Result = new RedirectResult("~/Login/LogOff");
                //}

            }

        }
    }
}