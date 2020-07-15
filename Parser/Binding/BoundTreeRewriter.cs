using System;
using System.Collections.Immutable;

namespace Parser.Binding
{
    public abstract class BoundTreeRewriter
    {
        public virtual BoundStatement RewriteStatement(BoundStatement node)
        {
            return node.Kind switch
            {
                BoundNodeKind.AbstractMethodDeclaration =>
                    RewriteAbstractMethodDeclaration((BoundAbstractMethodDeclaration)node),
                BoundNodeKind.BlockStatement =>
                    RewriteBlockStatement((BoundBlockStatement)node),
                BoundNodeKind.ClassDeclaration =>
                    RewriteClassDeclaration((BoundClassDeclaration)node),
                BoundNodeKind.ConcreteMethodDeclaration =>
                    RewriteConcreteMethodDeclaration((BoundConcreteMethodDeclaration)node),
                BoundNodeKind.EmptyStatement =>
                    RewriteEmptyStatement((BoundEmptyStatement)node),
                BoundNodeKind.ExpressionStatement =>
                    RewriteExpressionStatement((BoundExpressionStatement)node),
                BoundNodeKind.ForStatement =>
                    RewriteForStatement((BoundForStatement)node),
                BoundNodeKind.FunctionDeclaration =>
                    RewriteFunctionDeclaration((BoundFunctionDeclaration)node),
                BoundNodeKind.IfStatement =>
                    RewriteIfStatement((BoundIfStatement)node),
                BoundNodeKind.SwitchStatement =>
                    RewriteSwitchStatement((BoundSwitchStatement)node),
                BoundNodeKind.TryCatchStatement =>
                    RewriteTryCatchStatement((BoundTryCatchStatement)node),
                BoundNodeKind.WhileStatement =>
                    RewriteWhileStatement((BoundWhileStatement)node),
                _ =>
                    throw new Exception($"Invalid statement kind {node.Kind}."),
            };
        }

