
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AWSS3ServiceLayer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AWSS3FileTransfer" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AWSS3FileTransfer.svc or AWSS3FileTransfer.svc.cs at the Solution Explorer and start debugging.


    public class AWSS3FileTransfer : AWSS3ServiceLayer.ServiceClass.ServiceBase, IAWSS3FileTransfer
    {
        AWSS3Cls objAWSs3 = new AWSS3Cls("");
        public void DoWork()
        {

        }
        /// <summary>
        /// To Create bucket in AWS s3
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketname"></param>
        /// <returns></returns>
        public bool CreateBucket(string Username, string client, string bucketname)
        {
            bool flag = false;
            try
            {
                objAWSs3 = new AWSS3Cls(Username);
                flag = objAWSs3.CreateABucket(bucketname);
                return flag;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in CreateBucket()");
            }
        }
        /// <summary>
        /// To get all buckets
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketname"></param>
        /// <returns></returns>
        public List<Bucket> GetBucketList(string Username, string client, string bucketname)
        {  
            //String buketName = "DEV-RSA"; // check for HCP 
            var objBucketList = new List<Bucket>();
            try
            {
                objBucketList = objAWSs3.ListingBuckets(bucketname);
                return objBucketList;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetBucketList()");
            }
        }
        /// <summary>
        /// To delete a bucket from AWS S3 
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public bool DeleteBucket(string Username, string client, string bucketName)
        {
            bool flag = false;
            try
            {
                flag = objAWSs3.DeleteBucket(bucketName);
                return flag;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in DeleteBucket()");
            }
        }
        /// <summary>
        /// To delete a file in a bucket
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="key"></param>
        public bool DeletingAnObject(string Username, string client, string bucketName, string key)
        {
            bool flag = false;
            try
            {
                flag = objAWSs3.DeletingAnObject(bucketName, key);
                return flag;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in DeletingAnObject()");
            }
        }
        /// <summary>
        /// To get all files from a bucket
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<Key> ListingObjects(string Username, string client, string bucketName, string fileName)
        {
            List<Key> objKeys = new List<Key>();
            try
            {
                objKeys = objAWSs3.ListingObjects(bucketName, fileName);
                return objKeys;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in ListingObjects()");
            }
        }


        /// <summary>
        /// To get all files from a bucket
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public List<Key> ListBucketItem(string bucketName, string fileName)
        {
            List<Key> objKeys = new List<Key>();
            // IEnumerable<string> objKeys;
            try
            {
                objKeys = objAWSs3.ListBucketItem(bucketName, fileName);
                return objKeys;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in List Bucket Item()");
            }
        }

        /// <summary>
        /// To write a file to a bucket
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="client"></param>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool WritingAnObject(string Username, string client, string bucketName, string fileName, string filePath)
        {
            bool IsCreated = false;
            try
            {
                IsCreated = objAWSs3.WritingAnObject(bucketName, fileName, filePath);
                return IsCreated;
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in WritingAnObject()");
            }
        }
        public byte[] GetHitachiObject(string bucketName, string filename, string DocumentPath)
        {
            try
            {
                return objAWSs3.GetHitachiObject(bucketName, filename, DocumentPath);
            }
            catch (Exception ex)
            {
                throw GetFaultException(ex, ex.Message, "Exception occured in GetHitachiObject()");
            }

        }

    }
}
