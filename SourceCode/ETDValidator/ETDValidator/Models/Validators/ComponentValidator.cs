using System.Collections.Generic;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace ETDVAlidator.Models.Validators
{
    public abstract class ComponentValidator
    {
        protected List<ComponentWarning> Warnings;
        protected List<ComponentError> Errors;
        protected WordprocessingDocument DocToValidate;

        public JObject Validate(WordprocessingDocument docToValidate)
        {
            DocToValidate = docToValidate;
            
            ParseContents();
            JObject componentValidation = new JObject();
            try
            {
                componentValidation = new JObject(
                        new JObject(
                            new JProperty("warnings",
                                new JArray(
                                    from warning in Warnings
                                    select new JObject(
                                        new JProperty(warning.WarningName, warning.WarningDescription)
                                    )
                                )
                            ),
                            new JProperty("errors",
                                new JArray(
                                    from error in Errors
                                    select new JObject(
                                        new JProperty(error.ErrorName, error.ErrorDescription)
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

        protected abstract void ParseContents();
    }
}