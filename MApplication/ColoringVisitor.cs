using Parser;
using Parser.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MApplication
{
    internal class ColoringVisitor : SyntaxWalker
    {
        private readonly ImmutableList<DisplayLine>.Builder Builder;
        private ImmutableList<DisplayLineChunk>.Builder CurrentLineBuilder;
        private readonly StyleScheme _scheme;
        private int _currentLine;

        public ColoringVisitor(StyleScheme scheme)
        {
            Builder = ImmutableList.CreateBuilder<DisplayLine>();
            CurrentLineBuilder = ImmutableList.CreateBuilder<DisplayLineChunk>();
            _currentLine = 0;
            _scheme = scheme;
        }

        public override void VisitToken(SyntaxToken token)
        {
            ProcessTriviaList(token.LeadingTrivia);
            CurrentLineBuilder.Add(new DisplayLineChunk(token.Text.AsMemory(), _scheme.DefaultToken));
            ProcessTriviaList (token.TrailingTrivia);
        }

        private void ProcessTriviaList(IReadOnlyList<SyntaxTrivia> triviaList)
        {
            foreach (var trivia in triviaList)
            {
                ProcessTrivia(trivia);
            }
        }

        private void FinishLine()
        {
            Builder.Add(new DisplayLine(CurrentLineBuilder.ToImmutable()));
            CurrentLineBuilder.Clear();
            _currentLine++;
        }

        private void ProcessTrivia(SyntaxTrivia trivia)
        {
            if (trivia.Text.Contains('\n'))
            {
                var first = true;
                foreach (var part in trivia.Text.Split('\n'))
                {
                    if (first)
                    {
                        first = false;
                    } else
                    {
                        FinishLine();
                    }
                    CurrentLineBuilder.Add(new DisplayLineChunk(part.AsMemory(), _scheme.Trivia));
                }
            }
            else
            {
                CurrentLineBuilder.Add(new DisplayLineChunk(trivia.Text.AsMemory(), _scheme.Trivia));
            }
        }

        public void AddToken(SyntaxToken token, Style style)
        {
            ProcessTriviaList(token.LeadingTrivia);
            CurrentLineBuilder.Add(new DisplayLineChunk(token.Text.AsMemory(), style));
            ProcessTriviaList(token.TrailingTrivia);
        }

        public override void VisitFile(FileSyntaxNode node)
        {
            Visit(node.StatementList);
            AddToken(node.EndOfFile, _scheme.Keyword);
        }

        public override void VisitBaseClassList(BaseClassListSyntaxNode node)
        {
            AddToken(node.LessSign, _scheme.Punctuation);
            Visit(node.BaseClasses);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntaxNode node)
        {
            AddToken(node.ClassdefKeyword, _scheme.Keyword);
            Visit(node.Attributes);
            AddToken(node.ClassName, _scheme.Identifier);
            Visit(node.BaseClassList);
            Visit(node.Nodes);
            AddToken(node.EndKeyword, _scheme.Keyword);
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
                        AddToken(token, _scheme.Identifier);
                    }
                    else if (SyntaxFacts.IsBracket(token.Kind))
                    {
                        AddToken(token, _scheme.Bracket);
                    }
                    else
                    {
                        AddToken(token, _scheme.Punctuation);
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
            AddToken(node.MethodsKeyword, _scheme.Keyword);
            Visit(node.Attributes);
            Visit(node.Methods);
            AddToken(node.EndKeyword, _scheme.Keyword);
        }

        public override void VisitPropertiesList(PropertiesListSyntaxNode node)
        {
            AddToken(node.PropertiesKeyword, _scheme.Keyword);
            Visit(node.Attributes);
            Visit(node.Properties);
            AddToken(node.EndKeyword, _scheme.Keyword);
        }

        public override void VisitConcreteMethodDeclaration(ConcreteMethodDeclarationSyntaxNode node)
        {
            AddToken(node.FunctionKeyword, _scheme.Keyword);
            Visit(node.OutputDescription);
            Visit(node.InputDescription);
            Visit(node.Commas);
            Visit(node.Body);
            Visit(node.EndKeyword);
        }

        public override void VisitEndKeyword(EndKeywordSyntaxNode node)
        {
            AddToken(node.EndKeyword, _scheme.Keyword);
        }

        public override void VisitFunctionDeclaration(FunctionDeclarationSyntaxNode node)
        {
            AddToken(node.FunctionKeyword, _scheme.Keyword);
            Visit(node.OutputDescription);
            AddToken(node.Name, _scheme.Identifier);
            Visit(node.InputDescription);
            Visit(node.Commas);
            Visit(node.Body);
            Visit(node.EndKeyword);
        }

        public override void VisitIfStatement(IfStatementSyntaxNode node)
        {
            AddToken(node.IfKeyword, _scheme.ControlKeyword);
            Visit(node.Condition);
            Visit(node.OptionalCommas);
            Visit(node.Body);
            Visit(node.ElseifClauses);
            Visit(node.ElseClause);
            AddToken(node.EndKeyword, _scheme.ControlKeyword);
        }

        public override void VisitElseClause(ElseClause node)
        {
            AddToken(node.ElseKeyword, _scheme.ControlKeyword);
            Visit(node.Body);
        }

        public override void VisitElseifClause(ElseifClause node)
        {
            AddToken(node.ElseifKeyword, _scheme.ControlKeyword);
            Visit(node.Condition);
            Visit(node.Body);
        }

        public override void VisitAbstractMethodDeclaration(AbstractMethodDeclarationSyntaxNode node)
        {
            Visit(node.OutputDescription);
            Visit(node.Name);
            Visit(node.InputDescription);
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntaxNode node)
        {
            Visit(node.Lhs);
            AddToken(node.AssignmentSign, _scheme.Operator);
            Visit(node.Rhs);
        }

        public override void VisitExpressionStatement(ExpressionStatementSyntaxNode node)
        {
            Visit(node.Expression);
        }

        public override void VisitArrayLiteralExpression(ArrayLiteralExpressionSyntaxNode node)
        {
            AddToken(node.OpeningSquareBracket, _scheme.Bracket);
            Visit(node.Nodes);
            AddToken(node.ClosingSquareBracket, _scheme.Bracket);
        }

        public override void VisitCellArrayLiteralExpression(CellArrayLiteralExpressionSyntaxNode node)
        {
            AddToken(node.OpeningBrace, _scheme.Bracket);
            Visit(node.Nodes);
            AddToken(node.ClosingBrace, _scheme.Bracket);
        }

        public override void VisitIdentifierNameExpression(IdentifierNameExpressionSyntaxNode node)
        {
            AddToken(node.Name, _scheme.Identifier);
        }

        public override void VisitForStatement(ForStatementSyntaxNode node)
        {
            AddToken(node.ForKeyword, _scheme.ControlKeyword);
            Visit(node.Assignment);
            Visit(node.OptionalCommas);
            Visit(node.Body);
            AddToken(node.EndKeyword, _scheme.ControlKeyword);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntaxNode node)
        {
            AddToken(node.SwitchKeyword, _scheme.ControlKeyword);
            Visit(node.SwitchExpression);
            Visit(node.OptionalCommas);
            Visit(node.Cases);
            AddToken(node.EndKeyword, _scheme.ControlKeyword);
        }

        public override void VisitWhileStatement(WhileStatementSyntaxNode node)
        {
            AddToken(node.WhileKeyword, _scheme.ControlKeyword);
            Visit(node.Condition);
            Visit(node.OptionalCommas);
            Visit(node.Body);
            AddToken(node.EndKeyword, _scheme.ControlKeyword);
        }

        public override void VisitUnquotedStringLiteralExpression(UnquotedStringLiteralExpressionSyntaxNode node)
        {
            AddToken(node.StringToken, _scheme.UnquotedStringLiteral);
        }

        public override void VisitStringLiteralExpression(StringLiteralExpressionSyntaxNode node)
        {
            AddToken(node.StringToken, _scheme.StringLiteral);
        }

        public override void VisitBinaryOperationExpression(BinaryOperationExpressionSyntaxNode node)
        {
            Visit(node.Lhs);
            AddToken(node.Operation, _scheme.Operator);
            Visit(node.Rhs);
        }

        public override void VisitFunctionCallExpression(FunctionCallExpressionSyntaxNode node)
        {
            Visit(node.FunctionName);
            AddToken(node.OpeningBracket, _scheme.Bracket);
            Visit(node.Nodes);
            AddToken(node.ClosingBracket, _scheme.Bracket);
        }

        public override void VisitSwitchCase(SwitchCaseSyntaxNode node)
        {
            AddToken(node.CaseKeyword, _scheme.ControlKeyword);
            Visit(node.CaseIdentifier);
            Visit(node.OptionalCommas);
            Visit(node.Body);
        }

        public override void VisitCatchClause(CatchClauseSyntaxNode node)
        {
            AddToken(node.CatchKeyword, _scheme.ControlKeyword);
            Visit(node.CatchBody);
        }

        public override void VisitTryCatchStatement(TryCatchStatementSyntaxNode node)
        {
            AddToken(node.TryKeyword, _scheme.ControlKeyword);
            Visit(node.TryBody);
            Visit(node.CatchClause);
            AddToken(node.EndKeyword, _scheme.ControlKeyword);
        }

        public override void VisitCommandExpression(CommandExpressionSyntaxNode node)
        {
            AddToken(node.CommandName, _scheme.Identifier);
            Visit(node.Arguments);
        }

        public override void VisitNumberLiteralExpression(NumberLiteralExpressionSyntaxNode node)
        {
            AddToken(node.Number, _scheme.NumberLiteral);
        }

        public override void VisitUnaryPrefixOperationExpression(UnaryPrefixOperationExpressionSyntaxNode node)
        {
            AddToken(node.Operation, _scheme.Operator);
            Visit(node.Operand);
        }

        public override void VisitUnaryPostfixOperationExpression(UnaryPostfixOperationExpressionSyntaxNode node)
        {
            Visit(node.Operand);
            AddToken(node.Operation, _scheme.Operator);
        }

        public override void VisitClassInvokationExpression(ClassInvokationExpressionSyntaxNode node)
        {
            Visit(node.MethodName);
            AddToken(node.AtSign, _scheme.Operator);
            Visit(node.BaseClassNameAndArguments);
        }

        public override void VisitAttributeAssignment(AttributeAssignmentSyntaxNode node)
        {
            AddToken(node.AssignmentSign, _scheme.Operator);
            Visit(node.Value);
        }

        public override void VisitAttribute(AttributeSyntaxNode node)
        {
            AddToken(node.Name, _scheme.Identifier);
            Visit(node.Assignment);
        }

        public override void VisitAttributeList(AttributeListSyntaxNode node)
        {
            AddToken(node.OpeningBracket, _scheme.Bracket);
            Visit(node.Nodes);
            AddToken(node.ClosingBracket, _scheme.Bracket);
        }

        public override void VisitCellArrayElementAccessExpression(CellArrayElementAccessExpressionSyntaxNode node)
        {
            Visit(node.Expression);
            AddToken(node.OpeningBrace, _scheme.Bracket);
            Visit(node.Nodes);
            AddToken(node.ClosingBrace, _scheme.Bracket);
        }

        public override void VisitCompoundNameExpression(CompoundNameExpressionSyntaxNode node)
        {
            Visit(node.Nodes);
        }

        public override void VisitDoubleQuotedStringLiteralExpression(DoubleQuotedStringLiteralExpressionSyntaxNode node)
        {
            AddToken(node.StringToken, _scheme.StringLiteral);
        }

        public override void VisitEmptyExpression(EmptyExpressionSyntaxNode node)
        {
        }

        public override void VisitEmptyStatement(EmptyStatementSyntaxNode node)
        {
            AddToken(node.Semicolon, _scheme.Punctuation);
        }

        public override void VisitEnumerationList(EnumerationListSyntaxNode node)
        {
            AddToken(node.EnumerationKeyword, _scheme.Keyword);
            Visit(node.Attributes);
            Visit(node.Items);
            AddToken(node.EndKeyword, _scheme.Keyword);
        }

        public override void VisitEventsList(EventsListSyntaxNode node)
        {
            AddToken(node.EventsKeyword, _scheme.Keyword);
            Visit(node.Attributes);
            Visit(node.Events);
            AddToken(node.EndKeyword, _scheme.Keyword);
        }

        public override void VisitEnumerationItemValue(EnumerationItemValueSyntaxNode node)
        {
            AddToken(node.OpeningBracket, _scheme.Punctuation);
            Visit(node.Values);
            AddToken(node.ClosingBracket, _scheme.Punctuation);
        }

        public override void VisitEnumerationItem(EnumerationItemSyntaxNode node)
        {
            AddToken(node.Name, _scheme.Identifier);
            Visit(node.Values);
            Visit(node.Commas);
        }

        public override void VisitFunctionInputDescription(FunctionInputDescriptionSyntaxNode node)
        {
            AddToken(node.OpeningBracket, _scheme.Bracket);
            Visit(node.ParameterList);
            AddToken(node.ClosingBracket, _scheme.Bracket);
        }

        public override void VisitFunctionOutputDescription(FunctionOutputDescriptionSyntaxNode node)
        {
            Visit(node.OutputList);
            AddToken(node.AssignmentSign, _scheme.Operator);
        }

        public override void VisitIndirectMemberAccessExpression(IndirectMemberAccessExpressionSyntaxNode node)
        {
            AddToken(node.OpeningBracket, _scheme.Bracket);
            Visit(node.Expression);
            AddToken(node.ClosingBracket, _scheme.Bracket);
        }

        public override void VisitLambdaExpression(LambdaExpressionSyntaxNode node)
        {
            AddToken(node.AtSign, _scheme.Operator);
            Visit(node.Input);
            Visit(node.Body);
        }

        public override void VisitNamedFunctionHandleExpression(NamedFunctionHandleExpressionSyntaxNode node)
        {
            AddToken(node.AtSign, _scheme.Operator);
            Visit(node.FunctionName);
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntaxNode node)
        {
            Visit(node.LeftOperand);
            AddToken(node.Dot, _scheme.Operator);
            Visit(node.RightOperand);
        }

        public override void VisitParenthesizedExpression(ParenthesizedExpressionSyntaxNode node)
        {
            AddToken(node.OpeningBracket, _scheme.Operator);
            Visit(node.Expression);
            AddToken(node.ClosingBracket, _scheme.Operator);
        }

        public ImmutableList<DisplayLine> GetLines()
        {
            return Builder.ToImmutable();
        }
    }
}
