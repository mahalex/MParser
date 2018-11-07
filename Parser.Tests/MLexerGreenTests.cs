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

        [Theory]
        [MemberData(nameof(PairTokensData))]
        public void MLexerGreen_Parses_PairOfTokens(TokenKind kind1, string text1, TokenKind kind2, string text2)
        {
            var text = text1 + text2;
            var tokens = ParseText(text).ToArray();
            Assert.Equal(2, tokens.Length);
            Assert.Equal(kind1, tokens[0].Kind);
            Assert.Equal(kind2, tokens[1].Kind);
        }

        [Theory]
        [MemberData(nameof(PairTokensWithSeparatorData))]
        public void MLexerGreen_Parses_PairOfTokensWithSeparator(TokenKind kind1, string text1, string separatorText, TokenKind kind2, string text2)
        {
            var text = text1 + separatorText + text2;
            var tokens = ParseText(text).ToArray();
            if (kind1 == TokenKind.Identifier && !ContainsNewLine(separatorText))
            {
                Assert.Equal(TokenKind.Identifier, tokens[0].Kind);
                foreach (var token in tokens.Skip(1))
                {
                    Assert.Equal(TokenKind.UnquotedStringLiteral, token.Kind);
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
            var fixedTokens = Enum.GetValues(typeof(TokenKind))
                .Cast<TokenKind>()
                .Select(k => (kind: k, text: SyntaxFacts.GetText(k)))
                .Where(t => !(t.text is null))
                .Where(t => !(SyntaxFacts.IsUnaryTokenKind(t.kind)
                              || SyntaxFacts.IsOpeningToken(t.kind)
                              || SyntaxFacts.IsClosingToken(t.kind)
                              || t.kind == TokenKind.Transpose));
            
            
            var dynamicTokens = new[]
            {
                (TokenKind.Identifier, "a"),
                (TokenKind.Identifier, "abc"),
                (TokenKind.NumberLiteral, "1"),
                (TokenKind.NumberLiteral, "123"),
                (TokenKind.NumberLiteral, "145.67"),
                (TokenKind.NumberLiteral, "14.5e-3"),
                (TokenKind.NumberLiteral, "3.14e8"),
                (TokenKind.StringLiteral, "'what is that'"),
                (TokenKind.DoubleQuotedStringLiteral, "\"Another ' string\"")
            };
            return fixedTokens.Concat(dynamicTokens);
        }

        public static IEnumerable<(TokenKind kind1, string text1, TokenKind kind2, string text2)> GetPairsOfTokens()
        {
            foreach (var token1 in GetTokens())
            {
                foreach (var token2 in GetTokens())
                {
                    if (!RequiresSeparator(token1.kind, token2.kind))
                    {
                        yield return (token1.kind, token1.text, token2.kind, token2.text);
                    }
                }
            }
        }
        
        public static IEnumerable<(TokenKind kind1, string text1, string separatorText, TokenKind kind2, string text2)> GetPairsOfTokensWithSeparators()
        {
            foreach (var token1 in GetTokens())
            {
                foreach (var token2 in GetTokens())
                {
                    if (RequiresSeparator(token1.kind, token2.kind))
                    {
                        foreach (var separatorText in GetSeparators())
                        {
                            yield return (token1.kind, token1.text, separatorText, token2.kind, token2.text);
                        }
                    }
                }
            }
        }

        private static bool RequiresSeparator(TokenKind kind1, TokenKind kind2)
        {
            if (kind1 == TokenKind.Less && kind2 == TokenKind.Equality)
            {
                return true;
            }

            if (kind1 == TokenKind.Less && kind2 == TokenKind.Assignment)
            {
                return true;
            }

            if (kind1 == TokenKind.Greater && kind2 == TokenKind.Equality)
            {
                return true;
            }

            if (kind1 == TokenKind.Greater && kind2 == TokenKind.Assignment)
            {
                return true;
            }

            if (kind1 == TokenKind.Not && kind2 == TokenKind.Assignment)
            {
                return true;
            }

            if (kind1 == TokenKind.Not && kind2 == TokenKind.Equality)
            {
                return true;
            }

            if (kind1 == TokenKind.Assignment && kind2 == TokenKind.Assignment)
            {
                return true;
            }

            if (kind1 == TokenKind.Assignment && kind2 == TokenKind.Equality)
            {
                return true;
            }

            if (kind1 == TokenKind.BitwiseAnd && kind2 == TokenKind.LogicalAnd)
            {
                return true;
            }

            if (kind1 == TokenKind.BitwiseAnd && kind2 == TokenKind.BitwiseAnd)
            {
                return true;
            }

            if (kind1 == TokenKind.BitwiseOr && kind2 == TokenKind.LogicalOr)
            {
                return true;
            }

            if (kind1 == TokenKind.BitwiseOr && kind2 == TokenKind.BitwiseOr)
            {
                return true;
            }

            if (kind1 == TokenKind.Dot && kind2 == TokenKind.Multiply)
            {
                return true;
            }

            if (kind1 == TokenKind.Dot && kind2 == TokenKind.Divide)
            {
                return true;
            }

            if (kind1 == TokenKind.Dot && kind2 == TokenKind.Backslash)
            {
                return true;
            }

            if (kind1 == TokenKind.Dot && kind2 == TokenKind.Power)
            {
                return true;
            }

            if (kind1 == TokenKind.Dot && kind2 == TokenKind.NumberLiteral)
            {
                return true;
            }

            if (kind1 == TokenKind.NumberLiteral && kind2 == TokenKind.Dot)
            {
                return true;
            }

            if (kind1 == TokenKind.Dot && kind2 == TokenKind.StringLiteral)
            {
                return true;
            }

            if (kind1 == TokenKind.NumberLiteral && kind2 == TokenKind.NumberLiteral)
            {
                return true;
            }

            if (kind1 == TokenKind.StringLiteral && kind2 == TokenKind.StringLiteral)
            {
                return true;
            }

            if (kind1 == TokenKind.DoubleQuotedStringLiteral && kind2 == TokenKind.DoubleQuotedStringLiteral)
            {
                return true;
            }

            if (kind1 == TokenKind.Identifier && kind2 == TokenKind.Identifier)
            {
                return true;
            }

            if (kind1 == TokenKind.Identifier && kind2 == TokenKind.NumberLiteral)
            {
                return true;
            }

            if (kind1 == TokenKind.Identifier && kind2 == TokenKind.StringLiteral)
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
