//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : RecordIdNameDescription.cs
// Program Description  : This class acts as a base class and provide RecordId, Name, Description to every derived class
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This class acts as a base class and provide RecordId, Name, Description to every derived class
    /// </summary>
    [DataContract]   
    [Serializable]
    public class RecordIdNameDescription //: IExtensibleDataObject
    {
        [DataMember]
        public int RecordId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        
        //public ExtensionDataObject ExtensionData  { get; set; }

        [DataMember]
        public string Query { get; set; }

        [DataMember]
        public string CountQuery { get; set; }    
    }
}
