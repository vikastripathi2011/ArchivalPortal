using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ServiceModel;
using System.Web.UI;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalExceptionLogger;
using WLCaxtonMvcWebPortal.Services;
using WLCaxtonMvcWebPortal.Util;
using System.Web.Security;
using WLCaxtonMvcWebPortal.Base;
using WLCaxtonMvcWebPortal.Models;
using WLCaxtonMvcWebPortal.Code;
using static WLCaxtonMvcWebPortal.Util.ControlUtil;

namespace WLCaxtonMvcWebPortal.Controllers
{
    public class LoginController : Controller
    {

        #region "Object Declration ==>>"
        WLCaxtonPortalBusinessEntity.Login login;
        public readonly string siteroot = System.Configuration.ConfigurationManager.AppSettings["siteroot"] == null ? " " : System.Configuration.ConfigurationManager.AppSettings["siteroot"];
        DateTime LastLoginTime;
        long IsExistUser = 0;
        string UniqueNumber = string.Empty;
        #endregion
        Common common;
         

        public LoginController()
        {
            //soiServiceProxy = new SoiServiceClient();
            common = new Common();
         }
        /// <summary>
        /// User login post action
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsUserLogin(WLCaxtonPortalBusinessEntity.Login objLogin)
        {
            try
            {
                WLCaxtonPortalServiceClient serviceClient = new WLCaxtonPortalServiceClient();
                // serviceClient.ClientCredentials.Windows.AllowNTLM = True
                User user = new User();
                if (SessionClass.loggedUser.LoginDetails == null)
                    login = new WLCaxtonPortalBusinessEntity.Login();
                else
                    login = SessionClass.loggedUser.LoginDetails;

                login.EmailId = objLogin.EmailId.Trim();
                login.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(objLogin.Password.Trim(), "SHA1");
                login.SessionId = Session.SessionID;
                login.SystemIP = Request.UserHostAddress;

                common.AddMessageHeader(serviceClient);
                user = serviceClient.AuthenticateUser(login);
                serviceClient.Close();

                SessionClass.loggedUser = user;
                SessionClass.loggedUser.UserEmailId = user.UserEmailId;
                SessionClass.loggedUser.UserId = user.UserId;
                SessionClass.loggedUser.FirstName = user.FirstName;
                SessionClass.loggedUser.LastName = user.LastName;

                if (user.LoginDetails.IsAuthenticated)
                {
                //SessionClass.loggedUser = user;
                //SessionClass.loggedUser.UserEmailId = user.UserEmailId;
                //SessionClass.loggedUser.UserId = user.UserId;
                //SessionClass.loggedUser.FirstName = user.FirstName;
                //SessionClass.loggedUser.LastName = user.LastName;
                    // Create forms authentication ticket
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                    user.LoginDetails.EmailId,// EmailID to be associated with this ticket
                    DateTime.Now, // Date/time ticket was issued
                    DateTime.Now.AddMinutes(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["SessionLogoutTime"])), // Date and time the cookie will expire
                    true, // if user has chcked rememebr me then create persistent cookie
                    user.RoleDetails.Name, // Role for various page
                    FormsAuthentication.FormsCookiePath); // Cookie path specified in the web.config file in <Forms> tag if any.

                    // To give more security it is suggested to hash it
                    string hashCookies = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies); // Hashed ticket
                    cookie.Expires=DateTime.Now.AddMinutes(Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["SessionLogoutTime"]));
                    // Add the cookie to the response, user browser
                    Response.Cookies.Add(cookie);
                    ViewBag.ChkUser = user.LoginDetails.EmailId;
                    UniqueNumber = Guid.NewGuid().ToString("n").Substring(0, 6);
                    Response.Cookies["UserUniqueNumber"].Value = UniqueNumber;

