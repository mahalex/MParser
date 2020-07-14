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
            int startingLine = 0,
            int cursorAbsoluteColumn = 0,
            int cursorAbsoluteLine = 0,
            int virtualCursorAbsoluteColumn = 0)
        {
            Text = text;
            Width = width;
            Height = height;
            StartingColumn = startingColumn;
            StartingLine = startingLine;
            CursorAbsoluteColumn = cursorAbsoluteColumn;
            CursorAbsoluteLine = cursorAbsoluteLine;
            VirtualCursorAbsoluteColumn = virtualCursorAbsoluteColumn;
        }

        public DisplayText Text { get; }

        public int Width { get; }

        public int Height { get; }

        public int StartingColumn { get; }

        public int StartingLine { get; }

        public int CursorRelativeColumn => CursorAbsoluteColumn - StartingColumn;

        public int CursorRelativeLine => CursorAbsoluteLine - StartingLine;

        public int CursorAbsoluteColumn { get; }

        public int CursorAbsoluteLine { get; }

        public int VirtualCursorAbsoluteColumn { get; }

        public int CurrentLineWidth => Text.Lines[CursorAbsoluteLine].Width;

        public DisplayTextViewPort CursorLeft(out bool needsRedraw)
        {
            var newCursorAbsoluteColumn = Math.Max(CursorAbsoluteColumn - 1, 0);
            if (CursorRelativeColumn == 0)
            {
                return With(
                    out needsRedraw,
                    cursorAbsoluteColumn: newCursorAbsoluteColumn,
                    virtualCursorAbsoluteColumn: newCursorAbsoluteColumn,
                    startingColumn: Math.Max(StartingColumn - 1, 0));
            }
            else
            {
                return With(
                    out needsRedraw,
                    cursorAbsoluteColumn: newCursorAbsoluteColumn,
                    virtualCursorAbsoluteColumn: newCursorAbsoluteColumn);
            }
        }

        public DisplayTextViewPort CursorRight(out bool needsRedraw)
        {
            var newCursorAbsoluteColumn = Math.Min(CursorAbsoluteColumn + 1, CurrentLineWidth);
            if (CursorRelativeColumn == Width - 1)
            {
                return With(
                    out needsRedraw,
                    cursorAbsoluteColumn: newCursorAbsoluteColumn,
                    virtualCursorAbsoluteColumn: newCursorAbsoluteColumn,
                    startingColumn: StartingColumn + 1);
            }
            else
            {
                return With(
                    out needsRedraw,
                    cursorAbsoluteColumn: newCursorAbsoluteColumn,
                    virtualCursorAbsoluteColumn: newCursorAbsoluteColumn);
            }
        }

        private DisplayTextViewPort SnapToLine(out bool needsRedraw)
        {
            var cursorAbsoluteColumn = Math.Min(VirtualCursorAbsoluteColumn, CurrentLineWidth);
            if (cursorAbsoluteColumn < StartingColumn)
            {
                return With(
                    out needsRedraw,
                    startingColumn: cursorAbsoluteColumn,
                    cursorAbsoluteColumn: cursorAbsoluteColumn);
            }

            if (cursorAbsoluteColumn > StartingColumn + Width - 1)
            {
                return With(
                    out needsRedraw,
                    startingColumn: cursorAbsoluteColumn - Width + 1,
                    cursorAbsoluteColumn: cursorAbsoluteColumn);
            }

            return With(
                out needsRedraw,
                cursorAbsoluteColumn: cursorAbsoluteColumn);
        }

        public DisplayTextViewPort CursorUp(out bool needsRedraw)
        {
            bool changed1;
            var result1 = CursorRelativeLine switch
            {
                0 => With(
                    out changed1,
                    startingLine: Math.Max(StartingLine - 1, 0),
                    cursorAbsoluteLine: Math.Max(StartingLine - 1, 0)),
                _ => With(out changed1, cursorAbsoluteLine: CursorAbsoluteLine - 1),
            };
            var result = result1.SnapToLine(out var changed2);
            needsRedraw = changed1 || changed2;
            return result;
        }

        public DisplayTextViewPort CursorDown(out bool needsRedraw)
        {
            bool changed1;
            var result1 = CursorRelativeLine switch
            {
                _ when CursorRelativeLine == Height - 1 =>
                    With(
                        out changed1,
                        startingLine: Math.Min(CursorAbsoluteLine + 1, Text.Lines.Count - 1) - Height + 1,
                        cursorAbsoluteLine: Math.Min(CursorAbsoluteLine + 1, Text.Lines.Count - 1)),
                _ => With(
                    out changed1,
                    cursorAbsoluteLine: CursorAbsoluteLine + 1),
            };
            var result = result1.SnapToLine(out var changed2);
            needsRedraw = changed1 || changed2;
            return result;
        }

        internal DisplayTextViewPort PageDown(out bool needsRedraw)
        {
            var result1 = With(
                out var changed1,
                startingLine: Math.Min(StartingLine + Height, Text.Lines.Count - 1),
                cursorAbsoluteLine: Math.Min(CursorAbsoluteLine + Height, Text.Lines.Count - 1));
            var result = result1.SnapToLine(out var changed2);
            needsRedraw = changed1 || changed2;
            return result;
        }

        internal DisplayTextViewPort PageUp(out bool needsRedraw)
        {
            var result1 = With(
                out var changed1,
                startingLine: Math.Max(StartingLine - Height, 0),
                cursorAbsoluteLine: Math.Max(CursorAbsoluteLine - Height, 0));
            var result = result1.SnapToLine(out var changed2);
            needsRedraw = changed1 || changed2;
            return result;
        }

        public DisplayTextViewPort CursorHome(out bool needsRedraw)
        {
            return With(
                out needsRedraw,
                startingColumn: 0,
                cursorAbsoluteColumn: 0,
                virtualCursorAbsoluteColumn: 0);
        }

        public DisplayTextViewPort CursorEnd(out bool needsRedraw)
        {
            var lineWidth = Text.Lines[CursorAbsoluteLine].Width;
            var toAdd = lineWidth - CursorAbsoluteColumn;
            if (toAdd == 0)
            {
                needsRedraw = false;
                return this;
            }

            if (CursorRelativeColumn + toAdd < Width)
            {
                return With(
                    out needsRedraw,
                    cursorAbsoluteColumn: CursorAbsoluteColumn + toAdd,
                    virtualCursorAbsoluteColumn: CursorAbsoluteColumn + toAdd);
            }

            return With(
                out needsRedraw,
                startingColumn: lineWidth - Width + 1,
                cursorAbsoluteColumn: lineWidth,
                virtualCursorAbsoluteColumn: lineWidth);
        }

        public void RenderTo(IOutputView view)
        {
            view.HideCursor();
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
            view.MoveCursorTo(column: CursorRelativeColumn, line: CursorRelativeLine);
            view.ShowCursor();
        }

        public DisplayTextViewPort With(
            out bool changed,
            int? width = null,
            int? height = null,
            int? startingColumn = null,
            int? startingLine = null,
            int? cursorAbsoluteColumn = null,
            int? cursorAbsoluteLine = null,
            int? virtualCursorAbsoluteColumn = null)
        {
            var widthValue = width ?? Width;
            var heightValue = height ?? Height;
            var startingColumnValue = startingColumn ?? StartingColumn;
            var startingLineValue = startingLine ?? StartingLine;
            var cursorAbsoluteColumnValue = cursorAbsoluteColumn ?? CursorAbsoluteColumn;
            var cursorAbsoluteLineValue = cursorAbsoluteLine ?? CursorAbsoluteLine;
            var virtualCursorAbsoluteColumnValue = virtualCursorAbsoluteColumn ?? VirtualCursorAbsoluteColumn;
            if (widthValue == Width &&
                heightValue == Height &&
                startingColumnValue == StartingColumn &&
                startingLineValue == StartingLine &&
                cursorAbsoluteColumnValue == CursorAbsoluteColumn &&
                cursorAbsoluteLineValue == CursorAbsoluteLine &&
                virtualCursorAbsoluteColumnValue == VirtualCursorAbsoluteColumn)
            {
                changed = false;
                return this;
            }

            changed = true;
            return new DisplayTextViewPort(
                text: Text,
                width: widthValue,
                height: heightValue,
                startingColumn: startingColumnValue,
                startingLine: startingLineValue,
                cursorAbsoluteColumn: cursorAbsoluteColumnValue,
                cursorAbsoluteLine: cursorAbsoluteLineValue,
                virtualCursorAbsoluteColumn: virtualCursorAbsoluteColumnValue);
        }
    }
}
