//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : EmailMessage.cs
// Program Description  : This class works as Business Entity for EmailMessage.
// Programmed By        : Naushad Ali
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
    /// This class works as Business Entity for EmailMessage.
    /// </summary>
    [DataContract]
    public class EmailMessage
    {
        [DataMember]
        public string[] Names { get; set; }

        [DataMember]
        public string[] Values { get; set; }
    }
}
