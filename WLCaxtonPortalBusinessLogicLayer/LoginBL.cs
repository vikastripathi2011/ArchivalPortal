//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : LoginBL.cs
// Program Description  : This class is used for Login realated opeartion
// Programmed By        : Nadeem Ishrat
// Programmed On        : 10-December-2012
// Version              : 1.0.0
//==========================================================================================

using System;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalDataLayer;
using WLCaxtonPortalExceptionLogger;


namespace WLCaxtonPortalBusinessLogicLayer
{
    /// <summary>
    /// This class is used for Users realated opeartion
    /// </summary>
    public class LoginBL
    {
        /// <summary>
        /// To Authenticate the User
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public User AuthenticateUser(Login login)
        {
            try
            {
                return AuthenticateUserInternal(login);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To Logout the User from the application.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message Logout(User user)
        {
            try
            {
                user.UserActivity = UserActivityType.Logout;
                return this.CheckUpdateActiveSessionDetails(user);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To check the User Session is active or not.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Message CheckUserSession(User user)
        {
            try
            {
                return this.CheckUserSessionInternal(user);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// Internally called by public CheckUserSession method.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Message CheckUserSessionInternal(User user)
        {
            try
            {
                LoginDL loginDL = new LoginDL();
                return loginDL.CheckUserSession(user);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Internally called by public AuthenticateUser method.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private User AuthenticateUserInternal(Login login)
        {
            try
            {
                User user = new User();
                LoginDL loginDL = new LoginDL();
                Role role = new Role();
                Tenant tenant = new Tenant();
                Message message = loginDL.AuthenticateUser(ref login, out role, out tenant);
                user.MessageDetails = message;
                user.LoginDetails = login;
                user.TenantDetails = tenant;
                if (message.IsSuccess)
                {
                    if (login.RecordId > 0) // user has a valid account 
                    {
                        if (!login.IsAccountLocked)
                        {
                            user.LoginDetails.LoginAttempts++;


                            if (login.IsAuthenticated)
                            {

                                user.RoleDetails = role;
                                user.UserActivity = UserActivityType.Login;

                                // Check for Active session of this user
                                message = CheckUpdateActiveSessionDetails(user);

                                if (message.IsSuccess)
                                {
                                    //Get menu details
                                    message = GetUserMenuList(ref user);
                                    user.MessageDetails = message;

                                    user.UserActivity = UserActivityType.SuccessfulLogin;

                                    //log successful login
                                    AuditTrailDL.LogUserActivity(user);
                                    // Get tenant details


                                }
                                else
                                {
                                    user.LoginDetails.IsAuthenticated = false;
                                    user.MessageDetails = message;

                                    user.UserActivity = UserActivityType.MultipleSessionsLogout;

                                    //log MultipleSessionsLogout 
                                    AuditTrailDL.LogUserActivity(user);

                                }


                            }

                            else
                            {
                                user.LoginDetails.IsAuthenticated = false;
                                user.UserActivity = UserActivityType.UnSuccessfulLogin;
                                //log unsuccessful login
                                AuditTrailDL.LogUserActivity(user);
                                //Checking account lock configurations
                                CheckAccountLockConfigurations(ref user);

                            }
                        }
                    }
                    else
                    {
                        user.MessageDetails.Description = "Invalid User Id, account details not found";
                        user.MessageDetails.MessageId = "M00026";
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// to check User Account Lock details
        /// </summary>
        /// <param name="user"></param>
        private void CheckAccountLockConfigurations(ref User user)
        {
            try
            {
                LoginDL loginDL = new LoginDL();

                int loginAttemptsLimit = Convert.ToInt16(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MaxLoginAttempts"]));

                if (user.LoginDetails.LoginAttempts >= loginAttemptsLimit)
                {
                    AuditTrail auditTrail = new AuditTrail{
                    ModifiedBy = user.LoginDetails.RecordId,
                    ModifiedOn = DateTime.Now};

                    loginDL.AuditTrailDetails = auditTrail;
                    user.UserActivity = UserActivityType.AccountLock;
                    Message message = loginDL.UpdateUserAccount(ref user);

                    if (message.IsSuccess)
                    {
                        //log account lock
                        AuditTrailDL.LogUserActivity(user);
                        user.MessageDetails.Description = "Your account has been locked due to three incorrect attempts. Kindly contact administrator.";
                        user.MessageDetails.MessageId = "M00027";
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To get the Menu List for the User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Message GetUserMenuList(ref User user)
        {
            try
            {
                LoginDL loginDL = new LoginDL();

                Message message = loginDL.GetUserAccessModulesList(ref user);

                return message;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// To check Update active session details 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private Message  CheckUpdateActiveSessionDetails(User user)
        {
            try
            {
                LoginDL loginDL = new LoginDL();

                Message message = loginDL.CheckUpdateUserSessionDetails(user);

                return message;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("LoginBL", LogEventType.Error, ex.Message);
                throw;
            }
        }
    }
}
