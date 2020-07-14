using Parser.Internal;
using Parser.MFunctions;
using Parser.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Parser
{
    internal class Evaluator
    {
        private readonly SyntaxTree _syntaxTree;
        private readonly CompilationContext _context;
        private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();
        private bool _insideFunction = false;
        private readonly Stack<EvaluationScope> _scopeStack = new Stack<EvaluationScope>();

        public Evaluator(SyntaxTree syntaxTree, CompilationContext context)
        {
            _syntaxTree = syntaxTree;
            _context = context;
            var outerScope = new EvaluationScope();
            _scopeStack.Push(outerScope);
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
            return EvaluateExpression(expression.Expression);
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
            var arguments = new List<MObject>();
            var nodes = expression.Nodes.Where(n => n.IsNode).Select(n => (ExpressionSyntaxNode)n.AsNode()!);
            var allGood = true;
            foreach (var node in nodes)
            {
                var argument = EvaluateExpression(node);
                if (argument is null)
                {
                    _diagnostics.ReportCannotEvaluateExpression(
                        new TextSpan(node.Position, node.FullWidth));
                    allGood = false;
                }
                else
                {
                    arguments.Add(argument);
                }

            }
            if (!allGood)
            {
                return null;
            }

            var function = GetFunctionSymbol(expression.FunctionName);
            if (function.Name == "disp")
            {
                return EvaluateDisp(arguments);
            }
            else
            {
                throw new NotImplementedException("Functions are not supported.");
            }
        }

        private MObject? EvaluateDisp(List<MObject> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new NotImplementedException($"Cannot evaluate disp() with {arguments.Count} arguments.");
            }

            Console.WriteLine(arguments[0]);
            return null;
        }

        private FunctionSymbol GetFunctionSymbol(ExpressionSyntaxNode functionName)
        {
            if (functionName.Kind == TokenKind.IdentifierNameExpression)
            {
                return new FunctionSymbol(((IdentifierNameExpressionSyntaxNode)functionName).Text);
            }

            throw new NotImplementedException($"Unknown function symbol '{functionName.Text}'.");
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
            return expression.StringToken.Value switch
            {
                string s => MObject.CreateCharArray(s.ToCharArray()),
                _ => null,
            };
        }

        private MObject? EvaluateNumberLiteralExpression(NumberLiteralSyntaxNode expression)
        {
            return expression.Number.Value is double value
                ? MObject.CreateDoubleNumber(value)
                : null;
        }

        private MObject? EvaluateIdentifierNameExpression(IdentifierNameExpressionSyntaxNode expression)
        {
            var variableName = expression.Name.Text;
            var maybeValue = GetVariableValue(variableName);
            if (maybeValue is null)
            {
                _diagnostics.ReportVariableNotFound(
                    new TextSpan(expression.Name.Position, expression.Name.Text.Length),
                    variableName);
            }

            return maybeValue;
        }

        private MObject? EvaluateBinaryOperation(BinaryOperationExpressionSyntaxNode expression)
        {
            var left = EvaluateExpression(expression.Lhs);
            if (left is null)
            {
                return null;
            }

            var right = EvaluateExpression(expression.Rhs);
            if (right is null)
            {
                return null;
            }

            return expression.Operation.Kind switch
            {
                TokenKind.PlusToken => MOperations.Plus(left, right),
                TokenKind.MinusToken => MOperations.Minus(left, right),
                TokenKind.StarToken => MOperations.Star(left, right),
                TokenKind.SlashToken => MOperations.Slash(left, right),
                _ => throw new NotImplementedException($"Binary operation {expression.Operation.Kind} is not implemented."),
            };
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
            var rightValue = EvaluateExpression(expression.Rhs);
            if (rightValue is null)
            {
                _diagnostics.ReportCannotEvaluateExpression(
                    new TextSpan(expression.Rhs.Position, expression.Rhs.Position + expression.Rhs.FullWidth));
                return null;
            }

            var left = expression.Lhs;
            if (left.Kind == TokenKind.IdentifierNameExpression)
            {
                var leftIdentifier = (IdentifierNameExpressionSyntaxNode)left;
                var variableName = leftIdentifier.Name.Text;
                SetVariableValue(variableName, rightValue);
                return rightValue;
            }

            throw new NotImplementedException();
        }

        private MObject? GetVariableValue(string name)
        {
            if (_insideFunction)
            {
                if (_context.Variables.TryGetValue(name, out var globalValue))
                {
                    return globalValue;
                }

                var currentScope = _scopeStack.Peek();
                return currentScope.Variables.TryGetValue(name, out var localValue) ? globalValue : null;
            }
            else
            {
                if (_context.Variables.TryGetValue(name, out var globalValue))
                {
                    return globalValue;
                }

                return null;
            }
        }

        private void SetVariableValue(string name, MObject value)
        {
            if (_insideFunction)
            {
                if (_context.Variables.ContainsKey(name))
                {
                    _context.Variables[name] = value;
                }
                else
                {
                    var currentScope = _scopeStack.Peek();
                    currentScope.Variables[name] = value;
                }
            }
            else
            {
                _context.Variables[name] = value;
            }
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