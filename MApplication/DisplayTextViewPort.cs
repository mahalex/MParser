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
            int cursorRelativeColumn = 0,
            int cursorRelativeLine = 0)
        {
            Text = text;
            Width = width;
            Height = height;
            StartingColumn = startingColumn;
            StartingLine = startingLine;
            CursorRelativeColumn = cursorRelativeColumn;
            CursorRelativeLine = cursorRelativeLine;
        }

        public DisplayText Text { get; }

        public int Width { get; }

        public int Height { get; }

        public int StartingColumn { get; }

        public int StartingLine { get; }

        public int CursorRelativeColumn { get; }

        public int CursorRelativeLine { get; }

        public int CursorAbsoluteColumn => CursorRelativeColumn + StartingColumn;

        public int CursorAbsoluteLine => CursorRelativeLine + StartingLine;

        public int CurrentLineWidth => Text.Lines[CursorAbsoluteLine].Width;

        public DisplayTextViewPort CursorLeft(out bool needsRedraw)
        {
            if (CursorRelativeColumn == 0)
            {
                return ShiftLeft(out needsRedraw);
            }
            else
            {
                return With(out needsRedraw, cursorRelativeColumn: CursorRelativeColumn - 1);
            }
        }

        public DisplayTextViewPort CursorRight(out bool needsRedraw)
        {
            if (CursorRelativeColumn == Width - 1)
            {
                return ShiftRight(out needsRedraw);
            }
            else
            {
                return With(out needsRedraw, cursorRelativeColumn: CursorRelativeColumn + 1);
            }
        }

        private DisplayTextViewPort SnapToLine(out bool needsRedraw)
        {
            if (CursorAbsoluteColumn > CurrentLineWidth)
            {
                needsRedraw = true;
                var toSubtract = CursorAbsoluteColumn - CurrentLineWidth;
                if (toSubtract < CursorRelativeColumn)
                {
                    return With(out var _, cursorRelativeColumn: CursorRelativeColumn - toSubtract);
                }

                return With(
                    out var _,
                    startingColumn: StartingColumn - (toSubtract - CursorRelativeColumn),
                    cursorRelativeColumn: 0);
            }
            else
            {
                needsRedraw = false;
                return this;
            }
        }

        public DisplayTextViewPort CursorUp(out bool needsRedraw)
        {
            bool changed1;
            var result1 = CursorRelativeLine switch
            {
                0 => ShiftUp(out changed1),
                _ => With(out changed1, cursorRelativeLine: CursorRelativeLine - 1),
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
                _ when CursorRelativeLine == Height - 1 => ShiftDown(out changed1),
                _ => With(out changed1, cursorRelativeLine: CursorRelativeLine + 1),
            };
            var result = result1.SnapToLine(out var changed2);
            needsRedraw = changed1 || changed2;
            return result;
        }

        public DisplayTextViewPort CursorHome(out bool needsRedraw)
        {
            return With(out needsRedraw, startingColumn: 0, cursorRelativeColumn: 0);
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
                return With(out needsRedraw, cursorRelativeColumn: CursorRelativeColumn + toAdd);
            }

            return With(
                out needsRedraw,
                startingColumn: StartingColumn + CursorRelativeColumn + toAdd - Width + 1,
                cursorRelativeColumn: Width - 1);
        }

        public DisplayTextViewPort ShiftLeft(out bool needsRedraw)
        {
            return With(out needsRedraw, startingColumn: Math.Max(StartingColumn - 1, 0));
        }

        public DisplayTextViewPort ShiftRight(out bool needsRedraw)
        {
            return With(out needsRedraw, startingColumn: StartingColumn + 1);
        }

        public DisplayTextViewPort ShiftUp(out bool needsRedraw)
        {
            return With(out needsRedraw, startingLine: Math.Max(StartingLine - 1, 0));
        }

        public DisplayTextViewPort ShiftDown(out bool needsRedraw)
        {
            return With(out needsRedraw, startingLine: StartingLine + 1);
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
            int? cursorRelativeColumn = null,
            int? cursorRelativeLine = null)
        {
            var widthValue = width ?? Width;
            var heightValue = height ?? Height;
            var startingColumnValue = startingColumn ?? StartingColumn;
            var startingLineValue = startingLine ?? StartingLine;
            var cursorRelativeColumnValue = cursorRelativeColumn ?? CursorRelativeColumn;
            var cursorRelativeLineValue = cursorRelativeLine ?? CursorRelativeLine;
            if (widthValue == Width &&
                heightValue == Height &&
                startingColumnValue == StartingColumn &&
                startingLineValue == StartingLine &&
                cursorRelativeColumnValue == CursorRelativeColumn &&
                cursorRelativeLineValue == CursorRelativeLine)
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
                cursorRelativeColumn: cursorRelativeColumnValue,
                cursorRelativeLine: cursorRelativeLineValue);
        }
    }
}
