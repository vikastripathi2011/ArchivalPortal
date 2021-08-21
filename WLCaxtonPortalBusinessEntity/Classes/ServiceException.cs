//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ServiceException.cs
// Program Description  : This class works as Business Entity for ServiceException.
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
    /// This class works as Business Entity for ServiceException.
    /// </summary>
    [DataContract()]
    public class ServiceException
    {
        [DataMember()]
        public string Title { get; set; }

        [DataMember()]
        public string ExceptionMessage { get; set; }

        [DataMember()]
        public string InnerException { get; set; }

        [DataMember()]
        public string StackTrace { get; set; }
    }
}
