﻿using Parser.Internal;
using Parser.Lowering;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using static Parser.Binding.BoundNodeFactory;

namespace Parser.Binding
{
    public class Binder
    {
        private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();

        public static BoundProgram BindProgram(SyntaxTree syntaxTree)
        {
            var binder = new Binder();
            var boundRoot = binder.BindRoot(syntaxTree.NullRoot);
            var statements = ((BoundBlockStatement)boundRoot.File.Body).Statements;
            var functionsBuilder = ImmutableDictionary.CreateBuilder<FunctionSymbol, LoweredFunction>();
            var globalStatements = statements.Where(s => s.Kind != BoundNodeKind.FunctionDeclaration).ToArray();
            var mainFunction = (FunctionSymbol?)null;
            var scriptFunction = (FunctionSymbol?)null;
            if (globalStatements.Length > 0)
            {
                // we have to gather all bound expression statements into a "script" function.
                scriptFunction = new FunctionSymbol("%script");
                var body = Block(globalStatements[0].Syntax, globalStatements);
                var loweredBody = Lowerer.Lower(body);
                var declaration = new BoundFunctionDeclaration(
                    syntax: globalStatements[0].Syntax,
                    name: "%script",
                    inputDescription: ImmutableArray<ParameterSymbol>.Empty,
                    outputDescription: ImmutableArray<ParameterSymbol>.Empty,
                    body: body);
                var loweredFunction = LowerFunction(declaration);
                functionsBuilder.Add(scriptFunction, loweredFunction);
            }

            var functions = statements.OfType<BoundFunctionDeclaration>().ToArray();
            var first = true;
            foreach (var function in functions)
            {
                var functionSymbol = new FunctionSymbol(
                    name: function.Name);
                var loweredFunction = LowerFunction(function);
                functionsBuilder.Add(functionSymbol, loweredFunction);
                if (first && globalStatements.Length == 0)
                {
                    // the first function in a file will become "main".
                    first = false;
                    mainFunction = functionSymbol;
                }
            }

            return new BoundProgram(
                binder._diagnostics.ToImmutableArray(),
                mainFunction,
                scriptFunction,
                functionsBuilder.ToImmutable());
        }

        private static LoweredFunction LowerFunction(BoundFunctionDeclaration declaration)
        {
            var loweredBody = Lowerer.Lower(declaration.Body);
            return new LoweredFunction(
                declaration: declaration,
                name: declaration.Name,
                inputDescription: declaration.InputDescription,
                outputDescription: declaration.OutputDescription,
                body: loweredBody);
        }

        private BoundRoot BindRoot(RootSyntaxNode node)
        {
            var boundFile = BindFile(node.File);
            return Root(node, boundFile);
        }

        private BoundFile BindFile(FileSyntaxNode node)
        {
            var body = BindBlockStatement(node.Body);
            return File(node, body);
        }

