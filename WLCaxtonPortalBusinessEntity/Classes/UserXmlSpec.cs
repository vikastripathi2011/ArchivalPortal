//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : UserXmlSpec.cs
// Program Description  : This class works as Business Entity for UserXmlSpec.
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using WLCaxtonPortalBusinessEntity.Classes;
using System.Xml.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This class works as Business Entity for UserXmlSpec.
    /// </summary>
    [DataContract]
    [XmlRoot("userXmlSpec")] //[XmlRoot("userXml")]
    public class UserXmlSpec
    {
        [DataMember]
        public Root Root { get; set; }
    }
}
