using Parser.Binding;
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
        private readonly BoundProgram _program;
        private readonly CompilationContext _context;
        private readonly DiagnosticsBag _diagnostics = new DiagnosticsBag();
        private bool _inRepl = false;
        private readonly Stack<EvaluationScope> _scopeStack = new Stack<EvaluationScope>();
        private readonly Dictionary<FunctionSymbol, LoweredFunction> _functions = new Dictionary<FunctionSymbol, LoweredFunction>();

        public Evaluator(BoundProgram program, CompilationContext context, bool inRepl)
        {
            _program = program;
            _context = context;
            var outerScope = new EvaluationScope();
            _scopeStack.Push(outerScope);
            _inRepl = inRepl;
            foreach (var pair in program.Functions)
            {
                _functions[pair.Key] = pair.Value;
            }
        }

        internal EvaluationResult Evaluate()
        {
            if (_program.MainFunction is { } mainFunctionSymbol)
            {
                var mainFunction = _program.Functions[mainFunctionSymbol];
                if (mainFunction.InputDescription.Length > 0)
                {
                    _diagnostics.ReportNotEnoughInputs(
                        mainFunction.Body.Syntax.Span,
                        mainFunction.Name);
                return new EvaluationResult(null, _diagnostics.ToImmutableArray());
                }
                else
                {
                    var result = EvaluateBlockStatement(mainFunction.Body);
                    return new EvaluationResult(result, _diagnostics.ToImmutableArray());
                }
            }
            else if (_program.ScriptFunction is { } scriptFunctionSymbol)
            {
                var scriptFunction = _program.Functions[scriptFunctionSymbol];
                var result = EvaluateBlockStatement(scriptFunction.Body);
                return new EvaluationResult(result, _diagnostics.ToImmutableArray());
            }
            else
            {
                return new EvaluationResult(null, _diagnostics.ToImmutableArray());
            }
        }

        private MObject? EvaluateFile(BoundFile root)
        {
            return EvaluateStatement(root.Body);
        }

        private MObject? EvaluateBlockStatement(BoundBlockStatement node)
        {
            var labelToIndex = new Dictionary<BoundLabel, int>();
            for (var i = 0; i < node.Statements.Length; i++)
            {
                var statement = node.Statements[i];
                if (statement.Kind == BoundNodeKind.LabelStatement)
                {
                    labelToIndex[((BoundLabelStatement)statement).Label] = i;
                }
            }

            MObject? lastResult = null;
            var index = 0;
            while (index < node.Statements.Length)
            {
                var statement = node.Statements[index];
                switch (statement.Kind)
                {
                    case BoundNodeKind.GotoStatement:
                        var gs = (BoundGotoStatement)statement;
                        index = labelToIndex[gs.Label];
                        break;
                    case BoundNodeKind.ConditionalGotoStatement:
                        var cgs = (BoundConditionalGotoStatement)statement;
                        var value = EvaluateExpression(cgs.Condition);
                        var truth = IsTruthyValue(value);
                        if ((cgs.GotoIfTrue && truth) ||
                            (!cgs.GotoIfTrue && !truth))
                        {
                            index = labelToIndex[cgs.Label];
                        }
                        else
                        {
                            index++;
                        }
                        break;
                    case BoundNodeKind.LabelStatement:
                        index++;
                        break;
                    default:
                        lastResult = EvaluateStatement(statement) ?? lastResult;
                        index++;
                        break;
                }
            }

            return lastResult;
        }

        private bool IsTruthyValue(MObject? expression)
        {
            if (expression is MLogical { Value: true })
            {
                return true;
            }

            return false;
        }

        private MObject? EvaluateStatement(BoundStatement node)
        {
            return node.Kind switch
            {
                BoundNodeKind.AbstractMethodDeclaration =>
                    EvaluateAbstractMethodDeclaration((BoundAbstractMethodDeclaration)node),
                BoundNodeKind.BlockStatement =>
                    EvaluateBlockStatement((BoundBlockStatement)node),
                BoundNodeKind.ClassDeclaration =>
                    EvaluateClassDeclaration((BoundClassDeclaration)node),
                BoundNodeKind.EmptyStatement =>
                    EvaluateEmptyStatement((BoundEmptyStatement)node),
                BoundNodeKind.ExpressionStatement =>
                    EvaluateExpressionStatement((BoundExpressionStatement)node),
                BoundNodeKind.ForStatement =>
                    EvaluateForStatement((BoundForStatement)node),
                BoundNodeKind.FunctionDeclaration =>
                    EvaluateFunctionDeclaration((BoundFunctionDeclaration)node),
                BoundNodeKind.IfStatement =>
                    EvaluateIfStatement((BoundIfStatement)node),
                BoundNodeKind.ConcreteMethodDeclaration =>
                    EvaluateMethodDefinition((BoundConcreteMethodDeclaration)node),
                BoundNodeKind.SwitchStatement =>
                    EvaluateSwitchStatement((BoundSwitchStatement)node),
                BoundNodeKind.TryCatchStatement =>
                    EvaluateTryCatchStatement((BoundTryCatchStatement)node),
                BoundNodeKind.WhileStatement =>
                    EvaluateWhileStatement((BoundWhileStatement)node),
                _ => throw new NotImplementedException($"Invalid statement kind '{node.Kind}'."),
            };
        }

        private MObject? EvaluateClassDeclaration(BoundClassDeclaration node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateEmptyStatement(BoundEmptyStatement node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateTryCatchStatement(BoundTryCatchStatement node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateForStatement(BoundForStatement node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateIfStatement(BoundIfStatement node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateWhileStatement(BoundWhileStatement node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateSwitchStatement(BoundSwitchStatement node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateFunctionDeclaration(BoundFunctionDeclaration node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateAbstractMethodDeclaration(BoundAbstractMethodDeclaration node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateMethodDefinition(BoundConcreteMethodDeclaration node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            var result = EvaluateExpression(node.Expression);
            return node.DiscardResult ? null : result;
        }

        private MObject? EvaluateExpression(BoundExpression node)
        {
            return node.Kind switch
            {
                BoundNodeKind.ArrayLiteralExpression =>
                    EvaluateArrayLiteralExpression((BoundArrayLiteralExpression)node),
                BoundNodeKind.AssignmentExpression =>
                    EvaluateAssignmentExpression((BoundAssignmentExpression)node),
                BoundNodeKind.BinaryOperationExpression =>
                    EvaluateBinaryOperation((BoundBinaryOperationExpression)node),
                BoundNodeKind.CellArrayElementAccessExpression =>
                    EvaluateCellArrayElementAccess((BoundCellArrayElementAccessExpression)node),
                BoundNodeKind.CellArrayLiteralExpression =>
                    EvaluateCellArrayLiteralExpression((BoundCellArrayLiteralExpression)node),
                BoundNodeKind.ClassInvokationExpression =>
                    EvaluateClassInvokation((BoundClassInvokationExpression)node),
                BoundNodeKind.CommandExpression =>
                    EvaluateCommand((BoundCommandExpression)node),
                BoundNodeKind.CompoundNameExpression =>
                    EvaluateCompoundName((BoundCompoundNameExpression)node),
                BoundNodeKind.DoubleQuotedStringLiteralExpression =>
                    EvaluateDoubleQuotedStringLiteralExpression((BoundDoubleQuotedStringLiteralExpression)node),
                BoundNodeKind.EmptyExpression =>
                    EvaluateEmptyExpression((BoundEmptyExpression)node),
                BoundNodeKind.FunctionCallExpression =>
                    EvaluateFunctionCall((BoundFunctionCallExpression)node),
                BoundNodeKind.IdentifierNameExpression =>
                    EvaluateIdentifierNameExpression((BoundIdentifierNameExpression)node),
                BoundNodeKind.IndirectMemberAccessExpression =>
                    EvaluateIndirectMemberAccess((BoundIndirectMemberAccessExpression)node),
                BoundNodeKind.LambdaExpression =>
                    EvaluateLambdaExpression((BoundLambdaExpression)node),
                BoundNodeKind.MemberAccessExpression =>
                    EvaluateMemberAccess((BoundMemberAccessExpression)node),
                BoundNodeKind.NamedFunctionHandleExpression =>
                    EvaluateNamedFunctionHandleExpression((BoundNamedFunctionHandleExpression)node),
                BoundNodeKind.NumberDoubleLiteralExpression =>
                    EvaluateNumberDoubleLiteralExpression((BoundNumberDoubleLiteralExpression)node),
                BoundNodeKind.NumberIntLiteralExpression =>
                    EvaluateNumberIntLiteralExpression((BoundNumberIntLiteralExpression)node),
                BoundNodeKind.StringLiteralExpression =>
                    EvaluateStringLiteralExpression((BoundStringLiteralExpression)node),
                BoundNodeKind.UnaryOperationExpression =>
                    EvaluateUnaryOperationExpression((BoundUnaryOperationExpression)node),
                BoundNodeKind.UnquotedStringLiteralExpression =>
                    EvaluateUnquotedStringLiteralExpression((BoundUnquotedStringLiteralExpression)node),
                _ => throw new NotImplementedException($"Invalid expression kind '{node.Kind}'."),
            };
        }

        private MObject? EvaluateClassInvokation(BoundClassInvokationExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCommand(BoundCommandExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateIndirectMemberAccess(BoundIndirectMemberAccessExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateMemberAccess(BoundMemberAccessExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateFunctionCall(BoundFunctionCallExpression node)
        {
            var arguments = new List<MObject>();
            var allGood = true;
            foreach (var argument in node.Arguments)
            {
                var evaluatedArgument = EvaluateExpression(argument);
                if (evaluatedArgument is null)
                {
                    _diagnostics.ReportCannotEvaluateExpression(argument.Syntax.Span);
                    allGood = false;
                }
                else
                {
                    arguments.Add(evaluatedArgument);
                }

            }
            if (!allGood)
            {
                return null;
            }

            var function = GetFunctionSymbol(node.Name);
            if (function.Name == "disp")
            {
                return EvaluateDisp(arguments);
            }
            else
            {
                var resolvedFunction = ResolveFunction(function);
                if (resolvedFunction is null)
                {
                    _diagnostics.ReportFunctionNotFound(
                        node.Name.Syntax.Span,
                        function.Name);
                    return null;
                }
                else
                {
                    // bring arguments into context
                    var newScope = new EvaluationScope();
                    var counter = 0;
                    foreach (var expectedArgument in resolvedFunction.InputDescription)
                    {
                        if (counter >= arguments.Count)
                        {
                            break;
                        }
                        newScope.Variables.Add(expectedArgument.Name, arguments[counter]);
                        counter++;
                    }

                    if (counter < arguments.Count)
                    {
                        _diagnostics.ReportTooManyInputs(
                            node.Arguments[counter].Syntax.Span,
                            function.Name);
                        return null;
                    }
                    _scopeStack.Push(newScope);
                    var result = EvaluateBlockStatement(resolvedFunction.Body);
                    _scopeStack.Pop();
                    return result;
                }
            }
        }

        private LoweredFunction? ResolveFunction(UnresolvedFunctionSymbol functionSymbol)
        {
            var maybeKey = _functions.Keys.FirstOrDefault(k => k.Name == functionSymbol.Name);
            return maybeKey switch
            {
                { } key => _functions[key],
                _ => null,
            };
        }

        private MObject? EvaluateDisp(List<MObject> arguments)
        {
            if (arguments.Count != 1)
            {
                throw new NotImplementedException($"Cannot evaluate disp() with {arguments.Count} arguments.");
            }

            Console.WriteLine(arguments[0]);
            return arguments[0];
        }

        private UnresolvedFunctionSymbol GetFunctionSymbol(BoundExpression functionName)
        {
            if (functionName.Kind == BoundNodeKind.IdentifierNameExpression)
            {
                return new UnresolvedFunctionSymbol(((BoundIdentifierNameExpression)functionName).Name);
            }

            throw new NotImplementedException($"Unknown function symbol '{functionName.Syntax.Text}'.");
        }

        private MObject? EvaluateCellArrayElementAccess(BoundCellArrayElementAccessExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateCellArrayLiteralExpression(BoundCellArrayLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateArrayLiteralExpression(BoundArrayLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateUnquotedStringLiteralExpression(BoundUnquotedStringLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateDoubleQuotedStringLiteralExpression(BoundDoubleQuotedStringLiteralExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateStringLiteralExpression(BoundStringLiteralExpression node)
        {
            return node.Value switch
            {
                string s => MObject.CreateCharArray(s.ToCharArray()),
                _ => null,
            };
        }

        private MObject? EvaluateNumberDoubleLiteralExpression(BoundNumberDoubleLiteralExpression node)
        {
            return MObject.CreateDoubleNumber(node.Value);
        }

        private MObject? EvaluateNumberIntLiteralExpression(BoundNumberIntLiteralExpression node)
        {
            return MObject.CreateDoubleNumber(node.Value);
        }

        private MObject? EvaluateIdentifierNameExpression(BoundIdentifierNameExpression node)
        {
            var variableName = node.Name;
            var maybeValue = GetVariableValue(variableName);
            if (maybeValue is null)
            {
                _diagnostics.ReportVariableNotFound(
                    node.Syntax.Span,
                    variableName);
            }

            return maybeValue;
        }

        private MObject? EvaluateBinaryOperation(BoundBinaryOperationExpression node)
        {
            var left = EvaluateExpression(node.Left);
            if (left is null)
            {
                return null;
            }

            var right = EvaluateExpression(node.Right);
            if (right is null)
            {
                return null;
            }

            return node.Op.Kind switch
            {
                BoundBinaryOperatorKind.Plus => MOperations.Plus(left, right),
                BoundBinaryOperatorKind.Minus => MOperations.Minus(left, right),
                BoundBinaryOperatorKind.Star => MOperations.Star(left, right),
                BoundBinaryOperatorKind.Slash => MOperations.Slash(left, right),
                BoundBinaryOperatorKind.Greater => MOperations.Greater(left, right),
                BoundBinaryOperatorKind.GreaterOrEquals => MOperations.GreaterOrEquals(left, right),
                BoundBinaryOperatorKind.Less => MOperations.Less(left, right),
                BoundBinaryOperatorKind.LessOrEquals => MOperations.LessOrEquals(left, right),
                _ => throw new NotImplementedException($"Binary operation {node.Op.Kind} is not implemented."),
            };
        }

        private MObject? EvaluateCompoundName(BoundCompoundNameExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateUnaryOperationExpression(BoundUnaryOperationExpression node)
        {
            var operand = EvaluateExpression(node.Operand);
            if (operand is null)
            {
                return null;
            }

            return node.Op.Kind switch
            {
                BoundUnaryOperatorKind.Minus => MOperations.Minus(operand),
                _ => throw new NotImplementedException($"Unary operation {node.Op.Kind} is not implemented."),
            };
        }

        private MObject? EvaluateEmptyExpression(BoundEmptyExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateAssignmentExpression(BoundAssignmentExpression node)
        {
            var rightValue = EvaluateExpression(node.Right);
            if (rightValue is null)
            {
                _diagnostics.ReportCannotEvaluateExpression(
                    node.Right.Syntax.Span);
                return null;
            }

            var left = node.Left;
            if (left.Kind == BoundNodeKind.IdentifierNameExpression)
            {
                var leftIdentifier = (BoundIdentifierNameExpression)left;
                var variableName = leftIdentifier.Name;
                SetVariableValue(variableName, rightValue);
                return rightValue;
            }

            throw new NotImplementedException();
        }

        private MObject? GetVariableValue(string name)
        {
            if (_inRepl)
            {
                return _context.Variables.TryGetValue(name, out var globalValue) ? globalValue : null;
            }
            else
            {
                var currentScope = _scopeStack.Peek();
                return currentScope.Variables.TryGetValue(name, out var localValue) ? localValue : null;
            }
        }

        private void SetVariableValue(string name, MObject value)
        {
            if (_inRepl)
            {
                _context.Variables[name] = value;
            }
            else
            {
                var currentScope = _scopeStack.Peek();
                currentScope.Variables[name] = value;
            }
        }

        private MObject? EvaluateLambdaExpression(BoundLambdaExpression node)
        {
            throw new NotImplementedException();
        }

        private MObject? EvaluateNamedFunctionHandleExpression(BoundNamedFunctionHandleExpression node)
        {
            throw new NotImplementedException();
        }
    }
}