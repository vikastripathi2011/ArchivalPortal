using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace AWSS3ServiceLayer.ServiceClass
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