using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using WLCaxtonMvcWebPortal.Models;
using WLCaxtonMvcWebPortal.Services;
using WLCaxtonMvcWebPortal.Util;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalExceptionLogger;
using WLCaxtonMvcWebPortal.Code;
using static WLCaxtonMvcWebPortal.Util.ControlUtil;

namespace WLCaxtonMvcWebPortal.Areas.UserMaintenance.Controllers
{
    [CheckCurrentLoggedUserSession]
    public class DataController : Controller
    {

        WLCaxtonPortalServiceClient proxy;
        Common common;


        public DataController()
        {
            common = new Common();
        }
        /// <summary>
        /// Change Password for User for Post request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(Login model)
        {
            WLCaxtonPortalServiceClient proxy = null;
            try
            {
                if (ModelState.IsValid)
                {
                    char[] notAllowedChars = new char[] { ' ', ':', '/', '?', '|', '[', ']', '=', ',', '<', '>', '*', '{', '}', '.', '\\', '"', '\'' };
                    if (model.NewPassword.IndexOfAny(notAllowedChars) > -1)
                    {
                        ViewBag.Message = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", "revNewPassword"));

                        //return View(model);
                        return Json(new { ResultMsg = ViewBag.Message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        proxy = new WLCaxtonPortalServiceClient();
                       
                        WLCaxtonPortalBusinessEntity.User user = new WLCaxtonPortalBusinessEntity.User();
                        User loggedUser = SessionClass.loggedUser;

                        AuditTrail auditTrail = new AuditTrail { ModifiedBy = loggedUser.LoginDetails.RecordId, ModifiedOn = DateTime.Now };

                        WLCaxtonPortalBusinessEntity.Login login = new WLCaxtonPortalBusinessEntity.Login
                        {
                            Password = model.Password.Trim(),
                            NewPassword = model.NewPassword.Trim(),
                            EmailId = loggedUser.LoginDetails.EmailId
                        };

                        WLCaxtonPortalBusinessEntity.Tenant tenant = new WLCaxtonPortalBusinessEntity.Tenant
                        {
                            RecordId = loggedUser.TenantDetails.RecordId
                        };

                        user.LoginDetails = login;
                        user.TenantDetails = tenant;
                        common.AddMessageHeader(proxy);
                        Message message = proxy.ChangePassword(user, auditTrail);

                        if (message.IsSuccess)
                        {
                            SessionClass.IsChangedPwd = true;
                            //common.AddMessageHeader(proxy);
                            message = proxy.Logout(loggedUser);
                            Session.Clear();
                            if (loggedUser.LoginDetails.IsPasswordExpired || loggedUser.LoginDetails.IsTemporaryPassword)
                            {
                                //message = proxy.Logout(loggedUser);
                                if (message.IsSuccess)
                                {
                                    //return RedirectToAction("LogIn");  //Response.Redirect("~/Default.aspx?msg=1", false);
                                   // Session.Clear();
                                    return Json(new { ResultMsg = message.IsSuccess, MessageCode = message.MessageId }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                ViewBag.Message = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                            }
                        }
                        else
                        {
                            ViewBag.Message = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                        }

                        proxy.Close();
                        return Json(new { errorCode = "-2", errorMsg = ViewBag.Message, MessageCode = message.MessageId }, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch (Exception ex)
            {
                if (proxy != null)
                {
                    common.AddMessageHeader(proxy);
                 proxy.Abort();
                }
                LoggerFactory.Logger.Log(Login_STATUS.CPWD.ToString(), LogEventType.Error, ex.Message);
                ViewBag.Message = HttpContext.GetGlobalResourceObject("RSAPortalResource", "M00080");
            }
            return View(model);
        }

        /// <summary>
        /// Change Password for User for Get request
        /// </summary>
        public ActionResult cahngepassword()
        {
            return Json(new { ResultMsg = ViewBag.Message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Register user for Post request
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RegisterUser(User model)
        {
            //int UserId = Convert.ToInt16(UseId);
            User user = null;
            IList<Role> documentTypes = null;
            try
            {
                documentTypes = CommonBussinesLogic.GetRoles();
                proxy = new WLCaxtonPortalServiceClient();
                common.AddMessageHeader(proxy);
                if (model.UserId > 0)
                    user = proxy.GetUserList().Where(q => q.UserId == model.UserId).FirstOrDefault();

                if (documentTypes != null)
                {
                    ViewBag.Roles = documentTypes;
                }
                proxy.Close();
            }
            catch (Exception ex)
            {
                if (proxy != null)
                {
                    common.AddMessageHeader(proxy);
                    proxy.Abort();
                }
                LoggerFactory.Logger.Log("RegisterUser", LogEventType.Error, ex.Message);
            }
            return PartialView("_UserRegistration", user);
        }

        /// <summary>
        /// This action will help for CURD operation for user model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult CRUD_User(User model)
        {
            WLCaxtonPortalServiceClient proxy = new WLCaxtonPortalServiceClient();
            try
            {
                if (ModelState.IsValid)
                {
                    User loggedUser = SessionClass.loggedUser;
                    Message message = new Message();

                    proxy = new WLCaxtonPortalServiceClient();

                    User newuser = new User
                    {
                        RoleDetails = new Role() { RecordId = model.RoleDetails.RecordId },
                        UserEmailId = model.UserEmailId,
                        FirstName = model.FirstName.Trim(),
                        LastName = model.LastName.Trim(),
                        UserId = model.UserId,
                        LoginDetails = loggedUser.LoginDetails,
                        TenantDetails = loggedUser.TenantDetails,
                        UserActivity = UserActivityType.CreateUser
                    };
                    common.AddMessageHeader(proxy);
                    if (newuser.UserId > 0)
                    {
                        message = proxy.AddUpdateUser(newuser, false);
                    }
                    else
                    {
                        message = proxy.AddUpdateUser(newuser, true);
                    }
                    ViewBag.Message = message.Description;//Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                    //For grace full close
                    proxy.Close();
                    return Json(new { ResultMsg = ViewBag.Message, MessageCode = message.MessageId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ModelState.AddModelError(
                }

            }
            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log("UserList", LogEventType.Error, fault.Message);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log("UserList", LogEventType.Error, msg);
               if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
               }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("UserList");
        }



        /// <summary>
        /// This post action will help to change the user status
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeStatus(User model)
        {
            Message message = new Message();
            try
            {
               
                proxy = new WLCaxtonPortalServiceClient();

                User user = new User
                {
                    LoginDetails = new WLCaxtonPortalBusinessEntity.Login() { RecordId = model.UserId },

                    UserActivity = (model.StatusId == (int)WLCaxtonPortalBusinessEntity.Status.Active ? UserActivityType.SetInActive : UserActivityType.SetActive)
                };

                User loggedUser = SessionClass.loggedUser;

                AuditTrail objAuditTrial = new AuditTrail
                {
                    ModifiedBy = loggedUser.LoginDetails.RecordId,
                    ModifiedOn = DateTime.Now
                };
                common.AddMessageHeader(proxy);
                message = proxy.UpdateUserAccount(user, objAuditTrial);
                ViewBag.Message = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                //For grace full close
                proxy.Close();
            }

            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log("ChangeStatus", LogEventType.Error, fault.Message);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log("ChangeStatus", LogEventType.Error, msg);
                ViewBag.Message = msg;
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }
            //return RedirectToAction("UserList");
            return Json(new { ResultMsg = ViewBag.Message, MessageCode = message.MessageId }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// This post action will help to reset the user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(User model)
        {
            Message message = new Message();
            try
            {
                
                proxy = new WLCaxtonPortalServiceClient();

                User loggedUser = SessionClass.loggedUser;
                AuditTrail objAuditTrial = new AuditTrail
                {
                    ModifiedBy = loggedUser.LoginDetails.RecordId,
                    ModifiedOn = DateTime.Now
                };


                WLCaxtonPortalBusinessEntity.Login login = new WLCaxtonPortalBusinessEntity.Login() { EmailId = model.UserEmailId };
                Tenant tenant = new Tenant() { RecordId = loggedUser.TenantDetails.RecordId };
                WLCaxtonPortalBusinessEntity.User user = new WLCaxtonPortalBusinessEntity.User
                {
                    UserId = model.UserId,
                    LoginDetails = login,
                    TenantDetails = tenant
                };
                common.AddMessageHeader(proxy);
                message = proxy.ResetPassword(user, objAuditTrial);

                ViewBag.Message = message.Description;//Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                //For grace full close
                proxy.Close();
            }

            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log(WLCaxtonMvcWebPortal.Util.ControlUtil.Login_STATUS.RPWD.ToString(), LogEventType.Error, fault.Message);
              if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
              }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log(WLCaxtonMvcWebPortal.Util.ControlUtil.Login_STATUS.RPWD.ToString(), LogEventType.Error, msg);
                ViewBag.Message = msg;
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { ResultMsg = ViewBag.Message , MessageCode= message.MessageId }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This post action will help to unlock the user account
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnlockAcc(User model)
        {
            Message message = new Message();
            try
            {
               
                proxy = new WLCaxtonPortalServiceClient();

                User user = new User
                {
                    LoginDetails = new WLCaxtonPortalBusinessEntity.Login() { RecordId = model.UserId },
                    UserActivity = UserActivityType.AccountUnlock
                };

                User loggedUser = SessionClass.loggedUser;

                AuditTrail objAuditTrial = new AuditTrail
                {
                    ModifiedBy = loggedUser.LoginDetails.RecordId,
                    ModifiedOn = DateTime.Now
                };
                common.AddMessageHeader(proxy);
                message = proxy.UpdateUserAccount(user, objAuditTrial);

                ViewBag.Message = Convert.ToString(HttpContext.GetGlobalResourceObject("RSAPortalResource", message.MessageId));
                //For grace full close
                proxy.Close();

            }

            catch (FaultException<ServiceException> fault)
            {
                LoggerFactory.Logger.Log("UnlockAcc", LogEventType.Error, fault.Message);
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }
            //If service is down or some communication channel exception
            catch (CommunicationException commException)
            {
                string msg = "Service is not running, Error: -" + commException.Message;
                LoggerFactory.Logger.Log("UnlockAcc", LogEventType.Error, msg);
                ViewBag.Message = msg;
                if(proxy!=null)
                {
                common.AddMessageHeader(proxy);
                proxy.Abort();
                }
                return Json(new { ResultMsg = RSAPortalResource.M00080, MessageCode = "M00080" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { ResultMsg = ViewBag.Message, MessageCode = message.MessageId }, JsonRequestBehavior.AllowGet);
        }
    }
}