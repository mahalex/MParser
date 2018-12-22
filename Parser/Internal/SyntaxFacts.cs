using System;
using System.Collections.Generic;

namespace Parser.Internal
{
    public static class SyntaxFacts
    {
        public static readonly HashSet<string> Keywords;

        static SyntaxFacts()
        {
            Keywords = new HashSet<string>
            {
                "for", "if", "function", "while", "case", "try", "catch", "end",
                "switch", "classdef", "elseif", "persistent", "else"
            };
        }
        
        public enum Precedence
        {
            // see https://mathworks.com/help/matlab/matlab_prog/operator-precedence.html
            Expression = 0,
            Assignment,
            LogicalOr,
            LogicalAnd,
            BitwiseOr,
            BitwiseAnd,
            Relational,
            Colon,
            Additive,
            Multiplicative,
            Unary,
            WeirdPower,
            Power
        }

        public static Precedence GetPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.EqualsToken:
                    return Precedence.Assignment;
                case TokenKind.PipePipeToken:
                    return Precedence.LogicalOr;
                case TokenKind.AmpersandAmpersandToken:
                    return Precedence.LogicalAnd;
                case TokenKind.PipeToken:
                    return Precedence.BitwiseOr;
                case TokenKind.AmpersandToken:
                    return Precedence.BitwiseAnd;
                case TokenKind.LessToken:
                case TokenKind.LessOrEqualsToken:
                case TokenKind.GreaterToken:
                case TokenKind.GreaterOrEqualsToken:
                case TokenKind.EqualsEqualsToken:
                case TokenKind.TildeEqualsToken:
                    return Precedence.Relational;
                case TokenKind.ColonToken:
                    return Precedence.Colon;
                case TokenKind.PlusToken:
                case TokenKind.MinusToken:
                    return Precedence.Additive;
                case TokenKind.StarToken:
                case TokenKind.DotStarToken:
                case TokenKind.SlashToken:
                case TokenKind.DotSlashToken:
                case TokenKind.BackslashToken:
                case TokenKind.DotBackslashToken:
                    return Precedence.Multiplicative;
                case TokenKind.TildeToken:
                    return Precedence.Unary;
                case TokenKind.CaretToken:
                case TokenKind.DotCaretToken:
                case TokenKind.ApostropheToken:
                case TokenKind.DotApostropheToken:
                    return Precedence.Power;
                default:
                    return Precedence.Expression;
            }
        }

        public static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsDigitOrDot(char c)
        {
            return c == '.' || (c >= '0' && c <= '9');
        }

        public static bool IsEolOrEof(char c)
        {
            return c == '\n' || c == '\r' || c == '\0';
        }

        public static bool IsEof(char c)
        {
            return c == '\0';
        }

        public static bool IsEol(char c)
        {
            return c == '\n' || c == '\r';
        }

        public static bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }
        
        public static bool IsOpeningToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.OpenBraceToken:
                case TokenKind.OpenParenthesisToken:
                case TokenKind.OpenSquareBracketToken:
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool IsClosingToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.CloseBraceToken:
                case TokenKind.CloseParenthesisToken:
                case TokenKind.CloseSquareBracketToken:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBracket(TokenKind tokenKind)
        {
            return IsOpeningToken(tokenKind) || IsClosingToken(tokenKind);
        }
        
        public static TokenKind? OpeningFromClosing(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.CloseBraceToken:
                    return TokenKind.OpenBraceToken;
                case TokenKind.CloseParenthesisToken:
                    return TokenKind.OpenParenthesisToken;
                case TokenKind.CloseSquareBracketToken:
                    return TokenKind.OpenSquareBracketToken;
                default:
                    return null;
            }
        }

        private static readonly string?[] StringFromKind =
        {
            null, // None = 0,
            null, // BadToken = 1,
            null, // EndOfFile = 2,
            null, // Identifier = 3,
            null, // NumberLiteral = 4,
            null, // StringLiteral = 5,
            null, // DoubleQuotedStringLiteral = 6,
            null, // UnquotedStringLiteral = 7
            null, null, null, null, null, null, null, null, null, null, null, null,
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
            null, // DotDotDot = 56, // This is not used at the moment.
            
            "+", // UnaryPlus = 57,
            "-", // UnaryMinus = 58,
            "~", // UnaryNot = 59,
            "?", // UnaryQuestionMark = 60,
        };
        
        public static string? GetText(TokenKind kind)
        {
            if ((int) kind < (int) TokenKind.File)
            {
                return StringFromKind[(int) kind];
            }

            return null;
        }
        
        public static bool IsUnaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.PlusToken:
                case TokenKind.MinusToken:
                case TokenKind.TildeToken:
                case TokenKind.QuestionToken:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsUnaryTokenKind(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.UnaryPlus:
                case TokenKind.UnaryMinus:
                case TokenKind.UnaryNot:
                case TokenKind.UnaryQuestionMark:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBinaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.EqualsToken:
                case TokenKind.PipePipeToken:
                case TokenKind.AmpersandAmpersandToken:
                case TokenKind.PipeToken:
                case TokenKind.AmpersandToken:
                case TokenKind.LessToken:
                case TokenKind.LessOrEqualsToken:
                case TokenKind.GreaterToken:
                case TokenKind.GreaterOrEqualsToken:
                case TokenKind.EqualsEqualsToken:
                case TokenKind.TildeEqualsToken:
                case TokenKind.ColonToken:
                case TokenKind.PlusToken:
                case TokenKind.MinusToken:
                case TokenKind.StarToken:
                case TokenKind.DotStarToken:
                case TokenKind.SlashToken:
                case TokenKind.DotSlashToken:
                case TokenKind.BackslashToken:
                case TokenKind.DotBackslashToken:
                case TokenKind.TildeToken:
                case TokenKind.CaretToken:
                case TokenKind.DotCaretToken:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsLeftAssociative(TokenKind kind)
        {
            return true; // TODO: really?
        }

        public static TokenKind ConvertToUnaryTokenKind(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.PlusToken:
                    return TokenKind.UnaryPlus;
                case TokenKind.MinusToken:
                    return TokenKind.UnaryMinus;
                case TokenKind.TildeToken:
                    return TokenKind.UnaryNot;
                case TokenKind.QuestionToken:
                    return TokenKind.UnaryQuestionMark;
                default:
                    throw new ArgumentException(nameof(kind));
            }
        }
    }
}