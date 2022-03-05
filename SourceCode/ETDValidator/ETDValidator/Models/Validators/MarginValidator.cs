using System.Collections.Generic;

namespace ETDVAlidator.Models.Validators
{
    public class MarginValidator : ComponentValidator
    {
        public MarginValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
        }


        protected override void ParseContents()
        {
            // do parsing here, add errors and warnings
            // DocToValidate is WordprocessignDocument object to parse through 
            
            // these can be deleted - just examples
            Warnings.Add(new ComponentWarning("margin_warning_1", "warning 1 description"));
            Warnings.Add(new ComponentWarning("margin_warning_2", "warning 2 description"));
            Warnings.Add(new ComponentWarning("margin_warning_3", "warning 3 description"));


            Errors.Add(new ComponentError("margin_error_1", "error 1 description"));
            Errors.Add(new ComponentError("margin_error_2", "error 2 description"));
            Errors.Add(new ComponentError("margin_error_3", "error 3 description"));
        }
    }
}