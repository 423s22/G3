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
        // warnings and errors will be added to a list and serialized into JSON
        public List<ComponentWarning> Warnings;
        public List<ComponentError> Errors;
        protected WordprocessingDocument DocToValidate;

        // generic method to format validations into a JObject 
        public JObject Validate(WordprocessingDocument docToValidate)
        {
            DocToValidate = docToValidate;
            
            // parse contents into error and warning lists
            
            ParseContents();
            JObject componentValidation = new JObject();
            
            try
            {
                // top level JObject will have name of the specific ComponentValidation subclass
                // the warnings and errors are then added as arrays of objects (ComponentWarning and ComponentError objects specifically)
                componentValidation = new JObject(
                        new JObject(
                            new JProperty("warnings",
                                new JArray(
                                    from warning in Warnings
                                    select new JObject(
                                        new JProperty("warning_name", warning.WarningName),
                                        new JProperty("warning_description", warning.WarningDescription)
                                    )
                                )
                            ),
                            new JProperty("errors",
                                new JArray(
                                    from error in Errors
                                    select new JObject(
                                        new JProperty("error_name", error.ErrorName),
                                        new JProperty("error_description", error.ErrorDescription)
                                    )
                                )
                            )
                        )
                );
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            
            return componentValidation;
        }

        /// <summary>
        ///  Method that will be overriden in all ComponentValidator subclasses.
        ///  It will search through a WordprocessingDocument object and find all
        ///     warnings and errors that are apropraite for the specific component(i.e. margins, spacing, color, etc.)
        /// </summary>
        protected abstract void ParseContents();
    }
}