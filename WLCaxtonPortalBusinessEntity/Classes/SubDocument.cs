//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SubDocument.cs
// Program Description  : This class works as Business Entity for Sub Document.
// Programmed By        : Manoj Pachauri
// Programmed On        : 05-Jan-2013 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This class works as Business Entity for Sub Document.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SubDocument : RecordIdNameDescription
    {
        [DataMember]
        public string ParentDocGuid { get; set; }
        [DataMember]
        public string ParentDocAccountNumber { get; set; }
        [DataMember]
        public int? SubDocumentId { get; set; }
        [DataMember]
        public string SubDocumentName { get; set; }
        [DataMember]
        public int? SubDocumentStartPage { get; set; }
        [DataMember]
        public int? PageCount { get; set; }
        
    }
}
