using System;

namespace MApplication
{
    internal class Style
    {
        public Style(
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public ConsoleColor ForegroundColor { get; }

        public ConsoleColor BackgroundColor { get; }

        public static Style Color(ConsoleColor foregroundColor)
        {
            return new Style(foregroundColor, ConsoleColor.Black);
        }

        public static Style ColorWithBackGround(
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor)
        {
            return new Style(foregroundColor, backgroundColor);
        }
    }
}
