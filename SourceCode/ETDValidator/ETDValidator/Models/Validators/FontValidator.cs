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
        private int error_count = 0;
        public FontValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
            
            invalidFamilies.Add(4, "script");
            invalidFamilies.Add(5, "decorative");
        }
        
        protected override void ParseContents()
        {
            ValidateFamilies();
            ValidateSizes();
        }

        private void ValidateFamilies()
        {
            // get list of font classes from document font table 
            var fonts = DocToValidate.MainDocumentPart.FontTablePart.Fonts;

            
            foreach (var font in fonts)
            {
                // each font is represented as an array - the 3rd index holds the font family object
                var fontFamily = font.ToList()[2];
                
                // verify that we pulled the correct attribute
                if ("family" == fontFamily.LocalName)
                {
                    // get the font family name as a string
                    var family = fontFamily.GetAttributes()[0].Value;
                    
                    // create warning if font family is in list of invalid font families
                    if (invalidFamilies.ContainsValue(family))
                    {    
                        Warnings.Add(new ComponentWarning("Invalid Font", "A potentially non-standard font family was used: " + family));
                    }
                }
            }
        }

        private void ValidateSizes()
        {
            var styleProps = DocToValidate.MainDocumentPart.StyleDefinitionsPart.Styles;

            try
            {
                foreach (var prop in styleProps)
                {
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

                            if (FONT_SIZE != fontSizeVal)
                            {
                                string sizeMsg = FONT_SIZE > fontSizeVal ? "smaller" : "larger";
                                Errors.Add(new ComponentError("Invalid Font Size", "A font size " + sizeMsg + " than 12pt was found"));
                            }
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

        public bool StyleNeedsChecked(Style styleToCheck)
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
    }
}