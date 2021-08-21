//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ProductCategory.cs
// Program Description  : This class works as Business Entity for ProductCategory.
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
    /// This class works as Business Entity for ProductCategory.
    /// </summary>
    [DataContract]
    [Serializable]
    public class ProductCategory : RecordIdNameDescription
    {
        [DataMember]
        public int ProductCategoryId { get; set; }
        
        [DataMember]
        public string ProductCategoryName { get; set; }

        [DataMember]
        public string ProductCategoryDescription { get; set; }
    }
}
