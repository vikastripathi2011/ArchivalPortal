//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ResetPasswordEmail.cs
// Program Description  : Send the email to ECircle API                  
// Programmed By        : Naushad Ali.
// Programmed On        : 24-Jan-2013
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalBusinessEntity.Classes;
using WLCaxtonPortalBusinessEntity;
using System.Configuration;
#endregion
namespace WLCaxtonPortalEmailComponent
{
    public class ResetPasswordEmail : ECircleEmailer
    {
        #region Constructors
        public ResetPasswordEmail() { }

        public ResetPasswordEmail(UserXmlSpec userXmlSpec)
            : base(userXmlSpec) { }        
        #endregion

        #region Overrided methods
        public override bool SendMail(EmailMessage emailMessage)
        {
            try
            {
                base.CreateSession();
                base.CreateUser();

                string singleMessageId = Convert.ToString(ConfigurationManager.AppSettings["SingleMessageId_ResetPassword"]);
                base.EcMSoapBridgeServiceProxy.sendParametrizedSingleMessageToUser(base.Session, singleMessageId, base.UserId, emailMessage.Names, emailMessage.Values);
                Close();
                return true;
            }
            catch
            {
                Abort();
                throw;
            }
        } 
        #endregion
    }
}
