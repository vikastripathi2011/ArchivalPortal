using Amazon.S3;
using Amazon.S3.Model;
using AWSS3ServiceLayer.ServiceClass;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace AWSS3ServiceLayer
{

    public class AWSS3Cls : DBConnection
    {
        IAmazonS3 client;
        UserConfigDetails userConfigDetails = new UserConfigDetails();
        UserConfiguration userConfiguration = new UserConfiguration();
        string pathForLog = System.Configuration.ConfigurationManager.AppSettings["PathForLog"].ToString();
        public AWSS3Cls(string userName)
        {
            AmazonS3Config config = new AmazonS3Config();

            string accessKey = string.Empty;
            string secretKey = string.Empty;
            // GetDBConnection();

            userConfiguration = userConfigDetails.GetUserConfigDetails(userName);
            if (userConfiguration != null)
            {
                accessKey = userConfiguration.accessKey;
                secretKey = userConfiguration.secreateKey;
                if (userConfiguration.storageType == System.Configuration.ConfigurationManager.AppSettings["StorageType"])
                {
                    string endPoint = System.Configuration.ConfigurationManager.AppSettings["RegionEndpoint"];

                    switch (endPoint)
                    {
                        case "EAST":
                            config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
                            break;
                        case "WEST":
                            config.RegionEndpoint = Amazon.RegionEndpoint.USWest1;
                            break;
                        default:
                            config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;
                            break;


                    }
                    //    config.RegionEndpoint = Amazon.RegionEndpoint.USEast1;

                }
                else if (userConfiguration.storageType == System.Configuration.ConfigurationManager.AppSettings["StorageTypeHCP"])
                {
                    config.ServiceURL = "https://" + userConfiguration.tenant + "." + userConfiguration.hostname + "/";
                    config.UseHttp = true;

                }
                client = new AmazonS3Client(accessKey, secretKey, config);
                ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            }
        }

        #region  "Amazon Storage Server region ==>>"
        public bool CreateABucket(string bucketName)
        {
            bool flag = false;
            try
            {
                PutBucketRequest request = new PutBucketRequest();
                request.BucketName = bucketName;
                client.PutBucket(request);
                flag = true;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    flag = false;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
        public List<Bucket> ListingBuckets(string bucketName)
        {
            List<Bucket> lstBucket = new List<Bucket>();
            int Count = 1;
            try
            {
                ListBucketsResponse response = client.ListBuckets();
                // GetBucketPolicyResponse BPresponse = client.GetBucketPolicy("vikasBucket2018");//check to apply policy on bucket
                foreach (S3Bucket bucket in response.Buckets)
                {
                    if (bucket.BucketName.Contains(bucketName) || bucketName == "")
                    {
                        lstBucket.Add(new Bucket
                        {
                            BucketID = Count,
                            BucketName = bucket.BucketName,
                            CreatedDate = Convert.ToString(bucket.CreationDate)
                        });
                        Count += 1;
                    }
                }
                return lstBucket;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
        public bool DeleteBucket(string bucketName)
        {
            bool flag = false;
            try
            {
                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = bucketName;
                ListObjectsResponse response = client.ListObjects(request);
                foreach (S3Object entry in response.S3Objects)
                {
                    DeletingAnObject(bucketName, entry.Key);
                }
                DeleteBucketRequest delRequest = new DeleteBucketRequest();
                delRequest.BucketName = bucketName;
                client.DeleteBucket(delRequest);
                flag = true;


            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    flag = false;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
        public bool DeletingAnObject(string bucketName, string key)
        {
            bool flag = false;
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key

                };

                client.DeleteObject(request);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    flag = false;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
        public bool WritingAnObject(string bucketName, string fileName, string filePath)
        {
            bool flag = false;
            try
            {
                // simple object put
                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = bucketName;
                request.Key = "reports/" + fileName;
                request.FilePath = filePath;

                // Each user defined metadata must start from "x-amz-meta-"
                request.Metadata.Add("x-amz-meta-guid", "Test GUID data");
                request.CannedACL = S3CannedACL.PublicRead;
                client.PutObject(request);

                // client.PutObject(request);
                flag = true;

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    flag = false;
                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
        public List<Key> ListingObjects(string bucketName, string fileName)
        {
            List<Key> lstKey = new List<Key>();
            List<Key> verlstKey = new List<Key>();
            int Count = 1;
            string S3_KEY = "x-amz-meta-guid";
            try
            {
                /************************ UUID with Document  *************************************/
                ListObjectsRequest request = new ListObjectsRequest();
                ListVersionsRequest requestVer = new ListVersionsRequest();
                request.BucketName = requestVer.BucketName = bucketName;

                ListVersionsResponse responseVersion = client.ListVersions(requestVer);
                foreach (S3ObjectVersion version in responseVersion.Versions)
                {
                    string filenam = version.Key.Split('/')[version.Key.Split('/').Length - 1];
                    if (fileName == "" || filenam.ToLower().Contains(fileName.ToLower()))
                    {
                        verlstKey.Add(new Key
                        {
                            ID = Count,
                            VersionID = version.VersionId,
                            FileName = version.Key,
                            DocumentID = Convert.ToString(Count),
                            BucketName = version.BucketName,
                            CreatedDate = Convert.ToString(version.LastModified),
                            Url = version.BucketName + "/" + version.Key
                        });
                        Count += 1;
                    }
                }
                /************************ fetch Document  *************************************/
                ListObjectsResponse response = client.ListObjects(request);
                foreach (S3Object entry in response.S3Objects)
                {

                    string filename = entry.Key.Split('/')[entry.Key.Split('/').Length - 1];
                    if (fileName == "" || filename.ToLower().Contains(fileName.ToLower()))
                    {
                        lstKey.Add(new Key
                        {
                            ID = Count,
                            VersionID = Convert.ToString((from x in verlstKey select new { x.VersionID, x.FileName }).Where(a => a.FileName == filename)),
                            DocumentID = Convert.ToString(Count),
                            BucketName = entry.BucketName,
                            FileName = filename,
                            CreatedDate = Convert.ToString(entry.LastModified),
                            Url = entry.BucketName + "/" + entry.Key
                        });
                        Count += 1;
                    }
                }

                var Lst = (from x in lstKey
                           join y in verlstKey
                           on x.FileName equals y.FileName
                           select x.VersionID = String.Format("{0} {1} {2} {3} {4} {5}", y.VersionID, x.ID, x.BucketName, x.FileName, x.CreatedDate, x.Url)).ToList();


                // list only things starting with "foo"
                return verlstKey;

                //return lstKey;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region  "Hitachi Storage Server region ==>>"

        //fetch list form bucket from Hitachi Storage Server
        public List<Key> ListBucketItem(string bucketName, string fileName)
        {
            List<Key> lstKey = new List<Key>();
            try
            {
                ListObjectsRequest requestList = new ListObjectsRequest();
                requestList.BucketName = bucketName;
                Log.Success("fetching List of Item  from Bucket ", DateTime.Now.ToString(), pathForLog);
                ListObjectsResponse response = client.ListObjects(requestList);
                foreach (S3Object entry in response.S3Objects)
                {
                    string filename = entry.Key.Split('/')[entry.Key.Split('/').Length - 1];
                    if (fileName == "" || filename.ToLower().Contains(fileName.ToLower()))
                    {
                        lstKey.Add(new Key
                        {
                            BucketName = entry.BucketName,
                            FileName = filename,
                            CreatedDate = Convert.ToString(entry.LastModified),
                            //Url = entry.BucketName + "/" + entry.Key
                            Url = entry.Key
                        });
                    }

                }
                Log.Success("List of item - " + lstKey.Count + " are available in Bucket ", DateTime.Now.ToString(), pathForLog);
                return lstKey;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    Log.Failed("Invalid AccessKeyId & Invalid Security", DateTime.Now.ToString(), pathForLog);
                else
                    Log.Failed("Something went wrong", DateTime.Now.ToString(), pathForLog);
            }
            catch (Exception ex)
            {
                Log.Failed(" ListBucketItem Error-: " + ex.Message, DateTime.Now.ToString(), pathForLog);
            }
            return lstKey;
        }

        //List of available Bucket
        public List<Bucket> ListOfBucketsFromHCP(string bucketName)
        {
            List<Bucket> lstBucket = new List<Bucket>();
            int Count = 1;
            try
            {
                Log.Success("fetching List of Bucket ", DateTime.Now.ToString(), pathForLog);

                ListBucketsResponse response = client.ListBuckets();
                foreach (S3Bucket bucket in response.Buckets)
                {
                    if (bucket.BucketName.Contains(bucketName) || bucketName == "")
                    {
                        lstBucket.Add(new Bucket
                        {
                            BucketID = Count,
                            BucketName = bucket.BucketName,
                            CreatedDate = Convert.ToString(bucket.CreationDate)
                        });
                        Count += 1;
                    }
                }
                Log.Success("Total - " + lstBucket.Count + "Bucket are available. ", DateTime.Now.ToString(), pathForLog);
                return lstBucket;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    Log.Failed("Invalid AccessKeyId & Invalid Security", DateTime.Now.ToString(), pathForLog);
                else
                    Log.Failed("Something went wrong", DateTime.Now.ToString(), pathForLog);
            }
            catch (Exception ex)
            {
                Log.Failed("ListOfBucketsFromHCP Error-: " + ex.Message, DateTime.Now.ToString(), pathForLog);
            }
            return lstBucket;
        }


        //Download specific Items from Bucket  
        public byte[] GetHitachiObject(string bucketName, string filename, string DocumentPath)
        {
            byte[] pdfDocument = null;
            try
            {
                //using (client = new AmazonS3Client(base64UserName, md5Password, config))
                //{
                //    ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                //Console.WriteLine("Retriveing A Bucket Item:");
                /*******************LOG Write ************************/
                using (System.IO.FileStream fs = new System.IO.FileStream(System.Web.HttpContext.Current.Server.MapPath("~/Log/ProgramLog" + "-" + DateTime.Today.ToString("yyyyMMdd") + "." + "txt"), System.IO.FileMode.Append, System.IO.FileAccess.Write))
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                {
                    sw.WriteLine("Retriveing A Bucket Item: DocumentPath  ==> " + DocumentPath);
                }
                /*******************************************/
                GetObjectRequest requestObj = new GetObjectRequest();
                requestObj.BucketName = bucketName;
                requestObj.Key = filename;
                // GetObjectResponse Objresponse = client.GetObject(requestObj);
                // Objresponse.WriteResponseStreamToFile(@"D:\JD\DEV_RSA_Banner_Template.pdf");
                
                using (GetObjectResponse Objresponse = client.GetObject(requestObj))
                using (Stream responseStream = Objresponse.ResponseStream)
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        using (MemoryStream memstream = new MemoryStream())
                        {
                            reader.BaseStream.CopyTo(memstream);
                            pdfDocument = memstream.ToArray();
                        }
                    }

               Log.Success(" DocumentPath -: " + DocumentPath, DateTime.Now.ToString(), pathForLog); 

}
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    Log.Failed("Invalid AccessKeyId & Invalid Security", DateTime.Now.ToString(), pathForLog);
                else
                    Log.Failed("Something went wrong ==> " + amazonS3Exception.Message, DateTime.Now.ToString(), pathForLog);
            }
            catch (Exception ex)
            {
                Log.Failed(" GetHitachiObject Error-: " + ex.Message, DateTime.Now.ToString(), pathForLog);
            }
            return pdfDocument;
        }

        //List of Bucket Items are mentioned below
        public bool ListingOfHitachFiles(string bucketName, string ObjectName)
        {
            bool flag = false;
            try
            {
                //using (client = new AmazonS3Client(base64UserName, md5Password, config))
                //{
                //    ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Console.WriteLine("List of Bucket Items are mentioned below:");
                ListObjectsRequest requestList = new ListObjectsRequest();
                requestList.BucketName = bucketName;
                ListObjectsResponse response = client.ListObjects(requestList);
                foreach (S3Object o in response.S3Objects)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", o.Key, o.Size, o.LastModified);
                }
                flag = true;
                //}
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    Log.Failed("Invalid AccessKeyId & Invalid Security", DateTime.Now.ToString(), pathForLog);
                else
                    Log.Failed("Something went wrong", DateTime.Now.ToString(), pathForLog);
            }
            catch (Exception ex)
            {
                Log.Failed(" ListingOfHitachFiles Error-: " + ex.Message, DateTime.Now.ToString(), pathForLog);
            }
            return flag;
        }

        public bool DeletingAnObjectHitachi(string bucketName, string ObjectName)
        {
            bool flag = false;
            try
            {
                //using (client = new AmazonS3Client(base64UserName, md5Password, config))
                //{
                //    ServicePointManager.ServerCertificateValidationCallback += delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                Console.WriteLine("Delete a Bucket Item:");
                DeleteObjectRequest request = new DeleteObjectRequest();
                request.BucketName = bucketName;
                request.Key = "RSA_Full3.xml";
                client.DeleteObject(request);

                // DeletingAnObject(bucketName, "RSA_Full3.xml");
                Console.WriteLine("Item deleted Sucessfully:");
                flag = true;
                // }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    Log.Failed("Invalid AccessKeyId & Invalid Security", DateTime.Now.ToString(), pathForLog);
                else
                    Log.Failed("Something went wrong", DateTime.Now.ToString(), pathForLog);
            }
            catch (Exception ex)
            {
                Log.Failed("DeletingAnObjectHitachi Error-: " + ex.Message, DateTime.Now.ToString(), pathForLog);
            }
            return flag;
        }

        #endregion

        private byte[] SplitAndSavePDF(string pdfFilePath)
        {
            byte[] pdfDocument = null;
            using (var fsStream =new  FileStream(pdfFilePath, FileMode.Open))
            {
                byte[] bytes = new byte[fsStream.Length];
                fsStream.Read(bytes, 0, Convert.ToInt32(fsStream.Length));
                pdfDocument = bytes;
                return pdfDocument;

            }
          
        }
    }
}