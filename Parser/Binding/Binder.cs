using Parser.Internal;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Parser.Binding
{
    public class Binder
    {
        private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();

        private BoundRoot BindRoot(RootSyntaxNode node)
        {
            var boundFile = BindFile(node.File);
            return new BoundRoot(node, boundFile);
        }

        private BoundFile BindFile(FileSyntaxNode node)
        {
            var statements = BindStatementList(node.StatementList);
            return new BoundFile(node, statements);
        }

        private BoundStatement BindStatement(StatementSyntaxNode node)
        {
            return node.Kind switch
            {
                TokenKind.AbstractMethodDeclaration =>
                    BindAbstractMethodDeclaration((AbstractMethodDeclarationSyntaxNode)node),
                TokenKind.ClassDeclaration =>
                    BindClassDeclaration((ClassDeclarationSyntaxNode)node),
                TokenKind.ConcreteMethodDeclaration =>
                    BindConcreteMethodDeclaration((ConcreteMethodDeclarationSyntaxNode)node),
                TokenKind.EmptyStatement =>
                    BindEmptyStatement((EmptyStatementSyntaxNode)node),
                TokenKind.ExpressionStatement =>
                    BindExpressionStatement((ExpressionStatementSyntaxNode)node),
                TokenKind.ForStatement =>
                    BindForStatement((ForStatementSyntaxNode)node),
                TokenKind.FunctionDeclaration =>
                    BindFunctionDeclaration((FunctionDeclarationSyntaxNode)node),
                TokenKind.IfStatement =>
                    BindIfStatement((IfStatementSyntaxNode)node),
                TokenKind.SwitchStatement =>
                    BindSwitchStatement((SwitchStatementSyntaxNode)node),
                TokenKind.TryCatchStatement =>
                    BindTryCatchStatement((TryCatchStatementSyntaxNode)node),
                TokenKind.WhileStatement =>
                    BindWhileStatement((WhileStatementSyntaxNode)node),
                _ =>
                    throw new Exception($"Invalid statement node kind '{node.Kind}'."),
            };
        }

        private BoundWhileStatement BindWhileStatement(WhileStatementSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundTryCatchStatement BindTryCatchStatement(TryCatchStatementSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundSwitchStatement BindSwitchStatement(SwitchStatementSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundIfStatement BindIfStatement(IfStatementSyntaxNode node)
        {
            var condition = BindExpression(node.Condition);
            var body = BindBlockStatement(node.Body);
            var elseIfClauses = node.ElseifClauses
                .Where(n => n.IsNode)
                .Select(n => (ElseifClause)n.AsNode()!);
            var builder = ImmutableArray.CreateBuilder<BoundElseIfClause>();
            foreach (var elseIfClause in elseIfClauses)
            {
                var clause = BindElseIfClause(elseIfClause);
                builder.Add(clause);
            }
            var maybeElseClause = node.ElseClause switch
            {
                { } elseClause => BindElseClause(elseClause),
                _ => null,
            };

            return new BoundIfStatement(node, condition, body, elseIfClauses, maybeElseClause);
        }

        private BoundElseClause BindElseClause(ElseClause node)
        {
            var body = BindBlockStatement(node.Body);
            return new BoundElseClause(node, body);
        }

        private BoundBlockStatement BindBlockStatement(BlockStatementSyntaxNode node)
        {
            var boundStatements = BindStatementList(node.Statements);
            return new BoundBlockStatement(node, boundStatements);
        }

        private ImmutableArray<BoundStatement> BindStatementList(SyntaxNodeOrTokenList list)
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var statements = list.Where(s => s.IsNode).Select(s => (StatementSyntaxNode)s.AsNode()!);
            foreach (var statement in statements)
            {
                var boundStatement = BindStatement(statement);
                builder.Add(boundStatement);
            }

            return builder.ToImmutable();
        }

        private ImmutableArray<BoundExpression> BindExpressionList(SyntaxNodeOrTokenList list)
        {
            var builder = ImmutableArray.CreateBuilder<BoundExpression>();
            var expressions = list.Where(s => s.IsNode).Select(s => (ExpressionSyntaxNode)s.AsNode()!);
            foreach (var expression in expressions)
            {
                var boundExpression = BindExpression(expression);
                builder.Add(boundExpression);
            }

            return builder.ToImmutable();
        }

        private BoundElseIfClause BindElseIfClause(ElseifClause node)
        {
            var condition = BindExpression(node.Condition);
            var body = BindBlockStatement(node.Body);
            return new BoundElseIfClause(node, condition, body);
        }

        private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundForStatement BindForStatement(ForStatementSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundExpressionStatement BindExpressionStatement(ExpressionStatementSyntaxNode node)
        {
            var expression = BindExpression(node.Expression);
            return new BoundExpressionStatement(node, expression);
        }

        private BoundExpression BindExpression(ExpressionSyntaxNode node)
        {
            return node.Kind switch
            {
                TokenKind.ArrayLiteralExpression =>
                    BindArrayLiteralExpression((ArrayLiteralExpressionSyntaxNode)node),
                TokenKind.AssignmentExpression =>
                    BindAssignmentExpression((AssignmentExpressionSyntaxNode)node),
                TokenKind.BinaryOperationExpression =>
                    BindBinaryOperationExpression((BinaryOperationExpressionSyntaxNode)node),
                TokenKind.CellArrayElementAccessExpression =>
                    BindCellArrayElementAccessExpression((CellArrayElementAccessExpressionSyntaxNode)node),
                TokenKind.CellArrayLiteralExpression =>
                    BindCellArrayLiteralExpression((CellArrayLiteralExpressionSyntaxNode)node),
                TokenKind.ClassInvokationExpression =>
                    BindClassInvokationExpression((ClassInvokationExpressionSyntaxNode)node),
                TokenKind.CommandExpression =>
                    BindCommandExpression((CommandExpressionSyntaxNode)node),
                TokenKind.CompoundNameExpression =>
                    BindCompoundNameExpression((CompoundNameExpressionSyntaxNode)node),
                TokenKind.DoubleQuotedStringLiteralExpression =>
                    BindDoubleQuotedStringLiteralExpression((DoubleQuotedStringLiteralExpressionSyntaxNode)node),
                TokenKind.EmptyExpression =>
                    BindEmptyExpression((EmptyExpressionSyntaxNode)node),
                TokenKind.FunctionCallExpression =>
                    BindFunctionCallExpression((FunctionCallExpressionSyntaxNode)node),
                TokenKind.IdentifierNameExpression =>
                    BindIdentifierNameExpression((IdentifierNameExpressionSyntaxNode)node),
                TokenKind.IndirectMemberAccessExpression =>
                    BindIndirectMemberAccessExpression((IndirectMemberAccessExpressionSyntaxNode)node),
                TokenKind.LambdaExpression =>
                    BindLambdaExpression((LambdaExpressionSyntaxNode)node),
                TokenKind.MemberAccessExpression =>
                    BindMemberAccessExpression((MemberAccessExpressionSyntaxNode)node),
                TokenKind.NamedFunctionHandleExpression =>
                    BindNamedFunctionHandleExpression((NamedFunctionHandleExpressionSyntaxNode)node),
                TokenKind.NumberLiteralExpression =>
                    BindNumberLiteralExpression((NumberLiteralExpressionSyntaxNode)node),
                TokenKind.ParenthesizedExpression =>
                    BindParenthesizedExpression((ParenthesizedExpressionSyntaxNode)node),
                TokenKind.StringLiteralExpression =>
                    BindStringLiteralExpression((StringLiteralExpressionSyntaxNode)node),
                TokenKind.UnaryPrefixOperationExpression =>
                    BindUnaryPrefixOperationExpression((UnaryPrefixOperationExpressionSyntaxNode)node),
                TokenKind.UnaryPostfixOperationExpression =>
                    BindUnaryPostfixOperationExpression((UnaryPostfixOperationExpressionSyntaxNode)node),
                TokenKind.UnquotedStringLiteralExpression =>
                    BindUnquotedStringLiteralExpression((UnquotedStringLiteralExpressionSyntaxNode)node),
                _ =>
                    throw new Exception($"Invalid statement node kind '{node.Kind}'."),
            };
        }

        private BoundArrayLiteralExpression BindArrayLiteralExpression(ArrayLiteralExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundAssignmentExpression BindAssignmentExpression(AssignmentExpressionSyntaxNode node)
        {
            var left = BindExpression(node.Lhs);
            var right = BindExpression(node.Rhs);
            return new BoundAssignmentExpression(node, left, right);
        }

        private BoundBinaryOperationExpression BindBinaryOperationExpression(BinaryOperationExpressionSyntaxNode node)
        {
            var left = BindExpression(node.Lhs);
            var right = BindExpression(node.Rhs);
            var op = BindBinaryOperator(node.Operation);
            return new BoundBinaryOperationExpression(node, left, op, right);
        }

        private BoundBinaryOperator BindBinaryOperator(SyntaxToken token)
        {
            return BoundBinaryOperator.GetOperator(token.Kind)
                ?? throw new Exception($"Unexpected binary operator kind {token.Kind}.");
        }

        private BoundCellArrayElementAccessExpression BindCellArrayElementAccessExpression(CellArrayElementAccessExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundCellArrayLiteralExpression BindCellArrayLiteralExpression(CellArrayLiteralExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundClassInvokationExpression BindClassInvokationExpression(ClassInvokationExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundCommandExpression BindCommandExpression(CommandExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundCompoundNameExpression BindCompoundNameExpression(CompoundNameExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundDoubleQuotedStringLiteralExpression BindDoubleQuotedStringLiteralExpression(DoubleQuotedStringLiteralExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundEmptyExpression BindEmptyExpression(EmptyExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundFunctionCallExpression BindFunctionCallExpression(FunctionCallExpressionSyntaxNode node)
        {
            var name = BindExpression(node.FunctionName);
            var arguments = BindExpressionList(node.Nodes);
            return new BoundFunctionCallExpression(node, name, arguments);
        }

        private BoundIdentifierNameExpression BindIdentifierNameExpression(IdentifierNameExpressionSyntaxNode node)
        {
            return new BoundIdentifierNameExpression(node, node.Name.Text);
        }

        private BoundIndirectMemberAccessExpression BindIndirectMemberAccessExpression(IndirectMemberAccessExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundLambdaExpression BindLambdaExpression(LambdaExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundMemberAccessExpression BindMemberAccessExpression(MemberAccessExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundNamedFunctionHandleExpression BindNamedFunctionHandleExpression(NamedFunctionHandleExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundNumberLiteralExpression BindNumberLiteralExpression(NumberLiteralExpressionSyntaxNode node)
        {
            var value = (double)node.Number.Value!;
            return new BoundNumberLiteralExpression(node, value);
        }

        private BoundParenthesizedExpression BindParenthesizedExpression(ParenthesizedExpressionSyntaxNode node)
        {
            var expression = BindExpression(node.Expression);
            return new BoundParenthesizedExpression(node, expression);
        }

        private BoundStringLiteralExpression BindStringLiteralExpression(StringLiteralExpressionSyntaxNode node)
        {
            var value = (string)node.StringToken.Value!;
            return new BoundStringLiteralExpression(node, value);
        }

        private BoundUnaryPrefixOperationExpression BindUnaryPrefixOperationExpression(UnaryPrefixOperationExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundUnaryPostfixOperationExpression BindUnaryPostfixOperationExpression(UnaryPostfixOperationExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundUnquotedStringLiteralExpression BindUnquotedStringLiteralExpression(UnquotedStringLiteralExpressionSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundEmptyStatement BindEmptyStatement(EmptyStatementSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundConcreteMethodDeclaration BindConcreteMethodDeclaration(ConcreteMethodDeclarationSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundClassDeclaration BindClassDeclaration(ClassDeclarationSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundAbstractMethodDeclaration BindAbstractMethodDeclaration(AbstractMethodDeclarationSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        public static BoundProgram BindProgram(SyntaxTree syntaxTree)
        {
            var binder = new Binder();
            var boundRoot = binder.BindRoot(syntaxTree.NullRoot);
            return new BoundProgram(boundRoot, binder._diagnostics.ToImmutableArray());
        }
    }
}
