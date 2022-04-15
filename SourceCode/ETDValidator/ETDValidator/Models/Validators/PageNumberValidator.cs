using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ETDVAlidator.Models.Validators
{
    public class PageNumberValidator : ComponentValidator
    {
        // track types of page numbers found - need one of each
        private bool foundRNPageNumbers;
        private bool foundArabicPageNumbers;
        
        
        public PageNumberValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
        }

        protected override void ParseContents()
        {
            ValidatePageNumbers();

            if (false == foundRNPageNumbers)
            {
                Errors.Add(new ComponentError("no_numeral_page_numbers", "No roman numeral page numbers were found"));
            }
            
            if (false == foundArabicPageNumbers)
            {
                Errors.Add(new ComponentError("no_page_numbers", "No numeric page numbers were found"));
            }
        }

        private void ValidatePageNumbers()
        {
            var bodyChildren = DocToValidate.MainDocumentPart.Document.Body.ChildElements;
            
            // section properties, which hold page number info, can show up in anywhere in the body or in paragraph elements
            //    so we'll iterate over everything
            foreach (var element in bodyChildren)
            {
                var type = element.GetType();
                
                // if we hit a paragraph, we need to check if it has non-null paragraph properties
                //    and pull its section properties if they exist
                if (type == typeof(Paragraph))
                {
                    Paragraph curParagraph = (Paragraph) element;

                    if (null != curParagraph.ParagraphProperties)
                    {
                        SectionProperties props = curParagraph.ParagraphProperties.SectionProperties;

                        if (null != props)
                        {
                            CheckProperties(props);
                        }
                    }
                } else if (type == typeof(SectionProperties))
                {
                    CheckProperties((SectionProperties) element);
                }
            }
        }

        private void CheckProperties(SectionProperties props)
        {
            foreach (var prop in props)
            {
                // locate page number property within passed props
                if ("pgNumType" == prop.LocalName)
                {
                    // cast the current prop to the correct type
                    var pageNum  = (PageNumberType) prop;
                    
                    // check for lower roman numeral page numbers and flag
                    if ("lowerRoman" == pageNum.Format)
                    {
                        foundRNPageNumbers = true;
                    }

                    // check for 1-indexed regular page numbers and flag
                    if (null == pageNum.Format && 1 == pageNum.Start)
                    {
                        foundArabicPageNumbers = true;
                    }
                }
            }
        }
    }
}