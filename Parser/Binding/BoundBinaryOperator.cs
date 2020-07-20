using System.Linq;

namespace Parser.Binding
{
    public class BoundBinaryOperator
    {
        private static BoundBinaryOperator[] _specificOperators =
        {
            new BoundBinaryOperator(
                TokenKind.LessToken,
                BoundBinaryOperatorKind.Less,
                TypeSymbol.Int,
                TypeSymbol.Boolean),
            new BoundBinaryOperator(
                TokenKind.PlusToken,
                BoundBinaryOperatorKind.Plus,
                TypeSymbol.Int),
        };

        private static BoundBinaryOperator[] _defaultOperators =
        {
            new BoundBinaryOperator(
                TokenKind.EqualsToken,
                BoundBinaryOperatorKind.Equals,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.PipePipeToken,
                BoundBinaryOperatorKind.PipePipe,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.AmpersandAmpersandToken,
                BoundBinaryOperatorKind.AmpersandAmpersand,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.PipeToken,
                BoundBinaryOperatorKind.Pipe,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.AmpersandToken,
                BoundBinaryOperatorKind.Ampersand,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.LessToken,
                BoundBinaryOperatorKind.Less,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.LessOrEqualsToken,
                BoundBinaryOperatorKind.LessOrEquals,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.GreaterToken,
                BoundBinaryOperatorKind.Greater,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.GreaterOrEqualsToken,
                BoundBinaryOperatorKind.GreaterOrEquals,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.EqualsEqualsToken,
                BoundBinaryOperatorKind.EqualsEquals,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.TildeEqualsToken,
                BoundBinaryOperatorKind.TildeEquals,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.ColonToken,
                BoundBinaryOperatorKind.Colon,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.PlusToken,
                BoundBinaryOperatorKind.Plus,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.MinusToken,
                BoundBinaryOperatorKind.Minus,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.StarToken,
                BoundBinaryOperatorKind.Star,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.DotStarToken,
                BoundBinaryOperatorKind.DotStar,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.SlashToken,
                BoundBinaryOperatorKind.Slash,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.DotSlashToken,
                BoundBinaryOperatorKind.DotSlash,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.BackslashToken,
                BoundBinaryOperatorKind.Backslash,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.DotBackslashToken,
                BoundBinaryOperatorKind.DotBackslash,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.TildeToken,
                BoundBinaryOperatorKind.Tilde,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.CaretToken,
                BoundBinaryOperatorKind.Caret,
                TypeSymbol.MObject),
            new BoundBinaryOperator(
                TokenKind.DotCaretToken,
                BoundBinaryOperatorKind.DotCaret,
                TypeSymbol.MObject),
        };

        public BoundBinaryOperator(
            TokenKind syntaxKind,
            BoundBinaryOperatorKind kind,
            TypeSymbol type)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            Left = type;
            Right = type;
            Result = type;
        }

        public BoundBinaryOperator(
            TokenKind syntaxKind,
            BoundBinaryOperatorKind kind,
            TypeSymbol operand,
            TypeSymbol result)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            Left = operand;
            Right = operand;
            Result = result;
        }

        public BoundBinaryOperator(
            TokenKind syntaxKind,
            BoundBinaryOperatorKind kind,
            TypeSymbol left,
            TypeSymbol right,
            TypeSymbol result)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            Left = left;
            Right = right;
            Result = result;
        }

        public TokenKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public TypeSymbol Left { get; }
        public TypeSymbol Right { get; }
        public TypeSymbol Result { get; }

        internal static BoundBinaryOperator? GetOperator(TokenKind kind, TypeSymbol left, TypeSymbol right)
        {
            return _specificOperators.FirstOrDefault(op => op.SyntaxKind == kind && op.Left == left && op.Right == right)
                ?? _defaultOperators.FirstOrDefault(op => op.SyntaxKind == kind);
        }
    }
}
