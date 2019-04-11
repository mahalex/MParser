namespace Parser
{
    public class SyntaxDiagnostic
    {
        public int Position { get; }

        public static SyntaxDiagnostic From(Internal.TokenDiagnostic diagnostic, int Position)
        {
            switch (diagnostic)
            {
                case Internal.MissingTokenDiagnostic missingToken:
                    return new MissingTokenSyntaxDiagnostic(Position, missingToken.Kind);
            }

            throw new System.ArgumentOutOfRangeException(nameof(diagnostic));
        }

        protected SyntaxDiagnostic(int position)
        {
            Position = position;
        }
    }
}