//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Login.cs
// Program Description  : This class works as Business Entity for Login.
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This class works as Business Entity for Login.
    /// </summary>
    [DataContract]
    [Serializable]
    public class Login : RecordIdNameDescription
    {
        [DataMember]
        [Required]
        public string EmailId { get; set; }
        [DataMember]
        //[Required]
        public string Password { get; set; }
        [DataMember]
        public bool IsAuthenticated { get; set; }
        [DataMember]
        public bool IsTemporaryPassword { get; set; }
        [DataMember]
        public string NewPassword { get; set; }
        [DataMember]
        public string SessionId { get; set; }
        [DataMember]
        public string SystemIP { get; set; }
        [DataMember]
        public bool IsPasswordExpired { get; set; }
        [DataMember]
        public int LoginAttempts
        {
            get;set;
           
        }
        [DataMember]
        public bool IsAccountLocked { get; set; }
    }
}
