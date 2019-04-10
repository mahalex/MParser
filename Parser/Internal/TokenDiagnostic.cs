namespace Parser.Internal
{
    public class TokenDiagnostic
    {
        protected TokenDiagnostic()
        {
        }

        public static TokenDiagnostic MissingToken(TokenKind kind)
        {
            return new MissingTokenDiagnostic(kind);
        }
    }

    public class MissingTokenDiagnostic : TokenDiagnostic
    {
        internal MissingTokenDiagnostic(TokenKind kind)
        {
            Kind = kind;
        }

        public TokenKind Kind { get; }
    }
}
