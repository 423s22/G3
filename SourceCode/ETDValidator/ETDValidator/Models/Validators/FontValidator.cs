using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ETDVAlidator.Models.Validators
{
    public class FontValidator : ComponentValidator
    {
        private readonly Dictionary<int, string> invalidFamilies = new Dictionary<int, string>();
        private const int FONT_SIZE = 24; // standard 12 point font - openxml stores in half-point sizes

        // we'll only report one error for each potential outcome
        private bool foundSmallerSize = false;
        private bool foundLargerSize = false;

        public FontValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();

            Name = "fonts";
            
            invalidFamilies.Add(4, "script");
            invalidFamilies.Add(5, "decorative");
        }
        
        protected override void ParseContents()
        {
            ValidateFamilies();
            
            // font sizes show up both in document settings and explicitly in paragraph run properties
            //   in turn, we need to check the settings and every paragraph to confirm font sizes
            ValidateDocumentFontSizes();
            ValidateParagraphFontSizes();
        }

        private void ValidateFamilies()
        {
            // get list of font classes from document font table 
            var fonts = DocToValidate.MainDocumentPart.FontTablePart.Fonts;

            try
            {
                foreach (Font font in fonts)
                {
                    if (null != font.FontFamily)
                    {
                        // we'll allow "decorative" family if it is a symbol
                        if (invalidFamilies.ContainsValue(font.FontFamily.Val.ToString()) &&
                            "symbol" != font.Name.ToString().ToLower())
                        {
                            Warnings.Add(new ComponentWarning("Invalid Font", "A potentially non-standard font family was used"));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // should never happen, FontTablePart.Fonts should only contain fonts
                //   but we don't want to reject the document if something wonky happens
                Console.WriteLine("Issue parsing fonts...");
                Console.WriteLine(e.StackTrace);
            }
            
        }

        private void ValidateDocumentFontSizes()
        {
            var styleProps = DocToValidate.MainDocumentPart.StyleDefinitionsPart.Styles;

            try
            {
                foreach (var prop in styleProps)
                {
                    if (foundLargerSize && foundSmallerSize) break;
                    
                    if (typeof(Style) != prop.GetType())
                    {
                        continue;
                    }

                    if (StyleNeedsChecked((Style) prop))
                    {
                        StyleRunProperties runProps = ((Style) prop).StyleRunProperties;

                        if (null != runProps.FontSize)
                        {
                            int fontSizeVal = Int32.Parse(runProps.FontSize.Val);

                            CheckFontSize(fontSizeVal);
                        }

                        /*
                        // everything I've seen points to this value being the same as props.FontSize.Val - so we'll ignore it for now
                        if (null != props.FontSizeComplexScript)
                        {
                            int complexFontSizeVal = Int32.Parse(props.FontSizeComplexScript.Val);
                        }*/
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private bool StyleNeedsChecked(Style styleToCheck)
        {
            if (null == styleToCheck.StyleRunProperties)
            {
                return false;
            }

            string styleId = ((string) styleToCheck.StyleId).ToLower();
            
            if (styleId != "normal" && !styleId.Contains("heading") && !styleId.Contains("quote"))
            {
                return false;
            }

            return true;
        }

        private void ValidateParagraphFontSizes()
        {
            var paragraphs = DocToValidate.MainDocumentPart.Document.Body.ChildElements;

            foreach (var paragraph in paragraphs)
            {
                if (foundLargerSize && foundSmallerSize) break;
                
                // confirm we're dealing with a paragraph
                if (typeof(Paragraph) == paragraph.GetType())
                {
                    var paragraphObj = (Paragraph) paragraph;
                    
                    if (null!=paragraphObj.ParagraphProperties && null != paragraphObj.ParagraphProperties.ParagraphMarkRunProperties)
                    {
                        foreach (var prop in paragraphObj.ParagraphProperties.ParagraphMarkRunProperties)
                        {
                            if ("sz" == prop.LocalName)
                            {
                                int fontSizeVal = Int32.Parse(((FontSize) prop).Val);
                                CheckFontSize(fontSizeVal);
                            }
                        }
                    }
                }
            }
        }

        private void CheckFontSize(int fontSizeVal)
        {
            if (fontSizeVal > FONT_SIZE && !foundLargerSize)
            {
                Errors.Add(new ComponentError("Invalid Font Size", "A font size larger than 12pt was found"));
                foundLargerSize = true;
            }

            if (fontSizeVal < FONT_SIZE && !foundSmallerSize)
            {
                Errors.Add(new ComponentError("Invalid Font Size", "A font size smaller than 12pt was found"));
                foundSmallerSize = true;
            }
            
        }
    }
}