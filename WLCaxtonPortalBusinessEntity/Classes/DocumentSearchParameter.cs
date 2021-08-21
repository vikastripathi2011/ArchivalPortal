//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DocumentSearchParameter.cs
// Program Description  : This class works as Business Entity for DocumentSearchParameter.
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
    /// This class works as Business Entity for DocumentSearchParameter.
    /// </summary>
    [DataContract]
    public class DocumentSearchParameter : RecordIdNameDescription
    {
        [DataMember]
        public int TenantId { get; set; }

        [DataMember]
        public string UserEmailId { get; set; }

        [DataMember]
        public User UserDetails { get; set; }  
    }
}
