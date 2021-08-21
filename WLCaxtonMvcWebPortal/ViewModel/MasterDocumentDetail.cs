using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WLCaxtonMvcWebPortal.ViewModel
{
    public class MasterDocumentDetail
    {
        public string AccountNumber { get; set; }

        public string DocumentGUID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentDate { get; set; }
        public string Postcode { get; set; }

        public string SpoolName { get; set; }
        public string StreamName { get; set; }

        public int PageCount { get; set; }
        public int PageCountHidden { get; set; }
        public int StartPageNo { get; set; }
        
        public int SubDocumentStartPage { get; set; }

        public string DyanamicColumn { get; set; }
        public string DyanamicRows { get; set; }
        public string pageURL { get; set; }

    }
}