using System.Collections.Generic;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace ETDVAlidator.Models.Validators
{
    // this will be the abstract class inherited by every component validator
    public abstract class ComponentValidator
    {
        // warnings and errors will be added to a list
        public List<ComponentWarning> Warnings;
        public List<ComponentError> Errors;
        protected WordprocessingDocument DocToValidate;

        public string Name;

        // generic method to validate using a componentvalidator 
        public void Validate(WordprocessingDocument docToValidate)
        {
            // define file to validate
            DocToValidate = docToValidate;
            
            // parse contents into error and warning list
            ParseContents();
        }

        /// <summary>
        ///  Method that will be overriden in all ComponentValidator subclasses.
        ///  It will search through a WordprocessingDocument object and find all
        ///     warnings and errors that are apropraite for the specific component(i.e. margins, spacing, color, etc.)
        /// </summary>
        protected abstract void ParseContents();
    }
}