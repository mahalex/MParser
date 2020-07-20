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

        public static BoundConversionExpression Conversion(SyntaxNode syntax, TypeSymbol targetType, BoundExpression expression)
        {
            return new BoundConversionExpression(syntax, targetType, expression);
        }

        public static BoundExpressionStatement ExpressionStatement(
            SyntaxNode syntax,
            BoundExpression expression,
            bool discardResult)
        {
            return new BoundExpressionStatement(syntax, expression, discardResult);
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

        public static BoundForStatement ForStatement(
            SyntaxNode syntax,
            BoundIdentifierNameExpression loopVariable,
            BoundExpression loopExpression,
            BoundStatement body)
        {
            return new BoundForStatement(syntax, loopVariable, loopExpression, body);
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
            BoundBinaryOperator op,
            BoundExpression right)
        {
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

        public static BoundWhileStatement WhileStatement(
            SyntaxNode syntax,
            BoundExpression condition,
            BoundStatement body)
        {
            return new BoundWhileStatement(syntax, condition, body);
        }

        public static BoundIdentifierNameExpression Identifier(
            SyntaxNode syntax,
            string name)
        {
            return new BoundIdentifierNameExpression(syntax, name);
        }

        public static BoundNumberDoubleLiteralExpression NumberDoubleLiteral(
            SyntaxNode syntax,
            double value)
        {
            return new BoundNumberDoubleLiteralExpression(syntax, value);
        }

        public static BoundNumberIntLiteralExpression NumberIntLiteral(
            SyntaxNode syntax,
            int value)
        {
            return new BoundNumberIntLiteralExpression(syntax, value);
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

        public static BoundTypedVariableDeclaration TypedVariableDeclaration(
            SyntaxNode syntax,
            TypedVariableSymbol variable,
            BoundExpression initializer)
        {
            return new BoundTypedVariableDeclaration(syntax, variable, initializer);
        }

        public static BoundTypedVariableExpression TypedVariableExpression(
            SyntaxNode syntax,
            TypedVariableSymbol variable)
        {
            return new BoundTypedVariableExpression(syntax, variable);
        }

        public static BoundUnaryOperationExpression UnaryOperation(
            SyntaxNode syntax,
            BoundUnaryOperator op,
            BoundExpression operand)
        {
            return new BoundUnaryOperationExpression(syntax, op, operand);
        }

        public static BoundExpression TypedFunctionCall(
            SyntaxNode syntax,
            TypedFunctionSymbol function,
            ImmutableArray<BoundExpression> arguments)
        {
            return new BoundTypedFunctionCallExpression(
                syntax,
                function,
                arguments);
        }
    }
}
