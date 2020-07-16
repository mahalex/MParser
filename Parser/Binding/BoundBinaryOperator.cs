using System.Linq;

namespace Parser.Binding
{
    public class BoundBinaryOperator
    {
        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(TokenKind.EqualsToken, BoundBinaryOperatorKind.Equals),
            new BoundBinaryOperator(TokenKind.PipePipeToken, BoundBinaryOperatorKind.PipePipe),
            new BoundBinaryOperator(TokenKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.AmpersandAmpersand),
            new BoundBinaryOperator(TokenKind.PipeToken, BoundBinaryOperatorKind.Pipe),
            new BoundBinaryOperator(TokenKind.AmpersandToken, BoundBinaryOperatorKind.Ampersand),
            new BoundBinaryOperator(TokenKind.LessToken, BoundBinaryOperatorKind.Less),
            new BoundBinaryOperator(TokenKind.LessOrEqualsToken, BoundBinaryOperatorKind.LessOrEquals),
            new BoundBinaryOperator(TokenKind.GreaterToken, BoundBinaryOperatorKind.Greater),
            new BoundBinaryOperator(TokenKind.GreaterOrEqualsToken, BoundBinaryOperatorKind.GreaterOrEquals),
            new BoundBinaryOperator(TokenKind.EqualsEqualsToken, BoundBinaryOperatorKind.EqualsEquals),
            new BoundBinaryOperator(TokenKind.TildeEqualsToken, BoundBinaryOperatorKind.TildeEquals),
            new BoundBinaryOperator(TokenKind.ColonToken, BoundBinaryOperatorKind.Colon),
            new BoundBinaryOperator(TokenKind.PlusToken, BoundBinaryOperatorKind.Plus),
            new BoundBinaryOperator(TokenKind.MinusToken, BoundBinaryOperatorKind.Minus),
            new BoundBinaryOperator(TokenKind.StarToken, BoundBinaryOperatorKind.Star),
            new BoundBinaryOperator(TokenKind.DotStarToken, BoundBinaryOperatorKind.DotStar),
            new BoundBinaryOperator(TokenKind.SlashToken, BoundBinaryOperatorKind.Slash),
            new BoundBinaryOperator(TokenKind.DotSlashToken, BoundBinaryOperatorKind.DotSlash),
            new BoundBinaryOperator(TokenKind.BackslashToken, BoundBinaryOperatorKind.Backslash),
            new BoundBinaryOperator(TokenKind.DotBackslashToken, BoundBinaryOperatorKind.DotBackslash),
            new BoundBinaryOperator(TokenKind.TildeToken, BoundBinaryOperatorKind.Tilde),
            new BoundBinaryOperator(TokenKind.CaretToken, BoundBinaryOperatorKind.Caret),
            new BoundBinaryOperator(TokenKind.DotCaretToken, BoundBinaryOperatorKind.DotCaret),
        };

        public BoundBinaryOperator(TokenKind syntaxKind, BoundBinaryOperatorKind kind)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
        }

        public TokenKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }

        internal static BoundBinaryOperator? GetOperator(TokenKind kind)
        {
            return _operators.FirstOrDefault(op => op.SyntaxKind == kind);
        }
    }
}
