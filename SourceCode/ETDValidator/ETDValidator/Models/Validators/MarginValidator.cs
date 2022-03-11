using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ETDVAlidator.Models.Validators
{
    public class MarginValidator : ComponentValidator
    {
        public MarginValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
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
                foreach (Paragraph paragraph in paragraphs)
                {
                    if (paragraph.FirstChild is not null)
                    {
                        // To find out if the paragraph has styling Can also use OuterXml
                        string paragraphRawXml = paragraph.FirstChild.InnerXml;
                        
                        if (paragraphRawXml.Contains("pgMar"))
                        {
                            // one inch would be roughly 1440 plus or minus 150
                            // It may be a problem with the bottom margin
                            var rawXmlSplit = paragraphRawXml.Split(">");
                            
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
                                var marginSides = xmlRaw[1..5];
                                foreach (string side in marginSides)
                                {
                                    int marginNum = parseMarginToInt(side);
                                    // Check if its way off 
                                    if (marginNum is <= 1260 or >= 1620)
                                    {
                                        Errors.Add(new ComponentError("margin_error_1", "Margin is not within bounds of suggested 1 inch margins"));
                                    }
                                    else if (marginNum != 1440)
                                    {
                                        Warnings.Add(new ComponentWarning("margin_warning_1", "Margins may be off. Inspect to confirm that they are set to 1 inch"));
                                    }
                                }
                            }
                        }
                    }
                }
        }
        
        
        private static Int32 parseMarginToInt(string marginSide)
        {
            string marginSideNum = new String(marginSide.Where(Char.IsDigit).ToArray());
            int marginIntVal = Int32.Parse(marginSideNum);
            return marginIntVal;
        }
        
    }
}