        public virtual BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteTryCatchStatement(BoundTryCatchStatement node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteSwitchStatement(BoundSwitchStatement node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            var condition = RewriteExpression(node.Condition);
            var body = RewriteStatement(node.Body);
            ImmutableArray<BoundElseifClause>.Builder? builder = null;
            for (var i = 0; i < node.ElseifClauses.Length; i++)
            {
                var oldClause = node.ElseifClauses[i];
                var newClause = RewriteElseifClause(oldClause);
                if (oldClause != newClause && builder is null)
                {
                    builder = ImmutableArray.CreateBuilder<BoundElseifClause>(node.ElseifClauses.Length);
                    for (var j = 0; j < i; i++)
                    {
                        builder.Add(node.ElseifClauses[j]);
                    }
                }
                if (builder is not null)
                {
                    builder.Add(newClause);
                }

            }

            var elseIfClauses = builder is null ? node.ElseifClauses : builder.MoveToImmutable();
            var elseClause = node.ElseClause is null ? null : RewriteElseifClause(node.ElseClause);
            if (condition == node.Condition &&
                body == node.Body &&
                elseIfClauses == node.ElseifClauses &&
                elseClause == node.ElseClause )
            {
                return node;
            }

            return new BoundIfStatement(node.Syntax, condition, body, elseIfClauses, elseClause);
        }

        public virtual BoundElseClause RewriteElseifClause(BoundElseClause node)
        {
            var body = RewriteStatement(node.Body);
            if (body == node.Body)
            {
                return node;
            }

            return new BoundElseClause(node.Syntax, body);
        }

        public virtual BoundElseifClause RewriteElseifClause(BoundElseifClause node)
        {
            var condition = RewriteExpression(node.Condition);
            var body = RewriteStatement(node.Body);
            if (condition == node.Condition && body == node.Body)
            {
                return node;
            }

            return new BoundElseifClause(node.Syntax, condition, body);
        }

        public virtual BoundStatement RewriteFunctionDeclaration(BoundFunctionDeclaration node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteForStatement(BoundForStatement node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteExpressionStatement(BoundExpressionStatement node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
            {
                return node;
            }

            return new BoundExpressionStatement(node.Syntax, expression);
        }

        public virtual BoundStatement RewriteEmptyStatement(BoundEmptyStatement node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteConcreteMethodDeclaration(BoundConcreteMethodDeclaration node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteClassDeclaration(BoundClassDeclaration node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundStatement RewriteBlockStatement(BoundBlockStatement node)
        {
            ImmutableArray<BoundStatement>.Builder? builder = null;
            for (var i = 0; i < node.Statements.Length; i++)
            {
                var oldStatement = node.Statements[i];
                var newStatement = RewriteStatement(oldStatement);
                if (oldStatement != newStatement && builder is null)
                {
                    builder = ImmutableArray.CreateBuilder<BoundStatement>(node.Statements.Length);
                    for (var j = 0; j < i; i++)
                    {
                        builder.Add(node.Statements[j]);
                    }
                }
                if (builder is not null)
                {
                    builder.Add(newStatement);
                }

            }

            if (builder is null)
            {
                return node;
            }

            return new BoundBlockStatement(node.Syntax, builder.MoveToImmutable());
        }

        public virtual BoundStatement RewriteAbstractMethodDeclaration(BoundAbstractMethodDeclaration node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteExpression(BoundExpression node)
        {
            return node.Kind switch
            {
                BoundNodeKind.ArrayLiteralExpression =>
                    RewriteArrayLiteralExpression((BoundArrayLiteralExpression)node),
                BoundNodeKind.AssignmentExpression =>
                    RewriteAssignmentExpression((BoundAssignmentExpression)node),
                BoundNodeKind.BinaryOperationExpression =>
                    RewriteBinaryOperationExpression((BoundBinaryOperationExpression)node),
                BoundNodeKind.CellArrayElementAccessExpression =>
                    RewriteCellArrayElementAccessExpression((BoundCellArrayElementAccessExpression)node),
                BoundNodeKind.CellArrayLiteralExpression =>
                    RewriteCellArrayLiteralExpression((BoundCellArrayLiteralExpression)node),
                BoundNodeKind.ClassInvokationExpression =>
                    RewriteClassInvokationExpression((BoundClassInvokationExpression)node),
                BoundNodeKind.CommandExpression =>
                    RewriteCommandExpression((BoundCommandExpression)node),
                BoundNodeKind.CompoundNameExpression =>
                    RewriteCompoundNameExpression((BoundCompoundNameExpression)node),
                BoundNodeKind.DoubleQuotedStringLiteralExpression =>
                    RewriteDoubleQuotedStringLiteralExpression((BoundDoubleQuotedStringLiteralExpression)node),
                BoundNodeKind.EmptyExpression =>
                    RewriteEmptyExpression((BoundEmptyExpression)node),
                BoundNodeKind.FunctionCallExpression =>
                    RewriteFunctionCallExpression((BoundFunctionCallExpression)node),
                BoundNodeKind.IdentifierNameExpression =>
                    RewriteIdentifierNameExpression((BoundIdentifierNameExpression)node),
                BoundNodeKind.IndirectMemberAccessExpression =>
                    RewriteIndirectMemberAccessExpression((BoundIndirectMemberAccessExpression)node),
                BoundNodeKind.LambdaExpression =>
                    RewriteLambdaExpression((BoundLambdaExpression)node),
                BoundNodeKind.MemberAccessExpression =>
                    RewriteMemberAccessExpression((BoundMemberAccessExpression)node),
                BoundNodeKind.NamedFunctionHandleExpression =>
                    RewriteNamedFunctionHandleExpression((BoundNamedFunctionHandleExpression)node),
                BoundNodeKind.NumberLiteralExpression =>
                    RewriteNumberLiteralExpression((BoundNumberLiteralExpression)node),
                BoundNodeKind.ParenthesizedExpression =>
                    RewriteParenthesizedExpression((BoundParenthesizedExpression)node),
                BoundNodeKind.StringLiteralExpression =>
                    RewriteStringLiteralExpression((BoundStringLiteralExpression)node),
                BoundNodeKind.UnaryPrefixOperationExpression =>
                    RewriteUnaryPrefixOperationExpression((BoundUnaryPrefixOperationExpression)node),
                BoundNodeKind.UnaryPostfixOperationExpression =>
                    RewriteUnaryPostfixOperationExpression((BoundUnaryPostfixOperationExpression)node),
                BoundNodeKind.UnquotedStringLiteralExpression =>
                    RewriteUnquotedStringLiteralExpression((BoundUnquotedStringLiteralExpression)node),
                _ =>
                    throw new Exception($"Invalid expression kind {node.Kind}."),
            };
        }

        public virtual BoundExpression RewriteUnquotedStringLiteralExpression(BoundUnquotedStringLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteUnaryPostfixOperationExpression(BoundUnaryPostfixOperationExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteUnaryPrefixOperationExpression(BoundUnaryPrefixOperationExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteStringLiteralExpression(BoundStringLiteralExpression node)
        {
            return node;
        }

        public virtual BoundExpression RewriteParenthesizedExpression(BoundParenthesizedExpression node)
        {
            var expression = RewriteExpression(node.Expression);
            if (expression == node.Expression)
            {
                return node;
            }

            return new BoundParenthesizedExpression(node.Syntax, expression);
        }

        public virtual BoundExpression RewriteNumberLiteralExpression(BoundNumberLiteralExpression node)
        {
            return node;
        }

        public virtual BoundExpression RewriteNamedFunctionHandleExpression(BoundNamedFunctionHandleExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteMemberAccessExpression(BoundMemberAccessExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteLambdaExpression(BoundLambdaExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteIndirectMemberAccessExpression(BoundIndirectMemberAccessExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteIdentifierNameExpression(BoundIdentifierNameExpression node)
        {
            return node;
        }

        public virtual BoundExpression RewriteFunctionCallExpression(BoundFunctionCallExpression node)
        {

            ImmutableArray<BoundExpression>.Builder? builder = null;
            for (var i = 0; i < node.Arguments.Length; i++)
            {
                var oldArgument = node.Arguments[i];
                var newArgument = RewriteExpression(oldArgument);
                if (oldArgument != newArgument && builder is null)
                {
                    builder = ImmutableArray.CreateBuilder<BoundExpression>(node.Arguments.Length);
                    for (var j = 0; j < i; i++)
                    {
                        builder.Add(node.Arguments[j]);
                    }
                }
                if (builder is not null)
                {
                    builder.Add(newArgument);
                }

            }

            if (builder is null)
            {
                return node;
            }

            return new BoundFunctionCallExpression(node.Syntax, node.Name, builder.MoveToImmutable());
        }

        public virtual BoundExpression RewriteEmptyExpression(BoundEmptyExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteDoubleQuotedStringLiteralExpression(BoundDoubleQuotedStringLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteCommandExpression(BoundCommandExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteCompoundNameExpression(BoundCompoundNameExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteClassInvokationExpression(BoundClassInvokationExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteCellArrayLiteralExpression(BoundCellArrayLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteCellArrayElementAccessExpression(BoundCellArrayElementAccessExpression node)
        {
            throw new NotImplementedException();
        }

        public virtual BoundExpression RewriteBinaryOperationExpression(BoundBinaryOperationExpression node)
        {
            var left = RewriteExpression(node.Left);
            var right = RewriteExpression(node.Right);
            if (left == node.Left && right == node.Right)
            {
                return node;
            }

            return new BoundBinaryOperationExpression(node.Syntax, left, node.Op, right);
        }

        public virtual BoundExpression RewriteAssignmentExpression(BoundAssignmentExpression node)
        {
            var left = RewriteExpression(node.Left);
            var right = RewriteExpression(node.Right);
            if (left == node.Left && right == node.Right)
            {
                return node;
            }

            return new BoundAssignmentExpression(node.Syntax, left, right);
        }

        public virtual BoundExpression RewriteArrayLiteralExpression(BoundArrayLiteralExpression node)
        {
            throw new NotImplementedException();
        }
    }
}
