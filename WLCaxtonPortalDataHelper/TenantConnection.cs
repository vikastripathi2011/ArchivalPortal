//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : TenantConnection.cs
// Program Description  : This class is used for tenant connection. It handles connection related to Tenant / Customer.
// Programmed By        : Satyendra Gupta
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WLCaxtonPortalDataHelper
{
    /// <summary>
    /// This class inherits the connection handler.
    /// </summary>
    public class TenantConnection : ConnectionHandler
    {
        /// <summary>
        /// This class get the database instance of tenants connection on the basis of tenant.
        /// </summary>
        /// <param name="teenantId"></param>
        /// <returns></returns>
        public override IDatabase GetDatabaseInstance(int teenantId)
        {
            IDbCommand dbCommand = null;
            NonTenantConnection nonTenantConnection = new WLCaxtonPortalDataHelper.NonTenantConnection();
            IDatabase databaseInstance = nonTenantConnection.GetDatabaseInstance(0);

            try
            {
                dbCommand = databaseInstance.CreateSqlQuery(string.Format("{0}{1}",
                "Select DatabaseConnectionString from Tenant_Master WHERE PK_Tenant_Master_TenantId = ", teenantId));
                string connectionString = (string)dbCommand.ExecuteScalar();

                if (String.IsNullOrEmpty(connectionString))
                    throw new Exception("The teenand connection string can not be null");
                return new EnterpriseLibraryData("CustomerDatabase", connectionString);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dbCommand != null)
                    dbCommand.Dispose();
            }           
        }
    }
}
