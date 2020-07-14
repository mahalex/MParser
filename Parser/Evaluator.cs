using Parser.Internal;
using Parser.Objects;
using System;
using System.Collections.Immutable;

namespace Parser
{
    internal class Evaluator
    {
        private SyntaxTree _syntaxTree;
        private CompilationContext _context;
        private DiagnosticsBag _diagnostics;

        public Evaluator(SyntaxTree syntaxTree, CompilationContext context)
        {
            _syntaxTree = syntaxTree;
            _context = context;
            _diagnostics = new DiagnosticsBag();
        }

        internal EvaluationResult Evaluate()
        {
            var result = EvaluateFile(_syntaxTree.Root);
            return new EvaluationResult(result, _diagnostics.ToImmutableArray());
        }

        private MObject? EvaluateFile(FileSyntaxNode root)
        {
            MObject? lastResult = null;
            foreach (var nodeOrToken in root.StatementList)
            {
                if (nodeOrToken.IsNode)
                {
                    var statement = (StatementSyntaxNode)nodeOrToken.AsNode()!;
                    lastResult = EvaluateStatement(statement) ?? lastResult;
                }
            }

            return lastResult;
        }

        private MObject? EvaluateStatement(StatementSyntaxNode statement)
        {
            return statement.Kind switch
            {
                TokenKind.ExpressionStatement =>
                    EvaluateExpressionStatement((ExpressionStatementSyntaxNode)statement),
                TokenKind.MethodDefinition =>
                    EvaluateMethodDefinition((MethodDefinitionSyntaxNode)statement),
                TokenKind.AbstractMethodDeclaration =>
                    EvaluateAbstractMethodDeclaration((AbstractMethodDeclarationSyntaxNode)statement),
                TokenKind.FunctionDeclaration =>
                    EvaluateFunctionDeclaration((FunctionDeclarationSyntaxNode)statement),
                TokenKind.SwitchStatement =>
                    EvaluateSwitchStatement((SwitchStatementSyntaxNode)statement),
                TokenKind.WhileStatement =>
                    EvaluateWhileStatement((WhileStatementSyntaxNode)statement),
                TokenKind.IfStatement =>
                    EvaluateIfStatement((IfStatementSyntaxNode)statement),
                TokenKind.ForStatement =>
                    EvaluateForStatement((ForStatementSyntaxNode)statement),
                TokenKind.TryCatchStatement =>
                    EvaluateTryCatchStatement((TryCatchStatementSyntaxNode)statement),
                TokenKind.EmptyStatement =>
                    EvaluateEmptyStatement((EmptyStatementSyntaxNode)statement),
                TokenKind.ClassDeclaration =>
                    EvaluateClassDeclaration((ClassDeclarationSyntaxNode)statement),
                _ => throw new NotImplementedException($"Invalid statement kind '{statement.Kind}'."),
            };
        }

