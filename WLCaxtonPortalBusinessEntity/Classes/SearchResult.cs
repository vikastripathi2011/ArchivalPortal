//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SearchRecord.cs
// Program Description  : This class works as Business Entity for SearchRecord.
// Programmed By        : Naushad Ali
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace WLCaxtonPortalBusinessEntity.Classes
{
    /// <summary>
    /// This class works as Business Entity for SearchRecord.
    /// </summary>
    [DataContract]
    [KnownType(typeof(DataTable))]
    public class SearchResult
    {
        [DataMember]
        public DataTable SearchRecord
        {
            get;
            set;
        }

        [DataMember]
        public DataTable SearchRecordSubDocument
        {
            get;
            set;
        }

        [DataMember]
        public string GroupByColumnName { get; set; }

        [DataMember]
        public string GridHeaderText { get; set; }

        [DataMember]
        public Message MessageDetails { get; set; }
    }
}
