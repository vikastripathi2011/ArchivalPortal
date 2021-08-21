using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WLCaxtonMvcWebPortal.Models;
using WLCaxtonMvcWebPortal.Services;
using WLCaxtonMvcWebPortal.Util;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonMvcWebPortal.Code;
using WLCaxtonPortalExceptionLogger;
using Resources;

namespace WLCaxtonMvcWebPortal.Areas.UserMaintenance.Controllers
{
    [CheckCurrentLoggedUserSession]
    public class UserMntnancController : Controller
    {

        WLCaxtonPortalServiceClient proxy;
        Common common;


        public UserMntnancController()
        {
            common = new Common();
        }
        /// <summary>
        /// This method is use to list all users based on role
        /// </summary>
        public ActionResult UserListing()
        {
            try
            {

                proxy = new WLCaxtonPortalServiceClient();
                User loggedUser = SessionClass.loggedUser;
                common.AddMessageHeader(proxy);
                if (TempData["Message"] != null)
                {
                    ViewBag.Message = Convert.ToString(TempData["Message"]);
                }
                if (loggedUser.RoleDetails.RecordId == (int)UserType.WLSuperUser)
                {
                    userList = proxy.GetUserList();
                }
                else if (loggedUser.RoleDetails.RecordId == (int)UserType.TenantAdmin)
                {
                    userList = proxy.GetUserList().Where(q => q.RoleDetails.RecordId != (int)UserType.WLSuperUser).ToList();

                }
                if (userList != null)
                {
                    var query = from u in userList where u.RoleDetails.RecordId == (int)UserType.TenantAdmin && u.StatusId == (int)WLCaxtonPortalBusinessEntity.Status.Active select u;
                    if (query.FirstOrDefault() != null)
                    {

                        ViewBag.MessageForTentantUser = string.Format(RSAPortalResource.MessageForTentantUser, query.FirstOrDefault().RoleDetails.Name, query.Count());
                    }
                    else
                    {
                        ViewBag.MessageForTentantUser = string.Format(RSAPortalResource.MessageForTentantUser0);
                    }
                    query = from u in userList where u.RoleDetails.RecordId == (int)UserType.User && u.StatusId == (int)WLCaxtonPortalBusinessEntity.Status.Active select u;
                    if (query.FirstOrDefault() != null)
                    {
                        ViewBag.MessageForActiveUser = string.Format(RSAPortalResource.MessageForTentantUser, query.FirstOrDefault().RoleDetails.Name, query.Count());
                    }
                    else
                    {
                        ViewBag.MessageForActiveUser = string.Format(RSAPortalResource.MessageForActiveUser);
                    }
                    query = from u in userList where u.RoleDetails.RecordId == (int)UserType.WLSuperUser && u.StatusId == (int)WLCaxtonPortalBusinessEntity.Status.Active select u;
                    if (query.FirstOrDefault() != null)
                    {
                        ViewBag.MessageForActiveSuperUser = string.Format(RSAPortalResource.MessageForTentantUser, query.FirstOrDefault().RoleDetails.Name, query.Count());
                    }
                    else
                    {
                        ViewBag.MessageForActiveSuperUser = string.Format(RSAPortalResource.MessageForActiveSuperUser);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserList", LogEventType.Error, ex.Message);
                if (proxy != null)
                {
                    common.AddMessageHeader(proxy);
                    proxy.Abort();
                }


            }
            return View("UserListing", userList);
        }

        /// <summary>
        /// This method will allow to change password
        /// </summary>
        /// <param name="objLogin"></param>
        /// <returns></returns>
        public ActionResult ChangePassword(WLCaxtonPortalBusinessEntity.Login objLogin)
        {

            if (SessionClass.loggedUser.LoginDetails.EmailId != null || SessionClass.loggedUser.LoginDetails.EmailId != "")
            {
                return View(objLogin);
            }
            else
            {

            }
            return View(objLogin);
        }

        /// <summary>
        /// This method help to create new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult CreateUser(User model)
        {
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
            return View();
        }


        #region "Method ==>>"

        /// <summary>
        /// This property help to return user list in IList
        /// </summary>
        /// <returns></returns>
        private IList<User> userList
        {
            get
            {
                IList<User> userList = null;
                if (TempData["UserList"] != null)
                    userList = (IList<User>)TempData["UserList"];

                return userList;
            }
            set { TempData["UserList"] = value; }
        }
        #endregion

    }
}