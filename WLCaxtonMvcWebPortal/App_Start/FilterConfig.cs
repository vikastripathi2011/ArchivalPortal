using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WLCaxtonMvcWebPortal.Models;

namespace WLCaxtonMvcWebPortal
{
    public class FilterConfig
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Remove this filter because we want to handle errors ourselves via the ErrorPage controller
            filters.Add(new HandleErrorAttribute());
           // filters.Add(new CheckCurrentLoggedUserSession());
        }
    }
}