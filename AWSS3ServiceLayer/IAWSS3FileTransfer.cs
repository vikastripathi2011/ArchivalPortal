using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AWSS3ServiceLayer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAWSS3FileTransfer" in both code and config file together.
    [ServiceContract]
    public interface IAWSS3FileTransfer
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool CreateBucket(string Username, string client, string bucketname);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<Bucket> GetBucketList(string Username, string client, string bucketname);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool DeleteBucket(string Username, string client, string bucketName);
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool DeletingAnObject(string Username, string client, string bucketName, string key);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        List<Key> ListBucketItem(string bucketName, string fileName);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        //IEnumerable<string> ListingObjects(string Username, string client, string bucketName, string fileName);
       List<Key> ListingObjects(string Username, string client, string bucketName, string fileName);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool WritingAnObject(string Username, string client, string bucketName, string fileName, string filePath);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        byte[] GetHitachiObject(string bucketName, string filename, string DocumentPath);

    }

    [DataContract]
    [Serializable]
    public class Bucket
    {
        [DataMember]
        public int BucketID { get; set; }
        [DataMember]
        public string BucketName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
    }
    [DataContract]
    [Serializable]
    public class Key
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string DocumentID { get; set; }
        [DataMember]
        public string VersionID { get; set; }
        [DataMember]
        public string BucketName { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}
