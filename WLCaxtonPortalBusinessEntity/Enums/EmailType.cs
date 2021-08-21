//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : EmailType.cs
// Program Description  : This Enum is a collection of distinct value that declares a set of named constants act as a EmailType
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity.Enums
{
    /// <summary>
    /// This Enum is a collection of distinct value that declares a set of named constants act as a EmailType
    /// </summary>
    [DataContract]
    public enum EmailType
    {
        [EnumMember]
        NewUserPassword,
        
        [EnumMember]
        ResetPassword,

        [EnumMember]
        ForgetPassword
    }
}
