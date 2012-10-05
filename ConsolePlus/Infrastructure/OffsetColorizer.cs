using System.Collections.Generic;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ConsolePlus.Infrastructure
{
    public class OffsetColorizer : DocumentColorizingTransformer
    {
        public IList<LineColor> LineColors;

        public int StartOffset { get; set; }
        public int EndOffset { get; set; }

        protected override void ColorizeLine(DocumentLine line)
        {
            if (line.Length == 0)
                return;

            if (line.Offset < StartOffset || line.Offset > EndOffset)
                return;


            int start = line.Offset > StartOffset ? line.Offset : StartOffset;
            int end = EndOffset > line.EndOffset ? line.EndOffset : EndOffset;

            ChangeLinePart(start, end, element => element.TextRunProperties.SetForegroundBrush(Brushes.Red));
        }
    }

    public class LineColor
    {
        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
        public SolidColorBrush Colour { get; set; }
    }
}