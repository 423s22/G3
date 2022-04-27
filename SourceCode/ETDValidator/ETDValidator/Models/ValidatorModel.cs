using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using ETDVAlidator.Models.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ETDVAlidator.Models
{
    public class ValidatorModel
    {
        private WordprocessingDocument DocToValidate { get; set; }
        private string FileName { get; set; }

        public DocumentResultsViewModel ValidationResults;
        
        
        public ValidatorModel(WordprocessingDocument docToValidate, string fileName)
        {
            DocToValidate = docToValidate;
            FileName = fileName;
            ValidationResults = new DocumentResultsViewModel(fileName);
        }


        public DocumentResultsViewModel Validate()
        {
            List<ComponentValidator> validators = new List<ComponentValidator>();
            validators.Add(new ColorValidator());
            validators.Add(new FontValidator());
            validators.Add(new MarginValidator());
            validators.Add(new PageNumberValidator());
            validators.Add(new FigureValidator());


            foreach (ComponentValidator validator in validators)
            {
                try
                {
                    validator.Validate(DocToValidate);
                    ValidationResults.AllErrors.AddRange(validator.Errors);
                    ValidationResults.AllWarnings.AddRange(validator.Warnings);
                }
                catch (Exception e)
                {
                    string validationErrorMessage =
                        "We had an issue validating your " + validator.Name + ". Please check manually.";
                    ValidationResults.AllErrors.Add(new ComponentError("Validation Issue", validationErrorMessage));
                    Console.WriteLine(e.StackTrace);
                }
            }
            
            ValidationResults.FilterDuplicatesByDescription();
            
            return ValidationResults;
        }
        
    }
}