using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ETDVAlidator.Models.Validators
{
    // TODO: Expand validator to include images as well as tables.
    public class FigureValidator : ComponentValidator
    {
        private const string Top = "top";
        private const string Bottom = "bottom";

        private const string Inline = "inline";
        private const string Grouped = "grouped";

        private const int TripleSpaceUpperBound = 720; // 12pt triple spacing
        private const int TripleSpaceLowerBound = 600; // 10pt triple spacing
        private const int SingleSpaceUpperBound = 240; // 12pt single spacing
        private const int SingleSpaceLowerBound = 200; // 10pt single spacing
        public FigureValidator()
        {
            Warnings = new List<ComponentWarning>();
            Errors = new List<ComponentError>();

            Name = "figures";
        }

        protected override void ParseContents()
        {
            bool topCaptions = false;
            bool bottomCaptions = false;
            bool inlineFigures = false;
            bool groupedFigures = false;
            
            IEnumerable<Table> tables = DocToValidate.MainDocumentPart.Document.Body.Elements<Table>();
            foreach (Table table in tables)
            {
                string captionPosition = FindCaption(table);
                switch (captionPosition)
                {
                    case Top:
                        topCaptions = true;
                        break;
                    
                    case Bottom:
                        bottomCaptions = true;
                        break;
                    
                    case null:
                        Errors.Add(new ComponentError(
                                "Caption Error", 
                                "A table in your document does not have a caption."
                            )
                        );
                        break;
                }

                string tablePosition = FindPosition(table, captionPosition);
                if (tablePosition == Inline) inlineFigures = true;
                else if (tablePosition == Grouped) groupedFigures = true;

                // MeasureSpacing function is still a work in progress
                // MeasureSpacing(table, captionPosition);
            }

            if (topCaptions && bottomCaptions)
            {
                Errors.Add(new ComponentError(
                        "Caption Error", 
                        "Make sure your captions are either above or below your figures, not both."
                    )
                );
            }

            if (inlineFigures && groupedFigures)
            {
                Warnings.Add(new ComponentWarning(
                        "Figure Warning",
                        "Your document may have a combination of figures that are inline and grouped at the end of each chapter, verify that you are only doing one or the other."
                    )
                );
            }
        }

        // TODO: HasCaption currently only catches captions for tables using Office2007 table captions
        //       it should be expanded to also account for Office365 table captions
        private static string FindCaption(Table table)
        {
            Paragraph prevElement = table.PreviousSibling() is Paragraph ? (Paragraph) table.PreviousSibling() : null;
            Paragraph nextElement = table.NextSibling() is Paragraph ? (Paragraph) table.NextSibling() : null;
            
            if (prevElement != null && prevElement.ParagraphProperties != null)
            {
                if (prevElement.ParagraphProperties.ParagraphStyleId.Val == "Table")
                {
                    if (!DetectWhitespace(prevElement)) return Top;
                }
            }

            if (nextElement != null && nextElement.ParagraphProperties != null)
            {
                if (nextElement.ParagraphProperties.ParagraphStyleId.Val == "Table")
                {
                    if (!DetectWhitespace(nextElement)) return Bottom;
                }
            }
            
            return null;
        }

        // TODO: Need to add support for identifying figures at the end of each chapter
        // TODO: FindPosition currently only handles captions for tables using Office2007 table captions
        //       it should be expanded to also account for Office365 table captions
        private string FindPosition(Table table, string captionPosition)
        {
            bool hasWhitespace = false;
            OpenXmlElement prev;
            switch (captionPosition)
            {
                case Top:
                    Paragraph caption = (Paragraph) table.PreviousSibling();
                    prev = caption.PreviousSibling();
                    break;
                
                case Bottom:
                    prev = table.PreviousSibling();
                    break;
                
                default:
                    prev = null;
                    break;
            }
            
            while (prev != null && DetectWhitespace(prev))
            {
                if (DetectPageBreak(prev) && hasWhitespace)
                {
                    Warnings.Add(new ComponentWarning(
                            "Figure Warning",
                            "A table at the start of a page may not start on the first line of that page."
                        )
                    );
                    
                    return Inline;
                }

                hasWhitespace = true;
                prev = prev.PreviousSibling();
            }

            return null;
        }

        // This function is a tangled mess because of all the different ways spacing can be measured
        // around an element. Programmer beware.
        //
        // This is still a work in progress, and needs to have better checking for empty elements surrounding
        // the figure.
        //
        // TODO: MeasureSpacing currently only handles captions for tables using Office2007 table captions
        //       it should be expanded to also account for Office365 table captions
        private void MeasureSpacing(Table table, string captionPosition)
        {
            SpacingBetweenLines dblAbove = GetSpacing(table.PreviousSibling().PreviousSibling());
                SpacingBetweenLines above = GetSpacing(table.PreviousSibling());
                SpacingBetweenLines below = GetSpacing(table.NextSibling());
                SpacingBetweenLines dblBelow = GetSpacing(table.NextSibling().NextSibling());
                bool pageBreak = false;
                int captionTotal;
                int totalAboveSpacing;
                int aboveAfter;
                int aboveLine;
                int totalBelowSpacing;
                int aboveBefore;
                int dblAboveAfter;
                int dblAboveLine;
                switch (captionPosition)
                {
                    case Top:
                        ///////////////////////////////
                        //  Measure Caption Spacing  //
                        ///////////////////////////////

                        if (above != null)
                        {
                            aboveAfter = above.After != null ? int.Parse(above.After) : 0;
                            aboveLine = above.Line != null ? int.Parse(above.Line) : 0;
                            captionTotal = aboveAfter + aboveLine;
                            if (captionTotal < SingleSpaceLowerBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Caption Warning",
                                        "The spacing between one of your tables and its caption may be too small."
                                    )
                                );
                            }

                            else if (captionTotal > SingleSpaceUpperBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Caption Warning",
                                        "The spacing between one of your tables and its caption may be too large."
                                    )
                                );
                            }
                        }

                        /////////////////////////////
                        //  Measure Above Spacing  //
                        /////////////////////////////

                        if (dblAbove != null) // the element above the caption has spacing data
                        {
                            dblAboveAfter = dblAbove.After != null ? int.Parse(dblAbove.After) : 0;
                            dblAboveLine = dblAbove.Line != null ? int.Parse(dblAbove.Line) : 0;
                            aboveBefore = above.Before != null ? int.Parse(above.Before) : 0;
                            totalAboveSpacing = dblAboveAfter + dblAboveLine + aboveBefore;
                        }
                        
                        else // the element above the caption does not have spacing data 
                        {
                            totalAboveSpacing = above.Before != null ? int.Parse(above.Before) : 0;
                        }
                        
                        if (!DetectPageBreak(table.PreviousSibling().PreviousSibling())) {
                            if (totalAboveSpacing < TripleSpaceLowerBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Figure Warning", 
                                        "The spacing above one of your tables may be too small (expected triple spacing)"
                                    )
                                );
                            } 
                            
                            else if (totalAboveSpacing > TripleSpaceUpperBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Figure Warning", 
                                        "The spacing above one of your tables may be too large (expected triple spacing)"
                                    )
                                );
                            }
                        }
                        
                        /////////////////////////////
                        //  Measure Below Spacing  //
                        /////////////////////////////

                        if (below == null) // the element below the table has no spacing data
                        {
                            totalBelowSpacing = 0;
                        }
                        
                        else if (dblBelow == null) // no spacing data for the element twice below the table
                        {
                            int belowAfter = below.After != null ? int.Parse(below.After) : 0;
                            int belowLine = below.Line != null ? int.Parse(below.Line) : 0;
                            totalBelowSpacing = belowAfter + belowLine;
                        }
                        
                        else // spacing data exists for both elements 
                        {
                            int belowAfter = below.After != null ? int.Parse(below.After) : 0;
                            int belowLine = below.Line != null ? int.Parse(below.Line) : 0;
                            int dblBelowBefore = dblBelow.Before != null ? int.Parse(dblBelow.Before) : 0;
                            totalBelowSpacing = belowAfter + belowLine + dblBelowBefore;
                        }
                        
                        if (totalBelowSpacing < TripleSpaceLowerBound)
                        {
                            Warnings.Add(new ComponentWarning(
                                    "Figure Warning", 
                                    "The spacing below one of your tables may be too small (expected triple spacing)"
                                )
                            );
                        } 
                            
                        else if (totalBelowSpacing > TripleSpaceUpperBound)
                        {
                            Warnings.Add(new ComponentWarning(
                                    "Figure Warning", 
                                    "The spacing below one of your tables may be too large (expected triple spacing)"
                                )
                            );
                        }
                        break;
                    
                    
                    case Bottom:
                        ///////////////////////////////
                        //  Measure Caption Spacing  //
                        ///////////////////////////////

                        if (below != null)
                        {
                            captionTotal = below.Before != null ? int.Parse(below.Before) : 0;
                            if (captionTotal < SingleSpaceLowerBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Caption Warning",
                                        "The spacing between one of your tables and its caption may be too small."
                                    )
                                );
                            }

                            else if (captionTotal > SingleSpaceUpperBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Caption Warning",
                                        "The spacing between one of your tables and its caption may be too large."
                                    )
                                );
                            }
                        }

                        /////////////////////////////
                        //  Measure Above Spacing  //
                        /////////////////////////////
                        
                        if (above == null) // the element above the figure has no spacing data
                        {
                            totalAboveSpacing = 0;
                        }
                        
                        else if (dblAbove == null) // the element twice above the figure has no spacing data
                        {
                            aboveAfter = above.After != null ? int.Parse(above.After) : 0;
                            aboveLine = above.Line != null ? int.Parse(above.Line) : 0;
                            totalAboveSpacing = aboveAfter + aboveLine;
                            
                            if (DetectWhitespace(table.PreviousSibling()))
                            {
                                aboveBefore = above.Before != null ? int.Parse(above.Before) : 0;
                                totalAboveSpacing = aboveAfter + aboveLine + aboveBefore;
                                pageBreak = DetectPageBreak(table.PreviousSibling());
                            }
                        }
                        
                        else // both elements have spacing data
                        {
                            aboveAfter = above.After != null ? int.Parse(above.After) : 0;
                            aboveLine = above.Line != null ? int.Parse(above.Line) : 0;
                            totalAboveSpacing = aboveAfter + aboveLine;
                            
                            if (DetectWhitespace(table.PreviousSibling()))
                            {
                                aboveBefore = above.Before != null ? int.Parse(above.Before) : 0;
                                dblAboveAfter = dblAbove.After != null ? int.Parse(dblAbove.After) : 0;
                                dblAboveLine = dblAbove.Line != null ? int.Parse(dblAbove.Line) : 0;
                                totalAboveSpacing = aboveAfter + aboveLine + aboveBefore + dblAboveAfter + dblAboveLine;
                                pageBreak = DetectPageBreak(table.PreviousSibling().PreviousSibling());
                            }
                        }
                        
                        if (!pageBreak) {
                            if (totalAboveSpacing < TripleSpaceLowerBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Figure Warning", 
                                        "The spacing above one of your tables may be too small (expected triple spacing)"
                                    )
                                );
                            } 
                            
                            else if (totalAboveSpacing > TripleSpaceUpperBound)
                            {
                                Warnings.Add(new ComponentWarning(
                                        "Figure Warning", 
                                        "The spacing above one of your tables may be too large (expected triple spacing)"
                                    )
                                );
                            }
                        }
                        
                        /////////////////////////////
                        //  Measure Below Spacing  //
                        /////////////////////////////

                        if (dblBelow == null) // no spacing data for the element below the caption
                        {
                            int belowAfter = below.After != null ? int.Parse(below.After) : 0;
                            int belowLine = below.Line != null ? int.Parse(below.Line) : 0;
                            totalBelowSpacing = belowAfter + belowLine;
                        }
                        
                        else // spacing data exists for the caption and the element below it
                        {
                            int belowAfter = below.After != null ? int.Parse(below.After) : 0;
                            int belowLine = below.Line != null ? int.Parse(below.Line) : 0;
                            int dblBelowBefore = dblBelow.Before != null ? int.Parse(dblBelow.Before) : 0;
                            totalBelowSpacing = belowAfter + belowLine + dblBelowBefore;
                        }
                        
                        if (totalBelowSpacing < TripleSpaceLowerBound)
                        {
                            Warnings.Add(new ComponentWarning(
                                    "Figure Warning", 
                                    "The spacing below one of your tables may be too small (expected triple spacing)"
                                )
                            );
                        } 
                            
                        else if (totalBelowSpacing > TripleSpaceUpperBound)
                        {
                            Warnings.Add(new ComponentWarning(
                                    "Figure Warning", 
                                    "The spacing below one of your tables may be too large (expected triple spacing)"
                                )
                            );
                        }
                        break;
                }
        }

        private static SpacingBetweenLines GetSpacing(OpenXmlElement element)
        {
            if (element is Paragraph)
            {
                ParagraphProperties props = element.Elements<ParagraphProperties>().First();
                return props.SpacingBetweenLines;
            }

            return null;
        }
        
        private static bool DetectWhitespace(OpenXmlElement element)
        {
            if (element is Paragraph)
            {
                return element.InnerText.Length <= 0;
            }

            return false;
        }
        
        private static bool DetectPageBreak(OpenXmlElement element)
        {
            if (element is Paragraph)
            {
                foreach (Run run in element.Elements<Run>())
                {
                    foreach (Break b in run.Elements<Break>())
                    {
                        if (b.Type == BreakValues.Page) return true;
                    }
                }
            }

            return false;
        }

        private static IList<OpenXmlElement> FindChapterHeadings()
        {
            throw new NotImplementedException();
        }
    }
}