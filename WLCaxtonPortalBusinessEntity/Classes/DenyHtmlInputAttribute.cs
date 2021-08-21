using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace WLCaxtonPortalBusinessEntity
{
    public class DenyHtmlInputAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var tagWithoutClosingRegex = new Regex(@"<[^>]+>");

            var hasTags = tagWithoutClosingRegex.IsMatch(Convert.ToString( value));

            if (!hasTags) return ValidationResult.Success;
            return new ValidationResult
            (String.Format("{0} cannot contain html tags", validationContext.DisplayName));
        }
    }
}
