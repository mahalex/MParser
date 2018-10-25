using Parser.Internal;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Parser.Tests
{
    public class MLexerGreenTests
    {
        private IEnumerable<Internal.SyntaxToken> ParseText(string text)
        {
            var lexer = new MLexerGreen(new TextWindowWithNull(text));
            return lexer.ParseAll().Select(x => x.Item1).Where(x => x.Kind != TokenKind.EndOfFile);
        }

        [Theory]
        [MemberData(nameof(SingleTokensData))]
        public void MLexerGreen_Parses_Token(TokenKind kind, string text)
        {
            var tokens = ParseText(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
        }

        public static IEnumerable<object[]> SingleTokensData()
        {
            return SingleTokens().Select(pair => new object[] { pair.kind, pair.text });
        }

        public static IEnumerable<(TokenKind kind, string text)> SingleTokens()
        {
            return new[]
            {
                (TokenKind.Identifier, "a"),
                (TokenKind.Identifier, "abc"),
                (TokenKind.NumberLiteral, "1"),
                (TokenKind.NumberLiteral, "123"),
                (TokenKind.NumberLiteral, "145.67"),
                (TokenKind.NumberLiteral, "14.5e-3"),
                (TokenKind.NumberLiteral, "3.14e8"),
                (TokenKind.StringLiteral, "'what is that'"),
                (TokenKind.DoubleQuotedStringLiteral, "\"Another ' string\""),

                (TokenKind.Assignment, "="),
                (TokenKind.Equality, "=="),
                (TokenKind.Inequality, "~="),
                (TokenKind.LogicalAnd, "&&"),
                (TokenKind.LogicalOr, "||"),
                (TokenKind.BitwiseAnd, "&"),
                (TokenKind.BitwiseOr, "|"),
                (TokenKind.Less, "<"),
                (TokenKind.LessOrEqual, "<="),
                (TokenKind.Greater, ">"),
                (TokenKind.GreaterOrEqual, ">="),
                (TokenKind.Not, "~"),
                (TokenKind.Plus, "+"),
                (TokenKind.Minus, "-"),
                (TokenKind.Multiply, "*"),
                (TokenKind.Divide, "/"),
                (TokenKind.Power, "^"),
                (TokenKind.Backslash, "\\"),
                (TokenKind.DotMultiply, ".*"),
                (TokenKind.DotDivide, "./"),
                (TokenKind.DotPower, ".^"),
                (TokenKind.DotBackslash, ".\\"),
                (TokenKind.DotTranspose, ".'"),
                (TokenKind.At, "@"),
                (TokenKind.Colon, ":"),
                (TokenKind.QuestionMark, "?"),
                (TokenKind.Comma, ","),
                (TokenKind.Semicolon, ";"),
                //(TokenKind.OpeningBrace, "{"),
                //(TokenKind.ClosingBrace, "}"),
                //(TokenKind.OpeningSquareBracket, "["),
                //(TokenKind.ClosingSquareBracket, "]"),
                //(TokenKind.OpeningBracket, "("),
                //(TokenKind.ClosingBracket, ")"),
                (TokenKind.Dot, "."),
            };
        }
    }
}
