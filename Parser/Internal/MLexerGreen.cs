﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Internal
{
    internal class MLexerGreen : ILexer<(SyntaxToken, Position)>
    {
        private class TokenInfo
        {
            public TokenKind Kind { get; set; }
            public string Text { get; set; }
            public string StringValue { get; set; }
            public double DoubleValue { get; set; }
        }

        private ITextWindow Window { get; }
        private SyntaxToken LastToken { get; set; }
        private int TokensSinceNewLine { get; set; }
        private Stack<TokenKind> TokenStack { get; }

        public DiagnosticsBag Diagnostics { get; } = new DiagnosticsBag();

        public MLexerGreen(ITextWindow window)
        {
            Window = window;
            TokenStack = new Stack<TokenKind>();
        }

        private SyntaxTrivia LexComment()
        {
            if (TokensSinceNewLine == 0 && Window.PeekChar(1) == '{')
            {
                return LexMultilineComment();
            } 
            var n = 1;
            while (!SyntaxFacts.IsEolOrEof(Window.PeekChar(n)))
            {
                n++;
            }

            return TokenFactory.CreateTrivia(TokenKind.Comment, Window.GetAndConsumeChars(n));
        }

        private SyntaxTrivia LexMultilineComment()
        {
            var n = 2;
            var metPercentSign = false;
            var atFirstLine = true;
            while (true)
            {
                var c = Window.PeekChar(n);
                if (c == '\0')
                {
                    Diagnostics.ReportUnexpectedEndOfFile(new TextSpan(Window.Position.Offset, 0));
                    return TokenFactory.CreateTrivia(TokenKind.Comment, Window.GetAndConsumeChars(n));
                }

                if (c == '\n' || (c == '\r' && Window.PeekChar(n + 1) == '\n'))
                {
                    atFirstLine = false;
                }

                if (atFirstLine && !SyntaxFacts.IsWhitespace(c)) // this is a one-line comment
                {
                    while (!SyntaxFacts.IsEolOrEof(Window.PeekChar(n)))
                    {
                        n++;
                    }

                    return TokenFactory.CreateTrivia(TokenKind.Comment, Window.GetAndConsumeChars(n));
                }

                if (metPercentSign && c == '}')
                {
                    return TokenFactory.CreateTrivia(TokenKind.Comment, Window.GetAndConsumeChars(n+1));
                }

                metPercentSign = c == '%';

                n++;
            }
        }

        private List<SyntaxTrivia> LexCommentAfterDotDotDot()
        {
            var n = 0;
            while (!SyntaxFacts.IsEolOrEof(Window.PeekChar(n)))
            {
                n++;
            }

            var comment = TokenFactory.CreateTrivia(TokenKind.Comment, Window.GetAndConsumeChars(n));
            var result = new List<SyntaxTrivia> { comment };
            var character = Window.PeekChar();
            if (character == '\n' || character == '\r')
            {
                Window.ConsumeChar();
                result.Add(TokenFactory.CreateTrivia(TokenKind.Whitespace, character.ToString()));
            }

            return result;
        }

        private List<SyntaxTrivia> LexTrivia(bool isTrailing)
        {
            var triviaList = new List<SyntaxTrivia>();
            var whitespaceCache = new StringBuilder();

            void FlushWhitespaceCache()
            {
                if (whitespaceCache.Length > 0)
                {
                    triviaList.Add(TokenFactory.CreateTrivia(TokenKind.Whitespace, whitespaceCache.ToString()));
                }

                whitespaceCache.Clear();
            }
            
            while (true)
            {
                var character = Window.PeekChar();
                switch (character)
                {
                    case ' ':
                    case '\t':
                        Window.ConsumeChar();
                        whitespaceCache.Append(character);
                        break;
                    case '\r':
                    case '\n':
                        FlushWhitespaceCache();
                        Window.ConsumeChar();
                        triviaList.Add(TokenFactory.CreateTrivia(TokenKind.Newline, character.ToString()));
                        if (isTrailing)
                        {
                            return triviaList;
                        }

                        break;
                    case '%':
                        FlushWhitespaceCache();
                        triviaList.Add(LexComment());
                        break;
                    case '.':
                        if (Window.PeekChar(1) == '.' && Window.PeekChar(2) == '.')
                        {
                            FlushWhitespaceCache();
                            triviaList.AddRange(LexCommentAfterDotDotDot());                       
                        }
                        else
                        {
                            FlushWhitespaceCache();
                            return triviaList;
                        }
                        break;
                    default:
                        FlushWhitespaceCache();
                        return triviaList;
                }
            }
        }

        private static bool IsLetterOrDigitOrUnderscore(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || (c == '_');
        }


        private bool ContinueLexingIdentifier(ref TokenInfo tokenInfo)
        {
            var n = 1;
            while (IsLetterOrDigitOrUnderscore(Window.PeekChar(n)))
            {
                n++;
            }

            var identifier = Window.GetAndConsumeChars(n);
            tokenInfo.Kind = TokenKind.Identifier;
            tokenInfo.Text = identifier;
            return true;
        }
        
        private bool ContinueParsingUnquotedStringLiteral(ref TokenInfo tokenInfo)
        {
            var n = 0;
            while (true)
            {
                var c = Window.PeekChar(n);
                if (c == ' ' || c == '\n' || c == '\0')
                {
                    var literal = Window.GetAndConsumeChars(n);
                    tokenInfo.Kind = TokenKind.UnquotedStringLiteral;
                    tokenInfo.Text = literal;
                    tokenInfo.StringValue = literal;
                    return true;
                }

                n++;
            }
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

        private bool ContinueLexingNumber(ref TokenInfo tokenInfo)
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
                        if (SyntaxFacts.IsDigitOrDot(c))
                        {
                            state = NumberParsingState.DigitsBeforeDot;
                        }
                        else
                        {
                            throw new Exception($"Unexpected symbol '{c}' at the beginning of number literal.");
                        }
                        break;
                    case NumberParsingState.DigitsBeforeDot:
                        if (SyntaxFacts.IsDigit(c))
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
                        if (SyntaxFacts.IsDigit(c))
                        {
                            state = NumberParsingState.DigitsAfterDot;
                        }
                        else if (c == 'e' || c == 'E')
                        {
                            state = NumberParsingState.AfterE;
                        }
                        else if (SyntaxFacts.IsWhitespace(c) || c == ';' || c == ']' || c == ')' || c == '}')
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
                        if (SyntaxFacts.IsDigit(c))
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
                        if (SyntaxFacts.IsDigit(c))
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
                        if (SyntaxFacts.IsDigit(c))
                        {
                            state = NumberParsingState.DigitsAfterE;
                        }
                        else
                        {
                            fail = true;
                        }

                        break;
                    case NumberParsingState.DigitsAfterE:
                        if (SyntaxFacts.IsDigit(c))
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
                    var s = Window.GetAndConsumeChars(n);
                    tokenInfo.Kind = TokenKind.NumberLiteral;
                    tokenInfo.Text = s;
                    return false;
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
                if (Window.PeekChar(n) == 'i' || Window.PeekChar(n) == 'j')
                {
                    n++;
                }
                var s = Window.GetAndConsumeChars(n);

                tokenInfo.Kind = TokenKind.NumberLiteral;
                tokenInfo.Text = s;
                return true;
            }

            return false;
        }

        private bool ContinueLexingGeneralStringLiteral(ref TokenInfo tokenInfo, char quote)
        {
            var status = 0; // no errors
            Window.ConsumeChar();
            var textBuilder = new StringBuilder();
            textBuilder.Append(quote);
            var valueBuilder = new StringBuilder();
            var n = 0;
            while (true)
            {
                if (Window.PeekChar(n) == quote)
                {
                    if (Window.PeekChar(n + 1) == quote)
                    {
                        var piece = Window.GetAndConsumeChars(n);
                        textBuilder.Append(piece);
                        valueBuilder.Append(piece);
                        Window.ConsumeChar();
                        Window.ConsumeChar();
                        textBuilder.Append(quote);
                        textBuilder.Append(quote);
                        valueBuilder.Append(quote);
                        n = -1;
                    }
                    else
                    {
                        break;
                    }
                }
                if (SyntaxFacts.IsEof(Window.PeekChar(n)))
                {
                    status = 1;
                    break;
                }
                if (SyntaxFacts.IsEol(Window.PeekChar(n)))
                {
                    status = 2;
                    break;
                }
                n++;
            }

            var lastPiece = Window.GetAndConsumeChars(n);
            textBuilder.Append(lastPiece);
            valueBuilder.Append(lastPiece);
            switch (status) {
                case 0:
                    Window.ConsumeChar();
                    textBuilder.Append(quote);
                    break;
                case 1:
                    Diagnostics.ReportUnexpectedEndOfFile(new TextSpan(Window.Position.Offset, 1));
                    break;
                case 2:
                    Diagnostics.ReportUnexpectedEOLWhileParsingString(new TextSpan(Window.Position.Offset, 1));
                    break;
                default:
                    throw new Exception($"Unexpected status of parsing string literal: {status}.");
            }

            tokenInfo.Text = textBuilder.ToString();
            tokenInfo.StringValue = valueBuilder.ToString();
            return status == 0;
        }

        private bool ContinueLexingStringLiteral(ref TokenInfo tokenInfo)
        {
            ContinueLexingGeneralStringLiteral(ref tokenInfo, '\'');
            tokenInfo.Kind = TokenKind.StringLiteral;
            return true;
        }

        private bool ContinueLexingDoubleQuotedStringLiteral(ref TokenInfo tokenInfo)
        {
            ContinueLexingGeneralStringLiteral(ref tokenInfo, '"');
            tokenInfo.Kind = TokenKind.StringLiteral;
            return true;
        }

        private bool LexTokenWithoutTrivia(List<SyntaxTrivia> leadingTrivia, ref TokenInfo tokenInfo)
        {
            var character = Window.PeekChar();
            if (character == '\0')
            {
                tokenInfo.Kind = TokenKind.EndOfFile;
                tokenInfo.Text = "";
                return true;
            }
            
            if (TokensSinceNewLine == 1
                && !TokenStack.Any()
                && LastToken.Kind == TokenKind.Identifier
                && LastToken.TrailingTrivia.Any()
                && character != '='
                && character != '('
                && !SyntaxFacts.Keywords.Contains(LastToken.Text))
            {
                return ContinueParsingUnquotedStringLiteral(ref tokenInfo);
            }
            if (LastToken?.Kind == TokenKind.UnquotedStringLiteral
                && !TokenStack.Any()
                && TokensSinceNewLine > 0)
            {
                return ContinueParsingUnquotedStringLiteral(ref tokenInfo);
            }

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
                    return ContinueLexingIdentifier(ref tokenInfo);
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
                    var parsedNumber = ContinueLexingNumber(ref tokenInfo);
                    if (!parsedNumber)
                    {
                        Diagnostics.ReportUnexpectedCharacterWhileParsingNumber(new TextSpan(Window.Position.Offset, 1), Window.PeekChar());
                    }
                    return true;
                case '=':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        tokenInfo.Kind = TokenKind.Equality;
                    }
                    else
                    {
                        tokenInfo.Kind = TokenKind.Assignment;
                    }

                    return true;
                case '.':
                    if (SyntaxFacts.IsDigit(Window.PeekChar(1)))
                    {
                        var possiblyNumberToken2 = ContinueLexingNumber(ref tokenInfo);
                        if (!possiblyNumberToken2)
                        {
                            Diagnostics.ReportUnexpectedCharacterWhileParsingNumber(new TextSpan(Window.Position.Offset, 1), Window.PeekChar());
                        }

                        return true;
                    }
                    Window.ConsumeChar();
                    var c = Window.PeekChar();
                    switch (c)
                    {
                        case '*':
                            Window.ConsumeChar();
                            tokenInfo.Kind = TokenKind.DotMultiply;
                            break;
                        case '/':
                            Window.ConsumeChar();
                            tokenInfo.Kind = TokenKind.DotDivide;
                            break;
                        case '^':
                            Window.ConsumeChar();
                            tokenInfo.Kind = TokenKind.DotPower;
                            break;
                        case '\\':
                            Window.ConsumeChar();
                            tokenInfo.Kind = TokenKind.DotBackslash;
                            break;
                        case '\'':
                            Window.ConsumeChar();
                            tokenInfo.Kind = TokenKind.DotTranspose;
                            break;
                        default:
                            tokenInfo.Kind = TokenKind.Dot;
                            break;
                    }

                    return true;
                case '(':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.OpeningBracket;
                    return true;
                case ')':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.ClosingBracket;
                    return true;
                case '[':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.OpeningSquareBracket;
                    return true;
                case ']':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.ClosingSquareBracket;
                    return true;
                case '{':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.OpeningBrace;
                    return true;
                case '}':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.ClosingBrace;
                    return true;
                case ',':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Comma;
                    return true;
                case ';':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Semicolon;
                    return true;
                case '&':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '&')
                    {
                        Window.ConsumeChar();
                        tokenInfo.Kind = TokenKind.LogicalAnd;
                    }
                    else
                    {
                        tokenInfo.Kind = TokenKind.BitwiseAnd;
                    }

                    return true;
                case '|':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '|')
                    {
                        Window.ConsumeChar();
                        tokenInfo.Kind = TokenKind.LogicalOr;
                    }
                    else
                    {
                        tokenInfo.Kind = TokenKind.BitwiseOr;

                    }

                    return true;
                case '<':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        tokenInfo.Kind = TokenKind.LessOrEqual;
                    }
                    else
                    {
                        tokenInfo.Kind = TokenKind.Less;
                    }

                    return true;
                case '>':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        tokenInfo.Kind = TokenKind.GreaterOrEqual;
                    }
                    else
                    {
                        tokenInfo.Kind = TokenKind.Greater;
                    }

                    return true;
                case '~':
                    Window.ConsumeChar();
                    if (Window.PeekChar() == '=')
                    {
                        Window.ConsumeChar();
                        tokenInfo.Kind = TokenKind.Inequality;
                    }
                    else
                    {
                        tokenInfo.Kind = TokenKind.Not;                        
                    }

                    return true;
                case '+':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Plus;
                    return true;
                case '-':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Minus;
                    return true;
                case '*':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Multiply;
                    return true;
                case '/':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Divide;
                    return true;
                case '\\':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Backslash;
                    return true;
                case '^':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Power;
                    return true;
                case '@':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.At;
                    return true;
                case ':':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.Colon;
                    return true;
                case '?':
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.QuestionMark;
                    return true;
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
                            tokenInfo.Kind = TokenKind.Transpose;
                            return true;
                        }
                    }
                    return ContinueLexingStringLiteral(ref tokenInfo);
                case '"':
                    return ContinueLexingDoubleQuotedStringLiteral(ref tokenInfo);
                case '\0':
                    tokenInfo.Kind = TokenKind.EndOfFile;
                    return true;
                default:
                    Diagnostics.ReportUnknownSymbol(new TextSpan(Window.Position.Offset, 1), character);
                    Window.ConsumeChar();
                    tokenInfo.Kind = TokenKind.BadToken;
                    tokenInfo.Text = character.ToString();
                    return true;
            }
        }

        public (SyntaxToken, Position) NextToken()
        {
            var leadingTrivia = LexTrivia(false);
            var position = Window.Position;
            var tokenInfo = new TokenInfo();
            LexTokenWithoutTrivia(leadingTrivia, ref tokenInfo);
            var trailingTrivia = LexTrivia(true);
            if (trailingTrivia.Any(t => t.Kind == TokenKind.Newline))
            {
                TokensSinceNewLine = 0;
            }
            else
            {
                TokensSinceNewLine++;
            }
            
            if (SyntaxFacts.IsOpeningToken(tokenInfo.Kind))
            {
                TokenStack.Push(tokenInfo.Kind);
            }

            if (SyntaxFacts.IsClosingToken(tokenInfo.Kind))
            {
                if (TokenStack.Count > 0)
                {
                    var t = TokenStack.Peek();
                    if (t == SyntaxFacts.OpeningFromClosing(tokenInfo.Kind))
                    {
                        TokenStack.Pop();
                    }
                    else
                    {
                        throw new ParsingException($"Unmatched \"{tokenInfo.Text}\" at {Window.Position}.");
                    }
                }
                else
                {
                    throw new ParsingException($"Unmatched \"{tokenInfo.Text}\" at {Window.Position}.");
                }
            }

            if (tokenInfo.Kind == TokenKind.EndOfFile
                && TokenStack.Any())
            {
                throw new ParsingException($"Unmatched \"{TokenStack.Pop()}\" by the end of file.");
            }

            var result = Create(
                tokenInfo,
                leadingTrivia,
                trailingTrivia);
            LastToken = result;
            return (result, position);
        }

        private SyntaxToken Create(
            TokenInfo tokenInfo,
            List<SyntaxTrivia> leadingTrivia,
            List<SyntaxTrivia> trailingTrivia)
        {
            switch (tokenInfo.Kind)
            {
                case TokenKind.Identifier:
                    return TokenFactory.CreateIdentifier(
                        tokenInfo.Text,
                        leadingTrivia,
                        trailingTrivia);
                case TokenKind.UnquotedStringLiteral:
                    return TokenFactory.CreateUnquotedStringLiteral(
                        tokenInfo.Text,
                        tokenInfo.StringValue,
                        leadingTrivia,
                        trailingTrivia);
                case TokenKind.NumberLiteral:
                    return TokenFactory.CreateTokenWithValueAndTrivia<double>(
                        tokenInfo.Kind,
                        tokenInfo.Text,
                        tokenInfo.DoubleValue,
                        leadingTrivia,
                        trailingTrivia);
                case TokenKind.StringLiteral:
                    return TokenFactory.CreateTokenWithValueAndTrivia<string>(
                        tokenInfo.Kind,
                        tokenInfo.Text,
                        tokenInfo.StringValue,
                        leadingTrivia,
                        trailingTrivia);
                default:
                    return TokenFactory.CreateTokenWithTrivia(
                        tokenInfo.Kind,
                        leadingTrivia,
                        trailingTrivia);
            }
        }

        public List<(SyntaxToken, Position)> ParseAll()
        {
            var result = new List<(SyntaxToken, Position)>();
            while (true)
            {
                var pair = NextToken();
                var (token, _) = pair;
                if (token == null)
                {
                    throw new ParsingException($"Unexpected character: '{Window.PeekChar()}' at {Window.Position}.");
                }
                result.Add(pair);
                if (token.Kind == TokenKind.EndOfFile)
                {
                    return result;
                }
            }

        }
    }
}