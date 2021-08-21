//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SubModule.cs
// Program Description  : This class works as Business Entity for SubModule.
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
    /// This class works as Business Entity for SubModule.
    /// </summary>
    [DataContract]
   public class SubModule : RecordIdNameDescription
    {
        [DataMember]
        public Module ParentModule { get; set; }
        [DataMember]
        public int DisplayOrder { get; set; }
        [DataMember]
        public string PageURL { get; set; }
        [DataMember]
        public int ParentId { get; set; }
    }
}
