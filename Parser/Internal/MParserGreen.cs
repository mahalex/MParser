using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Internal
{
    internal class MParserGreen
    {
        private List<(SyntaxToken token, Position position)> Pairs { get; }
        private int _index;
        private SyntaxToken CurrentToken => Pairs[_index].token;
        private Position CurrentPosition => Pairs[_index].position;
        private SyntaxToken PeekToken(int n) => Pairs[_index + n].token;
        private SyntaxFactory Factory { get; }
        public DiagnosticsBag Diagnostics { get; }

        public MParserGreen(List<(SyntaxToken, Position)> pairs, SyntaxFactory factory)
        {
            Pairs = pairs;
            Factory = factory;
            Diagnostics = new DiagnosticsBag();
        }

        private SyntaxToken EatToken()
        {
            var token = CurrentToken;
            _index++;
            return token;
        }

        private SyntaxToken EatToken(TokenKind kind)
        {
            var token = CurrentToken;
            if (token.Kind != kind)
            {
                Diagnostics.ReportUnexpectedToken(kind, token.Kind);
                return TokenFactory.CreateMissing(kind, null, null);
            }
            _index++;
            return token;
        }

        private SyntaxToken EatIdentifier()
        {
            return EatToken(TokenKind.IdentifierToken);
        }

        private bool IsIdentifier(SyntaxToken token, string s)
        {
            return token.Kind == TokenKind.IdentifierToken && token.Text == s;
        }

        private SyntaxToken EatIdentifier(string s)
        {
            var token = CurrentToken;
            if (token.Kind != TokenKind.IdentifierToken)
            {
                Diagnostics.ReportUnexpectedToken(TokenKind.IdentifierToken, token.Kind);
                return TokenFactory.CreateMissing(TokenKind.IdentifierToken, null, null);
            }

            if (token.Text != s)
            {
                Diagnostics.ReportUnexpectedToken(TokenKind.IdentifierToken, token.Kind);
                return TokenFactory.CreateMissing(TokenKind.IdentifierToken, null, null);
            }

            _index++;
            return token;
        }

        private SyntaxToken? PossiblyEatIdentifier(string s)
        {
            var token = CurrentToken;
            if (token.Kind == TokenKind.IdentifierToken && token.Text == s)
            {
                return EatToken();
            }

            return null;
        }

        private SyntaxToken EatPossiblyMissingIdentifier(string s)
        {
            var token = CurrentToken;
            if (token.Kind == TokenKind.IdentifierToken && token.Text == s)
            {
                return EatToken();
            }

            return TokenFactory.CreateMissing(
                TokenKind.IdentifierToken,
                new List<SyntaxTrivia>(),
                new List<SyntaxTrivia>());
        }

        private SyntaxList? ParseFunctionOutputList()
        {
            var outputs = new SyntaxListBuilder();
            var firstToken = true;
            while (CurrentToken.Kind != TokenKind.CloseSquareBracketToken)
            {
                if (!firstToken && CurrentToken.Kind == TokenKind.CommaToken)
                {
                    outputs.Add(EatToken());
                }

                firstToken = false;
                outputs.Add(Factory.IdentifierNameSyntax(EatToken(TokenKind.IdentifierToken)));
            }

            return outputs.ToList();
        }

        private FunctionOutputDescriptionSyntaxNode? ParseFunctionOutputDescription()
        {
            SyntaxToken assignmentSign;
            var builder = new SyntaxListBuilder();
            
            if (CurrentToken.Kind == TokenKind.OpenSquareBracketToken)
            {
                builder.Add(EatToken());
                var outputs = ParseFunctionOutputList();
                if (outputs != null)
                {
                    builder.AddRange(outputs);
                }

                builder.Add(EatToken(TokenKind.CloseSquareBracketToken));
                assignmentSign = EatToken(TokenKind.EqualsToken);
            }
            else if (CurrentToken.Kind == TokenKind.IdentifierToken)
            {
                if (PeekToken(1).Kind == TokenKind.EqualsToken)
                {
                    var identifierToken = EatIdentifier();
                    builder.Add(Factory.IdentifierNameSyntax(identifierToken));
                    assignmentSign = EatToken(TokenKind.EqualsToken);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return Factory.FunctionOutputDescriptionSyntax(builder.ToList(), assignmentSign);
        }

        private SyntaxList ParseParameterList()
        {
            var builder = new SyntaxListBuilder();
            var firstToken = true;
            while (CurrentToken.Kind != TokenKind.CloseParenthesisToken)
            {
                if (!firstToken)
                {
                    builder.Add(EatToken(TokenKind.CommaToken));
                }
                else
                {
                    firstToken = false;
                }

                if (CurrentToken.Kind == TokenKind.TildeToken)
                {
                    var notToken = EatToken();
                    builder.Add(notToken);
                }
                else
                {
                    var identifierToken = EatToken(TokenKind.IdentifierToken);
                    builder.Add(Factory.IdentifierNameSyntax(identifierToken));
                }
            }

            return builder.ToList();
        }

        private FunctionInputDescriptionSyntaxNode? ParseFunctionInputDescription()
        {
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                var openingBracket = EatToken(TokenKind.OpenParenthesisToken);
                var parameterList = ParseParameterList();
                var closingBracket = EatToken(TokenKind.CloseParenthesisToken);
                return Factory.FunctionInputDescriptionSyntax(
                    openingBracket,
                    parameterList,
                    closingBracket);
            }
            else
            {
                return null;
            }
        } 

        private FunctionDeclarationSyntaxNode ParseFunctionDeclaration()
        {
            var functionKeyword = EatIdentifier("function");
            var outputDescription = ParseFunctionOutputDescription();
            var name = EatToken(TokenKind.IdentifierToken);
            var inputDescription = ParseFunctionInputDescription();
            var commas = ParseOptionalCommas();
            var body = ParseStatementList();
            var endKeyword = ParseEndKeyword();
            //var endKeyword = 
            return Factory.FunctionDeclarationSyntax(
                functionKeyword,
                outputDescription,
                name,
                inputDescription,
                commas,
                body,
                endKeyword);
        }

        private EndKeywordSyntaxNode? ParseEndKeyword()
        {
            var keyword = EatPossiblyMissingIdentifier("end");
            return keyword is null ? null : Factory.EndKeywordSyntax(keyword);
        }

        internal struct ParseOptions
        {
            public bool ParsingArrayElements { get; set; }
            
            public static ParseOptions Default = new ParseOptions { ParsingArrayElements = false };
        }

        private ExpressionSyntaxNode? ParseExpression()
        {
            return ParseExpression(ParseOptions.Default);
        }

        private ExpressionSyntaxNode? ParseExpression(ParseOptions options)
        {
            return ParseSubExpression(options, SyntaxFacts.Precedence.Expression);
        }

        private ArrayLiteralExpressionSyntaxNode ParseArrayLiteral()
        {
            var openingSquareBracket = EatToken(TokenKind.OpenSquareBracketToken);
            var elements = ParseArrayElementList(TokenKind.CloseSquareBracketToken);
            var closingSquareBracket = EatToken(TokenKind.CloseSquareBracketToken);
            return Factory.ArrayLiteralExpressionSyntax(
                openingSquareBracket,
                elements,
                closingSquareBracket);
        }

        private CellArrayLiteralExpressionSyntaxNode ParseCellArrayLiteral()
        {
            var openingBrace = EatToken(TokenKind.OpenBraceToken);
            var elements = ParseArrayElementList(TokenKind.CloseBraceToken);
            var closingBrace = EatToken(TokenKind.CloseBraceToken);
            return Factory.CellArrayLiteralExpressionSyntax(
                openingBrace,
                elements,
                closingBrace);
        }


        private SyntaxList ParseArrayElementList(TokenKind closing)
        {
            var builder = new SyntaxListBuilder();
            var firstToken = true;
            while (CurrentToken.Kind != closing)
            {
                if (!firstToken)
                {
                    if (CurrentToken.Kind == TokenKind.CommaToken
                        || CurrentToken.Kind == TokenKind.SemicolonToken)
                    {
                        builder.Add(EatToken());
                    }
                }
                else
                {
                    firstToken = false;
                }

                var expression = ParseExpression(new ParseOptions { ParsingArrayElements = true });
                if (expression != null)
                {
                    builder.Add(expression);
                }
            }

            return builder.ToList();
        }

        private ExpressionSyntaxNode? ParseTerm(ParseOptions options)
        {
            var token = CurrentToken;
            ExpressionSyntaxNode? expression = null;
            switch (token.Kind)
            {
                case TokenKind.NumberLiteralToken:
                    expression = Factory.NumberLiteralSyntax(EatToken());
                    break;
                case TokenKind.StringLiteralToken:
                    expression = Factory.StringLiteralSyntax(EatToken());
                    break;
                case TokenKind.DoubleQuotedStringLiteralToken:
                    expression = Factory.DoubleQuotedStringLiteralSyntax(EatToken());
                    break;
                case TokenKind.OpenSquareBracketToken:
                    expression = ParseArrayLiteral();
                    break;
                case TokenKind.OpenBraceToken:
                    expression = ParseCellArrayLiteral();
                    break;
                case TokenKind.ColonToken:
                    expression = Factory.EmptyExpressionSyntax();
                    break;
                case TokenKind.OpenParenthesisToken:
                    expression = ParseParenthesizedExpression();
                    break;
                default:
                    var id = EatToken(TokenKind.IdentifierToken);
                    expression = Factory.IdentifierNameSyntax(id);
                    break;
            }

            if (expression == null)
            {
                return null;
            }
            return ParsePostfix(options, expression);
        }

        private ExpressionSyntaxNode ParsePostfix(ParseOptions options, ExpressionSyntaxNode expression)
        {
            while (true)
            {
                var token = CurrentToken;
                switch (token.Kind)
                {
                    case TokenKind.OpenBraceToken: // cell array element access
                        if (options.ParsingArrayElements && expression.TrailingTrivia.Any())
                        {
                            return expression;
                        }
                        var openingBrace = EatToken();
                        var indices = ParseArrayElementList(TokenKind.CloseBraceToken);
                        var closingBrace = EatToken(TokenKind.CloseBraceToken);
                        expression = Factory.CellArrayElementAccessExpressionSyntax(
                            expression,
                            openingBrace,
                            indices,
                            closingBrace
                        );
                        break;
                    case TokenKind.OpenParenthesisToken: // function call
                        if (options.ParsingArrayElements && expression.TrailingTrivia.Any())
                        {
                            return expression;
                        }
                        var openingBracket = EatToken();
                        var parameters = ParseFunctionCallParameterList();
                        var closingBracket = EatToken(TokenKind.CloseParenthesisToken);
                        expression = Factory.FunctionCallExpressionSyntax(
                            expression,
                            openingBracket,
                            parameters,
                            closingBracket);
                        break;
                    case TokenKind.DotToken: // member access
                        if (expression is IdentifierNameSyntaxNode
                            || expression is MemberAccessSyntaxNode
                            || expression is FunctionCallExpressionSyntaxNode
                            || expression is CellArrayElementAccessExpressionSyntaxNode)
                        {
                            var dot = EatToken();
                            var member = ParseMemberAccess();
                            expression = Factory.MemberAccessSyntax(expression, dot, member);
                        }
                        else
                        {
                            throw new ParsingException(
                                $"Unexpected token {token} at {CurrentPosition}.");
                        }

                        break;
                    case TokenKind.ApostropheToken:
                    case TokenKind.DotApostropheToken:
                        var operation = EatToken();
                        expression = Factory.UnaryPostixOperationExpressionSyntax(expression, operation);
                        break;
                    case TokenKind.UnquotedStringLiteralToken:
                        return ParseCommandExpression(expression);
                    case TokenKind.AtToken:
                        if (expression.TrailingTrivia.Any())
                        {
                            return expression;
                        }
                        return ParseBaseClassInvokation(expression);
                    default:
                        return expression;
                }
            }
        }

        private CommandExpressionSyntaxNode ParseCommandExpression(ExpressionSyntaxNode expression)
        {
            if (expression is IdentifierNameSyntaxNode idNameNode)
            {
                var builder = new SyntaxListBuilder<UnquotedStringLiteralSyntaxNode>();
                while (CurrentToken.Kind == TokenKind.UnquotedStringLiteralToken)
                {
                    builder.Add(Factory.UnquotedStringLiteralSyntax(EatToken()));
                }

                return Factory.CommandExpressionSyntax(idNameNode, builder.ToList());
            }

            if (expression is null)
            {
                throw new Exception("Command expression identifier cannot be empty.");
            }
            throw new ParsingException($"Unexpected token \"{CurrentToken}\" while parsing expression \"{expression.FullText}\" at {CurrentPosition}.");
        }

        private BaseClassInvokationSyntaxNode ParseBaseClassInvokation(ExpressionSyntaxNode expression)
        {
            if (expression is IdentifierNameSyntaxNode methodName
                && !expression.TrailingTrivia.Any())
            {
                var atToken = EatToken();
                var baseClassNameWithArguments = ParseExpression();
                if (baseClassNameWithArguments is null)
                {
                    throw new Exception($"Base class name cannot be empty.");
                }
                return Factory.BaseClassInvokationSyntax(methodName, atToken, baseClassNameWithArguments);
            }
            if (expression is MemberAccessSyntaxNode memberAccess
                && !expression.TrailingTrivia.Any())
            {
                var atToken = EatToken();
                var baseClassNameWithArguments = ParseExpression();
                if (baseClassNameWithArguments is null)
                {
                    throw new Exception($"Base class name cannot be empty.");
                }
                return Factory.BaseClassInvokationSyntax(memberAccess, atToken, baseClassNameWithArguments);
            }
            throw new ParsingException($"Unexpected token \"{CurrentToken}\" at {CurrentPosition}.");
        }

        private ExpressionSyntaxNode ParseMemberAccess()
        {
            if (CurrentToken.Kind == TokenKind.IdentifierToken)
            {
                return Factory.IdentifierNameSyntax(EatToken());
            }
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                var openingBracket = EatToken();
                var indirectMember = ParseExpression();
                if (indirectMember is null)
                {
                    throw new Exception("Indirect member invokation cannot be empty.");
                }
                var closingBracket = EatToken(TokenKind.CloseParenthesisToken);
                return Factory.IndirectMemberAccessSyntax(
                    openingBracket,
                    indirectMember,
                    closingBracket);
            }
            throw new ParsingException($"Unexpected token {CurrentToken} at {CurrentPosition}.");
        }

        private SyntaxList ParseFunctionCallParameterList()
        {
            var builder = new SyntaxListBuilder();
            var firstToken = true;
            while (CurrentToken.Kind != TokenKind.CloseParenthesisToken)
            {
                if (!firstToken)
                {
                    builder.Add(EatToken(TokenKind.CommaToken));
                }
                else
                {
                    firstToken = false;
                }

                var expression = ParseExpression();
                if (expression is null)
                {
                    throw new Exception("Function call parameter cannot be empty.");
                }
                builder.Add(expression);
            }

            return builder.ToList();
        }

        private ParenthesizedExpressionSyntaxNode ParseParenthesizedExpression()
        {
            var openParen = EatToken(TokenKind.OpenParenthesisToken);
            var expression = ParseExpression();
            if (expression is null)
            {
                throw new Exception("Parenthesized expression cannot be empty.");
            }
            var closeParen = EatToken(TokenKind.CloseParenthesisToken);
            return Factory.ParenthesizedExpressionSyntax(
                openParen,
                expression,
                closeParen);
        }

        private CompoundNameSyntaxNode ParseCompoundName()
        {
            var lastToken = EatToken(TokenKind.IdentifierToken);
            var firstName = lastToken;
            var builder = new SyntaxListBuilder();
            builder.Add(firstName);
            while (CurrentToken.Kind == TokenKind.DotToken
                   && !lastToken.TrailingTrivia.Any())
            {
                var dot = EatToken();
                builder.Add(dot);
                lastToken = EatToken(TokenKind.IdentifierToken);
                builder.Add(lastToken);
            }

            return Factory.CompoundNameSyntax(builder.ToList());
        }

        private FunctionHandleSyntaxNode ParseFunctionHandle()
        {
            var atSign = EatToken();
            if (CurrentToken.Kind == TokenKind.IdentifierToken)
            {
                var compoundName = ParseCompoundName();
                return Factory.NamedFunctionHandleSyntax(
                    atSign,
                    compoundName);
            }
            else if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                var inputs = ParseFunctionInputDescription();
                if (inputs is null)
                {
                    throw new Exception($"Lambda expression inputs cannot be empty.");
                }
                var body = ParseExpression();
                if (body is null)
                {
                    throw new Exception($"Lambda expression body cannot be empty.");
                }
                return Factory.LambdaSyntax(atSign, inputs, body);
            }
            throw new ParsingException($"Unexpected token {CurrentToken} while parsing function handle at {CurrentPosition}.");
        }

        private ExpressionSyntaxNode? ParseSubExpression(
            ParseOptions options,
            SyntaxFacts.Precedence precedence)
        {
            ExpressionSyntaxNode? lhs;
            if (SyntaxFacts.IsUnaryOperator(CurrentToken.Kind))
            {
                var operation = EatToken();
                var unaryTokenKind = SyntaxFacts.ConvertToUnaryTokenKind(operation.Kind);
                var newPrecedence = SyntaxFacts.GetPrecedence(unaryTokenKind);
                var operand = ParseSubExpression(options, newPrecedence);
                if (operand == null)
                {
                    if (options.ParsingArrayElements && operation.Kind == TokenKind.TildeToken)
                    {
                        operand = Factory.EmptyExpressionSyntax();
                    }
                    else
                    {
                        throw new ParsingException($"Unexpected token {CurrentToken} at {CurrentPosition}.");
                    }
                }
                lhs = Factory.UnaryPrefixOperationExpressionSyntax(operation, operand);
            }
            else if (CurrentToken.Kind == TokenKind.AtToken)
            {
                return ParseFunctionHandle();
            }
            else
            {
                lhs = ParseTerm(options);
                if (lhs is null)
                {
                    throw new Exception("Left-hand side in subexpression cannot be empty.");
                }
            }

            while (true)
            {
                var token = CurrentToken;
                if (SyntaxFacts.IsBinaryOperator(token.Kind))
                {
                    var newPrecedence = SyntaxFacts.GetPrecedence(token.Kind);
                    if (newPrecedence < precedence)
                    {
                        break;
                    }

                    if (newPrecedence == precedence && SyntaxFacts.IsLeftAssociative(token.Kind))
                    {
                        break;
                    }

                    EatToken();
                    var rhs = ParseSubExpression(options, newPrecedence);
                    if (rhs == null)
                    {
                        if (token.Kind == TokenKind.ColonToken) // for parsing things like a{:}
                        {
                            rhs = Factory.EmptyExpressionSyntax();
                        }
                        else
                        {
                            throw new Exception("Right-hand side in subexpression cannot be empty.");
                        }
                    }
                    
                    if (token.Kind == TokenKind.EqualsToken)
                    {
                        lhs = Factory.AssignmentExpressionSyntax(lhs, token, rhs);
                    }
                    else
                    {
                        lhs = Factory.BinaryOperationExpressionSyntax(lhs, token, rhs);
                    }
                }
                else
                {
                    break;
                }
            }

            return lhs;
        }

        private SyntaxList<SyntaxToken> ParseOptionalCommas()
        {
            var builder = new SyntaxListBuilder<SyntaxToken>();
            while (CurrentToken.Kind == TokenKind.CommaToken)
            {
                builder.Add(EatToken());
            }

            return builder.ToList();
        }

        private SyntaxList<SyntaxToken> ParseOptionalSemicolonsOrCommas()
        {
            var builder = new SyntaxListBuilder<SyntaxToken>();
            while (CurrentToken.Kind == TokenKind.CommaToken
                   || CurrentToken.Kind == TokenKind.SemicolonToken)
            {
                builder.Add(EatToken());
            }

            return builder.ToList();
        }

        private SwitchCaseSyntaxNode ParseSwitchCase()
        {
            var caseKeyword = EatIdentifier("case");
            var caseId = ParseExpression();
            if (caseId is null)
            {
                throw new Exception("Case label cannot be empty.");
            }
            var commas = ParseOptionalCommas();
            var statementList = ParseStatementList();
            return Factory.SwitchCaseSyntax(caseKeyword, caseId, commas, statementList);
        }

        private SwitchStatementSyntaxNode ParseSwitchStatement()
        {
            var switchKeyword = EatIdentifier("switch");
            var expression = ParseExpression();
            if (expression is null)
            {
                throw new Exception("Match expression in switch statement cannot be empty.");
            }
            var commas = ParseOptionalCommas();
            var builder = new SyntaxListBuilder<SwitchCaseSyntaxNode>();
            while (IsIdentifier(CurrentToken, "case"))
            {
                builder.Add(ParseSwitchCase());
            }
            var endKeyword = EatIdentifier("end");
            return Factory.SwitchStatementSyntax(
                switchKeyword,
                expression,
                commas,
                builder.ToList(),
                endKeyword);
        }

        private WhileStatementSyntaxNode ParseWhileStatement()
        {
            var whileKeyword = EatIdentifier("while");
            var condition = ParseExpression();
            if (condition is null)
            {
                throw new Exception("Condition in while statement cannot be empty.");
            }

            var commas = ParseOptionalCommas();
            var body = ParseStatementList();
            var endKeyword = EatIdentifier("end");
            return Factory.WhileStatementSyntax(
                whileKeyword,
                condition,
                commas,
                body,
                endKeyword);
        }

        private ElseifClause ParseElseifClause()
        {
            var elseifKeyword = EatIdentifier("elseif");
            var condition = ParseExpression();
            if (condition is null)
            {
                throw new Exception("Condition in elseif clause cannot be empty.");
            }
            var commas = ParseOptionalCommas();
            var body = ParseStatementList();
            return Factory.ElseifClause(elseifKeyword, condition, commas, body);
        }

        private ElseClause ParseElseClause()
        {
            var elseKeyword = EatIdentifier("else");
            var body = ParseStatementList();
            return Factory.ElseClause(elseKeyword, body);
        }

        private IfStatementSyntaxNode ParseIfStatement()
        {
            var ifKeyword = EatIdentifier();
            var condition = ParseExpression();
            if (condition is null)
            {
                throw new Exception("Condition in if statement cannot be empty.");
            }
            var commas = ParseOptionalSemicolonsOrCommas();
            var body = ParseStatementList();
            var elseifClauses = new SyntaxListBuilder<ElseifClause>();
            ElseClause? elseClause = null;
            while (true)
            {
                var token = CurrentToken;
                if (IsIdentifier(token, "elseif"))
                {
                    var elseifClause = ParseElseifClause();
                    elseifClauses.Add(elseifClause);
                    continue;
                } else if (IsIdentifier(token, "else"))
                {
                    elseClause = ParseElseClause();
                    break;
                } else if (IsIdentifier(token, "end"))
                {
                    break;
                }
                throw new ParsingException($"Unexpected token \"{token}\" while parsing \"if\" statement at {CurrentPosition}.");
            }
            var endKeyword = EatIdentifier("end");
            return Factory.IfStatementSyntax(
                ifKeyword,
                condition,
                commas,
                body,
                elseifClauses.ToList(),
                elseClause,
                endKeyword);
        }

        private ForStatementSyntaxNode ParseForStatement()
        {
            var forKeyword = EatIdentifier("for");
            var expression = ParseExpression();
            if (!(expression is AssignmentExpressionSyntaxNode))
            {
                throw new ParsingException($"Unexpected expression \"{expression}\" while parsing FOR statement at {CurrentPosition}.");
            }

            var forAssignment = (AssignmentExpressionSyntaxNode) expression;
            var commas = ParseOptionalSemicolonsOrCommas();
            var body = ParseStatementList();
            var endKeyword = EatIdentifier("end");
            return Factory.ForStatementSyntax(
                forKeyword,
                forAssignment,
                commas,
                body,
                endKeyword);
        }

        private CatchClauseSyntaxNode? ParseCatchClause()
        {
            if (IsIdentifier(CurrentToken, "catch"))
            {
                var catchKeyword = EatIdentifier();
                var catchBody = ParseStatementList();
                return Factory.CatchClauseSyntax(
                    catchKeyword,
                    catchBody);
            }

            return null;
        }

        private TryCatchStatementSyntaxNode ParseTryCatchStatement()
        {
            var tryKeyword = EatIdentifier("try");
            var tryBody = ParseStatementList();
            var catchClause = ParseCatchClause();
            var endKeyword = EatIdentifier("end");
            return Factory.TryCatchStatementSyntax(tryKeyword, tryBody, catchClause, endKeyword);
        }

        private ExpressionStatementSyntaxNode ParseExpressionStatement()
        {
            var expression = ParseExpression();
            if (expression is null)
            {
                throw new Exception("Expression statement cannot be empty.");
            }

            return Factory.ExpressionStatementSyntax(expression);
        }

        private AttributeAssignmentSyntaxNode? ParseAttributeAssignment()
        {
            if (CurrentToken.Kind == TokenKind.EqualsToken)
            {
                var assignmentSign = EatToken();
                var value = ParseExpression();
                if (value is null)
                {
                    throw new Exception("Right-hand side in attribute assignment cannot be empty.");
                }
                return Factory.AttributeAssignmentSyntax(assignmentSign, value);
            }

            return null;
        }

        private AttributeSyntaxNode ParseAttribute()
        {
            var name = Factory.IdentifierNameSyntax(EatToken(TokenKind.IdentifierToken));
            var assignment = ParseAttributeAssignment();
            return Factory.AttributeSyntax(name, assignment);
        }

        private AttributeListSyntaxNode ParseAttributesList()
        {
            var openingBracket = EatToken();
            var first = true;
            var builder = new SyntaxListBuilder();
            while (CurrentToken.Kind != TokenKind.CloseParenthesisToken)
            {
                if (!first)
                {
                    var comma = EatToken(TokenKind.CommaToken);
                    builder.Add(comma);
                }

                first = false;
                builder.Add(ParseAttribute());
            }

            var closingBracket = EatToken();
            return Factory.AttributeListSyntax(openingBracket, builder.ToList(), closingBracket);
        }

        private AbstractMethodDeclarationSyntaxNode ParseAbstractMethodDeclaration()
        {
            var outputDescription = ParseFunctionOutputDescription();
            var name = ParseCompoundName();
            var inputDescription = ParseFunctionInputDescription();
            return Factory.AbstractMethodDeclarationSyntax(outputDescription, name, inputDescription);
        }

        private StatementSyntaxNode ParseMethodDeclaration()
        {
            if (IsIdentifier(CurrentToken, "function"))
            {
                return ParseMethodDefinition();
            }

            if (CurrentToken.Kind == TokenKind.OpenSquareBracketToken
                || CurrentToken.Kind == TokenKind.IdentifierToken)
            {
                return ParseAbstractMethodDeclaration();
            }

            if (CurrentToken.Kind == TokenKind.SemicolonToken)
            {
                return Factory.EmptyStatementSyntax(EatToken());
            }
            throw new ParsingException($"Unexpected token {CurrentToken} while parsing method declaration at {CurrentPosition}.");
        }

        private MethodDefinitionSyntaxNode ParseMethodDefinition()
        {
            var functionKeyword = EatIdentifier("function");
            var outputDescription = ParseFunctionOutputDescription();
            var name = ParseCompoundName();
            var inputDescription = ParseFunctionInputDescription();
            var commas = ParseOptionalCommas();
            var body = ParseStatementList();
            var endKeyword = ParseEndKeyword();
            return Factory.MethodDefinitionSyntax(
                functionKeyword,
                outputDescription,
                name,
                inputDescription,
                commas,
                body,
                endKeyword);
        }

        private MethodsListSyntaxNode ParseMethods()
        {
            var methodsKeyword = EatToken();
            AttributeListSyntaxNode? attributes = null;
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                attributes = ParseAttributesList();
            }

            var builder = new SyntaxListBuilder();
            while (!IsIdentifier(CurrentToken, "end"))
            {
                var method = ParseMethodDeclaration();
                builder.Add(method);
            }

            var endKeyword = EatToken();
            return Factory.MethodsListSyntax(methodsKeyword, attributes, builder.ToList(), endKeyword);
        }

        private GreenNode? ParsePropertyDeclaration()
        {
            if (CurrentToken.Kind == TokenKind.CommaToken)
            {
                return EatToken();
            }
            return ParseStatement();
        }

        private SyntaxNode? ParseEventDeclaration()
        {
            return ParseStatement();
        }

        private PropertiesListSyntaxNode ParseProperties()
        {
            var propertiesKeyword = EatToken();
            AttributeListSyntaxNode? attributes = null;
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                attributes = ParseAttributesList();
            }

            var builder = new SyntaxListBuilder();
            while (!IsIdentifier(CurrentToken, "end"))
            {
                var declaration = ParsePropertyDeclaration();
                if (declaration is null)
                {
                    throw new Exception("Property declaration cannot be null.");
                }
                builder.Add(declaration);
            }

            var endKeyword = EatToken();
            return Factory.PropertiesListSyntax(propertiesKeyword, attributes, builder.ToList(), endKeyword);
        }

        private EnumerationItemValueSyntaxNode? ParseEnumerationValue()
        {
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                var builder = new SyntaxListBuilder();
                var openingBracket = EatToken(TokenKind.OpenParenthesisToken);
                var expression = ParseExpression() ?? Factory.EmptyExpressionSyntax();
                builder.Add(expression);
                while (CurrentToken.Kind == TokenKind.CommaToken)
                {
                    builder.Add(EatToken());
                    var nextExpression = ParseExpression();
                    if (nextExpression is null)
                    {
                        throw new Exception("Enumeration identifier cannot be empty.");
                    }
                    builder.Add(nextExpression);
                }
                var closingBracket = EatToken(TokenKind.CloseParenthesisToken);
                return Factory.EnumerationItemValueSyntax(openingBracket, builder.ToList(), closingBracket);
            }
            return null;
        }

        private EnumerationItemSyntaxNode ParseEnumerationItem()
        {
            var name = Factory.IdentifierNameSyntax(EatToken());
            var values = ParseEnumerationValue();
            var commas = ParseOptionalCommas();
            return Factory.EnumerationItemSyntax(name, values, commas);
        }

        private EnumerationListSyntaxNode ParseEnumeration()
        {
            var enumerationKeyword = EatToken();
            var builder = new SyntaxListBuilder<EnumerationItemSyntaxNode>();
            AttributeListSyntaxNode? attributes = null;
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                attributes = ParseAttributesList();
            }
            while (!IsIdentifier(CurrentToken, "end"))
            {
                var item = ParseEnumerationItem();
                builder.Add(item);
            }

            var endkeyword = EatToken();
            return Factory.EnumerationListSyntax(
                enumerationKeyword,
                attributes,
                builder.ToList(),
                endkeyword);
        }

        private SyntaxNode ParseEvents()
        {
            var eventsKeyword = EatToken();
            AttributeListSyntaxNode? attributes = null;
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                attributes = ParseAttributesList();
            }

            var builder = new SyntaxListBuilder();
            while (!IsIdentifier(CurrentToken, "end"))
            {
                var eventDeclaration = ParseEventDeclaration();
                if (eventDeclaration is null)
                {
                    throw new Exception("Event declaration cannot be empty.");
                }
                builder.Add(eventDeclaration);
            }

            var endKeyword = EatToken();
            return Factory.EventsListSyntax(eventsKeyword, attributes, builder.ToList(), endKeyword);
        }

        private SyntaxList ParseBaseClassNames()
        {
            var builder = new SyntaxListBuilder();
            builder.Add(ParseCompoundName());
            while (CurrentToken.Kind == TokenKind.AmpersandToken)
            {
                builder.Add(EatToken());
                builder.Add(ParseCompoundName());
            }

            return builder.ToList();
        }

        private BaseClassListSyntaxNode ParseBaseClassList()
        {
            var lessSign = EatToken();
            var baseClassNames = ParseBaseClassNames();
            return Factory.BaseClassListSyntax(lessSign, baseClassNames);
        }
        
        private StatementSyntaxNode ParseClassDeclaration()
        {
            var classdefKeyword = EatToken();
            AttributeListSyntaxNode? attributes = null;
            if (CurrentToken.Kind == TokenKind.OpenParenthesisToken)
            {
                attributes = ParseAttributesList();
            }
            var className = Factory.IdentifierNameSyntax(EatToken(TokenKind.IdentifierToken));
            BaseClassListSyntaxNode? baseClassList = null;
            if (CurrentToken.Kind == TokenKind.LessToken)
            {
                baseClassList = ParseBaseClassList();
            }

            var builder = new SyntaxListBuilder();
            while (!IsIdentifier(CurrentToken, "end"))
            {
                if (IsIdentifier(CurrentToken, "methods"))
                {
                    var methods = ParseMethods();
                    builder.Add(methods);
                }
                else if (IsIdentifier(CurrentToken, "properties"))
                {
                    var properties = ParseProperties();
                    builder.Add(properties);
                }
                else if (IsIdentifier(CurrentToken, "events"))
                {
                    var events = ParseEvents();
                    builder.Add(events);
                }
                else if (IsIdentifier(CurrentToken, "enumeration"))
                {
                    var enumeration = ParseEnumeration();
                    builder.Add(enumeration);
                }
                else
                {
                    throw new ParsingException($"Unknown token \"{CurrentToken}\" while parsing class definition at {CurrentPosition}.");
                }
            }

            var endKeyword = EatToken();
            return Factory.ClassDeclarationSyntax(
                classdefKeyword,
                attributes,
                className,
                baseClassList,
                builder.ToList(),
                endKeyword);
        }

        private StatementSyntaxNode? ParseStatement()
        {
            if (CurrentToken.Kind == TokenKind.IdentifierToken)
            {
                switch (CurrentToken.Text)
                {
                    case "function":
                        return ParseFunctionDeclaration();
                    case "classdef":
                        return ParseClassDeclaration();
                    case "switch":
                        return ParseSwitchStatement();
                    case "while":
                        return ParseWhileStatement();
                    case "if":
                        return ParseIfStatement();
                    case "for":
                        return ParseForStatement();
                    case "try":
                        return ParseTryCatchStatement();
                    case "case":
                    case "catch":
                    case "else":
                    case "elseif":
                    case "end":
                        return null;
                }
            }

            if (CurrentToken.Kind == TokenKind.OpenSquareBracketToken)
            {
                return ParseExpressionStatement();
            }

            if (CurrentToken.Kind == TokenKind.SemicolonToken)
            {
                return Factory.EmptyStatementSyntax(EatToken());
            }
            return ParseExpressionStatement();
        }
        
        private SyntaxList ParseStatementList()
        {
            var builder = new SyntaxListBuilder();
            while (CurrentToken.Kind != TokenKind.EndOfFileToken)
            {
                var node = ParseStatement();
                if (node == null)
                {
                    break;
                }
                builder.Add(node);
                while (CurrentToken.Kind == TokenKind.CommaToken
                    || CurrentToken.Kind == TokenKind.SemicolonToken)
                {
                    builder.Add(EatToken());
                } 
            }

            return builder.ToList();
        }
        
        public FileSyntaxNode ParseFile()
        {
            var statementList = ParseStatementList();
            var endOfFileToken = EatToken();
            return Factory.FileSyntax(statementList, endOfFileToken);
        }

        public RootSyntaxNode ParseRoot()
        {
            var file = ParseFile();
            return Factory.RootSyntax(file);
        }
    }
}