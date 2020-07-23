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
            var leading = leadingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(leadingTrivia) : null;
            var trailing = trailingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(trailingTrivia) : null;
            return new SyntaxToken.SyntaxTokenWithTrivia(kind, leading, trailing);
        }

        public static SyntaxToken CreateTokenWithTrivia(
            TokenKind kind,
            GreenNode? leadingTrivia,
            GreenNode? trailingTrivia)
        {
            return new SyntaxToken.SyntaxTokenWithTrivia(kind, leadingTrivia, trailingTrivia);
        }

        public static SyntaxToken CreateIdentifier(
            string text,
            IReadOnlyList<SyntaxTrivia> leadingTrivia,
            IReadOnlyList<SyntaxTrivia> trailingTrivia)
        {
            var leading = leadingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(leadingTrivia) : null;
            var trailing = trailingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(trailingTrivia) : null;
            return new SyntaxToken.SyntaxIdentifierWithTrivia(text, leading, trailing);
        }

        public static SyntaxToken CreateIdentifier(
            string text,
            GreenNode? leadingTrivia,
            GreenNode? trailingTrivia)
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
            var leading = leadingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(leadingTrivia) : null;
            var trailing = trailingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(trailingTrivia) : null;
            return new SyntaxToken.SyntaxTokenWithValueAndTrivia<T>(kind, text, value, leading, trailing);
        }

        public static SyntaxToken CreateTokenWithValueAndTrivia<T>(
            TokenKind kind,
            string text,
            T value,
            GreenNode? leadingTrivia,
            GreenNode? trailingTrivia)
        {
            return new SyntaxToken.SyntaxTokenWithValueAndTrivia<T>(kind, text, value, leadingTrivia, trailingTrivia);
        }

        public static SyntaxToken CreateUnquotedStringLiteral(
            string text,
            string value,
            IReadOnlyList<SyntaxTrivia> leadingTrivia,
            IReadOnlyList<SyntaxTrivia> trailingTrivia)
        {
            var leading = leadingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(leadingTrivia) : null;
            var trailing = trailingTrivia.Count > 0 ? SyntaxList<SyntaxTrivia>.List(trailingTrivia) : null;
            return new SyntaxToken.SyntaxTokenWithValueAndTrivia<string>(
                TokenKind.UnquotedStringLiteralToken,
                text,
                value,
                leading,
                trailing);
        }

        public static SyntaxToken CreateUnquotedStringLiteral(
            string text,
            string value,
            GreenNode? leadingTrivia,
            GreenNode? trailingTrivia)
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
            var leading = (leadingTrivia is { } l && l.Count > 0) ? SyntaxList<SyntaxTrivia>.List(leadingTrivia) : null;
            var trailing = (trailingTrivia is { } c && c.Count > 0) ? SyntaxList<SyntaxTrivia>.List(trailingTrivia) : null;
            return new SyntaxToken.MissingTokenWithTrivia(kind, leading, trailing);
        }
    }
}