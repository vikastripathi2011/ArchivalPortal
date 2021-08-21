//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ECircleEmailer.cs
// Program Description  : This class provide the base funtionality for ForgetPasswordEmail,NewUserPasswordEmail, and ResetPasswordEmail classes.                            
// Programmed By        : Naushad Ali.
// Programmed On        : 19-Jan-2013
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalBusinessEntity.Classes;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Configuration;
using System.Xml.Linq; 
#endregion

namespace WLCaxtonPortalEmailComponent
{
    public abstract class ECircleEmailer
    {
        #region Private variables
        private UserXmlSpec userXmlSpec; 
        #endregion

        #region Construtors
        public ECircleEmailer()
        {
        }

        public ECircleEmailer(UserXmlSpec userXmlSpec)
        {
            this.userXmlSpec = userXmlSpec;
        } 
        #endregion

        #region Protected members
        protected ECircle_EcMSoapBridgeService_SSL.EcMSoapBridgeService EcMSoapBridgeServiceProxy { get; private set; }

        /// <summary>
        /// Establish the session to the ECircle API
        /// </summary>
        protected virtual void CreateSession()
        {
            try
            {
                string realm = Convert.ToString(ConfigurationManager.AppSettings["realm"]);
                string user = Convert.ToString(ConfigurationManager.AppSettings["user"]);
                string password = Convert.ToString(ConfigurationManager.AppSettings["password"]);

                this.EcMSoapBridgeServiceProxy = new ECircle_EcMSoapBridgeService_SSL.EcMSoapBridgeService();
                this.Session = EcMSoapBridgeServiceProxy.logon(realm, user, password);
            }
            catch
            {
                Abort();
                throw;
            }
        }

        /// <summary>
        /// Create the new user to the ECircle API to whom mail will be sent
        /// </summary>
        protected virtual void CreateUser()
        {
            try
            {
                string userXml = String.Empty;
                if (EmailUtility.Serialize<UserXmlSpec>(userXmlSpec, ref userXml))
                {
                    this.UserId = EcMSoapBridgeServiceProxy.createOrUpdateUserByEmail(this.Session, userXml, true);                                                           
                    
                    //var doc = XElement.Parse(userXml);
                    //foreach (var element in doc.Descendants("Root"))
                    //{
                    //   string email =  element.Elements("Email").Single().Value;
                    //   email = String.Format("{0}{1}", Guid.NewGuid().ToString(), email);
                    //   element.Elements("Email").Single().Value = email;
                    //   break;
                    //}
                    //userXml = doc.ToString();
                    //this.UserId = EcMSoapBridgeServiceProxy.createUser(this.Session, userXml);
                }
            }
            catch
            {
                Abort();
                throw;
            }
        }

        protected string Session { get; private set; }
        protected string UserId { get; private set; }

        /// <summary>
        /// Close the ECIrcle API Web service proxy in grace way
        /// </summary>
        protected void Close()
        {
            if (EcMSoapBridgeServiceProxy != null)
                EcMSoapBridgeServiceProxy.Dispose();
        }

        /// <summary>
        /// /// Abort the ECIrcle API Web service proxy in faulted condition
        /// </summary>
        protected void Abort()
        {
            if (EcMSoapBridgeServiceProxy != null)
                EcMSoapBridgeServiceProxy.Abort();
        } 
        #endregion

        #region Abstract members
        /// <summary>
        /// Method for ForgetPasswordEmail,NewUserPasswordEmail, and ResetPasswordEmail classes
        /// </summary>
        /// <param name="emailMessage">EmailMessage contains email addresses</param>
        /// <returns></returns>
        public abstract bool SendMail(EmailMessage emailMessage); 
        #endregion  
    }
}
