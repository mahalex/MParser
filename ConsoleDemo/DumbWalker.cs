using System;
using System.Linq;
using Parser;
using Semantics;

namespace ConsoleDemo
{
    public class DumbWalker : SyntaxWalker
    {
        private bool _insideMethod;
        private bool _insideFunction;
        private VariableAssignments _variableAssignments;
        private MethodAssignments _methodAssignments;
        private Context _context;

        public DumbWalker(Context context)
        {
            _context = context;
        }

        private void Assign(SyntaxNode lhs, SyntaxNode rhs)
        {
            switch (lhs.Kind)
            {
                case TokenKind.IdentifierName:
                    var name = ((IdentifierNameSyntaxNode) lhs).Name.Text;
                    Console.WriteLine($"Adding variable assignment for {name}");
                    _variableAssignments.Add(name, new Variable());
                    break;
                default:
                    break;
            }
        }
        
        public override void VisitAssignmentExpression(AssignmentExpressionSyntaxNode node)
        {
            if (_insideMethod || _insideFunction)
            {
                Console.Write($"Assignment: {node.Lhs} <- {node.Rhs}...");
                if (IsDefined(node.Rhs))
                {
                    Console.WriteLine("Ok.");
                    Assign(node.Lhs, node.Rhs);
                }
                else
                {
                    Console.WriteLine("Right-hand side is not defined!");
                }
            }
        }

        private bool IsDefinedToken(SyntaxToken token)
        {
            switch (token.Kind)
            {
                case TokenKind.Comma:
                    return true;
                default:
                    break;
            }

            return false;
        }

        private bool IsDefinedFunctionName(SyntaxNode node)
        {
            switch (node.Kind)
            {
                case TokenKind.IdentifierName:
                    var name = (IdentifierNameSyntaxNode) node;
                    if (_context.FindFunction(name.Name.Text))
                    {
                        return true;
                    }

                    if (_methodAssignments.Find(name.Text) != null)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        private bool IsDefined(SyntaxNode node)
        {
            Variable assignment;
            switch (node.Kind)
            {
                case TokenKind.IdentifierName:
                    assignment = _variableAssignments.Find(node.Text);
                    if (assignment != null || node.Text == "end")
                    {
                        return true;
                    }

                    break;
                case TokenKind.FunctionCall:
                    var functionCall = (FunctionCallExpressionSyntaxNode)node;
                    return
                        (IsDefined(functionCall.FunctionName) && IsDefined(functionCall.Nodes)) ||
                        (IsDefinedFunctionName(functionCall.FunctionName) && IsDefined(functionCall.Nodes));
                case TokenKind.CellArrayElementAccess:
                    var cellArrayElementAccess = (CellArrayElementAccessExpressionSyntaxNode) node;
                    return IsDefined(cellArrayElementAccess.Expression) && IsDefined(cellArrayElementAccess.Nodes);
                case TokenKind.List:
                    var list = (SyntaxNodeOrTokenList) node;
                    return list.All(x => x.IsNode ? IsDefined(x.AsNode()) : IsDefinedToken(x.AsToken()));
                case TokenKind.NumberLiteralExpression:
                    return true;
                case TokenKind.StringLiteralExpression:
                    return true;
                case TokenKind.BinaryOperation:
                    var binaryOperation = (BinaryOperationExpressionSyntaxNode) node;
                    return IsDefined(binaryOperation.Lhs) && IsDefined(binaryOperation.Rhs);
                case TokenKind.UnaryPrefixOperationExpression:
                    var unaryOperation = (UnaryPrefixOperationExpressionSyntaxNode) node;
                    return IsDefined(unaryOperation.Operand);
                case TokenKind.ArrayLiteralExpression:
                    var arrayLiteral = (ArrayLiteralExpressionSyntaxNode) node;
                    return arrayLiteral.Nodes == null || IsDefined(arrayLiteral.Nodes);
                default:
                    break;
            }
            return false;
        }

        public override void VisitFunctionDeclaration(FunctionDeclarationSyntaxNode node)
        {
            _insideFunction = true;
            _variableAssignments = new VariableAssignments();
            var parameterList = node.InputDescription.ParameterList;
            foreach (var parameter in parameterList)
            {
                if (parameter.IsNode)
                {
                    var parameterAsNode = parameter.AsNode();
                    Console.WriteLine($"Parameter node: {parameterAsNode}");
                    if (parameterAsNode.Kind == TokenKind.IdentifierName)
                    {
                        Console.WriteLine($"Adding variable assignment for {parameterAsNode.Text}");
                        _variableAssignments.Add(parameterAsNode.Text, new Variable());
                    }
                    else
                    {
                        Console.WriteLine($"Don't know how to add assignment for {parameterAsNode.Text}");                        
                    }
                }
                else
                {
                    Console.WriteLine($"Parameter token: {parameter.AsToken()}");                    
                }
            }
            base.VisitFunctionDeclaration(node);
            _variableAssignments = null;
            _insideFunction = false;
        }

        public override void VisitMethodDefinition(MethodDefinitionSyntaxNode node)
        {
            _insideMethod = true;
            base.VisitMethodDefinition(node);
            _insideMethod = false;
        }

        public override void VisitFile(FileSyntaxNode node)
        {
            _methodAssignments = new MethodAssignments();
            foreach (var nodeOrToken in node.StatementList)
            {
                if (nodeOrToken.IsToken)
                {
                    continue;
                }

                var n = nodeOrToken.AsNode();
                if (n.Kind == TokenKind.FunctionDeclaration)
                {
                    var functionDeclaration = (FunctionDeclarationSyntaxNode) n;
                    _methodAssignments.Add(functionDeclaration.Name.Text, new Variable());
                }
            }
            base.VisitFile(node);
        }
    }
}
