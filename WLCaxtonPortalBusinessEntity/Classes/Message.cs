//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Message.cs
// Program Description  : This class works as Business Entity for Message.
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
    /// This class works as Business Entity for Message.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Message : RecordIdNameDescription
    {
        [DataMember]
        public string MessageId { get; set; }

        [DataMember]
        public bool  IsSuccess { get; set; }
    }
}
