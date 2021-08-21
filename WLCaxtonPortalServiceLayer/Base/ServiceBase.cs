//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ServiceBase.cs
// Program Description  : Reusable fucntionalty for DataLayer logic
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using WLCaxtonPortalBusinessEntity.Classes; 
#endregion

namespace WLCaxtonPortalServiceLayer.Base
{
    public abstract class ServiceBase
    {
        #region Protectec members

        /// <summary>
        /// Return the soap spefic fault to the application
        /// </summary>
        /// <param name="ex">Normal Exception </param>
        /// <param name="reason">reason set by calling method</param>
        /// <param name="title">title set by calling method</param>
        /// <returns></returns>
        protected static FaultException<ServiceException> GetFaultException(Exception ex, string reason, string title)
        {
            ServiceException exception = new ServiceException
            {
                ExceptionMessage = title,
                InnerException = ex.Message,
                StackTrace = ex.StackTrace
            };
            return new FaultException<ServiceException>(exception, reason);
        } 
        #endregion
    }
}
