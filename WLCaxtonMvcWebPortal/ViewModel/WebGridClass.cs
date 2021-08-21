using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WLCaxtonMvcWebPortal.ViewModel
{
    public class WebGridClass
    {
        public List<dynamic> GridData = new List<dynamic>();
        public List<WebGridColumn> ColNames = new List<WebGridColumn>();
        public string Title { get; set; }
        public string KeyField { get; set; }

        public static WebGridClass HoldWebGridDetails = new WebGridClass();

        public static void GetDetailsForGrid<T>(List<T> list, string Title, string KeyField)
        {
            WebGridClass objWeb = new WebGridClass();

            var properties = typeof(T).GetProperties();
            List<string> ColNameList = new List<string>();
            ColNameList.AddRange(properties.Select(x => x.Name));

            //objWeb.ColNames.Add(new WebGridColumn() { Header = "Action", ColumnName = KeyField, Format = item => new MvcHtmlString("<a href='Edit?id=" + item[KeyField] + "'>Edit</a>") });
            objWeb.ColNames.AddRange(properties.Select(s => new WebGridColumn() { Header = s.Name, ColumnName = s.Name, CanSort = true }).ToList());
            objWeb.GridData = list.Cast<dynamic>().ToList();
            objWeb.Title = Title;
            HoldWebGridDetails = objWeb;
        }
    }
}