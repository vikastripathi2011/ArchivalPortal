using System.Runtime.Serialization;

namespace AWSS3ServiceLayer.ServiceClass
{
    [DataContract]
    public class ServiceException
    {
        [DataMember()]
        public string Title { get; set; }

        [DataMember()]
        public string ExceptionMessage { get; set; }

        [DataMember()]
        public string InnerException { get; set; }

        [DataMember()]
        public string StackTrace { get; set; }
    }
}