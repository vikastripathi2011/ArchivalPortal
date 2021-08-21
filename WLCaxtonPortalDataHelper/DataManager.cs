//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DataManager.cs
// Program Description  : This class is part of DataManager. It handles the instances of connections.
// Programmed By        : Satyendra Gupta
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace WLCaxtonPortalDataHelper
{
    /// <summary>
    /// This class manage the database instances.
    /// </summary>
    public class DataManager
    {
        /// <summary>
        /// Construct the constructor for DataManager.
        /// </summary>
        private DataManager()
        { }
         
        /// <summary>
        /// This Interface is used to get the database instance on the basis of tenant / customer.
        /// </summary>
        /// <param name="teenantId"></param>
        /// <returns></returns>
        public static IDatabase GetDatabaseInstance(int teenantId = 0)
        {
            NonTenantConnection nonTenantConnection = new NonTenantConnection();
            TenantConnection tenantConnection = new TenantConnection();
            nonTenantConnection.SetNextConnection(tenantConnection);
            
            return nonTenantConnection.GetDatabaseInstance(teenantId);            
        }        
    }
}
