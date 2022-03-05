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
            var fontValidation = new Validators.FontValidator().Validate(DocToValidate);
            var spaceValidation = new Validators.SpacingValidator().Validate(DocToValidate);
            var marginValidation = new Validators.MarginValidator().Validate(DocToValidate);

            var returnValueObj = new
            {
                document = new
                {
                    document_name = FileName,
                    fontValidation,
                    spaceValidation,
                    marginValidation
                }
            };

            return JsonConvert.SerializeObject(returnValueObj, Formatting.Indented);
        }
        
    }
}