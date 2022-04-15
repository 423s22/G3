using DocumentFormat.OpenXml.Packaging;
using ETDVAlidator.Models.Validators;
using Newtonsoft.Json;

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
            

            // this is the object returned to the front end
            // for now it has the name of the passed document and the three validated components
            var returnValueObj = new
            {
                document = new
                {
                    document_name = FileName,
                    fontValidation,
                    marginValidation,
                    pageNumberValidation
                }
            };

            return JsonConvert.SerializeObject(returnValueObj);
        }
        
    }
}