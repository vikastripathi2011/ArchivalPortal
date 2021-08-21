//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Tenant.cs
// Program Description  : This class works as Business Entity for Tenant.
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
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
    /// This class works as Business Entity for Tenant.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Tenant : RecordIdNameDescription
    {
        [DataMember]
        public int UsersLimit{ get; set; }
        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
        public string DatabaseName { get; set; }
        [DataMember]
        public string DatabaseUser { get; set; }
        [DataMember]
        public string DatabasePassword { get; set; }
        [DataMember]
        public string DatabaseInstance { get; set; }
        [DataMember]
        public int MetadataDatabaseMappingId { get; set; }
    }
}
