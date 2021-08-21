//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : UserDL.cs
// Program Description  : Contains user management related fucntionality such as ForgetPassword,ChangePassword,ResetPassword,GetTenant,GetUserRoles() etc..                                                  
// Programmed By        : Nadeem Ishrat
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================


#region Namespaces
using System;
using System.Collections.Generic;
using WLCaxtonPortalBusinessEntity;
using System.Data;
using WLCaxtonPortalDataHelper;
using System.Data.Common;
using WLCaxtonPortalExceptionLogger; 
#endregion

namespace WLCaxtonPortalDataLayer.Classes
{
    public class UserDL : DataLayerBase
    {
        #region Private Variables
        private static IDatabase databaseInstance;
        private static IDataReader reader = null;
        private static IDbCommand dbCommand = null; 
        #endregion

        #region Public methods
        /// <summary>
        /// Use to reset the forget password 
        /// </summary>
        /// <param name="login">logic details such as emaild amd password</param>
        /// <param name="auditTrail">audit trail object.such datetime,created or modified by</param>
        /// <returns>Is forget password sent</returns>
        public static bool ForgetPassword(Login login, AuditTrail auditTrail)
        {
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                DbCommand dbCommand = databaseInstance.CreateCommand("Proc_ForgetPassword");
                databaseInstance.AddInParameter(dbCommand, "@p_UserEmailID", DbType.String, login.EmailId);
                databaseInstance.AddInParameter(dbCommand, "@p_Password", DbType.String, login.NewPassword);
                SetCommonParameters(databaseInstance, dbCommand, auditTrail);

                databaseInstance.ExecuteNonQuery(dbCommand);
                return (bool)dbCommand.Parameters["@p_IsSuccess"].Value;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Use to ChangePassword old password with new one
        /// </summary>
        /// <param name="user">logic details such as emaild,teenantid,old password, new password</param>
        /// <param name="auditTrail">audit trail object.such datetime,created or modified by</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public static Message ChangePassword(User user, AuditTrail auditTrail)
        {
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                DbCommand dbCommand = databaseInstance.CreateCommand("Proc_UpdateUserPassword");
                databaseInstance.AddInParameter(dbCommand, "@p_UserEmailID ", DbType.String, user.LoginDetails.EmailId);
                databaseInstance.AddInParameter(dbCommand, "@p_TenantId", DbType.Int16, user.TenantDetails.RecordId);
                databaseInstance.AddInParameter(dbCommand, "@p_OldPassword", DbType.String, user.LoginDetails.Password);
                databaseInstance.AddInParameter(dbCommand, "@p_NewPassword", DbType.String, user.LoginDetails.NewPassword);
                SetCommonParameters(databaseInstance, dbCommand, auditTrail);

                databaseInstance.ExecuteNonQuery(dbCommand);

                bool isSuccess = (bool)dbCommand.Parameters["@p_IsSuccess"].Value;
                string messageId = (string)dbCommand.Parameters["@p_MessageID"].Value;
                string message = (string)dbCommand.Parameters["@p_MessageText"].Value;
                return new Message { MessageId = messageId, Description = message, IsSuccess = isSuccess };
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Use to reset the existing password 
        /// </summary>
        /// <param name="user">logic details such as userid,emailid,teenantid, new password</param>
        /// <param name="auditTrail">audit trail object.such datetime,created or modified by</param>
        /// <returns>Is password reset or not</returns>
        public static bool ResetPassword(User user, AuditTrail auditTrail)
        {
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                DbCommand dbCommand = databaseInstance.CreateCommand("Proc_ResetPassword");
                databaseInstance.AddInParameter(dbCommand, "@p_UserId", DbType.Int32, user.UserId);
                databaseInstance.AddInParameter(dbCommand, "@p_UserEmailID", DbType.String, user.LoginDetails.EmailId);
                databaseInstance.AddInParameter(dbCommand, "@p_TenantId", DbType.Int16, user.TenantDetails.RecordId);
                databaseInstance.AddInParameter(dbCommand, "@p_NewPassword", DbType.String, user.LoginDetails.NewPassword);
                SetCommonParameters(databaseInstance, dbCommand, auditTrail);

                databaseInstance.ExecuteNonQuery(dbCommand);
                return (bool)dbCommand.Parameters["@p_IsSuccess"].Value;
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Use to fetch the all tenents details
        /// </summary>
        /// <returns></returns>
        public IList<Tenant> GetTenant()
        {
            List<Tenant> tenants = new List<Tenant>();
            IDataReader reader = null;
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                dbCommand = databaseInstance.CreateSqlQuery("select * from vw_Tenant");
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    tenants.Add(new Tenant
                    {
                        RecordId = GetValue<int>(reader, "TenantId"),
                        Name = GetValue<string>(reader, "TenantName"),
                        MetadataDatabaseMappingId = GetValue<int>(reader, "TenantMappingId"),
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return tenants;
        }

        /// <summary>
        /// Use to fetch the all roles defined for portal
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetUserRoles()
        {
            List<Role> roles = new List<Role>();
            IDataReader reader = null;
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                dbCommand = databaseInstance.CreateSqlQuery("select * from vw_Roles");
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(new Role
                    {
                        RecordId = GetValue<int>(reader, "RoleId"),
                        Name = GetValue<string>(reader, "RoleName"),
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return roles;
        }

        /// <summary>
        /// Use to fetch all users stored in portal database
        /// </summary>
        /// <returns></returns>
        public IList<User> GetUserList()
        {
            List<User> users = new List<User>();
            IDataReader reader = null;
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                dbCommand = databaseInstance.CreateSqlQuery("Select * from vw_Users");
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        UserId = GetValue<int>(reader, "UserId"),
                        RoleDetails = new Role() { RecordId = GetValue<int>(reader, "RoleId"), Name = GetValue<string>(reader, "RoleName") },
                        UserEmailId = GetValue<string>(reader, "UserEmailId"),
                        FirstName = GetValue<string>(reader, "FirstName"),
                        LastName = GetValue<string>(reader, "LastName"),
                        StatusName = GetValue<string>(reader, "StatusName"),
                        StatusId = GetValue<int>(reader, "StatusId"),
                        IsDisabled = GetValue<bool>(reader, "Disabled")
                    });
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return users;
        }

        /// <summary>
        /// Create the new user 
        /// </summary>
        /// <param name="user">user object stored user details</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message AddNewUser(User user)
        {
            Message message = new Message();
            try
            {
                DatabaseInstance = DataManager.GetDatabaseInstance();

                if (DatabaseInstance != null)
                {
                    using (DbCommand command = DatabaseInstance.CreateCommand("Proc_SaveUser"))
                    {
                        databaseInstance.AddInParameter(command, "@p_UserEmailId", DbType.String, user.UserEmailId);
                        databaseInstance.AddInParameter(command, "@p_FirstName", DbType.String, user.FirstName);
                        databaseInstance.AddInParameter(command, "@p_LastName", DbType.String, user.LastName);
                        databaseInstance.AddInParameter(command, "@p_StatusID", DbType.Int32, (int)Status.Active);
                        databaseInstance.AddInParameter(command, "@p_TenantID", DbType.Int32, user.TenantDetails.RecordId);
                        databaseInstance.AddInParameter(command, "@p_RoleID", DbType.Int32, user.RoleDetails.RecordId);
                        databaseInstance.AddInParameter(command, "@p_Password", DbType.String, user.LoginDetails.NewPassword);
                        databaseInstance.AddInParameter(command, "@p_IsTemporaryPassword", DbType.Boolean, true);
                        databaseInstance.AddInParameter(command, "@p_CreatedBy", DbType.Int32, this.AuditTrailDetails.CreatedBy);
                        databaseInstance.AddInParameter(command, "@p_CreatedOn", DbType.DateTime, this.AuditTrailDetails.CreatedOn);
                        databaseInstance.AddInParameter(command, "@p_Disabled", DbType.Boolean, user.IsDisabled);
                        databaseInstance.AddInParameter(command, "@p_DBAction", DbType.Int32, (int)DBAction.Save);
                        databaseInstance.AddOutParameter(command, "@p_IsSuccess", DbType.Boolean, 1);
                        databaseInstance.AddOutParameter(command, "@p_MessageID", DbType.String, 10);
                        databaseInstance.AddOutParameter(command, "@p_MessageText", DbType.String, 200);
                        databaseInstance.AddOutParameter(command, "@p_InsertedID", DbType.Int16, 10);

                        databaseInstance.ExecuteNonQuery(command);

                        message.IsSuccess = Convert.ToBoolean(command.Parameters["@p_IsSuccess"].Value);
                        message.MessageId = Convert.ToString(command.Parameters["@p_MessageID"].Value);
                        message.Description = Convert.ToString(command.Parameters["@p_MessageText"].Value);
                        if (command.Parameters["@p_InsertedID"].Value != DBNull.Value)
                        {
                            message.RecordId = Convert.ToInt32(command.Parameters["@p_InsertedID"].Value);
                        }
                    }
                }
            }
            catch (Exception sqlEx)
            {
                message.IsSuccess = false;
                message.Description = sqlEx.Message;
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, sqlEx.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, null);
            }
            return message;
        }

        /// <summary>
        /// Use to update the existing user
        /// </summary>
        /// <param name="user">user object coniating the user information</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message UpdateUser(User user)
        {
            Message message = new Message();
            try
            {
                DatabaseInstance = DataManager.GetDatabaseInstance();

                if (DatabaseInstance != null)
                {
                    using (DbCommand command = DatabaseInstance.CreateCommand("Proc_SaveUser"))
                    {
                        databaseInstance.AddInParameter(command, "@p_RecordID", DbType.String, user.UserId);
                        databaseInstance.AddInParameter(command, "@p_UserEmailId", DbType.String, user.UserEmailId);
                        databaseInstance.AddInParameter(command, "@p_FirstName", DbType.String, user.FirstName);
                        databaseInstance.AddInParameter(command, "@p_LastName", DbType.String, user.LastName);
                        databaseInstance.AddInParameter(command, "@p_TenantID", DbType.Int32, user.TenantDetails.RecordId);
                        databaseInstance.AddInParameter(command, "@p_RoleID", DbType.Int32, user.RoleDetails.RecordId);
                        databaseInstance.AddInParameter(command, "@p_ModifiedBy", DbType.Int32, this.AuditTrailDetails.ModifiedBy);
                        databaseInstance.AddInParameter(command, "@p_ModifiedOn", DbType.DateTime, this.AuditTrailDetails.ModifiedOn);
                        databaseInstance.AddInParameter(command, "@p_DBAction", DbType.Int32, (int)DBAction.Update);
                        databaseInstance.AddOutParameter(command, "@p_IsSuccess", DbType.Boolean, 1);
                        databaseInstance.AddOutParameter(command, "@p_MessageID", DbType.String, 10);
                        databaseInstance.AddOutParameter(command, "@p_MessageText", DbType.String, 200);
                        databaseInstance.AddOutParameter(command, "@p_InsertedID", DbType.Int16, 10);

                        databaseInstance.ExecuteNonQuery(command);

                        message.IsSuccess = Convert.ToBoolean(command.Parameters["@p_IsSuccess"].Value);
                        message.MessageId = Convert.ToString(command.Parameters["@p_MessageID"].Value);
                        message.Description = Convert.ToString(command.Parameters["@p_MessageText"].Value);
                        if (command.Parameters["@p_InsertedID"].Value != DBNull.Value)
                        {
                            message.RecordId = Convert.ToInt32(command.Parameters["@p_InsertedID"].Value);
                        }
                    }
                }
            }
            catch (Exception sqlEx)
            {
                message.IsSuccess = false;
                message.Description = sqlEx.Message;
                LoggerFactory.Logger.Log("UserDL", LogEventType.Error, sqlEx.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, null);
            }
            return message;
        }

        /// <summary>
        /// Use the use account details
        /// </summary>
        /// <param name="user">user object coniating the user information</param>
        /// <returns>Message object contains message description and issuccess</returns>
        public Message UpdateUserAccount(User user)
        {
            LoginDL loginDL = new LoginDL();
            loginDL.AuditTrailDetails = this.AuditTrailDetails;
            Message message = loginDL.UpdateUserAccount(ref user);

            return message;

        } 
        #endregion
    }
}
