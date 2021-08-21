//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : ConnectionHandler.cs
// Program Description  : This class is part of Connection Handler. It handles the connection of user.
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
    /// This class is use to handle the connections.
    /// </summary>
    public abstract class ConnectionHandler
    {
        /// <summary>
        /// To get or set the next connection.
        /// </summary>
        public ConnectionHandler NextConnection { get; private set; }

        /// <summary>
        /// This class set the next connection to the user.
        /// </summary>
        /// <param name="nextConnection"></param>
        public void SetNextConnection(ConnectionHandler nextConnection)
        {
            this.NextConnection = nextConnection;
        }

        /// <summary>
        /// This class is use the get the connection instance on the basis of tenant / customer.
        /// </summary>
        /// <param name="teenantId"></param>
        /// <returns></returns>
        public abstract IDatabase GetDatabaseInstance(int teenantId);
    }
}
