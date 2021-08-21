//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : AuditTrailDL.cs
// Program Description  : AuditTrailDL class for logging activity
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
using WLCaxtonPortalExceptionLogger;
using WLCaxtonPortalDataHelper;
using System.Data;
using System.Data.Common;
#endregion

namespace WLCaxtonPortalDataLayer
{
    public class AuditTrailDL
    {
        #region Private variables
        private static IDatabase databaseInstance;
        private static IDbCommand dbCommand = null;
        #endregion

        #region Public methods
        /// <summary> Modified by : VRT: 22-MAR-2018,
        /// Log the user activity
        /// </summary>
        /// <param name="user">use infromation</param>
        /// <returns>Message object contains message description and issuccess</returns>

        public static Message LogUserActivity(User user)
        {
            databaseInstance = DataManager.GetDatabaseInstance();

            Message message = new Message();
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                DbCommand dbCommand = databaseInstance.CreateCommand("Proc_LogUserActivity");
                databaseInstance.AddInParameter(dbCommand, "@RecordId", DbType.Int32, user.LoginDetails.RecordId);
                databaseInstance.AddInParameter(dbCommand, "@DocumentGuid", DbType.Int32, user.DocumentGuid);
                databaseInstance.AddInParameter(dbCommand, "@UserActivity", DbType.String, Convert.ToInt16(user.UserActivity));
                databaseInstance.AddInParameter(dbCommand, "@p_CreatedOn", DbType.DateTime, DateTime.Now);
                databaseInstance.AddOutParameter(dbCommand, "@p_IsSuccess", DbType.Boolean, 0);
                databaseInstance.AddOutParameter(dbCommand, "@p_MessageID", DbType.String, 10);
                databaseInstance.AddOutParameter(dbCommand, "@p_MessageText", DbType.String, 200);

                int result = databaseInstance.ExecuteNonQuery(dbCommand);

                message.IsSuccess = (bool)dbCommand.Parameters["@p_IsSuccess"].Value;
                message.MessageId = (string)dbCommand.Parameters["@p_MessageID"].Value;
                message.Description = (string)dbCommand.Parameters["@p_MessageText"].Value;

                if (result > -1)
                {
                    message.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("AuditTrailDL", LogEventType.Error, ex.Message);
                message.IsSuccess = false;
                throw;
            }

            return message;
        }


        //public static Message LogUserActivity(User user)
        //{
        //    databaseInstance = DataManager.GetDatabaseInstance();

        //    string logQuery = "INSERT INTO UserActivity_Log( FK_User_Master_UserId, ActivityTimeStamp, ActivityTimeStampUTC, DocumentGuid, FK_UserActivity_Master_ActivityId, Disabled ) " +
        //                       " VALUES ( " + user.LoginDetails.RecordId + ",'" + DateTime.Now + "', GETUTCDATE(),'" + user.DocumentGuid + "'," + Convert.ToInt16(user.UserActivity) + ", 0)";


        //    Message message = new Message();

        //    try
        //    {
        //        dbCommand = databaseInstance.CreateSqlQuery(logQuery);
        //        int result = dbCommand.ExecuteNonQuery();
        //        if (result > -1)
        //        {
        //            message.IsSuccess = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerFactory.Logger.Log("AuditTrailDL", LogEventType.Error, ex.Message);
        //        message.IsSuccess = false;
        //        throw;
        //    }

        //    return message;
        //} 
        #endregion
    }
}
