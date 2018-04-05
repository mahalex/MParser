using System.Linq;
using NUnit.Framework;

namespace Lexer.Tests
{
    public class MLexerShould
    {
        private static MLexer CreateLexer(string text)
        {
            var window = new TextWindowWithNull(text);
            return new MLexer(window, new PureTokenFactory(window));
        }

        [Test]
        public void ParseSequenceOfIdentifiers()
        {
            var sut = CreateLexer("undefined is not\n a function");
            var tokens = sut.ParseAll();
            Assert.AreEqual(6, tokens.Count);
            CollectionAssert.AreEqual(
                new[] {"undefined", "is", "not", "a", "function"},
                tokens.Take(5).Select(token => token.PureToken.LiteralText));
            CollectionAssert.AreEqual(
                new[] { TokenKind.Identifier, TokenKind.UnquotedStringLiteral, TokenKind.UnquotedStringLiteral,
                        TokenKind.Identifier, TokenKind.UnquotedStringLiteral },
                tokens.Take(5).Select(token => token.Kind));
        }
        
        [Test]
        public void ParseIdentifierAndBrackets()
        {
            var sut = CreateLexer("undefined()");
            var tokens = sut.ParseAll();
            Assert.AreEqual(4, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.Identifier,
                    TokenKind.OpeningBracket,
                    TokenKind.ClosingBracket,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }

        [Test]
        public void ParseTransposeSignAfterClosingSquareBracket()
        {
            var sut = CreateLexer("[undefined]'");
            var tokens = sut.ParseAll();
            Assert.AreEqual(5, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.OpeningSquareBracket,
                    TokenKind.Identifier,
                    TokenKind.ClosingSquareBracket, 
                    TokenKind.Transpose,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }

        [Test]
        public void ParseTransposeSignAfterClosingBrace()
        {
            var sut = CreateLexer("{undefined}'");
            var tokens = sut.ParseAll();
            Assert.AreEqual(5, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.OpeningBrace,
                    TokenKind.Identifier,
                    TokenKind.ClosingBrace, 
                    TokenKind.Transpose,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }

        [Test]
        public void ParseTransposeSignAfterClosingBracket()
        {
            var sut = CreateLexer("undefined()'");
            var tokens = sut.ParseAll();
            Assert.AreEqual(5, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.Identifier,
                    TokenKind.OpeningBracket,
                    TokenKind.ClosingBracket, 
                    TokenKind.Transpose,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }

        [Test]
        public void ParseTransposeSignAfterIdentifier()
        {
            var sut = CreateLexer("undefined'");
            var tokens = sut.ParseAll();
            Assert.AreEqual(3, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.Identifier,
                    TokenKind.Transpose,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }
        
        [Test]
        public void ParseTransposeSignAfterDot()
        {
            var sut = CreateLexer("undefined.'");
            var tokens = sut.ParseAll();
            Assert.AreEqual(3, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.Identifier,
                    TokenKind.DotTranspose,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }
        
        [Test]
        public void ParseDotPowerAfterNumber()
        {
            var sut = CreateLexer("26.^[1]");
            var tokens = sut.ParseAll();
            Assert.AreEqual(6, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.NumberLiteral,
                    TokenKind.DotPower,
                    TokenKind.OpeningSquareBracket,
                    TokenKind.NumberLiteral,
                    TokenKind.ClosingSquareBracket,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }

        [Test]
        public void ParseDotInNumberBeforeSemicolon()
        {
            var sut = CreateLexer("42.;");
            var tokens = sut.ParseAll();
            Assert.AreEqual(3, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.NumberLiteral,
                    TokenKind.Semicolon,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }
        
        [Test]
        public void ParseEAfterDotInANumber()
        {
            var sut = CreateLexer("42.e-5");
            var tokens = sut.ParseAll();
            Assert.AreEqual(2, tokens.Count);
            CollectionAssert.AreEqual(
                new[]
                {
                    TokenKind.NumberLiteral,
                    TokenKind.EndOfFile
                },
                tokens.Select(token => token.PureToken.Kind));
        }

        [Test]
        public void ParseEmptyLine()
        {
            var sut = CreateLexer("\n\nfunction shmunction\n\n\n");
            var tokens = sut.ParseAll();
            Assert.AreEqual(3, tokens.Count);
        }

        [Test]
        public void ParseCommentsAfterDotDotDot()
        {
            var sut = CreateLexer("something ... #$@#%*^!@#\n");
            var tokens = sut.ParseAll();
            Assert.AreEqual(2, tokens.Count);
        }

        [TestCase("something ... #$@#%*^!@#\n")]
        [TestCase("undefined is not a function")]
        [TestCase("\n\nfunction shmunction\n\n\n")]
        public void ReconstructTest(string s)
        {
            var sut = CreateLexer(s);
            var tokens = sut.ParseAll();
            var actual = string.Join("", tokens.Select(token => token.FullText));
            Assert.AreEqual(s, actual);
        }

        [Test]
        public void ParseStringLiteral()
        {
            var sut = CreateLexer("'just a string'");
            var tokens = sut.ParseAll();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenKind.StringLiteral, tokens[0].Kind);
            Assert.AreEqual("just a string", tokens[0].PureToken.Value);
        }

        [Test]
        public void ParseStringLiteralWithEscapedQuotes()
        {
            var sut = CreateLexer("'just a ''string'''");
            var tokens = sut.ParseAll();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenKind.StringLiteral, tokens[0].Kind);
            Assert.AreEqual("just a 'string'", tokens[0].PureToken.Value);
        }

        [Test]
        public void ParseNumberStartingWithDot()
        {
            var sut = CreateLexer(".42");
            var tokens = sut.ParseAll();
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual(TokenKind.NumberLiteral, tokens[0].Kind);
        }
    }
}