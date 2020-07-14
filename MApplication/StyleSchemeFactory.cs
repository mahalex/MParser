using System;

namespace MApplication
{
    internal static class StyleSchemeFactory
    {
        private static StyleScheme _defaultScheme;

        static StyleSchemeFactory()
        {
            _defaultScheme = new StyleScheme(
                defaultToken: Style.Color(ConsoleColor.Gray),
                keyword: Style.Color(ConsoleColor.Green),
                controlKeyword: Style.Color(ConsoleColor.Yellow),
                trivia: Style.Color(ConsoleColor.DarkGray),
                punctuation: Style.Color(ConsoleColor.DarkBlue),
                @operator: Style.Color(ConsoleColor.Cyan),
                identifier: Style.Color(ConsoleColor.White),
                unquotedStringLiteral: Style.Color(ConsoleColor.Blue),
                stringLiteral: Style.Color(ConsoleColor.Magenta),
                numberLiteral: Style.Color(ConsoleColor.DarkGreen),
                bracket: Style.Color(ConsoleColor.DarkYellow));
        }

        public static StyleScheme GetDefaultScheme() => _defaultScheme;
    }
}
