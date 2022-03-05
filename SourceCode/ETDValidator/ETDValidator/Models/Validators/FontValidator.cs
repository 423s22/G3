using System.Collections.Generic;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using DocumentFormat.OpenXml.Math;

namespace ETDVAlidator.Models.Validators
{
    public class FontValidator : ComponentValidator
    {
        
        public FontValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
        }
        
        protected override void ParseContents()
        {
            // do parsing here, add errors and warnings
            // DocToValidate is WordprocessignDocument object to parse through 

            
            // these can be deleted - just examples
            Warnings.Add(new ComponentWarning("font_warning_1", "warning 1 description"));
            Warnings.Add(new ComponentWarning("font_warning_2", "warning 2 description"));
            Warnings.Add(new ComponentWarning("font_warning_3", "warning 3 description"));


            Errors.Add(new ComponentError("font_error_1", "error 1 description"));
            Errors.Add(new ComponentError("font_error_2", "error 2 description"));
            Errors.Add(new ComponentError("font_error_3", "error 3 description"));
        }
    }
}