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

        public static IEnumerable<object[]> SingleTokensData()
        {
            return GetTokens().Select(pair => new object[] { pair.kind, pair.text });
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
    }
}
