//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : AuditTrailBL.cs
// Program Description  : This class is used to log user Activity in DB using DataLayer.
// Programmed By        : Naushad Ali
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalDataLayer;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonPortalBusinessLogicLayer
{
    /// <summary>
    /// This class is used to log user Activity in DB using DataLayer.
    /// </summary>
    public static class AuditTrailBL
    {
        /// <summary>
        /// This function is used to log user Activity in DB by calling DL's LogUserActivity function.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static Message LogUserActivity(User user)
        {
            try
            {
                return AuditTrailDL.LogUserActivity(user);
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("AuditTrailBL", LogEventType.Error, ex.Message);
                throw;
            }
        }
    }
}
