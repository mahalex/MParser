using System.Collections.Generic;
using System.Text;

namespace Lexer
{
    public class MLexer : ILexer<Token>
    {
        private ITextWindow Window { get; }
        private Token LastToken { get; set; }
        private PureTokenFactory PureTokenFactory { get; }

        public MLexer(ITextWindow window, PureTokenFactory pureTokenFactory)
        {
            Window = window;
            PureTokenFactory = pureTokenFactory;
        }

        private static bool IsEolOrEof(char c)
        {
            return c == '\n' || c == '\r' || c == '\0';
        }

        private Trivia LexComment()
        {
            var n = 1;
            while (!IsEolOrEof(Window.PeekChar(n)))
            {
                n++;
            }

            return new Trivia(TriviaType.Comment, Window.GetAndConsumeChars(n));
        }

        private List<Trivia> LexCommentAfterDotDotDot()
        {
            var n = 0;
            while (!IsEolOrEof(Window.PeekChar(n)))
            {
                n++;
            }

            var comment = new Trivia(TriviaType.Comment, Window.GetAndConsumeChars(n));
            var result = new List<Trivia> { comment };
            var character = Window.PeekChar();
            if (character == '\n' || character == '\r')
            {
                Window.ConsumeChar();
                result.Add(new Trivia(TriviaType.Whitespace, character.ToString()));
            }

            return result;
        }

        private List<Trivia> LexTrivia(bool isTrailing)
        {
            var triviaList = new List<Trivia>();
            var whiteSpaceCache = new StringBuilder();
            while (true)
            {
                var character = Window.PeekChar();
                switch (character)
                {
                    case ' ':
                    case '\t':
                        Window.ConsumeChar();
                        whiteSpaceCache.Append(character);
                        break;
                    case '\r':
                    case '\n':
                        Window.ConsumeChar();
                        whiteSpaceCache.Append(character);
                        var whiteSpace = whiteSpaceCache.ToString();
                        triviaList.Add(new Trivia(TriviaType.Whitespace, whiteSpace));
                        if (isTrailing)
                        {
                            return triviaList;
                        }

                        whiteSpaceCache.Clear();
                        break;
                    case '%':
                        if (whiteSpaceCache.Length > 0)
                        {
                            triviaList.Add(new Trivia(TriviaType.Whitespace, whiteSpaceCache.ToString()));
                        }

                        whiteSpaceCache.Clear();
                        triviaList.Add(LexComment());
                        break;
                    case '.':
                        if (Window.PeekChar(1) == '.' && Window.PeekChar(2) == '.')
                        {
                            if (whiteSpaceCache.Length > 0)
                            {
                                triviaList.Add(new Trivia(TriviaType.Whitespace, whiteSpaceCache.ToString()));
                            }

                            whiteSpaceCache.Clear();
                            triviaList.AddRange(LexCommentAfterDotDotDot());                       
                        }
                        else
                        {
                            if (whiteSpaceCache.Length > 0)
                            {
                                triviaList.Add(new Trivia(TriviaType.Whitespace, whiteSpaceCache.ToString()));
                            }
                            return triviaList;
                        }
                        break;
                    default:
                        if (whiteSpaceCache.Length > 0)
                        {
                            triviaList.Add(new Trivia(TriviaType.Whitespace, whiteSpaceCache.ToString()));
                        }
                        return triviaList;
                }
            }
        }

        private static bool IsLetterOrDigitOrUnderscore(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || (c == '_');
        }

        private PureToken ContinueParsingIdentifier()
        {
            var n = 1;
            while (IsLetterOrDigitOrUnderscore(Window.PeekChar(n)))
            {
                n++;
            }

            var identifier = Window.GetAndConsumeChars(n);
            return PureTokenFactory.CreateIdentifier(identifier);
        }

        private enum NumberParsingState
        {
            Start,
            DigitsBeforeDot,
            AfterDot,
            DigitsAfterDot,
            AfterE,
            SignAfterE,
            DigitsAfterE
        }

        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private static bool IsDigitOrDot(char c)
        {
            return c == '.' || (c >= '0' && c <= '9');
        }

