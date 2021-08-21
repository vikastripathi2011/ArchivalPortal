using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using WLCaxtonMvcWebPortal.HCPServiceReference;
using WLCaxtonMvcWebPortal.Services;
using WLCaxtonPortalExceptionLogger;

namespace WLCaxtonMvcWebPortal.Code
{
    public class Common
    {
        public WLCaxtonPortalServiceClient AddMessageHeader(WLCaxtonPortalServiceClient obj)
        {
            try{

            string path = System.Configuration.ConfigurationManager.AppSettings["AuthenticationFilePath"];
            string userName = string.Empty;
            string usrpwd = string.Empty;
            Dictionary<string, string> keyValuePairs = GetServiceUserNameNPasword(path);
            userName = keyValuePairs["UserName"];
            usrpwd = keyValuePairs["UserPwd"];
            //userName = keyValuePairs.Values;
            SecureString secure = ToSecureString(usrpwd);
           
                obj.ClientCredentials.HttpDigest.ClientCredential = new NetworkCredential(userName, secure);
                obj.ClientCredentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
           }
            catch(Exception ex)
            {
                LoggerFactory.Logger.Log("Success", LogEventType.Info, ex.Message,ex);
                        

            }
                return obj;

        }

        public AWSS3FileTransferClient AWSAddMessageHeader(AWSS3FileTransferClient obj)
        {
            try
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["AuthenticationFilePath"];
                string userName = string.Empty;
                string usrpwd = string.Empty;
                Dictionary<string, string> keyValuePairs = GetServiceUserNameNPasword(path);
                userName = keyValuePairs["UserName"];
                usrpwd = keyValuePairs["UserPwd"];
                //userName = keyValuePairs.Values;
                SecureString secure = ToSecureString(usrpwd);

                obj.ClientCredentials.HttpDigest.ClientCredential = new NetworkCredential(userName, secure);
                obj.ClientCredentials.HttpDigest.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            }
            catch(Exception ex)
            {
                LoggerFactory.Logger.Log("Success", LogEventType.Info, ex.Message,ex);
                        

            }
                return obj;

        }
        public SecureString ToSecureString(string plainString)
        {
            if (plainString == null)
                return null;

            SecureString secureString = new SecureString();
            foreach (char c in plainString.ToCharArray())
            {
                secureString.AppendChar(c);


            }
            return secureString;
        }
        public Dictionary<string, string> GetServiceUserNameNPasword(string path)
        {
            string readText = string.Empty;
            Dictionary<string, string> d = new Dictionary<string, string>();

            try
            {

                using (var sr = new StreamReader(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string key = lines[i].Split('=')[0].Trim();
                        string val = lines[i].Split('=')[1].Trim();
                        d.Add(key, val);
                    }

                }

            }
            catch (System.IO.FileNotFoundException ex)
            {
                // Handle file not found.  

            }
            return d;

        }
    }
}