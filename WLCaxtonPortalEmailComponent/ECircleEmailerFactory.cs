//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ECircleEmailerFactory.cs
// Program Description  : Factory class for ForgetPasswordEmail,NewUserPasswordEmail, and ResetPasswordEmail instances.                       
// Programmed By        : Naushad Ali.
// Programmed On        : 19-Jan-2013
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalBusinessEntity.Enums;
using WLCaxtonPortalBusinessEntity;

#endregion
namespace WLCaxtonPortalEmailComponent
{
    internal class ECircleEmailerFactory
    {
        #region Public methods

        /// <summary>
        /// Returns the instace one of ForgetPasswordEmail,NewUserPasswordEmail, and ResetPasswordEmail class
        /// </summary>
        /// <param name="emailType">type of email to be sent such as ForgetPasswordEmail,NewUserPasswordEmail, and ResetPasswordEmail</param>
        /// <param name="userXmlSpec">xml message for ECircle API to send email</param>
        /// <returns></returns>
        internal ECircleEmailer GetECircleEmailer(EmailType emailType, UserXmlSpec userXmlSpec)
        {
            ECircleEmailer ECircleEmailerObj = null;
            switch (emailType)
            {
                case EmailType.NewUserPassword:
                    ECircleEmailerObj = new NewUserPasswordEmail(userXmlSpec);
                    break;
                case EmailType.ResetPassword:
                    ECircleEmailerObj = new ResetPasswordEmail(userXmlSpec);
                    break;
                case EmailType.ForgetPassword:
                    ECircleEmailerObj = new ForgetPasswordEmail(userXmlSpec);
                    break;
            }
            return ECircleEmailerObj;
        } 
        #endregion
    }
}
