using System.Linq;

namespace Parser.Binding
{
    public class BoundUnaryOperator
    {
        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(TokenKind.MinusToken, BoundUnaryOperatorKind.Minus),
        };

        public BoundUnaryOperator(TokenKind syntaxKind, BoundUnaryOperatorKind kind)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
        }

        public TokenKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }

        internal static BoundUnaryOperator? GetOperator(TokenKind kind)
        {
            return _operators.FirstOrDefault(op => op.SyntaxKind == kind);
        }
    }
}
