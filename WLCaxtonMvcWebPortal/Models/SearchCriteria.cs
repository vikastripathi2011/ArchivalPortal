using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace WLCaxtonMvcWebPortal.Models
{
    public class SearchCriteria
    {
        [Display(Name = "Policy No: ")]
        public string policyNo { get; set; }

        [Required]
        [Display(Name = "Product Category: ")]
        public string productCategory { get; set; }

        [Display(Name = "Document Type: ")]
        public string docType { get; set; }

        [Display(Name = "Zip Code: ")]
        public string Postcode { get; set; }

        [Display(Name = "Spool Name: ")]
        public string spoolName { get; set; }

        [Display(Name = "Date: ")]
        public DocumentDate docDate { get; set; }

        [Display(Name = "Page Count: ")]
        public PageCount pageCount { get; set; }

        [Display(Name = "Sort Expression1: ")]
        public SortExpression sortExp1 { get; set; }

        [Display(Name = "Sort Expression2: ")]
        public SortExpression sortExp2 { get; set; }

        [Display(Name = "Sort Expression3: ")]
        public SortExpression sortExp3 { get; set; }

    }
   
    
    
}