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

            Name = "colors";
        }

        // TODO: Expand this function to include text within tables
        protected override void ParseContents()
        {
            IEnumerable<Paragraph> paragraphs = DocToValidate.MainDocumentPart.Document.Body.Elements<Paragraph>();
            if (DocToValidate.MainDocumentPart.Document.DocumentBackground?.Color != null)
            {
                Warnings.Add(new ComponentWarning(
                        "Color Warning",
                        "Document contains pages with colored backgrounds."
                    )
                );
            }
            
            foreach (Paragraph paragraph in paragraphs)
            {
                foreach (Run run in paragraph.Elements<Run>())
                {
                    Color color = run.RunProperties?.Color != null ? run.RunProperties.Color : null;
                    if (color?.Val != null && color.Val != "000000")
                    {
                        Warnings.Add(new ComponentWarning(
                            "Color Warning",
                            "Document contains colored text."
                            )
                        );
                    }

                    if (run.RunProperties?.Highlight != null || run.RunProperties?.Shading?.Fill != null)
                    {
                        Warnings.Add(new ComponentWarning(
                                "Color Warning",
                                "Document contains highlighted text."
                            )
                        );
                    }
                }

                if (paragraph.ParagraphProperties?.Shading?.Fill != null)
                {
                    Warnings.Add(new ComponentWarning(
                            "Color Warning",
                            "Document contains paragraphs with background coloring."
                        )
                    );
                }
            }
        }
    }
}