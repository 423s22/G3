using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json;

namespace ETDVAlidator.Models
{
    public class ValidatorModel
    {
        private WordprocessingDocument DocToValidate { get; set; }
        private string FileName { get; set; }


        public ValidatorModel(WordprocessingDocument docToValidate, string fileName)
        {
            DocToValidate = docToValidate;
            FileName = fileName;
        }


        public string Validate()
        {
            // validate individual components and return their validation as a JObject
            var fontValidation = new Validators.FontValidator().Validate(DocToValidate);
            var spaceValidation = new Validators.SpacingValidator().Validate(DocToValidate);
            var marginValidation = new Validators.MarginValidator().Validate(DocToValidate);
            var pageNumberValidation = new Validators.PageNumberValidator().Validate(DocToValidate);

            // this is the object returned to the front end
            // for now it has the name of the passed document and the three validated components
            var returnValueObj = new
            {
                document = new
                {
                    document_name = FileName,
                    fontValidation,
                    spaceValidation,
                    marginValidation,
                    pageNumberValidation
                }
            };

            return JsonConvert.SerializeObject(returnValueObj);
        }
        
    }
}