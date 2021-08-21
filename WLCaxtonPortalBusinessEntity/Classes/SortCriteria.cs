//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SortCriteria.cs
// Program Description  : This class works as Business Entity for SortCriteria.
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
    /// This class works as Business Entity for SortCriteria.
    /// </summary>
    [DataContract]  
    public class SortCriteria
    {
        [DataMember]
        public int CriteriaId { get; set; }

        [DataMember]
        public string CriteriaName { get; set; }

        [DataMember]
        public string CriteriaValue { get; set; }

        [DataMember]
        public bool SortAsc { get; set; }        
    }
}
