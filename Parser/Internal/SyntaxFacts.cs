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
                case TokenKind.Assignment:
                    return Precedence.Assignment;
                case TokenKind.LogicalOr:
                    return Precedence.LogicalOr;
                case TokenKind.LogicalAnd:
                    return Precedence.LogicalAnd;
                case TokenKind.BitwiseOr:
                    return Precedence.BitwiseOr;
                case TokenKind.BitwiseAnd:
                    return Precedence.BitwiseAnd;
                case TokenKind.Less:
                case TokenKind.LessOrEqual:
                case TokenKind.Greater:
                case TokenKind.GreaterOrEqual:
                case TokenKind.Equality:
                case TokenKind.Inequality:
                    return Precedence.Relational;
                case TokenKind.Colon:
                    return Precedence.Colon;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return Precedence.Additive;
                case TokenKind.Multiply:
                case TokenKind.DotMultiply:
                case TokenKind.Divide:
                case TokenKind.DotDivide:
                case TokenKind.Backslash:
                case TokenKind.DotBackslash:
                    return Precedence.Multiplicative;
                case TokenKind.Not:
                    return Precedence.Unary;
                case TokenKind.Power:
                case TokenKind.DotPower:
                case TokenKind.Transpose:
                case TokenKind.DotTranspose:
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

        public static bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }
        
        public static bool IsOpeningToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.OpeningBrace:
                case TokenKind.OpeningBracket:
                case TokenKind.OpeningSquareBracket:
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool IsClosingToken(TokenKind tokenKind)
        {
            switch (tokenKind)
            {
                case TokenKind.ClosingBrace:
                case TokenKind.ClosingBracket:
                case TokenKind.ClosingSquareBracket:
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
                case TokenKind.ClosingBrace:
                    return TokenKind.OpeningBrace;
                case TokenKind.ClosingBracket:
                    return TokenKind.OpeningBracket;
                case TokenKind.ClosingSquareBracket:
                    return TokenKind.OpeningSquareBracket;
                default:
                    return null;
            }
        }

        private static readonly string[] StringFromKind =
        {
            null, // None = 0,
            "", // EndOfFile = 1,
            null, // Identifier = 2,
            null, // NumberLiteral = 3,
            null, // StringLiteral = 4,
            null, // DoubleQuotedStringLiteral = 5,
            null, // UnquotedStringLiteral = 6
            null, null, null, null, null, null, null, null, null, null, null, null, null,
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
            "?", // UnaryQuestionMark = 60,
        };
        
        public static string GetText(TokenKind kind)
        {
            return StringFromKind[(int) kind];
        }
        
        public static bool IsUnaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Not:
                case TokenKind.QuestionMark:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBinaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Assignment:
                case TokenKind.LogicalOr:
                case TokenKind.LogicalAnd:
                case TokenKind.BitwiseOr:
                case TokenKind.BitwiseAnd:
                case TokenKind.Less:
                case TokenKind.LessOrEqual:
                case TokenKind.Greater:
                case TokenKind.GreaterOrEqual:
                case TokenKind.Equality:
                case TokenKind.Inequality:
                case TokenKind.Colon:
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Multiply:
                case TokenKind.DotMultiply:
                case TokenKind.Divide:
                case TokenKind.DotDivide:
                case TokenKind.Backslash:
                case TokenKind.DotBackslash:
                case TokenKind.Not:
                case TokenKind.Power:
                case TokenKind.DotPower:
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
                case TokenKind.Plus:
                    return TokenKind.UnaryPlus;
                case TokenKind.Minus:
                    return TokenKind.UnaryMinus;
                case TokenKind.Not:
                    return TokenKind.UnaryNot;
                case TokenKind.QuestionMark:
                    return TokenKind.UnaryQuestionMark;
                default:
                    throw new ArgumentException(nameof(kind));
            }
        }
    }
}