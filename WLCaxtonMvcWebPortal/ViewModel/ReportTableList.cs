using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WLCaxtonMvcWebPortal.ViewModel
{
    public class ReportTableList
    {
        public string ReportName { get; set; }
        public DataTable ReportTable { get; set; }
        public string Message { get; set; }
    }
}