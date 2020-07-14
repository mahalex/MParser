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
                TokenKind.AbstractMethodDeclaration =>
                    EvaluateAbstractMethodDeclaration((AbstractMethodDeclarationSyntaxNode)statement),
                TokenKind.ClassDeclaration =>
                    EvaluateClassDeclaration((ClassDeclarationSyntaxNode)statement),
                TokenKind.EmptyStatement =>
                    EvaluateEmptyStatement((EmptyStatementSyntaxNode)statement),
                TokenKind.ExpressionStatement =>
                    EvaluateExpressionStatement((ExpressionStatementSyntaxNode)statement),
                TokenKind.ForStatement =>
                    EvaluateForStatement((ForStatementSyntaxNode)statement),
                TokenKind.FunctionDeclaration =>
                    EvaluateFunctionDeclaration((FunctionDeclarationSyntaxNode)statement),
                TokenKind.IfStatement =>
                    EvaluateIfStatement((IfStatementSyntaxNode)statement),
                TokenKind.ConcreteMethodDeclaration =>
                    EvaluateMethodDefinition((MethodDefinitionSyntaxNode)statement),
                TokenKind.SwitchStatement =>
                    EvaluateSwitchStatement((SwitchStatementSyntaxNode)statement),
                TokenKind.TryCatchStatement =>
                    EvaluateTryCatchStatement((TryCatchStatementSyntaxNode)statement),
                TokenKind.WhileStatement =>
                    EvaluateWhileStatement((WhileStatementSyntaxNode)statement),
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
                TokenKind.ArrayLiteralExpression =>
                    EvaluateArrayLiteralExpression((ArrayLiteralExpressionSyntaxNode)expression),
                TokenKind.AssignmentExpression =>
                    EvaluateAssignmentExpression((AssignmentExpressionSyntaxNode)expression),
                TokenKind.BinaryOperationExpression =>
                    EvaluateBinaryOperation((BinaryOperationExpressionSyntaxNode)expression),
                TokenKind.CellArrayElementAccessExpression =>
                    EvaluateCellArrayElementAccess((CellArrayElementAccessExpressionSyntaxNode)expression),
                TokenKind.CellArrayLiteralExpression =>
                    EvaluateCellArrayLiteralExpression((CellArrayLiteralExpressionSyntaxNode)expression),
                TokenKind.ClassInvokationExpression =>
                    EvaluateClassInvokation((BaseClassInvokationSyntaxNode)expression),
                TokenKind.CommandExpression =>
                    EvaluateCommand((CommandExpressionSyntaxNode)expression),
                TokenKind.CompoundNameExpression =>
                    EvaluateCompoundName((CompoundNameExpressionSyntaxNode)expression),
                TokenKind.DoubleQuotedStringLiteralExpression =>
                    EvaluateDoubleQuotedStringLiteralExpression((DoubleQuotedStringLiteralSyntaxNode)expression),
                TokenKind.EmptyExpression =>
                    EvaluateEmptyExpression((EmptyExpressionSyntaxNode)expression),
                TokenKind.FunctionCallExpression =>
                    EvaluateFunctionCall((FunctionCallExpressionSyntaxNode)expression),
                TokenKind.IdentifierNameExpression =>
                    EvaluateIdentifierNameExpression((IdentifierNameExpressionSyntaxNode)expression),
                TokenKind.IndirectMemberAccessExpression =>
                    EvaluateIndirectMemberAccess((IndirectMemberAccessSyntaxNode)expression),
                TokenKind.LambdaExpression =>
                    EvaluateLambdaExpression((LambdaExpressionSyntaxNode)expression),
                TokenKind.MemberAccessExpression =>
                    EvaluateMemberAccess((MemberAccessSyntaxNode)expression),
                TokenKind.NamedFunctionHandleExpression =>
                    EvaluateNamedFunctionHandleExpression((NamedFunctionHandleExpressionSyntaxNode)expression),
                TokenKind.NumberLiteralExpression =>
                    EvaluateNumberLiteralExpression((NumberLiteralSyntaxNode)expression),
                TokenKind.ParenthesizedExpression =>
                    EvaluateParenthesizedExpression((ParenthesizedExpressionSyntaxNode)expression),
                TokenKind.StringLiteralExpression =>
                    EvaluateStringLiteralExpression((StringLiteralSyntaxNode)expression),
                TokenKind.UnaryPrefixOperationExpression =>
                    EvaluateUnaryPrefixOperationExpression((UnaryPrefixOperationExpressionSyntaxNode)expression),
                TokenKind.UnaryPostfixOperationExpression =>
                    EvaluateUnaryPostfixOperationExpression((UnaryPostixOperationExpressionSyntaxNode)expression),
                TokenKind.UnquotedStringLiteralExpression =>
                    EvaluateUnquotedStringLiteralExpression((UnquotedStringLiteralSyntaxNode)expression),
                _ => throw new NotImplementedException($"Invalid expression kind '{expression.Kind}'."),
            };
        }

        private MObject? EvaluateParenthesizedExpression(ParenthesizedExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
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

        private MObject? EvaluateIdentifierNameExpression(IdentifierNameExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateBinaryOperation(BinaryOperationExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCompoundName(CompoundNameExpressionSyntaxNode expression)
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

        private MObject? EvaluateLambdaExpression(LambdaExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateNamedFunctionHandleExpression(NamedFunctionHandleExpressionSyntaxNode expression)
        {
            throw new NotImplementedException();
        }
    }
}