        private static bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        private PureToken? ContinueParsingNumber()
        {
            var state = NumberParsingState.Start;
            var n = 0;
            var left = Window.CharactersLeft();
            var success = false;
            var fail = false;
            while (n < left)
            {
                var c = Window.PeekChar(n);
                switch (state)
                {
                    case NumberParsingState.Start:
                        if (IsDigitOrDot(c))
                        {
                            state = NumberParsingState.DigitsBeforeDot;
                        }
                        else
                        {
                            fail = true;
                        }
                        break;
                    case NumberParsingState.DigitsBeforeDot:
                        if (IsDigit(c))
                        {
                        }
                        else if (c == '.')
                        {
                            state = NumberParsingState.AfterDot;
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = NumberParsingState.AfterE;
                        }
                        else
                        {
                            success = true;
                        }
                        break;
                    case NumberParsingState.AfterDot:
                        if (IsDigit(c))
                        {
                            state = NumberParsingState.DigitsAfterDot;
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = NumberParsingState.AfterE;
                        }
                        else if (IsWhitespace(c) || c == ';' || c == ']' || c == ')' || c == '}')
                        {
                            success = true;
                        }
                        else if (c == '^' || c == '*' || c == '/' || c == '\\' || c == '\'')
                        {
                            n -= 1;
                            success = true;
                        }
                        else
                        {
                            fail = true;
                        }

                        break;
                    case NumberParsingState.DigitsAfterDot:
                        if (IsDigit(c))
                        {
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = NumberParsingState.AfterE;
                        }
                        else
                        {
                            success = true;
                        }

                        break;
                    case NumberParsingState.AfterE:
                        if (IsDigit(c))
                        {
                            state = NumberParsingState.DigitsAfterE;
                        }
                        else if (c == '+' || c == '-')
                        {
                            state = NumberParsingState.SignAfterE;
                        }
                        else
                        {
                            fail = true;
                        }

                        break;
                    case NumberParsingState.SignAfterE:
                        if (IsDigit(c))
                        {
                            state = NumberParsingState.DigitsAfterE;
                        }
                        else
                        {
                            fail = true;
                        }

                        break;
                    case NumberParsingState.DigitsAfterE:
                        if (IsDigit(c))
                        {
                        }
                        else
                        {
                            success = true;
                        }

                        break;
                }

                if (fail)
                {
                    throw new ParsingException("Error while parsing number.");
                }

                if (success)
                {
                    break;
                }
                n++;
            }

            if (n >= left)
            {
                switch (state)
                {
                    case NumberParsingState.DigitsBeforeDot:
                    case NumberParsingState.DigitsAfterDot:
                    case NumberParsingState.DigitsAfterE:
                        success = true;
                        break;
                }
            }

            if (success)
            {
                var s = Window.GetAndConsumeChars(n);
                return PureTokenFactory.CreateNumberLiteral(s);
            }

            return null;
        }

        private PureToken ContinueParsingStringLiteral()
        {
            Window.ConsumeChar();
            var pieces = new List<string>();
            var n = 0;
            while (true) {
                if (Window.PeekChar(n) == '\'')
                {
                    if (Window.PeekChar(n + 1) == '\'')
                    {
                        var piece = Window.GetAndConsumeChars(n);
                        pieces.Add(piece);
                        Window.ConsumeChar();
                        Window.ConsumeChar();
                        pieces.Add("'");
                        n = -1;
                    }
                    else
                    {
                        break;
                    }
                }
                if (IsEolOrEof(Window.PeekChar(n)))
                {
                    throw new ParsingException("Unfinished string literal.");
                }
                n++;
            }

            var lastPiece = Window.GetAndConsumeChars(n);
            pieces.Add(lastPiece);
            var total = string.Join("", pieces);
            Window.ConsumeChar();
            return PureTokenFactory.CreateStringLiteral(total);
        }

        private PureToken ContinueParsingDoubleQuotedStringLiteral()
        {
            Window.ConsumeChar();
            var n = 0;
            while (Window.PeekChar(n) != '"')
            {
                n++;
            }

            var literal = Window.GetAndConsumeChars(n);
            Window.ConsumeChar();
            return PureTokenFactory.CreateDoubleQuotedStringLiteral(literal);
        }

        private PureToken LexTokenWithoutTrivia(List<Trivia> leadingTrivia)
        {
            var character = Window.PeekChar();
            switch (character)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    return ContinueParsingIdentifier();
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    var possiblyNumberToken = ContinueParsingNumber();
                    if (possiblyNumberToken == null)
                    {
                        throw new ParsingException($"Unexpected character \"{Window.PeekChar()}\" while parsing a number");
                    }

                    return (PureToken)possiblyNumberToken;
                case '=':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        return PureTokenFactory.CreatePunctuation(TokenKind.Equality);
                    }

