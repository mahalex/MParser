using System;
using Parser;
using Semantics;

namespace ConsoleDemo
{
    public class UsageGathering : SyntaxWalker
    {
        private MethodAssignments _methodAssignments;
        private VariableAssignments _variableAssignments;
        private bool _insideFunction;
        private bool _insideMethod;

        private Context _context;

        public UsageGathering(Context context)
        {
            _context = context;
        }

        public override void VisitFunctionCallExpression(FunctionCallExpressionSyntaxNode node)
        {
            if (!(_insideFunction || _insideMethod))
            {
                return;
            }
            var name = node.FunctionName.Text;
            if (_variableAssignments.Find(name) != null)
            {
                return;
            }
            Console.Write($"Function call: {name}...");
            if (_context.FindFunction(name) || _methodAssignments.Find(name) != null)
            {
                Console.WriteLine("found.");
            }
            else if (_context.FindClass(name))
            {
                Console.WriteLine("found class constructor.");
            }
            else
            {
                Console.WriteLine("NOT FOUND.");
            }
            base.VisitFunctionCallExpression(node);
        }

        private void Assign(SyntaxNode lhs, SyntaxNode rhs)
        {
            switch (lhs.Kind)
            {
                case TokenKind.IdentifierNameExpression:
                    var name = ((IdentifierNameExpressionSyntaxNode)lhs).Name.Text;
                    Console.WriteLine($"Adding variable assignment for {name}");
                    _variableAssignments.Add(name, new Variable());
                    break;
                default:
                    break;
            }
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntaxNode node)
        {
            if (!(_insideFunction || _insideMethod))
            {
                return;
            }
            Assign(node.Lhs, node.Rhs);
            base.VisitAssignmentExpression(node);
        }

        public override void VisitFile(FileSyntaxNode node)
        {
            _methodAssignments = new MethodAssignments();
            foreach (var nodeOrToken in node.Body.Statements)
            {
                if (nodeOrToken.IsToken)
                {
                    continue;
                }

                var n = nodeOrToken.AsNode();
                if (n.Kind == TokenKind.FunctionDeclaration)
                {
                    var functionDeclaration = (FunctionDeclarationSyntaxNode)n;
                    _methodAssignments.Add(functionDeclaration.Name.Text, new Variable());
                }
            }
            base.VisitFile(node);
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
                    if (parameterAsNode.Kind == TokenKind.IdentifierNameExpression)
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

        public override void VisitConcreteMethodDeclaration(ConcreteMethodDeclarationSyntaxNode node)
        {
            _insideMethod = true;
            _variableAssignments = new VariableAssignments();
            var parameterList = node.InputDescription.ParameterList;
            foreach (var parameter in parameterList)
            {
                if (parameter.IsNode)
                {
                    var parameterAsNode = parameter.AsNode();
                    Console.WriteLine($"Parameter node: {parameterAsNode}");
                    if (parameterAsNode.Kind == TokenKind.IdentifierNameExpression)
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
            base.VisitConcreteMethodDeclaration(node);
            _variableAssignments = null;
            _insideMethod = false;
        }

    }
}
