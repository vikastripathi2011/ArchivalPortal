//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : UserActivityType.cs
// Program Description  : This Enum is a collection of distinct value that declares a set of named constants act as a User Activity Type
// Programmed By        : Nadeem Ishrat
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This Enum is a collection of distinct value that declares a set of named constants act as a User Activity Type
    /// </summary>
    [DataContract]
    public enum UserActivityType
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        SuccessfulLogin = 1,
        [EnumMember]
        UnSuccessfulLogin = 2,
        [EnumMember]
        AccountLock = 3,
        [EnumMember]
        AccountUnlock = 4,
        [EnumMember]
        ViewDocumentFirstPage = 5,
        [EnumMember]
        ViewCompleteDocument = 6,
        [EnumMember]
        Login=7,
        [EnumMember]
        Logout=8,
        [EnumMember]
        MultipleSessionsLogout = 9,
        [EnumMember]
        ForgetPassword=10,
        [EnumMember]
        ResetPassword=11,
        [EnumMember]
        CreateUser=12,
        [EnumMember]
        SetActive = 13,
        [EnumMember]
        SetInActive = 14,
        [EnumMember]
        SessionExpired=15
    }
}
