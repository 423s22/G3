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
        
        public int TotalErrorCount { get; set; }
        public int TotalWarningCount { get; set; }
        
        public ValidatorModel(WordprocessingDocument docToValidate, string fileName)
        {
            DocToValidate = docToValidate;
            FileName = fileName;
        }


        public string Validate()
        {
            //TODO: this process should probably be done iteratively
            
            // validate individual components and return their validation as a JObject
            FontValidator fontValidator = new FontValidator();
            var fontValidation = fontValidator.Validate(DocToValidate);
            TotalErrorCount += fontValidator.Errors.Count;
            TotalWarningCount += fontValidator.Warnings.Count;
            
            MarginValidator marginValidator = new MarginValidator();
            var marginValidation = marginValidator.Validate(DocToValidate);
            TotalErrorCount += marginValidator.Errors.Count;
            TotalWarningCount += marginValidator.Warnings.Count;
            
            PageNumberValidator pageNumberValidator = new PageNumberValidator();
            var pageNumberValidation = pageNumberValidator.Validate(DocToValidate);
            TotalErrorCount += pageNumberValidator.Errors.Count;
            TotalWarningCount += pageNumberValidator.Warnings.Count;

            FigureValidator figureValidator = new FigureValidator();
            var figureValidation = figureValidator.Validate(DocToValidate);
            TotalErrorCount += figureValidator.Errors.Count;
            TotalWarningCount += figureValidator.Warnings.Count;

            ColorValidator colorValidator = new ColorValidator();
            var colorValidation = colorValidator.Validate(DocToValidate);
            TotalErrorCount += colorValidator.Errors.Count;
            TotalWarningCount += colorValidator.Warnings.Count;
            
            
            // this is the object returned to the front end
            // for now it has the name of the passed document and the three validated components
            var returnValueObj = new
            {
                document = new
                {
                    document_name = FileName,
                    fontValidation,
                    marginValidation,
                    pageNumberValidation,
                    figureValidation,
                    colorValidation
                }
            };

            return JsonConvert.SerializeObject(returnValueObj);
        }
        
    }
}