using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using SectionProperties = DocumentFormat.OpenXml.Wordprocessing.SectionProperties;

namespace ETDVAlidator.Models.Validators
{
    public class MarginValidator : ComponentValidator
    {
        public MarginValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();

            Name = "margins";
        }
        
        protected override void ParseContents()
        {
            // do parsing here, add errors and warnings
            // DocToValidate is WordprocessignDocument object to parse through 
            // Assign a reference to the existing document body.
            Body body = DocToValidate.MainDocumentPart.Document.Body;
                
            // Grab the results
            // The Raw view will hold all of the text in the body
            // Results will have each paragraph within it (A paragraph is any time a enter: so may have to be weary about line breaks in document)
            // Results are held in arrays of 100 elements

            IEnumerable<Paragraph> paragraphs = body.Elements<Paragraph>();
            IEnumerable<SectionProperties> sectionProp = body.Elements<SectionProperties>();
                

            foreach (SectionProperties property in sectionProp)
            {
                // Get the margin innerXml that includes margins and side of header 
                string sectionInnerXml = property.InnerXml;
                    
                // Parse this innerXml into an array that holds the pertinent information
                var headerMarginXml = parseInnerXml(sectionInnerXml) as string[];

                // Pass that to method for checking margins are set to 1" or 1440 twentieths of a point
                // Change size of this using twentieths of a point
                validateMarginSize(headerMarginXml[1..5], 1440);
                
                // Pass that to method for checking header is set to 1" or 1440 twentieths of a point
                validateHeaderSize(headerMarginXml[5], 1440);

            }
                
            // <w:pgSz w:w="12240" w:h="15840" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" /><w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1440" w:header="1440" w:footer="720" w:gutter="0" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" /><w:pgNumType w:start="1" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" /><w:cols w:space="720" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" /><w:titlePg xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" /><w:docGrid w:linePitch="360" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" />
                
            
        }
        private string[] parseInnerXml(string sectionInnerXml)
        {
            if (sectionInnerXml.Contains("pgMar"))
            {
                // one inch would be roughly 1440 plus or minus 150
                // It may be a problem with the bottom margin
                var rawXmlSplit = sectionInnerXml.Split(">");

                // Find the specific pgMar from split
                string pageMargin = null;
                foreach (string styleElement in rawXmlSplit)
                {
                    if (styleElement.Contains("pgMar"))
                    {
                        pageMargin = styleElement;
                    }
                }

                if (pageMargin is not null)
                {
                    string[] xmlRaw = pageMargin.Split(" ");

                    // Get integer value and then check if its within a range of 360 points or about 1/4"
                    // From the 1" or 1440 points
                    string[] validationData = xmlRaw[0..6];


                    return validationData;
                }
            }

            return null;
        }    

        private void validateHeaderSize(string headerData, int desiredSize)
        {

            int headerSize = parseXmlToInt(headerData);
                
            // Check if its way off 
            if (headerSize is <= 1260 or >= 1620)
            {
                Errors.Add(new ComponentError("header_size_error_1",
                    "Margin is not within bounds of suggested 1 inch header"));
            }
            else if (headerSize != 1440)
            {
                Warnings.Add(new ComponentWarning("header_size_warning_1",
                    "Header Size may be off. Inspect to confirm that they are set to 1 inch"));
            }
            
        }

        private void validateMarginSize(string[] marginSides, int i)
        {
            foreach (string side in marginSides)
            {
                int marginNum = parseXmlToInt(side);
                // Check if its way off 
                if (marginNum is <= 1260 or >= 1620)
                {
                    Errors.Add(new ComponentError("margin_error_1",
                        "Margin is not within bounds of suggested 1 inch margins"));
                }
                else if (marginNum != 1440)
                {
                    Warnings.Add(new ComponentWarning("margin_warning_1",
                        "Margins may be off. Inspect to confirm that they are set to 1 inch"));
                }
            }
        }
        


        private static Int32 parseXmlToInt(string marginSide)
        {
            string marginSideNum = new String(marginSide.Where(Char.IsDigit).ToArray());
            int intVal = Int32.Parse(marginSideNum);
            return intVal;
        }
        
    }
}