        private MObject? EvaluateClassDeclaration(ClassDeclarationSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateEmptyStatement(EmptyStatementSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateTryCatchStatement(TryCatchStatementSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateForStatement(ForStatementSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateIfStatement(IfStatementSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateWhileStatement(WhileStatementSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateSwitchStatement(SwitchStatementSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateFunctionDeclaration(FunctionDeclarationSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateAbstractMethodDeclaration(AbstractMethodDeclarationSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateMethodDefinition(MethodDefinitionSyntaxNode statement)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateExpressionStatement(ExpressionStatementSyntaxNode statement)
        {
            return EvaluateExpression(statement.Expression);
        }

        private MObject? EvaluateExpression(ExpressionSyntaxNode expression)
        {
            return expression.Kind switch
            {
                TokenKind.Lambda =>
                    EvaluateLambda((LambdaSyntaxNode)expression),
                TokenKind.AssignmentExpression =>
                    EvaluateAssignmentExpression((AssignmentExpressionSyntaxNode)expression),
                TokenKind.EmptyExpression =>
                    EvaluateEmptyExpression((EmptyExpressionSyntaxNode)expression),
                TokenKind.UnaryPrefixOperationExpression =>
                    EvaluateUnaryPrefixOperationExpression((UnaryPrefixOperationExpressionSyntaxNode)expression),
                TokenKind.CompoundName =>
                    EvaluateCompoundName((CompoundNameSyntaxNode)expression),
                TokenKind.BinaryOperation =>
                    EvaluateBinaryOperation((BinaryOperationExpressionSyntaxNode)expression),
                TokenKind.IdentifierName =>
                    EvaluateIdentifierName((IdentifierNameSyntaxNode)expression),
                TokenKind.NumberLiteralExpression =>
                    EvaluateNumberLiteralExpression((NumberLiteralSyntaxNode)expression),
                TokenKind.StringLiteralExpression =>
                    EvaluateStringLiteralExpression((StringLiteralSyntaxNode)expression),
                TokenKind.DoubleQuotedStringLiteralExpression =>
                    EvaluateDoubleQuotedStringLiteralExpression((DoubleQuotedStringLiteralSyntaxNode)expression),
                TokenKind.UnquotedStringLiteralExpression =>
                    EvaluateUnquotedStringLiteralExpression((UnquotedStringLiteralSyntaxNode)expression),
                TokenKind.ArrayLiteralExpression =>
                    EvaluateArrayLiteralExpression((ArrayLiteralExpressionSyntaxNode)expression),
                TokenKind.CellArrayLiteralExpression =>
                    EvaluateCellArrayLiteralExpression((CellArrayLiteralExpressionSyntaxNode)expression),
                TokenKind.ParenthesizedExpression =>
                    EvaluateNamedFunctionHandle((NamedFunctionHandleSyntaxNode)expression),
                TokenKind.CellArrayElementAccess =>
                    EvaluateCellArrayElementAccess((CellArrayElementAccessExpressionSyntaxNode)expression),
                TokenKind.FunctionCall =>
                    EvaluateFunctionCall((FunctionCallExpressionSyntaxNode)expression),
                TokenKind.MemberAccess =>
                    EvaluateMemberAccess((MemberAccessSyntaxNode)expression),
                TokenKind.UnaryPostfixOperationExpression =>
                    EvaluateUnaryPostfixOperationExpression((UnaryPostixOperationExpressionSyntaxNode)expression),
                TokenKind.IndirectMemberAccess =>
                    EvaluateIndirectMemberAccess((IndirectMemberAccessSyntaxNode)expression),
                TokenKind.Command =>
                    EvaluateCommand((CommandExpressionSyntaxNode)expression),
                TokenKind.ClassInvokation =>
                    EvaluateClassInvokation((BaseClassInvokationSyntaxNode)expression),
                _ => throw new NotImplementedException($"Invalid expression kind '{expression.Kind}'."),
            };
        }

        private MObject? EvaluateClassInvokation(BaseClassInvokationSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCommand(CommandExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateIndirectMemberAccess(IndirectMemberAccessSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateUnaryPostfixOperationExpression(UnaryPostixOperationExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateMemberAccess(MemberAccessSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateFunctionCall(FunctionCallExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCellArrayElementAccess(CellArrayElementAccessExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCellArrayLiteralExpression(CellArrayLiteralExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateArrayLiteralExpression(ArrayLiteralExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateUnquotedStringLiteralExpression(UnquotedStringLiteralSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateDoubleQuotedStringLiteralExpression(DoubleQuotedStringLiteralSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateStringLiteralExpression(StringLiteralSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateNumberLiteralExpression(NumberLiteralSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateIdentifierName(IdentifierNameSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateBinaryOperation(BinaryOperationExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCompoundName(CompoundNameSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateUnaryPrefixOperationExpression(UnaryPrefixOperationExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateEmptyExpression(EmptyExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateAssignmentExpression(AssignmentExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateLambda(LambdaSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateNamedFunctionHandle(NamedFunctionHandleSyntaxNode expression)
        {
            throw new NotImplementedException();
        }
    }
}