        private BoundStatement BindStatement(StatementSyntaxNode node)
        {
            return node.Kind switch
            {
                TokenKind.AbstractMethodDeclaration =>
                    BindAbstractMethodDeclaration((AbstractMethodDeclarationSyntaxNode)node),
                TokenKind.BlockStatement =>
                    BindBlockStatement((BlockStatementSyntaxNode)node),
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
            var body = BindStatement(node.Body);
            var elseifClauses = node.ElseifClauses
                .Where(n => n.IsNode)
                .Select(n => (ElseifClause)n.AsNode()!);
            var builder = ImmutableArray.CreateBuilder<BoundElseifClause>();
            foreach (var elseifClause in elseifClauses)
            {
                var clause = BindElseifClause(elseifClause);
                builder.Add(clause);
            }
            var maybeElseClause = node.ElseClause switch
            {
                { } elseClause => BindElseClause(elseClause),
                _ => null,
            };

            return IfStatement(node, condition, body, builder.ToImmutable(), maybeElseClause);
        }

        private BoundStatement BindElseClause(ElseClause node)
        {
            return BindStatement(node.Body);
        }

        private BoundBlockStatement BindBlockStatement(BlockStatementSyntaxNode node)
        {
            var boundStatements = BindStatementList(node.Statements);
            return Block(node, boundStatements.ToArray());
        }

        private IEnumerable<BoundStatement> BindStatementList(SyntaxNodeOrTokenList list)
        {
            var statements = list.Where(s => s.IsNode).Select(s => (StatementSyntaxNode)s.AsNode()!);
            foreach (var statement in statements)
            {
                yield return BindStatement(statement);
            }
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

        private BoundElseifClause BindElseifClause(ElseifClause node)
        {
            var condition = BindExpression(node.Condition);
            var body = BindStatement(node.Body);
            return ElseifClause(node, condition, body);
        }

        private BoundFunctionDeclaration BindFunctionDeclaration(FunctionDeclarationSyntaxNode node)
        {
            var inputDescription = BindInputDescription(node.InputDescription);
            var outputDescription = BindOutputDescription(node.OutputDescription);
            var body = BindStatement(node.Body);
            return new BoundFunctionDeclaration(node, node.Name.Text, inputDescription, outputDescription, body);
        }

        private ImmutableArray<ParameterSymbol> BindOutputDescription(FunctionOutputDescriptionSyntaxNode? node)
        {
            if (node is null)
            {
                return ImmutableArray<ParameterSymbol>.Empty;
            }
            var outputs = node.OutputList.Where(p => p.IsNode).Select(p => p.AsNode()!);
            var builder = ImmutableArray.CreateBuilder<ParameterSymbol>();
            foreach (var output in outputs)
            {
                if (output.Kind != TokenKind.IdentifierNameExpression)
                {
                    throw new Exception($"Invalid function output kind {output.Kind}.");
                }

                builder.Add(BindParameterSymbol((IdentifierNameExpressionSyntaxNode)output));
            }

            return builder.ToImmutable();
        }

        private ImmutableArray<ParameterSymbol> BindInputDescription(FunctionInputDescriptionSyntaxNode? node)
        {
            if (node is null)
            {
                return ImmutableArray<ParameterSymbol>.Empty;
            }

            var parameters = node.ParameterList.Where(p => p.IsNode).Select(p => p.AsNode()!);
            var builder = ImmutableArray.CreateBuilder<ParameterSymbol>();
            foreach (var parameter in parameters)
            {
                if (parameter.Kind != TokenKind.IdentifierNameExpression)
                {
                    throw new Exception($"Invalid function parameter kind {parameter.Kind}.");
                }

                builder.Add(BindParameterSymbol((IdentifierNameExpressionSyntaxNode)parameter));
            }

            return builder.ToImmutable();
        }

        private ParameterSymbol BindParameterSymbol(IdentifierNameExpressionSyntaxNode parameter)
        {
            return new ParameterSymbol(parameter.Text);
        }

        private BoundForStatement BindForStatement(ForStatementSyntaxNode node)
        {
            throw new NotImplementedException();
        }

        private BoundExpressionStatement BindExpressionStatement(ExpressionStatementSyntaxNode node)
        {
            var expression = BindExpression(node.Expression);
            return ExpressionStatement(node, expression);
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
            return Assignment(node, left, right);
        }

        private BoundBinaryOperationExpression BindBinaryOperationExpression(BinaryOperationExpressionSyntaxNode node)
        {
            var left = BindExpression(node.Lhs);
            var right = BindExpression(node.Rhs);
            return BinaryOperation(node, left, node.Operation.Kind, right);
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
            return FunctionCall(node, name, arguments);
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
            return NumberLiteral(node, value);
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntaxNode node)
        {
            return BindExpression(node.Expression);
        }

        private BoundStringLiteralExpression BindStringLiteralExpression(StringLiteralExpressionSyntaxNode node)
        {
            var value = (string)node.StringToken.Value!;
            return StringLiteral(node, value);
        }

        private BoundUnaryOperationExpression BindUnaryPrefixOperationExpression(UnaryPrefixOperationExpressionSyntaxNode node)
        {
            var operand = BindExpression(node.Operand);
            return UnaryOperation(node, node.Operation.Kind, operand);
        }

        private BoundUnaryOperationExpression BindUnaryPostfixOperationExpression(UnaryPostfixOperationExpressionSyntaxNode node)
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
    }
}
