using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WLCaxtonMvcWebPortal.ViewModel
{
    public class DocumentViewModel
    {

        public MasterDocumentDetail masterdocumentDetails { get; set; }
        public List<SubDocumentDetails> subdocumentDetails { get; set; }
        public List<MasterDocumentDetail> MasterdocList { get; set; }
        public List<MasterDocumentDetail> MasterdocColumnList { get; set; }
        public List<BucketDetails> BucketDetailsList { get; set; }
        public BucketDetails BucketDetailsModel { get; set; }

        public WebGridClass WebGridClassModel { get; set; }
        
    }
       
    

    
         
    

   

    
}