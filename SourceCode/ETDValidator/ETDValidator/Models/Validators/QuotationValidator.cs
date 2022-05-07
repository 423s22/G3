using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Vml.Office;

namespace ETDVAlidator.Models.Validators
{
    public class QuotationValidator : ComponentValidator
    {

        private const int QuoteLength = 430; // roughly how many characters are in 4 lines of text in 10 - 12 pt font
        
        public QuotationValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();
        }
        
        protected override void ParseContents()
        {
            IEnumerable<Paragraph> paragraphs = DocToValidate.MainDocumentPart.Document.Body.Elements<Paragraph>();
            foreach (Paragraph paragraph in paragraphs)
            {
                int count = 0;
                bool open = false;
                string content = paragraph.InnerText;

                // paragraph is a block quote
                // TODO: add checking to make sure block quotes are all a single quote
                if (content[0] == '"' && content[^1] == '"')
                {
                    if (paragraph.ParagraphProperties != null)
                    {
                        if (paragraph.ParagraphProperties.Alignment != TextAlignmentTypeValues.Justified)
                        {
                            Errors.Add(new ComponentError(
                                    "Quotation Error",
                                    "One of your block quotes is not justified!"
                                )
                            );
                        }

                        if (paragraph.ParagraphProperties.SpaceBefore?.SpacingPoints != null
                            && paragraph.ParagraphProperties.SpaceAfter?.SpacingPoints != null)
                        {
                            int before = paragraph.ParagraphProperties.SpaceBefore.SpacingPoints.Val;
                            int after = paragraph.ParagraphProperties.SpaceAfter.SpacingPoints.Val;
                            if (before != after)
                            {
                                Errors.Add(new ComponentError(
                                        "Quotation Error",
                                        "The indentation of one of your block quotes is not the same on both sides!"
                                    )
                                );
                            }
                        }
                        
                        // TODO: add checking for spacing around block quotes
                    }
                }

                // inline quotes
                else
                {
                    foreach (char c in content)
                    {
                        if (c == '"')
                        {
                            open = !open;

                            if (!open && count > QuoteLength)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Quotation Warning",
                                        "Your document contains an inline quotation that is " + count +
                                        " characters long. Consider converting this quotation into a block quote."
                                    )
                                );
                            }

                            count = 0;
                        }

                        if (open)
                        {
                            count++;
                        }
                    }

                    if (open)
                    {
                        Errors.Add(new ComponentError(
                                "Quotation Error",
                                "Your document contains an unclosed quotation mark, review your document to identify stray punctuation."
                            )
                        );
                    }
                }
            }
        }
    }
}