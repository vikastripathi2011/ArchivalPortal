//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : User.cs
// Program Description  : This class works as Business Entity for User.
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
    /// This class works as Business Entity for User.
    /// </summary>
    [DataContract]
    [Serializable]
    public class User 
    {
        private UserActivityType userActivity = UserActivityType.None;
            
        [DataMember]
        [Required]
        [DenyHtmlInput]
        public string FirstName { get; set; }
        [DataMember]
        [Required]
        [DenyHtmlInput]
        public string LastName { get; set; }
       
        [DataMember]
        [Required]
        public Role RoleDetails { get; set; }
        [DataMember]
        public IList<Module> AccessibleModuleList { get; set; }
        [DataMember]
        public IList<SubModule> AccessibleSubModuleList { get; set; }
        [DataMember]
        public Tenant TenantDetails { get; set; }

        [DataMember(EmitDefaultValue=true)]
        public UserActivityType UserActivity
        {
            get
            {
                return userActivity;
            }
            set
            {
                userActivity = value;
            }
        }

        [DataMember]
        public bool IsAccountLock { get; set; }
        [DataMember]
        public Message MessageDetails { get; set; }
        [DataMember]
        public string DocumentGuid { get; set; }
        [DataMember]
        public Login LoginDetails { get; set; }


        [DataMember]
        [Required]
        [DenyHtmlInput]
        public string UserEmailId { get; set; }
        [DataMember]
        public string StatusName { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public bool IsDisabled { get; set; }      
    }
}
