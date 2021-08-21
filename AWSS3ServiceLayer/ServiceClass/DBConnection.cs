using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWSS3ServiceLayer.ServiceClass
{
    public abstract class DBConnection
    {
        public Database GetDBConnection(string name = null)
        {
            Database dbcon;
            try
            {
                dbcon = DatabaseFactory.CreateDatabase();
            }
            catch
            {
                dbcon = null;
            }
            return dbcon;
        }

    }
}