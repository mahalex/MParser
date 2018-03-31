namespace Lexer
{
    public class PureTokenFactory
    {
        private ITextWindow Window { get; }

        public PureTokenFactory(ITextWindow window)
        {
            Window = window;
        }
        
        private static readonly string[] PureTokenOfKind =
        {
            null, // None = 0,
            null, // Identifier = 1,
            null, // NumberLiteral = 2,
            null, // StringLiteral = 3,
            null, // DoubleQuotedStringLiteral = 4,
            null, null, null, null, null, null, null, null, null, null, null, null, null, null, null,
            "=", // Assignment = 20,
            "==", // Equality = 21,
            "~=", // Inequality = 22,
            "&&", // LogicalAnd = 23,
            "||", // LogicalOr = 24,
            "&", // BitwiseAnd = 25,
            "|", // BitwiseOr = 26,
            "<", // Less = 27,
            "<=", // LessOrEqual = 28,
            ">", // Greater = 29,
            ">=", // GreaterOrEqual = 30,
            "~", // Not = 31,
            "+", // Plus = 32,
            "-", // Minus = 33,
            "*", // Multiply = 34,
            "/", // Divide = 35,
            "^", // Power = 36,
            "\\", // Backslash = 37,
            "'", // Transpose = 38,
            ".*", // DotMultiply = 39,
            "./", // DotDivide = 40,
            ".^", // DotPower = 41,
            ".\\", // DotBackslash = 42,
            ".'", // DotTranspose = 43,
            "@", // At = 44,
            ":", // Colon = 45,
            "?", // QuestionMark = 46,
            ",", // Comma = 47,
            ";", // Semicolon = 48,
            "{", // OpeningBrace = 49,
            "}", // ClosingBrace = 50,
            "[", // OpeningSquareBracket = 51,
            "]", // ClosingSquareBracket = 52,
            "(", // OpeningBracket = 53,
            ")", // ClosingBracket = 54,
            ".", // Dot = 55,
            "...", // DotDotDot = 56,
            
            "+", // UnaryPlus = 57,
            "-", // UnaryMinus = 58,
            "~", // UnaryNot = 59,

        };
        
        public PureToken CreatePunctuation(TokenKind kind)
        {
            return new PureToken(kind, PureTokenOfKind[(int)kind], null, Window.Position);
        }

        public PureToken CreateIdentifier(string s)
        {
            return new PureToken(TokenKind.Identifier, s, null, Window.Position);
        }

        public PureToken CreateNumberLiteral(string s)
        {
            return new PureToken(TokenKind.NumberLiteral, s, null, Window.Position); // TODO: actually parse number (here or in the lexer?)
        }

        public PureToken CreateStringLiteral(string s)
        {
            return new PureToken(TokenKind.StringLiteral, "'" + s + "'", s, Window.Position);
        }
        
        public PureToken CreateDoubleQuotedStringLiteral(string s)
        {
            return new PureToken(TokenKind.DoubleQuotedStringLiteral, "\"" + s + "\"", s, Window.Position);
        }

        public PureToken CreateEndOfFileToken()
        {
            return new PureToken(TokenKind.EndOfFile, "", null, Window.Position);
        }
    }
}