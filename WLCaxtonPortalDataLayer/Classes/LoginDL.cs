//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : LoginDL.cs
// Program Description  : Contains login related fucntionality such as AuthenticateUser,UpdateUserAccount,GetUserAccessModulesList,CheckUpdateUserSessionDetails                          
// Programmed By        : Nadeem Ishrat
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalDataHelper;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using WLCaxtonPortalExceptionLogger;
#endregion

namespace WLCaxtonPortalDataLayer
{
    public class LoginDL : DataLayerBase
    {
        #region Private variables
        private static IDataReader dataReader = null;
        private static IDbCommand dbCommand = null;
        #endregion

        #region Public methods
        /// <summary>
        /// Authniticate the user on portal
        /// </summary>
        /// <param name="login">Login object contains logic details</param>
        /// <param name="role">Super User,Tenant Admin,Noraml User</param>
        /// <param name="tenant">Client details</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message AuthenticateUser(ref Login login, out Role role, out Tenant tenant)
        {
            DatabaseInstance = DataManager.GetDatabaseInstance();
            role = new Role();
            Message message = new Message();
            tenant = new Tenant();
            bool isAuthenticationChecked = false;

            /*  string query = "SELECT RoleName, RoleId,StatusName,StatusId,UserId,UserEmailId,FirstName,LastName,Password,IsTemporaryPassword,Disabled,PasswordChangedDate, TenantName, Tenantid, TenantMappingId " +
                                 " FROM vw_Users WHERE UserEmailId = @userEmailId AND Password = @password AND Disabled = 0 AND StatusId = 1"; // 1 status of Active users
                                 */

            try
            {
                //Adding parameters
                SqlCommand sqlCommand = new SqlCommand("Proc_AuthenticateUser");
                SqlParameter paramUserId = new SqlParameter();
                paramUserId.ParameterName = "@userEmailId";
                paramUserId.Value = login.EmailId;
                sqlCommand.Parameters.Add(paramUserId);

                SqlParameter paramPwd = new SqlParameter();
                paramPwd.ParameterName = "@password";
                paramPwd.Value = login.Password;
                sqlCommand.Parameters.Add(paramPwd);
                sqlCommand.CommandType = CommandType.StoredProcedure; // line added by sandeep

                //Setting values for negative result
                login.IsAuthenticated = false;
                message.IsSuccess = true;
                message.MessageId = "M00021";
                message.Description = "UserId or Password is invalid";

                //Executing the query
                dataReader = DatabaseInstance.ExecuteReader(sqlCommand);

                if (dataReader != null)
                {
                    if (dataReader.FieldCount > 0)
                    {
                        while (dataReader.Read())
                        {
                            isAuthenticationChecked = true;
                            login.IsTemporaryPassword = GetValue<bool>(dataReader, "IsTemporaryPassword");
                            login.RecordId = GetValue<int>(dataReader, "UserId");
                            login.IsAuthenticated = true;
                            message.MessageId = "M00070";
                            message.Description = "User authenticated successfully";
                            login.IsAccountLocked = false;
                            login.Name = GetValue<string>(dataReader, "FirstName") + " " + GetValue<string>(dataReader, "LastName");

                            // retrieving tenant details
                            tenant.MetadataDatabaseMappingId = GetValue<int>(dataReader, "TenantMappingId");
                            tenant.RecordId = GetValue<int>(dataReader, "TenantId");
                            tenant.Name = GetValue<string>(dataReader, "TenantName");

                            // retrieving role details
                            role.Name = GetValue<string>(dataReader, "RoleName");
                            role.RecordId = GetValue<int>(dataReader, "RoleId");

                            if (dataReader["PasswordChangedDate"] == null)
                                login.IsPasswordExpired = false;
                            else
                            {
                                DateTime dtChangedTime = GetValue<DateTime>(dataReader, "PasswordChangedDate");
                                if (dtChangedTime.Date.Equals(DateTime.MinValue))
                                    login.IsPasswordExpired = false;
                                else
                                {

                                    //Get expiry days from Config file
                                    int passwordExpiryLimit = Convert.ToInt32(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PasswordExpiryLimit"]));

                                    TimeSpan tsCurrent = new TimeSpan(DateTime.Now.Ticks);
                                    TimeSpan tsChangedTime = new TimeSpan(dtChangedTime.Ticks);

                                    if (tsCurrent.TotalDays - tsChangedTime.TotalDays > passwordExpiryLimit)
                                    {
                                        login.IsPasswordExpired = true;
                                        message.MessageId = "M00022";
                                        message.Description = "Your password has been expired, kindly change your password";
                                    }
                                    else
                                        login.IsPasswordExpired = false;

                                }

                            }
                            message.IsSuccess = true;
                        }
                    }

                    dataReader.Close();

                    if (!isAuthenticationChecked)
                    {
                        /*query = "SELECT RoleName, RoleId,StatusName,StatusId,UserId,UserEmailId,FirstName,LastName,Password,IsTemporaryPassword,Disabled,PasswordChangedDate " +
                                 " FROM vw_Users WHERE UserEmailId = @userEmailId and Disabled = 0 ";*/

                        //Adding parameters
                        sqlCommand = new SqlCommand("Proc_IfNotAuthenticationChecked");
                        paramUserId = new SqlParameter();
                        paramUserId.ParameterName = "@userEmailId";
                        paramUserId.Value = login.EmailId;
                        sqlCommand.Parameters.Add(paramUserId);

                        sqlCommand.CommandType = CommandType.StoredProcedure; // line added by sandeep

                        int status = -1;
                        dataReader = DatabaseInstance.ExecuteReader(sqlCommand);

                        if (dataReader != null)
                        {
                            if (dataReader.FieldCount > 0)
                            {
                                while (dataReader.Read())
                                {
                                    login.RecordId = GetValue<int>(dataReader, "UserId");
                                    login.IsAuthenticated = false;
                                    status = GetValue<int>(dataReader, "StatusId");
                                    if (status == Convert.ToInt32(Status.Locked)) // Account locked
                                    {
                                        message.MessageId = "M00023";
                                        message.Description = "User account is locked, kindly contact administrator";
                                        login.IsAccountLocked = true;
                                    }
                                    else if (status == Convert.ToInt32(Status.Active))
                                    {
                                        message.MessageId = "M00021";
                                        message.Description = "UserId or Password is invalid";
                                        login.IsAccountLocked = false;
                                    }
                                    else if (status == Convert.ToInt32(Status.InActive))
                                    {
                                        message.MessageId = "M00024";
                                        message.Description = "User account is Inactive, kindly contact administrator";
                                        login.IsAccountLocked = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message.IsSuccess = false;
                LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
                throw;
            }

            return message;

        }

        //public Message AuthenticateUser(ref Login login, out Role role, out Tenant tenant)
        //{
        //    DatabaseInstance = DataManager.GetDatabaseInstance();
        //    role = new Role();
        //    Message message = new Message();
        //    tenant = new Tenant();
        //    bool isAuthenticationChecked = false;

        //    string query = "SELECT RoleName, RoleId,StatusName,StatusId,UserId,UserEmailId,FirstName,LastName,Password,IsTemporaryPassword,Disabled,PasswordChangedDate, TenantName, Tenantid, TenantMappingId " +
        //                       " FROM vw_Users WHERE UserEmailId = @userEmailId AND Password = @password AND Disabled = 0 AND StatusId = 1"; // 1 status of Active users


        //    try
        //    {
        //        //Adding parameters
        //        SqlCommand sqlCommand = new SqlCommand(query);
        //        SqlParameter paramUserId = new SqlParameter();
        //        paramUserId.ParameterName = "@userEmailId";
        //        paramUserId.Value = login.EmailId;
        //        sqlCommand.Parameters.Add(paramUserId);

        //        SqlParameter paramPwd = new SqlParameter();
        //        paramPwd.ParameterName = "@password";
        //        paramPwd.Value = login.Password;
        //        sqlCommand.Parameters.Add(paramPwd);

        //        //Setting values for negative result
        //        login.IsAuthenticated = false;
        //        message.IsSuccess = true;
        //        message.MessageId = "M00021";
        //        message.Description = "UserId or Password is invalid";

        //        //Executing the query
        //        dataReader = DatabaseInstance.ExecuteReader(sqlCommand);

        //        if (dataReader != null)
        //        {
        //            if (dataReader.FieldCount > 0)
        //            {
        //                while (dataReader.Read())
        //                {
        //                    isAuthenticationChecked = true;
        //                    login.IsTemporaryPassword = GetValue<bool>(dataReader, "IsTemporaryPassword");
        //                    login.RecordId = GetValue<int>(dataReader, "UserId");
        //                    login.IsAuthenticated = true;
        //                    message.MessageId = "M00070";
        //                    message.Description = "User authenticated successfully";
        //                    login.IsAccountLocked = false;

        //                    // retrieving tenant details
        //                    tenant.MetadataDatabaseMappingId = GetValue<int>(dataReader, "TenantMappingId");
        //                    tenant.RecordId = GetValue<int>(dataReader, "TenantId");
        //                    tenant.Name = GetValue<string>(dataReader, "TenantName");

        //                    // retrieving role details
        //                    role.Name = GetValue<string>(dataReader, "RoleName");
        //                    role.RecordId = GetValue<int>(dataReader, "RoleId");

        //                    if (dataReader["PasswordChangedDate"] == null)
        //                        login.IsPasswordExpired = false;
        //                    else
        //                    {
        //                        DateTime dtChangedTime = GetValue<DateTime>(dataReader, "PasswordChangedDate");
        //                        if (dtChangedTime.Date.Equals(DateTime.MinValue))
        //                            login.IsPasswordExpired = false;
        //                        else
        //                        {

        //                            //Get expiry days from Config file
        //                            int passwordExpiryLimit = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PasswordExpiryLimit"].ToString());

        //                            TimeSpan tsCurrent = new TimeSpan(DateTime.Now.Ticks);
        //                            TimeSpan tsChangedTime = new TimeSpan(dtChangedTime.Ticks);

        //                            if (tsCurrent.TotalDays - tsChangedTime.TotalDays > passwordExpiryLimit)
        //                            {
        //                                login.IsPasswordExpired = true;
        //                                message.MessageId = "M00022";
        //                                message.Description = "Your password has been expired, kindly change your password";
        //                            }
        //                            else
        //                                login.IsPasswordExpired = false;

        //                        }

        //                    }
        //                    message.IsSuccess = true;
        //                }
        //            }

        //            dataReader.Close();

        //            if (!isAuthenticationChecked)
        //            {
        //                query = "SELECT RoleName, RoleId,StatusName,StatusId,UserId,UserEmailId,FirstName,LastName,Password,IsTemporaryPassword,Disabled,PasswordChangedDate " +
        //                         " FROM vw_Users WHERE UserEmailId = @userEmailId and Disabled = 0 ";

        //                //Adding parameters
        //                sqlCommand = new SqlCommand(query);
        //                paramUserId = new SqlParameter();
        //                paramUserId.ParameterName = "@userEmailId";
        //                paramUserId.Value = login.EmailId;
        //                sqlCommand.Parameters.Add(paramUserId);

        //                int status = -1;

        //                dataReader = DatabaseInstance.ExecuteReader(sqlCommand);

        //                if (dataReader != null)
        //                {
        //                    if (dataReader.FieldCount > 0)
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            login.RecordId = GetValue<int>(dataReader, "UserId");
        //                            login.IsAuthenticated = false;
        //                            status = GetValue<int>(dataReader, "StatusId");
        //                            if (status == Convert.ToInt32(Status.Locked)) // Account locked
        //                            {
        //                                message.MessageId = "M00023";
        //                                message.Description = "User account is locked, kindly contact administrator";
        //                                login.IsAccountLocked = true;
        //                            }
        //                            else if (status == Convert.ToInt32(Status.Active))
        //                            {
        //                                message.MessageId = "M00021";
        //                                message.Description = "UserId or Password is invalid";
        //                                login.IsAccountLocked = false;
        //                            }
        //                            else if (status == Convert.ToInt32(Status.InActive))
        //                            {
        //                                message.MessageId = "M00024";
        //                                message.Description = "User account is Inactive, kindly contact administrator";
        //                                login.IsAccountLocked = false;
        //                            }

        //                        }
        //                    }

        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message.IsSuccess = false;
        //        LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
        //        throw;
        //    }

        //    return message;

        //}

        /// <summary>
        /// Update the existing user details into database
        /// </summary>
        /// <param name="user">user object contains user information</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message UpdateUserAccount(ref User user)
        {
            Message message = new Message();
            DatabaseInstance = DataManager.GetDatabaseInstance();

            try
            {
                DatabaseInstance = DataManager.GetDatabaseInstance();

                if (DatabaseInstance != null)
                {
                    using (DbCommand command = DatabaseInstance.CreateCommand("Proc_UpdateUserAccountStatus"))
                    {
                        DatabaseInstance.AddInParameter(command, "@p_UserId", DbType.String, user.LoginDetails.RecordId);
                        DatabaseInstance.AddInParameter(command, "@p_Action", DbType.String, user.UserActivity.ToString());
                        DatabaseInstance.AddInParameter(command, "@p_ModifiedBy", DbType.String, this.AuditTrailDetails.ModifiedBy);
                        DatabaseInstance.AddInParameter(command, "@p_ModifiedOn", DbType.DateTime, this.AuditTrailDetails.ModifiedOn);
                        DatabaseInstance.AddOutParameter(command, "@p_IsSuccess", DbType.Boolean, 1);
                        DatabaseInstance.AddOutParameter(command, "@p_MessageId", DbType.String, 10);
                        DatabaseInstance.AddOutParameter(command, "@p_MessageText", DbType.String, 200);

                        DatabaseInstance.ExecuteNonQuery(command);

                        message.IsSuccess = Convert.ToBoolean(command.Parameters["@p_IsSuccess"].Value);
                        message.MessageId = Convert.ToString(command.Parameters["@p_MessageId"].Value);
                        message.Description = Convert.ToString(command.Parameters["@p_MessageText"].Value);

                        user.IsAccountLock = true;
                        message.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                user.IsAccountLock = false;
                message.IsSuccess = false;
                LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
                throw;
            }


            return message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message GetUserAccessModulesList(ref User user)
        {
            Message message = new Message();
            DatabaseInstance = DataManager.GetDatabaseInstance();
            List<Module> lstModules = new List<Module>();
            List<SubModule> lstSubModules = new List<SubModule>();
            int parentId = -1;

            //string query = "SELECT ModuleId,ModuleName ,ModuleDescription ,ParentId, PageURL, DisplayOrder,RoleId,RoleName,RoleDescription FROM vw_Menu WHERE RoleId = @RoleId ";

            // Get the Modules from Database
            try
            {
                //Adding parameters
                SqlCommand sqlCommand = new SqlCommand("Proc_GetUserAccessModulesList");
                SqlParameter paramRoleId = new SqlParameter();
                paramRoleId.ParameterName = "@RoleId";
                paramRoleId.Value = user.RoleDetails.RecordId;
                sqlCommand.Parameters.Add(paramRoleId);
                sqlCommand.CommandType = CommandType.StoredProcedure; // line added by sandeep

                //Executing the query
                dataReader = DatabaseInstance.ExecuteReader(sqlCommand);

                if (dataReader != null)
                {
                    if (dataReader.FieldCount > 0)
                    {
                        while (dataReader.Read())
                        {

                            parentId = GetValue<int>(dataReader, "ParentId");
                            if (parentId > 0)
                            {
                                lstSubModules.Add(new SubModule
                                {
                                    Description = GetValue<string>(dataReader, "ModuleDescription"),
                                    Name = GetValue<string>(dataReader, "ModuleName"),
                                    DisplayOrder = GetValue<int>(dataReader, "DisplayOrder"),
                                    PageURL = GetValue<string>(dataReader, "PageURL"),
                                    ParentId = GetValue<int>(dataReader, "ParentId"),
                                    RecordId = GetValue<int>(dataReader, "ModuleId")
                                }
                                                );
                            }
                            else
                            {
                                lstModules.Add(new Module
                                {
                                    Description = GetValue<string>(dataReader, "ModuleDescription"),
                                    Name = GetValue<string>(dataReader, "ModuleName"),
                                    DisplayOrder = GetValue<int>(dataReader, "DisplayOrder"),
                                    RecordId = GetValue<int>(dataReader, "ModuleId"),
                                    PageURL = GetValue<string>(dataReader, "PageURL")
                                }
                                                  );
                            }

                        }
                    }

                    dataReader.Close();
                    message.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                message.IsSuccess = false;
                LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
                throw;
            }

            user.AccessibleModuleList = lstModules;
            user.AccessibleSubModuleList = lstSubModules;

            return message;

        }

        //public Message GetUserAccessModulesList(ref User user)
        //{
        //    Message message = new Message();
        //    DatabaseInstance = DataManager.GetDatabaseInstance();
        //    List<Module> lstModules = new List<Module>();
        //    List<SubModule> lstSubModules = new List<SubModule>();
        //    int parentId = -1;

        //    string query = "SELECT ModuleId,ModuleName ,ModuleDescription ,ParentId, PageURL, DisplayOrder,RoleId,RoleName,RoleDescription FROM vw_Menu WHERE RoleId = @RoleId ";
        //    // Get the Modules from Database
        //    try
        //    {
        //        //Adding parameters
        //        SqlCommand sqlCommand = new SqlCommand(query);
        //        SqlParameter paramRoleId = new SqlParameter();
        //        paramRoleId.ParameterName = "@RoleId";
        //        paramRoleId.Value = user.RoleDetails.RecordId;
        //        sqlCommand.Parameters.Add(paramRoleId);

        //        //Executing the query
        //        dataReader = DatabaseInstance.ExecuteReader(sqlCommand);

        //        if (dataReader != null)
        //        {
        //            if (dataReader.FieldCount > 0)
        //            {
        //                while (dataReader.Read())
        //                {

        //                    parentId = GetValue<int>(dataReader, "ParentId");
        //                    if (parentId > 0)
        //                    {
        //                        lstSubModules.Add(new SubModule
        //                                            {
        //                                                Description = GetValue<string>(dataReader, "ModuleDescription"),
        //                                                Name = GetValue<string>(dataReader, "ModuleName"),
        //                                                DisplayOrder = GetValue<int>(dataReader, "DisplayOrder"),
        //                                                PageURL = GetValue<string>(dataReader, "PageURL"),
        //                                                ParentId = GetValue<int>(dataReader, "ParentId"),
        //                                                RecordId = GetValue<int>(dataReader, "ModuleId")
        //                                            }
        //                                        );
        //                    }
        //                    else
        //                    {
        //                        lstModules.Add(new Module
        //                                            {
        //                                                Description = GetValue<string>(dataReader, "ModuleDescription"),
        //                                                Name = GetValue<string>(dataReader, "ModuleName"),
        //                                                DisplayOrder = GetValue<int>(dataReader, "DisplayOrder"),
        //                                                RecordId = GetValue<int>(dataReader, "ModuleId"),
        //                                                PageURL = GetValue<string>(dataReader, "PageURL")
        //                                            }
        //                                          );
        //                    }

        //                }
        //            }

        //            dataReader.Close();
        //            message.IsSuccess = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        message.IsSuccess = false;
        //        LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
        //        throw;
        //    }

        //    user.AccessibleModuleList = lstModules;
        //    user.AccessibleSubModuleList = lstSubModules;

        //    return message;

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message CheckUpdateUserSessionDetails(User user)
        {
            Message message = new Message();
            try
            {
                DatabaseInstance = DataManager.GetDatabaseInstance();

                if (DatabaseInstance != null)
                {
                    using (DbCommand command = DatabaseInstance.CreateCommand("Proc_CheckUpdateUserSessionDetails"))
                    {
                        DatabaseInstance.AddInParameter(command, "@p_UserId", DbType.String, user.LoginDetails.RecordId);
                        DatabaseInstance.AddInParameter(command, "@p_SessionId", DbType.String, user.LoginDetails.SessionId);
                        DatabaseInstance.AddInParameter(command, "@p_SystemIP", DbType.String, user.LoginDetails.SystemIP);
                        DatabaseInstance.AddInParameter(command, "@p_Action", DbType.String, Convert.ToString(user.UserActivity));
                        DatabaseInstance.AddOutParameter(command, "@p_IsSuccess", DbType.Boolean, 1);
                        DatabaseInstance.AddOutParameter(command, "@p_MessageId", DbType.String, 10);
                        DatabaseInstance.AddOutParameter(command, "@p_MessageText", DbType.String, 200);


                        DatabaseInstance.ExecuteNonQuery(command);

                        message.IsSuccess = Convert.ToBoolean(command.Parameters["@p_IsSuccess"].Value);
                        message.MessageId = Convert.ToString(command.Parameters["@p_MessageId"].Value);
                        message.Description = Convert.ToString(command.Parameters["@p_MessageText"].Value);

                    }
                }
            }
            catch (Exception ex)
            {
                message.IsSuccess = false;
                LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
                throw;
            }

            return message;
        }

        /// <summary>
        /// Check the seesion for multiple browser
        /// </summary>
        /// <param name="user">user object for user information</param>
        /// <returns>Message object contains message description and issuccess</returns>

        public Message CheckUserSession(User user)
        {
            Message message = new Message();
            DatabaseInstance = DataManager.GetDatabaseInstance();

            int count = -1;

            //string query = "SELECT UserId, IsActiveSession, ServerSessionId, SystemIP,AccountStatus FROM vw_SessionDetails WHERE UserId = @UserId AND ServerSessionId = @SessionId ";
            // Get the Modules from Database
            try
            {
                //Adding parameters
                SqlCommand sqlCommand = new SqlCommand("Proc_CheckUserSession");
                SqlParameter paramRoleId = new SqlParameter();
                paramRoleId.ParameterName = "@UserId";
                paramRoleId.Value = user.LoginDetails.RecordId;
                sqlCommand.Parameters.Add(paramRoleId);

                SqlParameter paramSessionId = new SqlParameter();
                paramSessionId.ParameterName = "@SessionId";
                paramSessionId.Value = user.LoginDetails.SessionId;
                sqlCommand.Parameters.Add(paramSessionId);

                sqlCommand.CommandType = CommandType.StoredProcedure; // line added by sandeep


                //Executing the query
                dataReader = DatabaseInstance.ExecuteReader(sqlCommand);
                int accountStatus = -1;
                bool isActive = false;
                if (dataReader != null)
                {
                    if (dataReader.FieldCount > 0)
                    {
                        while (dataReader.Read())
                        {
                            count++;
                            accountStatus = Convert.ToInt32(dataReader["AccountStatus"]);
                            isActive = Convert.ToBoolean(dataReader["IsActiveSession"]);
                        }
                    }
                    dataReader.Close();
                }

                if ((isActive == false && accountStatus == 1) || count == -1)
                {
                    message.IsSuccess = false;
                    message.MessageId = "M00072";
                    message.Description = "Already logged out, cannot proceed";
                }
                else
                {
                    if (accountStatus == (int)Status.Locked)
                    {
                        user.UserActivity = UserActivityType.Logout;
                        CheckUpdateUserSessionDetails(user);
                        message.IsSuccess = false;
                        message.MessageId = "M00084";
                    }
                    else
                        message.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                message.IsSuccess = false;
                LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
                throw;
            }
            return message;
        }


        //public Message CheckUserSession(User user)
        //{
        //    Message message = new Message();
        //    DatabaseInstance = DataManager.GetDatabaseInstance();

        //    int count = -1;

        //    string query = "SELECT UserId, IsActiveSession, ServerSessionId, SystemIP,AccountStatus FROM vw_SessionDetails WHERE UserId = @UserId AND ServerSessionId = @SessionId ";
        //    // Get the Modules from Database
        //    try
        //    {
        //        //Adding parameters
        //        SqlCommand sqlCommand = new SqlCommand(query);
        //        SqlParameter paramRoleId = new SqlParameter();
        //        paramRoleId.ParameterName = "@UserId";
        //        paramRoleId.Value = user.LoginDetails.RecordId;
        //        sqlCommand.Parameters.Add(paramRoleId);

        //        SqlParameter paramSessionId = new SqlParameter();
        //        paramSessionId.ParameterName = "@SessionId";
        //        paramSessionId.Value = user.LoginDetails.SessionId;
        //        sqlCommand.Parameters.Add(paramSessionId);

        //        //Executing the query
        //        dataReader = DatabaseInstance.ExecuteReader(sqlCommand);
        //        int accountStatus = -1;
        //        bool isActive = false;
        //        if (dataReader != null)
        //        {
        //            if (dataReader.FieldCount > 0)
        //            {
        //                while (dataReader.Read())
        //                {
        //                    count++;
        //                    accountStatus = Convert.ToInt32(dataReader["AccountStatus"]);
        //                    isActive = Convert.ToBoolean(dataReader["IsActiveSession"]);
        //                }
        //            }
        //            dataReader.Close();
        //        }

        //        if ((isActive == false && accountStatus ==1) || count == -1)
        //        {
        //            message.IsSuccess = false;
        //            message.MessageId = "M00072";
        //            message.Description = "Already logged out, cannot proceed";
        //        }
        //        else
        //        {
        //            if (accountStatus == (int)Status.Locked)
        //            {
        //                user.UserActivity = UserActivityType.Logout ;
        //                CheckUpdateUserSessionDetails(user);
        //                message.IsSuccess = false;
        //                message.MessageId = "M00084";
        //            }
        //            else
        //                message.IsSuccess = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message.IsSuccess = false;
        //        LoggerFactory.Logger.Log("LoginDL", LogEventType.Error, ex.Message);
        //        throw;
        //    }
        //    return message;
        //}

        #endregion
    }
}
