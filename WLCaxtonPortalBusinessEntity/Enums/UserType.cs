//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : UserType.cs
// Program Description  : This Enum is a collection of distinct value that declares a set of named constants act as a User Type
// Programmed By        : Manoj Pachauri
// Programmed On        : 07-Janauary-2013 
// Version              : 1.0.0
//==========================================================================================
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This Enum is a collection of distinct value that declares a set of named constants act as a User Type
    /// </summary>
    [DataContract]
    public enum UserType
    {
        [EnumMember]
        WLSuperUser = 1,
        [EnumMember]
        TenantAdmin = 2,
        [EnumMember]
        User = 3
    }
}
