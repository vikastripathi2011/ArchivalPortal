//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : UserBL.cs
// Program Description  : This class is used for Users realated opeartion
// Programmed By        : Nadeem IshraT.
// Programmed On        : 01-jAN-2013
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalBusinessEntity.Enums;
using WLCaxtonPortalBusinessLogicLayer.Rules;
using WLCaxtonPortalDataLayer.Classes;
//using WLCaxtonPortalEmailComponent;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonPortalBusinessLogicLayer
{
    /// <summary>
    /// This class is used for Users realated opeartion
    /// </summary>
    public class UserBL
    {
        /// <summary>
        /// To Send Message in case of forget password
        /// </summary>
        /// <param name="login"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public static Message ForgetPassword(Login login, AuditTrail auditTrail)
        {
            try
            {
                string hashPassword = string.Empty;
                string password = string.Empty;

                password = PasswordRules.GeneratePassword(8, 5, out hashPassword);
                login.NewPassword = hashPassword;

                if (UserDL.ForgetPassword(login, auditTrail))
                {                   
                    IList<User> users = new UserDL().GetUserList();

                    var userQuery = from u in users.ToList<User>() where u.UserEmailId.ToUpper().Equals(login.EmailId.ToUpper()) select u;
                    string firstName = userQuery.FirstOrDefault().FirstName;
                    string lastName = userQuery.FirstOrDefault().LastName;

                    Root root = new Root { Email = login.EmailId, FirstName = firstName, LastName = lastName, RealEmail = login.EmailId };
                    UserXmlSpec userXmlSpec = new UserXmlSpec { Root = root };

                    string[] names = new string[] { "TempPassword" };
                    string[] values = new string[] { password };

                    #region removeemailfunctionality
                    //EmailMessage emailMessage = new EmailMessage { Names = names, Values = values };

                    //bool returnValue = ECircleWrapper.SendMail(userXmlSpec, emailMessage, EmailType.ForgetPassword);
                    //if (returnValue)
                    //    return new Message { IsSuccess = true, MessageId = "M00029", Description = "Your temporary password has been sent on your email id" };
                    //else
                    //    return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to send password" };
                   #endregion

                    EmailMessage emailMessage = new EmailMessage { Names = names, Values = values };

                    bool returnValue = true;
                    if (returnValue)
                        return new Message { IsSuccess = true, MessageId = "M00029", Description = "Your temporary password is "+ password };
                    else
                        return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to set your password" };
                }
                else
                    return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to set your password" };
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }


        /// <summary>
        /// To change Password of the user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message ChangePassword(User user, AuditTrail auditTrail)
        {
            try
            {
                string messsageId;
                string messsage;
                PasswordRules passwordRules = new PasswordRules(user.LoginDetails.NewPassword);

                if (passwordRules.ValidatePassword(out messsageId, out messsage))
                {
                    if (passwordRules.MatchPasswordPattern(user.LoginDetails.Password, user.LoginDetails.NewPassword) == false)
                    {
                        user.LoginDetails.NewPassword = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(user.LoginDetails.NewPassword, "SHA1");
                        user.LoginDetails.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(user.LoginDetails.Password, "SHA1");
                        return UserDL.ChangePassword(user, auditTrail);
                    }
                    else
                    {
                        return new Message { MessageId = "M00028", Description = "Please change your password pattern.", IsSuccess = false };
                    }
                }
                else
                {
                    return new Message { MessageId = messsageId, Description = messsage, IsSuccess = false };
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To Reset Password of user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message ResetPassword(User user, AuditTrail auditTrail)
        {
            if (user != null)
            {
                try
                {
                    string hashPassword = string.Empty;
                    string password = string.Empty;

                    password = PasswordRules.GeneratePassword(8, 5, out hashPassword);
                    user.LoginDetails.NewPassword = hashPassword;
                    bool isSuccess = UserDL.ResetPassword(user, auditTrail);
                    if (isSuccess)
                    {
                        IList<User> users = new UserDL().GetUserList();

                        var userQuery = from u in users.ToList<User>() where u.UserEmailId.Equals(user.LoginDetails.EmailId) select u;
                        string firstName = userQuery.FirstOrDefault().FirstName;
                        string lastName = userQuery.FirstOrDefault().LastName;

                        Root root = new Root { Email = user.LoginDetails.EmailId, FirstName = firstName, LastName = lastName, RealEmail = user.LoginDetails.EmailId };
                        UserXmlSpec userXmlSpec = new UserXmlSpec { Root = root };

                        string[] names = new string[] { "TempPassword" };
                        string[] values = new string[] { password };

                        EmailMessage emailMessage = new EmailMessage { Names = names, Values = values };

                        #region removeEmailFunctionality
                    //    bool returnValue = ECircleWrapper.SendMail(userXmlSpec, emailMessage, EmailType.ResetPassword);
                        
                    //    if (returnValue)
                    //        return new Message { IsSuccess = true, MessageId = "M00005", Description = "The temporary password has been sent on user's email id" };
                    //    else
                    //        return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to send password" };                       
                    //}
                    //else
                    //    return new Message { MessageId = "M00030", IsSuccess = true, Description = "Unable to reset password" };
                     #endregion removeEmailFunctionality

                        bool returnValue = true;

                        if (returnValue)
                            return new Message { IsSuccess = true, MessageId = "M00005", Description = "Password reset successfully & The temporary password is: " +password };
                        else
                            return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to set user's password" };
                    }
                    else
                        return new Message { MessageId = "M00030", IsSuccess = true, Description = "Unable to reset user's password" };
                }
                catch (Exception ex)
                {
                    LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                    return new Message { MessageId = "M00030", IsSuccess = true, Description = "Unable to reset password" };
                }
            }
            return new Message { MessageId = "M00030", Description = "Unable to reset password", IsSuccess = false };
        }

        /// <summary>
        /// To get the Tenent List
        /// </summary>
        /// <returns></returns>
        public IList<Tenant> GetTenant()
        {
            try
            {
                return GetTenantInternal();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Internally called by public GetTenant method.
        /// </summary>
        /// <returns></returns>
        private IList<Tenant> GetTenantInternal()
        {
            try
            {
                return new UserDL().GetTenant();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To get the User Roles
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetUserRoles()
        {
            try
            {
                return GetUserRolesInternal();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Internally called by public GetUserRoles method.
        /// </summary>
        /// <returns></returns>
        private IList<Role> GetUserRolesInternal()
        {
            try
            {
                return new UserDL().GetUserRoles();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To get User List
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUserList()
        {
            try
            {
                return GetUserListInternal();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Internally called by public GetUserList method.
        /// </summary>
        /// <returns></returns>
        private IList<User> GetUserListInternal()
        {
            try
            {
                return new UserDL().GetUserList();
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To add and Update user details.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="IsAdd"></param>
        /// <returns></returns>
        public Message AddUpdateUser(User user, bool IsAdd)
        {
            try
            {
                if (IsAdd)
                    return AddNewUser(user);
                else
                    return UpdateUser(user);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To add new user details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Message AddNewUser(User user)
        {
            try
            {
                UserDL userDL = new UserDL();
                string hashPassword = string.Empty;
                string password = string.Empty;
                userDL.AuditTrailDetails = new AuditTrail() { CreatedBy = user.LoginDetails.RecordId, CreatedOn = DateTime.Now };
                user.LoginDetails = new Login();
                password = PasswordRules.GeneratePassword(8, 5, out hashPassword);
                user.LoginDetails.NewPassword = hashPassword;

                Message message = userDL.AddNewUser(user);


                if (message.IsSuccess)
                {
                    Root root = new Root { Email = user.UserEmailId, FirstName = user.FirstName, LastName = user.LastName, RealEmail = user.UserEmailId };
                    UserXmlSpec userXmlSpec = new UserXmlSpec { Root = root };

                    string[] names = new string[] { "TempPassword" };
                    string[] values = new string[] { password };

                    EmailMessage emailMessage = new EmailMessage { Names = names, Values = values };
                    #region RemoveEmailFunctionality
                    //bool returnValue = ECircleWrapper.SendMail(userXmlSpec, emailMessage, EmailType.NewUserPassword);
                    //if (returnValue)
                    //    return new Message { IsSuccess = true, MessageId = "M00060", Description = "Your temporary password has been sent on your email id" };
                    //else
                    //    return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to send password" };  
                    #endregion RemoveEmailFunctionality

                    bool returnValue = true;
                    if (returnValue)
                        return new Message { IsSuccess = true, MessageId = "M00060", Description = "User created successfully & the temporary password is: " + password  };
                    else
                        return new Message { IsSuccess = true, MessageId = "M00006", Description = "Unable to set password" };  
                   
                }

                return message;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To update user details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Message UpdateUser(User user)
        {
            try
            {
                UserDL userDL = new UserDL();

                userDL.AuditTrailDetails = new AuditTrail() { ModifiedBy = user.LoginDetails.RecordId, ModifiedOn = DateTime.Now };

                Message message = userDL.UpdateUser(user);

                return message;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To update user account details
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        public Message UpdateUserAccount(User user, AuditTrail auditTrail)
        {
            try
            {
                return UpdateUserAccountInternal(user, auditTrail);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Internally called by public UpdateUserAccount method.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="auditTrail"></param>
        /// <returns></returns>
        private Message UpdateUserAccountInternal(User user, AuditTrail auditTrail)
        {
            try
            {
                UserDL userDL = new UserDL();
                userDL.AuditTrailDetails = auditTrail;

                Message message = userDL.UpdateUserAccount(user);
                return message;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserBL", LogEventType.Error, ex.Message);
                throw;
            }
        }
    }
}
