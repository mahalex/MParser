using System;
using System.Linq;

namespace Parser.Binding
{
    public class BoundUnaryOperator
    {
        private static BoundUnaryOperator[] _specificOperators = Array.Empty<BoundUnaryOperator>();

        private static BoundUnaryOperator[] _defaultOperators =
        {
            new BoundUnaryOperator(TokenKind.MinusToken, BoundUnaryOperatorKind.Minus, TypeSymbol.MObject),
        };

        public BoundUnaryOperator(TokenKind syntaxKind, BoundUnaryOperatorKind kind, TypeSymbol type)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            Operand = type;
            Result = type;
        }

        public BoundUnaryOperator(TokenKind syntaxKind, BoundUnaryOperatorKind kind, TypeSymbol operand, TypeSymbol result)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            Operand = operand;
            Result = result;
        }

        public TokenKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public TypeSymbol Operand { get; }
        public TypeSymbol Result { get; }

        internal static BoundUnaryOperator? GetOperator(TokenKind kind, TypeSymbol operand)
        {
            return _specificOperators.FirstOrDefault(op => op.SyntaxKind == kind && op.Operand == operand)
                ?? _defaultOperators.FirstOrDefault(op => op.SyntaxKind == kind);
        }
    }
}
