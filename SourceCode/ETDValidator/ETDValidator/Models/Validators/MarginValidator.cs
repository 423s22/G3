using System.Collections.Generic;
using System.Collections.Specialized;
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

                MainDocumentPart mainDoc = DocToValidate.MainDocumentPart.Document.MainDocumentPart;
                IEnumerator<HeaderPart> enumerator = mainDoc.HeaderParts.GetEnumerator();

                IEnumerable<PageMargin> margins2 = mainDoc.Document.Elements<PageMargin>();

                Settings settings = mainDoc.DocumentSettingsPart.Settings;
                // Grab the results
                // The Raw view will hold all of the text in the body
                // Results will have each paragraph within it (A paragraph is any time a enter: so may have to be weary about line breaks in document)
                // Results are held in arrays of 100 elements

                IEnumerable<PageMargin> margins = body.Elements<PageMargin>();
            
                foreach (PageMargin margin in margins)
                {
                    var marginTop = margin.Top;
                    var marginBottom = margin.Bottom;
                    var marginLeft = margin.Left;
                    var marginRight = margin.Right;
                }

                IEnumerable<Section> sections = body.Elements<Section>();

                IEnumerable<Paragraph> paragraphs = body.Elements<Paragraph>();
                foreach (Paragraph paragraph in paragraphs)
                {
                    ParagraphProperties paragraphProperties = paragraph.ParagraphProperties;

                    if (paragraph.FirstChild != null)
                    {
                        IList<OpenXmlAttribute> openXmlAttributes = paragraph.GetAttributes();
                        
                        // Grab the text associated with it
                        string paragraphInnerText = paragraph.InnerText;
                        // Can use the HasChildren to see if there is another paragraph
                        // Can use NoSpellError to check for spelling errors

                        // To find out if the paragraph has styling Can also use OuterXml
                        string paragraphRawXml = paragraph.FirstChild.InnerXml;
                        
                        // Uncomment to show where you can find Fonts
                        //if (paragraph.InnerText.Contains("First Level Heading"))
                        //{
                            // Splitting the entire xml string into the seperate styling strings
                            string[] innerXmlWithFont = paragraph.InnerXml.Split(">");
                            
                            
                        //}
                        
                    }
                }
            
            // these can be deleted - just examples
            Warnings.Add(new ComponentWarning("margin_warning_1", "warning 1 description"));
            Warnings.Add(new ComponentWarning("margin_warning_2", "warning 2 description"));
            Warnings.Add(new ComponentWarning("margin_warning_3", "warning 3 description"));


            Errors.Add(new ComponentError("margin_error_1", "error 1 description"));
            Errors.Add(new ComponentError("margin_error_2", "error 2 description"));
            Errors.Add(new ComponentError("margin_error_3", "error 3 description"));
        }
    }
}