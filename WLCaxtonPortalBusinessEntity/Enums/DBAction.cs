//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DBAction.cs
// Program Description  : This Enum is a collection of distinct value that declares a set of named constants act as a DBAction in database
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    ///  This Enum acts as a fasad of DBAction in database
    /// </summary>
    [DataContract]
    public enum DBAction
    {
        [EnumMember]
        None=0,
        [EnumMember]
        Read = 1,
        [EnumMember]
        Save = 2,
        [EnumMember]
        Update = 3,
        [EnumMember]
        Deleted = 4

    }
}
