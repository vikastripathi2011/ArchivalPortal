//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Util.cs
// Program Description  : This class is used to store the sorting type of search data.
// Programmed By        : Satyendra Gupta
// Programmed On        : 31-December-2012 
// Version              : 1.0.0
//==========================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Linq.Expressions;

namespace WLCaxtonMvcWebPortal.Util
{
    public static class Util
    {
        /// <summary>
        /// This class is use to provide list of sorting expression, sorting direction in a list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="sortingExpression"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        internal static List<T> SortList<T>(IList<T> collection, string sortingExpression, SortDirection sortDirection)
        {
            List<T> srotedCollection = new List<T>();

            if (collection != null && collection.Count > 0)
            {
                var param = Expression.Parameter(typeof(T), sortingExpression);
                var sortExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(param, sortingExpression), typeof(object)), param);

                if (sortDirection == SortDirection.Ascending)
                {
                    srotedCollection = collection.AsQueryable<T>().OrderBy(sortExpression).ToList<T>();
                }
                else
                {
                    srotedCollection = collection.AsQueryable<T>().OrderByDescending(sortExpression).ToList<T>();
                };
            }

            return srotedCollection;
        }

        /// <summary>
        /// This class is use to handle the URL.
        /// </summary>
        public static string BaseUrl
        {
            get
            {                
                string url = string.Format(@"{0}://{1}/{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, 
                    HttpContext.Current.Request.ApplicationPath);                    

                return url;
            }
        }
    }
}