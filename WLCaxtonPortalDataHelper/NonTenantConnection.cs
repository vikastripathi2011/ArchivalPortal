//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : NonTenantConnection.cs
// Program Description  : This class is used for non tenant connection. It handles connection related to WLCaxtonWebPortal.
// Programmed By        : Satyendra Gupta
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WLCaxtonPortalDataHelper
{
    /// <summary>
    /// This class inherits the connection handler.
    /// </summary>
    public class NonTenantConnection : ConnectionHandler
    {
        /// <summary>
        /// This class get the database instance of tenants or WLCaxtonWebPortal connection on the basis of tenant.
        /// </summary>
        /// <param name="teenantId"></param>
        /// <returns></returns>
        public override IDatabase GetDatabaseInstance(int teenantId)
        {
            IDatabase iDatabaseInstance = null;
            if (teenantId <= 0)
            {
                iDatabaseInstance =  new EnterpriseLibraryData("Williams_LeaString");
            }
            else
            {
                if (NextConnection != null)
                    iDatabaseInstance =  NextConnection.GetDatabaseInstance(teenantId);
            }
            return iDatabaseInstance;
        }
    }
}
