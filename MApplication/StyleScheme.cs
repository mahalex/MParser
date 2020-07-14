namespace MApplication
{
    internal class StyleScheme
    {
        public StyleScheme(
            Style defaultToken,
            Style keyword,
            Style controlKeyword,
            Style trivia,
            Style punctuation,
            Style @operator,
            Style identifier,
            Style unquotedStringLiteral,
            Style stringLiteral,
            Style numberLiteral,
            Style bracket)
        {
            DefaultToken = defaultToken;
            Keyword = keyword;
            ControlKeyword = controlKeyword;
            Trivia = trivia;
            Punctuation = punctuation;
            Operator = @operator;
            Identifier = identifier;
            UnquotedStringLiteral = unquotedStringLiteral;
            StringLiteral = stringLiteral;
            NumberLiteral = numberLiteral;
            Bracket = bracket;
        }

        public Style DefaultToken { get; }

        public Style Keyword { get; }

        public Style ControlKeyword { get; }

        public Style Trivia { get; }

        public Style Punctuation { get; }

        public Style Operator { get; }

        public Style Identifier { get; }

        public Style UnquotedStringLiteral { get; }

        public Style StringLiteral { get; }

        public Style NumberLiteral { get; }

        public Style Bracket { get; }
    }
}
