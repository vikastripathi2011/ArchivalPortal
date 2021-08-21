//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Module.cs
// Program Description  : This class works as Business Entity for Module.
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
    /// This class works as Business Entity for Module.
    /// </summary>
    [DataContract]
    public class Module : RecordIdNameDescription
    {
        [DataMember]
        public int DisplayOrder { get; set; }
           [DataMember]
        public string PageURL { get; set; }
    }
}
