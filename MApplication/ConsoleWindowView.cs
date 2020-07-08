using System;

namespace MApplication
{
    internal class ConsoleWindowView : IOutputView
    {
        public ConsoleWindowView(int startingColumn, int startingLine, int width, int height)
        {
            StartingColumn = startingColumn;
            StartingLine = startingLine;
            Width = width;
            Height = height;
        }

        public int StartingColumn { get; }

        public int StartingLine { get; }

        public int Width { get; }

        public int Height { get; }

        public void MoveCursorTo(int column, int line)
        {
            Console.CursorLeft = StartingColumn + column;
            Console.CursorTop = StartingLine + line;
        }

        public void SetStyle(Style style)
        {
            if (Console.ForegroundColor != style.ForegroundColor)
            {
                Console.ForegroundColor = style.ForegroundColor;
            }

            if (Console.BackgroundColor != style.BackgroundColor)
            {
                Console.BackgroundColor = style.BackgroundColor;
            }
        }

        public void WriteText(string s)
        {
            Console.Write(s);
        }
    }
}
