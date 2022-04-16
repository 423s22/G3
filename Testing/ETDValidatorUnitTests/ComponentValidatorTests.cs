using System;
using Xunit;
using DocumentFormat.OpenXml.Packaging;
using ETDVAlidator.Models;
using ETDVAlidator.Models.Validators;
using Newtonsoft.Json.Linq;

namespace ETDValidatorUnitTests
{
    public class ComponentValidatorTests
    {

        [Fact]
        public void ValidDocumentTest()
        {
            WordprocessingDocument wordDocument = WordprocessingDocument.Open("TestDocuments/Completed_Example_ETD.docx", false);
            ValidatorModel validator = new ValidatorModel(wordDocument, "test_file");

            validator.Validate();
            
            Assert.Empty(validator.ValidationResults.AllErrors);
            Assert.Empty(validator.ValidationResults.AllWarnings);
        }
        
        [Fact]
        public void InvalidMarginTest()
        {
            const int EXPECTED_ERRORS = 3;
            const int EXPECTED_WARNINGS = 0;
            
            WordprocessingDocument wordDocument = WordprocessingDocument.Open("TestDocuments/InvalidMargins.docx", false);
            MarginValidator validator = new MarginValidator();
            validator.Validate(wordDocument);

            // document has 3 invalid margins, 1 valid - should be 3 errors
            Assert.True(EXPECTED_ERRORS == validator.Errors.Count);
            Assert.True(EXPECTED_WARNINGS == validator.Warnings.Count);
        }

        [Fact]
        public void InvalidFontFamilyTest()
        {
            const int EXPECTED_ERRORS = 0;
            const int EXPECTED_WARNINGS = 2;
            
            WordprocessingDocument wordDocument = WordprocessingDocument.Open("TestDocuments/InvalidFontFamily.docx", false);
            FontValidator validator = new FontValidator();
            validator.Validate(wordDocument);
            
            Assert.True(EXPECTED_ERRORS == validator.Errors.Count);
            Assert.True(EXPECTED_WARNINGS == validator.Warnings.Count);
        }

        [Fact]
        public void InvalidFontSizeTest()
        {
            const int EXPECTED_ERRORS = 2;
            const int EXPECTED_WARNINGS = 0;
            
            WordprocessingDocument wordDocument = WordprocessingDocument.Open("TestDocuments/InvalidFontSize.docx", false);
            FontValidator validator = new FontValidator();
            validator.Validate(wordDocument);
            
            Assert.True(EXPECTED_ERRORS == validator.Errors.Count);
            Assert.True(EXPECTED_WARNINGS == validator.Warnings.Count);
        }

        [Fact]
        public void NoPageNumbersTest()
        {
            const int EXPECTED_ERRORS = 2;
            const int EXPECTED_WARNINGS = 0;
            
            WordprocessingDocument wordDocument = WordprocessingDocument.Open("TestDocuments/NoPageNumbers.docx", false);
            PageNumberValidator validator = new PageNumberValidator();
            validator.Validate(wordDocument);
            
            Assert.True(EXPECTED_ERRORS == validator.Errors.Count);
            Assert.True(EXPECTED_WARNINGS == validator.Warnings.Count);
        }
        
        
    }
}