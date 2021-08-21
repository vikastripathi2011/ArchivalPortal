//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : TenantMasterDL.cs
// Program Description  : Use to get the teenant specific connection string                         
// Programmed By        : Nadeem Ishrat
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalDataHelper;
using System.Data;
using WLCaxtonPortalExceptionLogger;
using System.Data.Common;
#endregion

namespace WLCaxtonPortalDataLayer.Classes
{
    public class TenantMasterDL:DataLayerBase
    {
        #region Private variables
        private static IDatabase databaseInstance;
        private static IDbCommand dbCommand = null;
        private static IDataReader reader = null; 
        #endregion

        #region Public methods

        /// <summary>
        /// Use to get the client specific connection string
        /// </summary>
        /// <param name="tenantId">client id for getting specific connection string</param>
        /// <returns>teenant string</returns>
        
        public static string GetTenantConnectionString(int tenantId)
        {
            try
            {
                databaseInstance = DataManager.GetDatabaseInstance();
                if (databaseInstance != null)
                {
                    using (DbCommand Command = databaseInstance.CreateCommand("Proc_GetTenantConnectionString"))
                    {
                        databaseInstance.AddInParameter(Command, "@tenantId", DbType.String, tenantId);
                        reader = databaseInstance.ExecuteReader(Command);
                        if (reader.Read())
                        {
                            return GetValue<string>(reader, "DatabaseConnectionString");
                        }
                    }
                }

                //dbCommand = databaseInstance.CreateSqlQuery(string.Format("{0}{1}", "Select DatabaseConnectionString from Tenant_Master WHERE PK_Tenant_Master_TenantId = ", tenantId));
                //reader = dbCommand.ExecuteReader();
                //if (reader.Read())
                //{
                //    return GetValue<string>(reader, "DatabaseConnectionString");
                //}
            }
            catch (Exception ex)
            {
                LoggerFactory.Logger.Log("TenantMasterDL", LogEventType.Error, ex.Message);
                throw;
            }
            finally
            {
                Clean(dbCommand, dbCommand.Connection, reader);
            }
            return String.Empty;
        }

        //public static string GetTenantConnectionString(int tenantId)
        //{
        //    try
        //    {
        //        databaseInstance = DataManager.GetDatabaseInstance();
        //        dbCommand = databaseInstance.CreateSqlQuery(string.Format("{0}{1}", "Select DatabaseConnectionString from Tenant_Master WHERE PK_Tenant_Master_TenantId = ", tenantId));
        //        reader = dbCommand.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            return GetValue<string>(reader, "DatabaseConnectionString");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerFactory.Logger.Log("TenantMasterDL", LogEventType.Error, ex.Message);
        //        throw;
        //    }
        //    finally
        //    {
        //        Clean(dbCommand, dbCommand.Connection, reader);
        //    }
        //    return String.Empty;
        //}
        #endregion
    }
}
