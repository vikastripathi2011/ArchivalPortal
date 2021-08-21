//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : SessionClass.cs
// Program Description  : This class is used to maintain the session states on different pages.
// Programmed By        : Satyendra Gupta
// Programmed On        : 31-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System.Collections.Generic;
using System.Web;
using WLCaxtonPortalBusinessEntity;
using System.Data;
namespace WLCaxtonMvcWebPortal.Util
{
    /// <summary>
    /// Summary description for Session Class
    /// </summary>
    public class SessionClass
    {
        /// <summary>
        /// stores Loged User Details
        /// </summary>
        public static User loggedUser
        {
            get
            {
                if (HttpContext.Current.Session[SessionVariables.loggedUser] == null)
                    HttpContext.Current.Session[SessionVariables.loggedUser] = new User();

                return (User)HttpContext.Current.Session[SessionVariables.loggedUser];
            }
            set
            {
                HttpContext.Current.Session[SessionVariables.loggedUser] = value;
            }
        }

        /// <summary>
        /// Clear all session on logout.
        /// </summary>
        public static void SessionClear()
        {
            SessionClass.loggedUser = null;
        }

        /// <summary>
        /// stores Chagned PWD 
        /// </summary>
        public static bool IsChangedPwd
        {
            get
            {
                if (HttpContext.Current.Session[SessionVariables.IsChangedPwd] == null)
                    HttpContext.Current.Session[SessionVariables.IsChangedPwd] = new bool();

                return (bool)HttpContext.Current.Session[SessionVariables.IsChangedPwd];
            }
            set
            {
                HttpContext.Current.Session[SessionVariables.IsChangedPwd] = value;
            }
        }

        /// <summary>
        /// stores Search Query of the user
        /// </summary>
        public static string SearchQuery
        {

            get
            {
                string sessionVariableObj = string.Empty;
                if (HttpContext.Current.Session[SessionVariables.SearchQuery] != null)
                    return sessionVariableObj = HttpContext.Current.Session[SessionVariables.SearchQuery].ToString();
                else
                    return sessionVariableObj;
            }
            set
            {
                HttpContext.Current.Session[SessionVariables.SearchQuery] = value;
            }
        }

        /// <summary>
        /// stores Search Query of the user
        /// </summary>
        public static string SearchCountQuery
        {
            get
            {
                string sessionVariableObj = string.Empty;
                if (HttpContext.Current.Session[SessionVariables.SearchCountQuery] != null)
                    return sessionVariableObj= HttpContext.Current.Session[SessionVariables.SearchCountQuery].ToString();
                else
                    return sessionVariableObj;
            }
            set
            {
                HttpContext.Current.Session[SessionVariables.SearchCountQuery] = value;
            }
        }

        /// <summary>
        /// stores Current Search Result of the Search
        /// </summary>
        public static DataSet PrintCurrentResult
        {
            get
            {
                return (DataSet)HttpContext.Current.Session[SessionVariables.PrintCurrentResult];
            }
            set
            {
                HttpContext.Current.Session[SessionVariables.PrintCurrentResult] = value;
            }
        }

        public static Dictionary<string, string> SortCriteriaCollection
        {
            get;
            set;
        }
    }
}