                    return PureTokenFactory.CreatePunctuation(TokenKind.Assignment);
                case '.':
                    if (IsDigit(Window.PeekChar(1)))
                    {
                        var possiblyNumberToken2 = ContinueParsingNumber();
                        if (possiblyNumberToken2 == null)
                        {
                            throw new ParsingException($"Unexpected character \"{Window.PeekChar()}\" while parsing a number");
                        }
                        return (PureToken)possiblyNumberToken2;
                    }
                    Window.ConsumeChar();
                    var c = Window.PeekChar();
                    switch (c)
                    {
                        case '*':
                            Window.ConsumeChar();
                            return PureTokenFactory.CreatePunctuation(TokenKind.DotMultiply);
                        case '/':
                            Window.ConsumeChar();
                            return PureTokenFactory.CreatePunctuation(TokenKind.DotDivide);
                        case '^':
                            Window.ConsumeChar();
                            return PureTokenFactory.CreatePunctuation(TokenKind.DotPower);
                        case '\\':
                            Window.ConsumeChar();
                            return PureTokenFactory.CreatePunctuation(TokenKind.DotBackslash);
                        case '\'':
                            Window.ConsumeChar();
                            return PureTokenFactory.CreatePunctuation(TokenKind.DotTranspose);
                        default:
                            return PureTokenFactory.CreatePunctuation(TokenKind.Dot);
                    }
                case '(':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.OpeningBracket);
                case ')':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.ClosingBracket);
                case '[':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.OpeningSquareBracket);
                case ']':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.ClosingSquareBracket);
                case '{':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.OpeningBrace);
                case '}':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.ClosingBrace);
                case ',':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Comma);
                case ';':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Semicolon);
                case '&':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '&')
                    {
                        Window.ConsumeChar();
                        return PureTokenFactory.CreatePunctuation(TokenKind.LogicalAnd);
                    }
                    return PureTokenFactory.CreatePunctuation(TokenKind.BitwiseAnd);
                case '|':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '|')
                    {
                        Window.ConsumeChar();
                        return PureTokenFactory.CreatePunctuation(TokenKind.LogicalOr);
                    }
                    return PureTokenFactory.CreatePunctuation(TokenKind.BitwiseOr);
                case '<':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        return PureTokenFactory.CreatePunctuation(TokenKind.LessOrEqual);
                    }
                    return PureTokenFactory.CreatePunctuation(TokenKind.Less);
                case '>':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        return PureTokenFactory.CreatePunctuation(TokenKind.GreaterOrEqual);
                    }
                    return PureTokenFactory.CreatePunctuation(TokenKind.Greater);
                case '~':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        return PureTokenFactory.CreatePunctuation(TokenKind.Inequality);
                    }
                    return PureTokenFactory.CreatePunctuation(TokenKind.Not);
                case '+':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Plus);
                case '-':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Minus);
                case '*':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Multiply);
                case '/':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Divide);
                case '\\':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Backslash);
                case '^':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Power);
                case '@':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.At);
                case ':':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.Colon);
                case '?':
                    Window.ConsumeChar();
                    return PureTokenFactory.CreatePunctuation(TokenKind.QuestionMark);
                case '\'':
                    if (LastToken != null &&
                        (LastToken.Kind == TokenKind.ClosingBrace
                        || LastToken.Kind == TokenKind.ClosingBracket
                        || LastToken.Kind == TokenKind.ClosingSquareBracket
                        || LastToken.Kind == TokenKind.Identifier))
                    {
                        if (LastToken.TrailingTrivia.Count == 0 && leadingTrivia.Count == 0)
                        {
                            Window.ConsumeChar();
                            return PureTokenFactory.CreatePunctuation(TokenKind.Transpose);
                        }
                    }
                    return ContinueParsingStringLiteral();
                case '"':
                    return ContinueParsingDoubleQuotedStringLiteral();
                case '\0':
                    return PureTokenFactory.CreateEndOfFileToken();
                default:
                    throw new ParsingException(
                        $"Unknown symbol \"{character}\" at {Window.Position}."
                        );
            }
        }

        public Token NextToken()
        {
            var leadingTrivia = LexTrivia(false);
            var token = LexTokenWithoutTrivia(leadingTrivia);
            var trailingTrivia = LexTrivia(true);

            var result = new Token(token, leadingTrivia, trailingTrivia);
            LastToken = result;
            return result;
        }

        public List<Token> ParseAll()
        {
            var result = new List<Token>();
            while (true)
            {
                var token = NextToken();
                if (token == null)
                {
                    throw new ParsingException($"Unexpected character: '{Window.PeekChar()}' at {Window.Position}.");
                }
                result.Add(token);
                if (token.PureToken.Kind == TokenKind.EndOfFile)
                {
                    return result;
                }
            }
        }
    }
}