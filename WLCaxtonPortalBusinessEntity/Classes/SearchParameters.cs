//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SearchParameters.cs
// Program Description  : This class works as Business Entity for SearchParameters.
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
    /// This class works as Business Entity for SearchParameters.
    /// </summary>
    [DataContract]
    [Serializable]
    public class SearchParameters
    {
        [DataMember]
        public List<SearchConfigAttribute> SearchConfigAttributes { get; set; }            
    }
}