                    if (user.LoginDetails.IsTemporaryPassword)
                    {
                        var ResultMsg = user.LoginDetails.IsTemporaryPassword;
                        LoggerFactory.Logger.Log("Login Success", LogEventType.Info, "User " + user.LoginDetails.EmailId + ", Login Successfull  with passowrd change required.");
                        return Json(ResultMsg, JsonRequestBehavior.AllowGet);
                    }
                    else if (user.LoginDetails.IsPasswordExpired)
                    {
                        var ResultMsg = user.LoginDetails.IsPasswordExpired;
                        LoggerFactory.Logger.Log("Login failed", LogEventType.Info, "User " + user.LoginDetails.EmailId + ", Login failed because user credential has been expired.");
                        return Json(ResultMsg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LoggerFactory.Logger.Log("Login Success", LogEventType.Info, "User " + user.LoginDetails.EmailId + ", Login  Successfully.");
                        var ResultMsg = (int)WLCaxtonPortalBusinessEntity.UserType.User;
                        if (user.RoleDetails.RecordId == (int)WLCaxtonPortalBusinessEntity.UserType.User)
                        {
                            //Response.Redirect("Forms/UserActvity/Search.aspx", false);
                            return Json(new { SearchCode = ResultMsg }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            // Response.Redirect("Forms/UserMaintenance/UserList.aspx", false);
                            return Json(ResultMsg, JsonRequestBehavior.AllowGet);

                        }
                    }

                }
                else
                {
                    LoggerFactory.Logger.Log(Login_STATUS.LFL.ToString(), LogEventType.Info, "User " + user.LoginDetails.EmailId + "," + Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", user.MessageDetails.MessageId)));
                    ViewBag.lblMessage = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", user.MessageDetails.MessageId));
                    return Json(new { errorCode = "-2", errorMsg = ViewBag.lblMessage }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("Default", LogEventType.Error, ex.Message);
                ViewBag.lblMessage = Resources.RSAPortalResource.M00080;
            }

            return Json(new { errorCode = "-2", errorMsg = ViewBag.lblMessage }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// User login get action
        /// </summary>
        /// <returns></returns>

        public ActionResult UserLogin()
        {
            String pwdPath = System.Configuration.ConfigurationManager.AppSettings["RSA_HCP_PwdPath"];


            ViewBag.lblMessage = "";
            //default page load 
            if (Request.QueryString["PageName"] != null)
            {

                // Bug reported by Ray on 18/2/2013 miltiple session after logout fix
                WLCaxtonPortalServiceClient serviceClient = new WLCaxtonPortalServiceClient();
                if (SessionClass.loggedUser != null && SessionClass.loggedUser.LoginDetails != null && SessionClass.loggedUser.LoginDetails.EmailId != null)
                {
                    common.AddMessageHeader(serviceClient);
                    serviceClient.Logout(SessionClass.loggedUser);
                    ViewBag.lblMessage = Resources.RSAPortalResource.M00025;
                }
                else
                {
                    ViewBag.lblMessage = Resources.RSAPortalResource.M00061;
                }

            }
            // if (!IsPostBack)
            {
                if (Convert.ToString(Request.QueryString["msg"]) == "1" || Convert.ToString(Request.QueryString["msg"]) == "2")
                {
                    ViewBag.lblMessage = Resources.RSAPortalResource.M00019;
                    SessionClass.loggedUser = null;
                }
                if (Convert.ToString(Request.QueryString["msg"]) == "3")
                {
                    ViewBag.lblMessage = Resources.RSAPortalResource.M00020;
                    SessionClass.loggedUser = null;
                }
                if (Convert.ToString(Request.QueryString["msg"]) == "4")
                {
                    ViewBag.lblMessage = Resources.RSAPortalResource.M00075;
                    SessionClass.loggedUser = null;
                }
                if (Convert.ToString(Request.QueryString["msg"]) == "5")
                {
                    ViewBag.lblMessage = Resources.RSAPortalResource.M00084;
                    SessionClass.loggedUser = null;
                }
            }
            return View();

            // return View();

        }

        /// <summary>
        /// This action will render landing page
        /// </summary>
        /// <returns></returns>
        [CheckCurrentLoggedUserSession]
        public ActionResult Home()
        {
            try
            {
                ViewBag.ChkUser = SessionClass.loggedUser.LoginDetails.EmailId;

                if (ViewBag.ChkUser != "")
                {
                    ViewBag.MSG = "<script>alert('Login Success');</script>";
                    return View();
                }
                else
                {
                    return Redirect(siteroot);

                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("Default", LogEventType.Error, ex.Message);
                ViewBag.lblMessage = Resources.RSAPortalResource.M00080;
            }
            return Json(new { errorMsg = ViewBag.lblMessage }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Logoff post action
        /// </summary>
        /// <returns></returns>
      //  [HttpPost]
        public ActionResult LogOff()
        {

            if (SessionClass.loggedUser.LoginDetails != null)
            {
                if (SessionClass.loggedUser.LoginDetails.EmailId != "" || SessionClass.loggedUser.LoginDetails.RecordId > 0)
                {
                    Logout(false);
                    LoggerFactory.Logger.Log("Loggoff Success", LogEventType.Info, "User, Logoff  Successfully.");
                    return RedirectToAction("UserLogin", "Login", new { PageName = "myInput" });
                }
            }
            else
            {
                Session.Abandon();
                LoggerFactory.Logger.Log("Loggoff Success", LogEventType.Info, "User, Session has been expired.");

            }
            // return RedirectToAction("UserLogin", "Login");
            return RedirectToAction("UserLogin", "Login", new { PageName = "myInput" });
        }

        public ActionResult _ContactUs()
        {
            return PartialView();
        }

        /// <summary>
        /// Login to current user and Log out user login from another place.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //Not Required
        public ActionResult ConfirmUserLogin(Login model)
        {
            if (SessionClass.loggedUser.UserEmailId != "" && SessionClass.loggedUser.UserId > 0)
            {
                UniqueNumber = Guid.NewGuid().ToString("n").Substring(0, 6);

                Response.Cookies["UserUniqueNumber"].Value = UniqueNumber;

            }
            else
            {
                Response.Cookies["UserUniqueNumber"].Value = UniqueNumber;

            }
            return Json(true);
        }

        /// <summary>
        /// this action use to check user session
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckUserSession()
        {
            var IsAuthorizeUser = default(bool);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            string ResultMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(SessionClass.loggedUser.LoginDetails.ToString()))
                {
                    //Response.Redirect("~/Default.aspx?msg=4");
                    ResultMsg = "?msg=4";
                    return Json(ResultMsg, JsonRequestBehavior.AllowGet);
                }

                Login login = SessionClass.loggedUser.LoginDetails;

                WLCaxtonPortalServiceClient serviceClient =new WLCaxtonPortalServiceClient();
                common.AddMessageHeader(serviceClient);
                Message message = serviceClient.CheckUserSession(SessionClass.loggedUser);

                if (message.IsSuccess)
                {

                    //if (login != null)
                    //{
                    //   // lblUser.Text = login.EmailId;

                    //    if (login.IsPasswordExpired == true || login.IsTemporaryPassword == true)
                    //    {
                    //       // lftMenu.Visible = false;
                    //    }
                    //    else
                    //       /// lftMenu.Visible = true;
                    //}
                }
                else
                {
                    if (message.MessageId.Equals("M00084"))
                    {
                        //Response.Redirect("~/Default.aspx?msg=5");
                        ResultMsg = "?msg=5";
                    }
                    else if (SessionClass.IsChangedPwd == false && !message.MessageId.Equals("M00084"))
                    {
                        //Response.Redirect("~/Default.aspx?msg=3");
                        ResultMsg = "?msg=3";
                    }
                    else
                    {
                        if (login.IsPasswordExpired)
                        {

                            // Response.Redirect("~/Default.aspx?msg=1", false);
                            ResultMsg = "?msg=1";
                        }
                        else if (login.IsTemporaryPassword)
                        {
                            ResultMsg = "?msg=2";
                            // Response.Redirect("~/Default.aspx?msg=2", false);
                        }
                        else
                        {
                            ResultMsg = "";
                            // Response.Redirect("~/Home.aspx", false);
                        }
                    }

                }
                return Json(ResultMsg, JsonRequestBehavior.AllowGet);
            }

            catch (System.Threading.ThreadAbortException tex)
            {

            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Default.aspx?msg=4");
                ResultMsg = "?msg=4";
            }
            return Json(ResultMsg,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Redirect to login page
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToLogin()
        {
            SessionClass.SessionClear();
            Session.Abandon();
            ViewBag.LogoutUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["logout"]);
            //return View();
            // return RedirectToAction("UserLogin", "Login");
            return RedirectToRoute(siteroot);
        }

        /// <summary>
        /// This action is reponsible to render forgot password page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            Session.Abandon();
            Session.Clear();
            return View();
        }

        /// <summary>
        /// Password recovery action
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(Login model)
        {
            Services.WLCaxtonPortalServiceClient proxy = new Services.WLCaxtonPortalServiceClient();
            try
            {
                if (ModelState.IsValid)
                {

                    Login objLogin = new Login
                    {
                        EmailId = model.EmailId
                    };
                    AuditTrail objAuditTrial = new AuditTrail
                    {
                        ModifiedBy = -1, //As user is not logged in, we use -1 as no user logged in
                        ModifiedOn = DateTime.Now
                    };
                    common.AddMessageHeader(proxy);
                    Message message = proxy.ForgetPassword(objLogin, objAuditTrial);
                    if (message != null)
                    {
                        ViewBag.Message =message.Description;// Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                        proxy.Close();
                        return Json(new { ResultMsg = ViewBag.Message, MessageCode = message.MessageId }, JsonRequestBehavior.AllowGet);
                    }

                }

            }
            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log(Login_STATUS.FPWD.ToString(), LogEventType.Error, fault.Message);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                ViewBag.lblMessage = Resources.RSAPortalResource.M00080;
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log(Login_STATUS.FPWD.ToString(), LogEventType.Error, msg);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                ViewBag.lblMessage = Resources.RSAPortalResource.M00080;
            }
            return Json(new { errorCode = "-2", errorMsg = ViewBag.lblMessage, MessageCode = Resources.RSAPortalResource.M00080 }, JsonRequestBehavior.AllowGet);
        }

        private bool Logout(bool isCallbackCall)
        {
            WLCaxtonPortalServiceClient proxy = new WLCaxtonPortalServiceClient();
            bool isLogOff = false;
            try
            {
                Message message = new Message();
                proxy = new WLCaxtonPortalServiceClient();

                User user = new User();

                User loggedUser = SessionClass.loggedUser;
                AuditTrail objAuditTrial = new AuditTrail();
                objAuditTrial.ModifiedBy = loggedUser.LoginDetails.RecordId;
                objAuditTrial.ModifiedOn = DateTime.Now;

                user.UserActivity = UserActivityType.Logout;
                user.LoginDetails = new WLCaxtonPortalBusinessEntity.Login()
                {
                    RecordId = loggedUser.LoginDetails.RecordId,
                    SessionId = Session.SessionID,
                    SystemIP = Request.UserHostAddress
                };
                common.AddMessageHeader(proxy);
                message = proxy.Logout(user);
                if (message.IsSuccess)
                {
                    isLogOff = message.IsSuccess;
                    // message.
                }
                else
                    isLogOff = false;

                //For grace full close
                proxy.Close();
                Session.Abandon();

                FormsAuthentication.SignOut();
                if (isCallbackCall == false)
                    FormsAuthentication.RedirectToLoginPage();

                return isLogOff;

            }
            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log("MasterPage", LogEventType.Error, fault.Message);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                throw;
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log("MasterPage", LogEventType.Error, msg);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                throw;
            }
        }
         [HttpPost]
        public JsonResult LogOffUser()
        
        {

            if (SessionClass.loggedUser.LoginDetails != null)
            {
                if (SessionClass.loggedUser.LoginDetails.EmailId != "" || SessionClass.loggedUser.LoginDetails.RecordId > 0)
                {
                    Logout(false);
                    LoggerFactory.Logger.Log("Loggoff Success", LogEventType.Info, "User, Logoff  Successfully.");
                    
                }
            }
            else
            {
                WLCaxtonPortalServiceClient serviceClient = new WLCaxtonPortalServiceClient();
                string systemIP = string.Empty;
                systemIP = Request.UserHostAddress;
                    User user = new User();
                    user.UserActivity = UserActivityType.SessionExpired;
                    user.UserId = 0;
                    Login objLogIn = new Login();
                    objLogIn.SessionId = "";
                    objLogIn.SystemIP = systemIP;
                    user.LoginDetails = objLogIn;
                    common.AddMessageHeader(serviceClient);
                    serviceClient.Logout(user);
                    FormsAuthentication.SignOut();
                    LoggerFactory.Logger.Log("Loggoff Success", LogEventType.Info, "User, Session has been expired.");

            }
            // return RedirectToAction("UserLogin", "Login");
            return Json(true);
        }

    }

}
