//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : AuditTrail.cs
// Program Description  : This class works as Business Entity for AuditTrail.
// Programmed By        : Nadeem Ishrat
// Programmed On        : 26-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Runtime.Serialization;

namespace WLCaxtonPortalBusinessEntity
{
    /// <summary>
    /// This class works as Business Entity for AuditTrail.
    /// </summary>
    [DataContract]
    public class AuditTrail : RecordIdNameDescription
    {
        private DBAction dbAction = DBAction.None;

        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public int ModifiedBy { get; set; }
        [DataMember]
        public int DeletedBy { get; set; }
        [DataMember]
        public DateTime CreatedOn { get; set; }
        [DataMember]
        public DateTime ModifiedOn { get; set; }
        [DataMember]
        public DateTime DeletedOn { get; set; }
        [DataMember]
        public DBAction DBActionDetails
        {
            get

            { return dbAction; }
            set { dbAction = value; }
        }


    }
}
