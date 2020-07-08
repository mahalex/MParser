using System;

namespace MApplication
{
    internal class DisplayTextViewPort
    {
        public DisplayTextViewPort(
            DisplayText text,
            int width,
            int height,
            int startingColumn = 0,
            int startingLine = 0)
        {
            Text = text;
            Width = width;
            Height = height;
            StartingColumn = startingColumn;
            StartingLine = startingLine;
        }

        public DisplayText Text { get; }

        public int Width { get; }

        public int Height { get; }

        public int StartingColumn { get; }

        public int StartingLine { get; }

        public void RenderTo(IOutputView view)
        {
            for (var lineNumber = StartingLine; lineNumber < StartingLine + Height; lineNumber++)
            {
                view.MoveCursorTo(0, lineNumber - StartingLine);
                if (lineNumber >= Text.Lines.Count)
                {
                    view.WriteText(new string(' ', Width));
                    continue;
                }
                var line = Text.Lines[lineNumber];
                var startsIn = StartingColumn;
                foreach (var chunk in line.Chunks)
                {
                    var left = Math.Max(0, startsIn);
                    var right = Math.Min(chunk.Width, startsIn + Width);
                    if (left < right)
                    {
                        view.SetStyle(chunk.Style);
                        view.WriteText(chunk.Text[left..right].ToString());
                    }

                    startsIn -= chunk.Width;
                    if (startsIn + Width <= 0)
                    {
                        break;
                    }
                }

                if (startsIn + Width > 0)
                {
                    var numberOfSpaces = Math.Min(startsIn + Width, Width);
                    view.WriteText(new string(' ', numberOfSpaces));
                }
            }
        }

        public DisplayTextViewPort With(
            int? width = null,
            int? height = null,
            int? startingColumn = null,
            int? startingLine = null)
        {
            var widthValue = width ?? Width;
            var heightValue = height ?? Height;
            var startingColumnValue = startingColumn ?? StartingColumn;
            var startingLineValue = startingLine ?? StartingLine;
            if (widthValue == Width &&
                heightValue == Height &&
                startingColumnValue == StartingColumn &&
                startingLineValue == StartingLine)
            {
                return this;
            }

            return new DisplayTextViewPort(
                text: Text,
                width: widthValue,
                height: heightValue,
                startingColumn: startingColumnValue,
                startingLine: startingLineValue);
        }
    }
}
