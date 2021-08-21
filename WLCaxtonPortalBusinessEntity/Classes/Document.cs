//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Document.cs
// Program Description  : This class works as Business Entity for Document.
// Programmed By        : Naushad Ali
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This class works as Business Entity for Document.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Document : RecordIdNameDescription
    {
        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public int DocumentId{get;set;}

        [DataMember]
        public int JobId { get; set; }

        [DataMember]
        public string DocumentGuid { get; set; }

        [DataMember]
        public int AdditionalAccountID { get; set; }
                
        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string Address1 { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string Address3 { get; set; }

        [DataMember]
        public string Address4 { get; set; }

        [DataMember]
        public string Address5 { get; set; }

        [DataMember]
        public string PostCode { get; set; }

        [DataMember]
        public string Country { get; set; }
                
        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public DateTime? DocumentDate { get; set; }

        [DataMember]
        public DateTime? DocumentGenerationDateTime { get; set; }

        [DataMember]
        public string Inserts { get; set; }

        [DataMember]
        public ProductCategory ProductCategory { get; set; }

        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public byte[] Stream { get; set; }

        [DataMember]
        public string StreamName { get; set; }

        [DataMember]
        public string Spoolname { get; set; }

        [DataMember]
        public string Correspondencenumber { get; set; }

        [DataMember]
        public string DocumentName { get; set; }

        [DataMember]
        public List<SubDocument> SubDocumentDetails { get; set; }
    }
}