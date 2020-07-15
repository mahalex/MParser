using System;
using System.Collections.Immutable;

namespace Parser.Binding
{
    public static class BoundNodeFactory
    {
        public static BoundRoot Root(SyntaxNode syntax, BoundFile file)
        {
            return new BoundRoot(syntax, file);
        }

        public static BoundFile File(SyntaxNode syntax, BoundStatement body)
        {
            return new BoundFile(syntax, body);
        }

        public static BoundBlockStatement Block(SyntaxNode syntax, params BoundStatement[] statements)
        {
            return new BoundBlockStatement(syntax, statements.ToImmutableArray());
        }

        public static BoundBlockStatement Block(SyntaxNode syntax, ImmutableArray<BoundStatement> statements)
        {
            return new BoundBlockStatement(syntax, statements);
        }

        public static BoundExpressionStatement ExpressionStatement(SyntaxNode syntax, BoundExpression expression)
        {
            return new BoundExpressionStatement(syntax, expression);
        }

        public static BoundIfStatement IfStatement(
            SyntaxNode syntax,
            BoundExpression condition,
            BoundStatement body,
            ImmutableArray<BoundElseifClause> elseifClauses,
            BoundStatement? elseClause)
        {
            return new BoundIfStatement(syntax, condition, body, elseifClauses, elseClause);
        }

        public static BoundLabelStatement LabelStatement(
            SyntaxNode syntax,
            BoundLabel label)
        {
            return new BoundLabelStatement(syntax, label);
        }

        public static BoundAssignmentExpression Assignment(
            SyntaxNode syntax,
            BoundExpression left,
            BoundExpression right)
        {
            return new BoundAssignmentExpression(syntax, left, right);
        }

        public static BoundBinaryOperationExpression BinaryOperation(
            SyntaxNode syntax,
            BoundExpression left,
            TokenKind kind,
            BoundExpression right)
        {
            var op = BindBinaryOperator(kind);
            return new BoundBinaryOperationExpression(syntax, left, op, right);
        }

        public static BoundConditionalGotoStatement ConditionalGoto(
            SyntaxNode syntax,
            BoundExpression condition,
            BoundLabel label,
            bool gotoIfTrue)
        {
            return new BoundConditionalGotoStatement(
                syntax,
                condition,
                label,
                gotoIfTrue);
        }

        public static BoundConditionalGotoStatement GotoIfTrue(
            SyntaxNode syntax,
            BoundExpression condition,
            BoundLabel label)
        {
            return new BoundConditionalGotoStatement(
                syntax,
                condition,
                label,
                gotoIfTrue: true);
        }

        public static BoundConditionalGotoStatement GotoIfFalse(
            SyntaxNode syntax,
            BoundExpression condition,
            BoundLabel label)
        {
            return new BoundConditionalGotoStatement(
                syntax,
                condition,
                label,
                gotoIfTrue: false);
        }

        public static BoundFunctionCallExpression FunctionCall(
            SyntaxNode syntax,
            BoundExpression name,
            ImmutableArray<BoundExpression> arguments)
        {
            return new BoundFunctionCallExpression(syntax, name, arguments);
        }

        public static BoundGotoStatement Goto(
            SyntaxNode syntax,
            BoundLabel label)
        {
            return new BoundGotoStatement(syntax, label);
        }

        public static BoundIdentifierNameExpression Identifier(
            SyntaxNode syntax,
            string name)
        {
            return new BoundIdentifierNameExpression(syntax, name);
        }

        public static BoundNumberLiteralExpression NumberLiteral(
            SyntaxNode syntax,
            double value)
        {
            return new BoundNumberLiteralExpression(syntax, value);
        }

        public static BoundStringLiteralExpression StringLiteral(
            SyntaxNode syntax,
            string value)
        {
            return new BoundStringLiteralExpression(syntax, value);
        }

        public static BoundElseifClause ElseifClause(
            SyntaxNode syntax,
            BoundExpression condition,
            BoundStatement body)
        {
            return new BoundElseifClause(syntax, condition, body);
        }

        public static BoundUnaryOperationExpression UnaryOperation(
            SyntaxNode syntax,
            TokenKind kind,
            BoundExpression operand)
        {
            var op = BindUnaryOperator(kind);
            return new BoundUnaryOperationExpression(syntax, op, operand);
        }

        private static BoundUnaryOperator BindUnaryOperator(TokenKind kind)
        {
            return BoundUnaryOperator.GetOperator(kind)
                ?? throw new Exception($"Unexpected unary operator kind {kind}.");
        }

        private static BoundBinaryOperator BindBinaryOperator(TokenKind kind)
        {
            return BoundBinaryOperator.GetOperator(kind)
                ?? throw new Exception($"Unexpected binary operator kind {kind}.");
        }
    }
}
