namespace Parser
{
    public class MissingTokenSyntaxDiagnostic : SyntaxDiagnostic
    {
        public TokenKind Kind { get; }

        public MissingTokenSyntaxDiagnostic(int position, TokenKind tokenKind)
            : base(position)
        {
            Kind = tokenKind;
        }
    }
}