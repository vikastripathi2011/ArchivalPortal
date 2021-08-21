//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Root.cs
// Program Description  : This class works as Business Entity for Root.
// Programmed By        : Naushad Ali
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WLCaxtonPortalBusinessEntity.Classes
{
    /// <summary>
    /// This class works as Business Entity for Root.
    /// </summary>
    [DataContract]
    public class Root
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string FirstName { get; set; }


        [DataMember]
        public string LastName { get; set; }

        [DataMember(Name="Real_Email")]
        [XmlElement("Real_Email")]
        public string RealEmail { get; set; }
    }
}
