//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DataLayerBase.cs
// Program Description  : Reusable fucntionalty for DataLayer logic
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLCaxtonPortalBusinessEntity;
using WLCaxtonPortalDataHelper;
using System.Data;
using System.Data.Common; 
#endregion

namespace WLCaxtonPortalDataLayer
{
    public class DataLayerBase
    {
        #region Private variables
        public AuditTrail AuditTrailDetails { get; set; }
        public IDatabase DatabaseInstance { get; set; } 
        #endregion

        #region Protected members

        /// <summary>
        /// Return the values from reader class based on column name. It also returns defult value if null condition occurs
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="iDataReader">reader instance</param>
        /// <param name="coulmnName">column name mentioned in proc</param>
        /// <returns>value return by reader.</returns>
        protected static T GetValue<T>(IDataReader iDataReader, string coulmnName)
        {
            int ordinal = iDataReader.GetOrdinal(coulmnName);

            if (iDataReader.IsDBNull(ordinal) == false)
                return (T)iDataReader.GetValue(ordinal);
            return default(T);
        }

        /// <summary>
        /// Carbage collection for unmanaged resources
        /// </summary>
        /// <param name="dbCommand">database command</param>
        /// <param name="dbConnection">database connection</param>
        /// <param name="reader">reader object</param>
        protected static void Clean(IDbCommand dbCommand, IDbConnection dbConnection, IDataReader reader = null)
        {
            if (reader != null)
                reader.Close();
            if (dbCommand.Connection != null)
                dbCommand.Connection.Close();
            if (dbCommand != null)
                dbCommand.Dispose();
        }

        /// <summary>
        /// Default patameters set to common method
        /// </summary>
        /// <param name="database">database instance</param>
        /// <param name="dbCommand">command object</param>
        /// <param name="auditTrail">audit trail information</param>
        protected static void SetCommonParameters(IDatabase database, DbCommand dbCommand, AuditTrail auditTrail)
        {
            database.AddOutParameter(dbCommand, "@p_IsSuccess", DbType.Boolean, 1);
            database.AddOutParameter(dbCommand, "@p_MessageID", DbType.String, 10);
            database.AddOutParameter(dbCommand, "@p_MessageText", DbType.String, 200);
            database.AddInParameter(dbCommand, "@p_ModifiedBy", DbType.Int32, auditTrail.ModifiedBy);
            database.AddInParameter(dbCommand, "@p_ModifiedOn", DbType.DateTime, auditTrail.ModifiedOn);
        } 
        #endregion
    }
}
