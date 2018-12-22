using System.Collections.Generic;

namespace Parser.Internal
{
    internal static class TokenFactory
    {
        public static SyntaxTrivia CreateTrivia(TokenKind kind, string text)
        {
            return new SyntaxTrivia(kind, text);
        }

        public static SyntaxToken CreateTokenWithTrivia(
            TokenKind kind,
            IReadOnlyList<SyntaxTrivia> leadingTrivia,
            IReadOnlyList<SyntaxTrivia> trailingTrivia)
        {
            return new SyntaxToken.SyntaxTokenWithTrivia(kind, leadingTrivia, trailingTrivia);
        }

        public static SyntaxToken CreateIdentifier(
            string text,
            IReadOnlyList<SyntaxTrivia> leadingTrivia,
            IReadOnlyList<SyntaxTrivia> trailingTrivia)
        {
            return new SyntaxToken.SyntaxIdentifierWithTrivia(text, leadingTrivia, trailingTrivia);
        }

        public static SyntaxToken CreateTokenWithValueAndTrivia<T>(
            TokenKind kind,
            string text,
            T value,
            IReadOnlyList<SyntaxTrivia> leadingTrivia,
            IReadOnlyList<SyntaxTrivia> trailingTrivia)
        {
            return new SyntaxToken.SyntaxTokenWithValueAndTrivia<T>(kind, text, value, leadingTrivia, trailingTrivia);
        }

        public static SyntaxToken CreateUnquotedStringLiteral(
            string text,
            string value,
            IReadOnlyList<SyntaxTrivia> leadingTrivia,
            IReadOnlyList<SyntaxTrivia> trailingTrivia)
        {
            return new SyntaxToken.SyntaxTokenWithValueAndTrivia<string>(
                TokenKind.UnquotedStringLiteralToken,
                text,
                value,
                leadingTrivia,
                trailingTrivia);
        }

        public static SyntaxToken CreateMissing(
            TokenKind kind,
            IReadOnlyList<SyntaxTrivia>? leadingTrivia,
            IReadOnlyList<SyntaxTrivia>? trailingTrivia)
        {
            return new SyntaxToken.MissingTokenWithTrivia(kind, leadingTrivia ?? SyntaxToken.s_EmptySyntaxTriviaList, trailingTrivia ?? SyntaxToken.s_EmptySyntaxTriviaList);
        }
    }
}