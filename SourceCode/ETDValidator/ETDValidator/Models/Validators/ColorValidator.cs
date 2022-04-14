using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ETDVAlidator.Models.Validators
{
    public class ColorValidator : ComponentValidator
    {
        public ColorValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
        }

        protected override void ParseContents()
        {
            foreach (var element in DocToValidate.MainDocumentPart.Document.Body)
            {
                Paragraph par = element is Paragraph ? (Paragraph) element : null;
                if (par is null) { continue; }
                
                for (var i = 1; i < par.ChildElements.Count; i++)
                {
                    Run run = par.ChildElements[i] is Run ? (Run) par.ChildElements[i] : null;
                    if (run is null) { continue; }

                    if (run.RunProperties.Color != null && run.RunProperties.Color.GetAttributes()[0].Value != "000000")
                    {
                        Warnings.Add(new ComponentWarning(
                            "Color Warning", 
                            "Document contains colored text.")
                        );
                    }
                }
            }
        }
    }
}