using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WLCaxtonMvcWebPortal.Controllers;
using WLCaxtonMvcWebPortal.Models;

namespace WLCaxtonMvcWebPortal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string pdfTronKey = System.Configuration.ConfigurationManager.AppSettings["PDFNetLicenceKey"].ToString();
           // pdftron.PDFNetLoader loader = pdftron.PDFNetLoader.Instance();

            if (!string.IsNullOrEmpty(pdfTronKey))
               pdftron.PDFNet.Initialize(pdfTronKey);
            else
                pdftron.PDFNet.Initialize();
            AreaRegistration.RegisterAllAreas();
           // RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Add global exception filter 
            GlobalFilters.Filters.Add(new ExceptionLogWriteFilter());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }



        protected void Application_Error(Object sender, EventArgs e)
        {
            try
            {
                var exception = Server.GetLastError();

                /******************************************************************************/

                // Exception excep = Server.GetLastError();
                Server.ClearError();
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Error");
                routeData.Values.Add("action", "Error");
                routeData.Values.Add("exception", exception);
                if (exception.GetType() == typeof(HttpException))
                {
                    routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
                }
                else
                {
                    routeData.Values.Add("statusCode", 500);
                }
                IController controller = new ErrorController();
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Response.End();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
