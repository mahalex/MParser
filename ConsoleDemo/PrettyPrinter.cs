using System;
using Parser;
using Parser.Internal;

namespace ProjectConsole
{
    public class PrettyPrinter : SyntaxVisitor
    {
        public override void VisitFile(FileSyntaxNode node)
        {
            Visit(node.StatementList);
            OutputKeyword(node.EndOfFile);
        }

        private void PrintToken(SyntaxToken token, ConsoleColor color, bool useBold = false)
        {
            if (token == default(SyntaxToken))
            {
                return;
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var t in token.LeadingTrivia)
            {
                Console.Write(t);
            }

            Console.ForegroundColor = color;
            if (useBold)
            {
                BoldOn();
            }
            Console.Write(token.Text);
            if (useBold)
            {
                BoldOff();
            }
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var t in token.TrailingTrivia)
            {
                Console.Write(t);
            }
        }

        private static void UnderlineOn()
        {
            Console.Write("\x1b[4m");            
        }

        private static void UnderlineOff()
        {
            Console.Write("\x1b[0m");            
        }

        private static void BoldOn()
        {
            Console.Write("\x1b[1m");                        
        }

        private static void BoldOff()
        {
            Console.Write("\x1b[0m");                        
        }

        private void OutputKeyword(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.Green, useBold: true);
        }

        private void OutputControlKeyword(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.Yellow, useBold: true);
        }

        private void OutputPunctuation(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.DarkBlue);
        }

        private void OutputOperator(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.Cyan);
        }

        private void OutputIdentifier(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.White, useBold: true);
        }

        private void OutputUnquotedStringLiteral(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.Blue);
        }

        private void OutputStringLiteral(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.Magenta);
        }

        private void OutputNumberLiteral(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.DarkGreen);
        }

        private void OutputBracket(SyntaxToken token)
        {
            PrintToken(token, ConsoleColor.DarkYellow);
        }
        
        public override void VisitBaseClassList(BaseClassListSyntaxNode node)
        {
            OutputPunctuation(node.LessSign);
            Visit(node.BaseClasses);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntaxNode node)
        {
            OutputKeyword(node.ClassdefKeyword);
            Visit(node.Attributes);
            BoldOn();
            OutputIdentifier(node.ClassName);
            BoldOff();
            Visit(node.BaseClassList);
            Visit(node.Nodes);
            OutputKeyword(node.EndKeyword);
        }

        public override void DefaultVisit(SyntaxNode node)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var t in node.LeadingTrivia)
            {
                Console.Write(t);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(node.Text);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var t in node.TrailingTrivia)
            {
                Console.Write(t);
            }
        }

        public override void VisitList(SyntaxNodeOrTokenList list)
        {
            foreach (var nodeOrToken in list)
            {
                if (nodeOrToken.IsToken)
                {
                    var token = nodeOrToken.AsToken();
                    if (token.Kind == TokenKind.IdentifierToken)
                    {
                        OutputIdentifier(token);
                    }
                    else if (SyntaxFacts.IsBracket(token.Kind))
                    {
                        OutputBracket(token);
                    }
                    else
                    {
                        OutputPunctuation(token);
                    }
                }
                else
                {
                    Visit(nodeOrToken.AsNode());
                }
            }
        }

        public override void VisitMethodsList(MethodsListSyntaxNode node)
        {
            OutputKeyword(node.MethodsKeyword);
            Visit(node.Attributes);
            Visit(node.Methods);
            OutputKeyword(node.EndKeyword);
        }

        public override void VisitPropertiesList(PropertiesListSyntaxNode node)
        {
            OutputKeyword(node.PropertiesKeyword);
            Visit(node.Attributes);
            Visit(node.Properties);
            OutputKeyword(node.EndKeyword);
        }

        public override void VisitConcreteMethodDeclaration(ConcreteMethodDeclarationSyntaxNode node)
        {
            OutputKeyword(node.FunctionKeyword);
            Visit(node.OutputDescription);
            BoldOn();
            Visit(node.Name);
            BoldOff();
            Visit(node.InputDescription);
            Visit(node.Commas);
            Visit(node.Body);
            Visit(node.EndKeyword);
        }

        public override void VisitEndKeyword(EndKeywordSyntaxNode node)
        {
            OutputKeyword(node.EndKeyword);
        }

        public override void VisitFunctionDeclaration(FunctionDeclarationSyntaxNode node)
        {
            OutputKeyword(node.FunctionKeyword);
            Visit(node.OutputDescription);
            OutputIdentifier(node.Name);
            Visit(node.InputDescription);
            Visit(node.Commas);
            Visit(node.Body);
            Visit(node.EndKeyword);
        }

        public override void VisitIfStatement(IfStatementSyntaxNode node)
        {
            OutputControlKeyword(node.IfKeyword);
            Visit(node.Condition);
            Visit(node.OptionalCommas);
            Visit(node.Body);
            Visit(node.ElseifClauses);
            Visit(node.ElseClause);
            OutputControlKeyword(node.EndKeyword);
        }

        public override void VisitElseClause(ElseClause node)
        {
            OutputControlKeyword(node.ElseKeyword);
            Visit(node.Body);
        }

        public override void VisitElseifClause(ElseifClause node)
        {
            OutputControlKeyword(node.ElseifKeyword);
            Visit(node.Condition);
            Visit(node.Body);
        }

        public override void VisitAbstractMethodDeclaration(AbstractMethodDeclarationSyntaxNode node)
        {
            Visit(node.OutputDescription);
            BoldOn();
            Visit(node.Name);
            BoldOff();
            Visit(node.InputDescription);
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntaxNode node)
        {
            Visit(node.Lhs);
            OutputOperator(node.AssignmentSign);
            Visit(node.Rhs);
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntaxNode node)
        {
            Visit(node.Expression);
        }

        public override void VisitArrayLiteralExpression(ArrayLiteralExpressionSyntaxNode node)
        {
            OutputBracket(node.OpeningSquareBracket);
            Visit(node.Nodes);
            OutputBracket(node.ClosingSquareBracket);
        }

        public override void VisitCellArrayLiteralExpression(CellArrayLiteralExpressionSyntaxNode node)
        {
            OutputBracket(node.OpeningBrace);
            Visit(node.Nodes);
            OutputBracket(node.ClosingBrace);            
        }

        public override void VisitIdentifierNameExpression(IdentifierNameExpressionSyntaxNode node)
        {
            OutputIdentifier(node.Name);
        }

        public override void VisitForStatement(ForStatementSyntaxNode node)
        {
            OutputControlKeyword(node.ForKeyword);
            Visit(node.Assignment);
            Visit(node.OptionalCommas);
            Visit(node.Body);
            OutputControlKeyword(node.EndKeyword);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntaxNode node)
        {
            OutputControlKeyword(node.SwitchKeyword);
            Visit(node.SwitchExpression);
            Visit(node.OptionalCommas);
            Visit(node.Cases);
            OutputControlKeyword(node.EndKeyword);
        }

        public override void VisitWhileStatement(WhileStatementSyntaxNode node)
        {
            OutputControlKeyword(node.WhileKeyword);
            Visit(node.Condition);
            Visit(node.OptionalCommas);
            Visit(node.Body);
            OutputControlKeyword(node.EndKeyword);
        }

        public override void VisitUnquotedStringLiteralExpression(UnquotedStringLiteralExpressionSyntaxNode node)
        {
            OutputUnquotedStringLiteral(node.StringToken);
        }

        public override void VisitStringLiteralExpression(StringLiteralExpressionSyntaxNode node)
        {
            OutputStringLiteral(node.StringToken);
        }

        public override void VisitBinaryOperationExpression(BinaryOperationExpressionSyntaxNode node)
        {
            Visit(node.Lhs);
            OutputOperator(node.Operation);
            Visit(node.Rhs);
        }

        public override void VisitFunctionCallExpression(FunctionCallExpressionSyntaxNode node)
        {
            Visit(node.FunctionName);
            OutputBracket(node.OpeningBracket);
            Visit(node.Nodes);
            OutputBracket(node.ClosingBracket);
        }

        public override void VisitSwitchCase(SwitchCaseSyntaxNode node)
        {
            OutputControlKeyword(node.CaseKeyword);
            Visit(node.CaseIdentifier);
            Visit(node.OptionalCommas);
            Visit(node.Body);
        }

        public override void VisitCatchClause(CatchClauseSyntaxNode node)
        {
            OutputControlKeyword(node.CatchKeyword);
            Visit(node.CatchBody);
        }

        public override void VisitTryCatchStatement(TryCatchStatementSyntaxNode node)
        {
            OutputControlKeyword(node.TryKeyword);
            Visit(node.TryBody);
            Visit(node.CatchClause);
            OutputControlKeyword(node.EndKeyword);
        }

        public override void VisitCommandExpression(CommandExpressionSyntaxNode node)
        {
            OutputIdentifier(node.CommandName);
            Visit(node.Arguments);
        }

        public override void VisitNumberLiteralExpression(NumberLiteralExpressionSyntaxNode node)
        {
            OutputNumberLiteral(node.Number);
        }

        public override void VisitUnaryPrefixOperationExpression(UnaryPrefixOperationExpressionSyntaxNode node)
        {
            OutputOperator(node.Operation);
            Visit(node.Operand);
        }

        public override void VisitUnaryPostfixOperationExpression(UnaryPostfixOperationExpressionSyntaxNode node)
        {
            Visit(node.Operand);
            OutputOperator(node.Operation);
        }

        public override void VisitClassInvokationExpression(ClassInvokationExpressionSyntaxNode node)
        {
            Visit(node.MethodName);
            OutputOperator(node.AtSign);
            Visit(node.BaseClassNameAndArguments);
        }

        public override void VisitAttributeAssignment(AttributeAssignmentSyntaxNode node)
        {
            OutputOperator(node.AssignmentSign);
            Visit(node.Value);
        }

        public override void VisitAttribute(AttributeSyntaxNode node)
        {
            OutputIdentifier(node.Name);
            Visit(node.Assignment);
        }

        public override void VisitAttributeList(AttributeListSyntaxNode node)
        {
            OutputBracket(node.OpeningBracket);
            Visit(node.Nodes);
            OutputBracket(node.ClosingBracket);
        }

        public override void VisitCellArrayElementAccessExpression(CellArrayElementAccessExpressionSyntaxNode node)
        {
            Visit(node.Expression);
            OutputBracket(node.OpeningBrace);
            Visit(node.Nodes);
            OutputBracket(node.ClosingBrace);
        }

        public override void VisitCompoundNameExpression(CompoundNameExpressionSyntaxNode node)
        {
            Visit(node.Nodes);
        }

        public override void VisitDoubleQuotedStringLiteralExpression(DoubleQuotedStringLiteralExpressionSyntaxNode node)
        {
            OutputStringLiteral(node.StringToken);
        }

        public override void VisitEmptyExpression(EmptyExpressionSyntaxNode node)
        {
        }

        public override void VisitEmptyStatement(EmptyStatementSyntaxNode node)
        {
            OutputPunctuation(node.Semicolon);
        }

        public override void VisitEnumerationList(EnumerationListSyntaxNode node)
        {
            OutputKeyword(node.EnumerationKeyword);
            Visit(node.Attributes);
            Visit(node.Items);
            OutputKeyword(node.EndKeyword);
        }

        public override void VisitEventsList(EventsListSyntaxNode node)
        {
            OutputKeyword(node.EventsKeyword);
            Visit(node.Attributes);
            Visit(node.Events);
            OutputKeyword(node.EndKeyword);
        }

        public override void VisitEnumerationItemValue(EnumerationItemValueSyntaxNode node)
        {
            OutputPunctuation(node.OpeningBracket);
            Visit(node.Values);
            OutputPunctuation(node.ClosingBracket);
        }

        public override void VisitEnumerationItem(EnumerationItemSyntaxNode node)
        {
            OutputIdentifier(node.Name);
            Visit(node.Values);
            Visit(node.Commas);
        }

        public override void VisitFunctionInputDescription(FunctionInputDescriptionSyntaxNode node)
        {
            OutputBracket(node.OpeningBracket);
            Visit(node.ParameterList);
            OutputBracket(node.ClosingBracket);
        }

        public override void VisitFunctionOutputDescription(FunctionOutputDescriptionSyntaxNode node)
        {
            Visit(node.OutputList);
            OutputOperator(node.AssignmentSign);
        }

        public override void VisitIndirectMemberAccessExpression(IndirectMemberAccessExpressionSyntaxNode node)
        {
            OutputBracket(node.OpeningBracket);
            Visit(node.Expression);
            OutputBracket(node.ClosingBracket);
        }

        public override void VisitLambdaExpression(LambdaExpressionSyntaxNode node)
        {
            OutputOperator(node.AtSign);
            Visit(node.Input);
            Visit(node.Body);
        }

        public override void VisitNamedFunctionHandleExpression(NamedFunctionHandleExpressionSyntaxNode node)
        {
            OutputOperator(node.AtSign);
            Visit(node.FunctionName);
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntaxNode node)
        {
            Visit(node.LeftOperand);
            OutputOperator(node.Dot);
            Visit(node.RightOperand);
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntaxNode node)
        {
            OutputBracket(node.OpeningBracket);
            Visit(node.Expression);
            OutputBracket(node.ClosingBracket);
        }
    }
}