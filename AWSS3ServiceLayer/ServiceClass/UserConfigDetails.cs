
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AWSS3ServiceLayer
{
    public class UserConfiguration
    {
        public string userName { get; set; }
        public string userEmail { get; set; }
        public string storageType { get; set; }
        public string accessKey { get; set; }
        public string secreateKey { get; set; }
        public string tenant { get; set; }
        public string hostname { get; set; }
    }
    public class UserConfigDetails
    {
        string pathForLog = System.Configuration.ConfigurationManager.AppSettings["PathForLog"].ToString();

        public UserConfiguration GetUserConfigDetails(string userName)
        {
            UserConfiguration userConfiguration = new UserConfiguration();
            String username = ConfigurationManager.AppSettings["UserName"].ToString();
            String pwdPath = ConfigurationManager.AppSettings["RSA_HCP_PwdPath"];
            String tenantName = ConfigurationManager.AppSettings["TenantName"];
            String hcpHost = ConfigurationManager.AppSettings["Host"];
            try
            {

                string Sessionid = Guid.NewGuid().ToString();
                String b64Uname, md5Pword;
                MD5 md5 = System.Security.Cryptography.MD5.Create();
                b64Uname = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));
                md5Pword = MD5Encode(GetHCPPassword(pwdPath));
                byte[] data = Convert.FromBase64String(b64Uname);
                string decodedString = Encoding.UTF8.GetString(data);

                /************ For AWS S3 ************/
                //userConfiguration.accessKey = "AKIAI6CZCT6BRIHTFZIQ";//"AKIAJ6BB54BIAZCIWSAQ";//vikas rsa-dev// Convert.ToString(ds.Tables[0].Rows[0][2]);
                //userConfiguration.secreateKey = "PeVcubWAkBUR/RaA/kDcAoKsablcyEYTsXCTtuci";//"xhMka0zUsJ48lB6KqkKOUGfYuWoXsnA07OqdDdiP";//Convert.ToString(ds.Tables[0].Rows[0][3]);
                //userConfiguration.storageType = "AWS";
                /************ For HCP S3 ************/
                userConfiguration.accessKey = b64Uname;
                userConfiguration.secreateKey = md5Pword;
                userConfiguration.tenant = tenantName;
                userConfiguration.hostname = hcpHost;
                userConfiguration.storageType = "HCP";

            }
            catch (Exception ex)
            {
                userConfiguration = null;
            }
            return userConfiguration;
        }

        String MD5Encode(String inString)
        {
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();

            byte[] tBytes = Encoding.ASCII.GetBytes(inString);
            byte[] hBytes = hasher.ComputeHash(tBytes);

            StringBuilder sb = new StringBuilder();
            for (Int32 i = 0; i < hBytes.Length; i++)
                sb.AppendFormat("{0:x2}", hBytes[i]);

            return sb.ToString();
        }

        public string GetHCPPassword(string path)
        {
            string readText = string.Empty;
            try
            {
                using (var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.IO.FileNotFoundException fnfe)
            {
                // Handle file not found.  
                ServiceClass.Log.Success("HCP Password file not found - " + fnfe, DateTime.Now.ToString(), pathForLog);
            }
            return readText;
        }
    }
}