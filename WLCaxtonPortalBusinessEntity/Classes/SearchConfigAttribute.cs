//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SearchConfigAttribute.cs
// Program Description  : This class works as Business Entity for SearchConfigAttribute.
// Programmed By        : Naushad Ali
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity.Classes
{
    /// <summary>
    /// This class works as Business Entity for SearchConfigAttribute.
    /// </summary>
    [DataContract]
    public class SearchConfigAttribute:RecordIdNameDescription
    {        
        [DataMember]
        public int ParameterId { get; set; }

        [DataMember]
        public string ParameterName { get; set; }

        [DataMember]
        public string ParameterDescription{ get; set; }

        [DataMember]
        public string ParameterTypeName { get; set; }

        [DataMember]
        public int DisplayOrder { get; set; }

        [DataMember]
        public bool IsVisible { get; set; }

        [DataMember]
        public bool IsGroupBy { get; set; }        
    }
}
