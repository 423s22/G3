using System.Collections.Generic;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Xml;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Math;

namespace ETDVAlidator.Models.Validators
{
    public class FontValidator : ComponentValidator
    {
        private Dictionary<int, string> invalidFamilies = new Dictionary<int, string>();
        public FontValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
            
            invalidFamilies.Add(4, "script");
            invalidFamilies.Add(5, "decorative");
        }
        
        protected override void ParseContents()
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
    }
}