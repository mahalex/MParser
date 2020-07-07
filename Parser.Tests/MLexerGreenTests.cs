using System;
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
            return lexer.ParseAll().Select(x => x.Item1).Where(x => x.Kind != TokenKind.EndOfFileToken);
        }

        [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(SingleTokensData))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
        public void MLexerGreen_Parses_Token(TokenKind kind, string text)
        {
            var tokens = ParseText(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
        }

        [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(PairTokensData))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
        public void MLexerGreen_Parses_PairOfTokens(TokenKind kind1, string text1, TokenKind kind2, string text2)
        {
            var text = text1 + text2;
            var tokens = ParseText(text).ToArray();
            Assert.Equal(2, tokens.Length);
            Assert.Equal(kind1, tokens[0].Kind);
            Assert.Equal(kind2, tokens[1].Kind);
        }

        [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(PairTokensWithSeparatorData))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
        public void MLexerGreen_Parses_PairOfTokensWithSeparator(TokenKind kind1, string text1, string separatorText, TokenKind kind2, string text2)
        {
            var text = text1 + separatorText + text2;
            var tokens = ParseText(text).ToArray();
            if (kind1 == TokenKind.IdentifierToken && !ContainsNewLine(separatorText))
            {
                Assert.Equal(TokenKind.IdentifierToken, tokens[0].Kind);
                foreach (var token in tokens.Skip(1))
                {
                    Assert.Equal(TokenKind.UnquotedStringLiteralToken, token.Kind);
                }
            }
            else
            {
                Assert.Equal(2, tokens.Length);
                Assert.Equal(kind1, tokens[0].Kind);
                Assert.Equal(kind2, tokens[1].Kind);
            }
        }

        private static bool ContainsNewLine(string text)
        {
            return text.Contains('\r') || text.Contains('\n');
        }

        public static IEnumerable<object[]> SingleTokensData()
        {
            return GetTokens().Select(pair => new object[] { pair.kind, pair.text });
        }

        public static IEnumerable<object[]> PairTokensData()
        {
            return GetPairsOfTokens().Select(data => new object[] {data.kind1, data.text1, data.kind2, data.text2});
        }

        public static IEnumerable<object[]> PairTokensWithSeparatorData()
        {
            return GetPairsOfTokensWithSeparators().Select(data => new object[] {data.kind1, data.text1, data.separatorText, data.kind2, data.text2});
        }

        public static IEnumerable<(TokenKind kind, string text)> GetTokens()
        {
            var fixedTokens =
                from TokenKind kind in Enum.GetValues(typeof(TokenKind))
                let text = SyntaxFacts.GetText(kind)
                where !(text is null)
                where !(SyntaxFacts.IsUnaryTokenKind(kind)
                        || kind == TokenKind.ApostropheToken)
                select (kind, text);

        var dynamicTokens = new[]
            {
                (TokenKind.IdentifierToken, "a"),
                (TokenKind.IdentifierToken, "abc"),
                (TokenKind.NumberLiteralToken, "1"),
                (TokenKind.NumberLiteralToken, "123"),
                (TokenKind.NumberLiteralToken, "145.67"),
                (TokenKind.NumberLiteralToken, "14.5e-3"),
                (TokenKind.NumberLiteralToken, "3.14e8"),
                (TokenKind.StringLiteralToken, "'what is that'"),
                (TokenKind.DoubleQuotedStringLiteralToken, "\"Another ' string\"")
            };
            return fixedTokens.Concat(dynamicTokens);
        }

        public static IEnumerable<(TokenKind kind1, string text1, TokenKind kind2, string text2)> GetPairsOfTokens()
        {
            return
                from token1 in GetTokens()
                from token2 in GetTokens()
                where !RequiresSeparator(token1.kind, token2.kind)
                select (token1.kind, token1.text, token2.kind, token2.text);
        }
        
        public static IEnumerable<(TokenKind kind1, string text1, string separatorText, TokenKind kind2, string text2)> GetPairsOfTokensWithSeparators()
        {
            return
                from token1 in GetTokens()
                from token2 in GetTokens()
                where RequiresSeparator(token1.kind, token2.kind)
                from separatorText in GetSeparators()
                select (token1.kind, token1.text, separatorText, token2.kind, token2.text);
        }

        private static bool RequiresSeparator(TokenKind kind1, TokenKind kind2)
        {
            if (kind1 == TokenKind.LessToken && kind2 == TokenKind.EqualsEqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.LessToken && kind2 == TokenKind.EqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.GreaterToken && kind2 == TokenKind.EqualsEqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.GreaterToken && kind2 == TokenKind.EqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.TildeToken && kind2 == TokenKind.EqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.TildeToken && kind2 == TokenKind.EqualsEqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.EqualsToken && kind2 == TokenKind.EqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.EqualsToken && kind2 == TokenKind.EqualsEqualsToken)
            {
                return true;
            }

            if (kind1 == TokenKind.AmpersandToken && kind2 == TokenKind.AmpersandAmpersandToken)
            {
                return true;
            }

            if (kind1 == TokenKind.AmpersandToken && kind2 == TokenKind.AmpersandToken)
            {
                return true;
            }

            if (kind1 == TokenKind.PipeToken && kind2 == TokenKind.PipePipeToken)
            {
                return true;
            }

            if (kind1 == TokenKind.PipeToken && kind2 == TokenKind.PipeToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DotToken && kind2 == TokenKind.StarToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DotToken && kind2 == TokenKind.SlashToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DotToken && kind2 == TokenKind.BackslashToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DotToken && kind2 == TokenKind.CaretToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DotToken && kind2 == TokenKind.NumberLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.NumberLiteralToken && kind2 == TokenKind.DotToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DotToken && kind2 == TokenKind.StringLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.NumberLiteralToken && kind2 == TokenKind.NumberLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.StringLiteralToken && kind2 == TokenKind.StringLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.DoubleQuotedStringLiteralToken && kind2 == TokenKind.DoubleQuotedStringLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.IdentifierToken && kind2 == TokenKind.IdentifierToken)
            {
                return true;
            }

            if (kind1 == TokenKind.IdentifierToken && kind2 == TokenKind.NumberLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.IdentifierToken && kind2 == TokenKind.StringLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.CloseBraceToken && kind2 == TokenKind.StringLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.CloseParenthesisToken && kind2 == TokenKind.StringLiteralToken)
            {
                return true;
            }

            if (kind1 == TokenKind.CloseSquareBracketToken && kind2 == TokenKind.StringLiteralToken)
            {
                return true;
            }
            
            return false;
        }
        
        private static IEnumerable<string> GetSeparators()
        {
            return new[]
            {
                " ",
                "   ",
                "\r",
                "\n",
                "\r\n"
            };
        }
    }
}
