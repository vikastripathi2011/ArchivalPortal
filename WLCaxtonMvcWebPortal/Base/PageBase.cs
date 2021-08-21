//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : DataLayerBase.cs
// Program Description  : Reusable fucntionalty for DataLayer logic
// Programmed By        : Naushad Ali
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

#region Namespaces

using System.Web.UI.WebControls;
using System.Web;

#endregion

namespace WLCaxtonMvcWebPortal.Base
{
    #region PageBase UI Base Class
    
    public class PageBase : System.Web.UI.Page
    {
        #region PublicMethods
        
        /// <summary>
        /// This method kill the user current session
        /// </summary>
        [System.Web.Services.WebMethod]
        public static void BrowserCloseMethod()
        {
            HttpContext.Current.Session.Abandon();
        }

        #endregion
    }

    #endregion
}