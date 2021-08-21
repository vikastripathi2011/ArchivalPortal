//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ECircleWrapper.cs
// Program Description  : Entry point to the Email functionality                      
// Programmed By        : Naushad Ali.
// Programmed On        : 20-Jan-2013
// Version              : 1.0.0
//==========================================================================================


#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalBusinessEntity.Enums; 
#endregion

namespace WLCaxtonPortalEmailComponent
{
    public class ECircleWrapper
    {
        #region Public methods
        /// <summary>
        /// Send mail to the outside caller. Provide the abstraction to the internal email logic
        /// </summary>
        /// <param name="userXmlSpec">xml message for ECIrcle EPI email functionality</param>
        /// <param name="emailMessage">Contains the values such password etc...</param>
        /// <param name="emailType">email type to be sent such as ForgetPasswordEmail,NewUserPasswordEmail, and ResetPasswordEmail </param>
        /// <returns></returns>
        public static bool SendMail(UserXmlSpec userXmlSpec, EmailMessage emailMessage, EmailType emailType)
        {
            ECircleEmailerFactory factory = new ECircleEmailerFactory();
            ECircleEmailer eCircleEmailer = factory.GetECircleEmailer(emailType, userXmlSpec);
            try
            {
                eCircleEmailer.SendMail(emailMessage);
                return true;
            }
            catch
            {
                throw;
            }
        } 
        #endregion
    }
}
