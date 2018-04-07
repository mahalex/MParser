using System;
using System.Collections.Generic;
using System.Linq;
using Lexer;

namespace Parser
{
    public class MParser
    {
        public enum Precedence
        {
            // see https://mathworks.com/help/matlab/matlab_prog/operator-precedence.html
            Expression = 0,
            Assignment,
            LogicalOr,
            LogicalAnd,
            BitwiseOr,
            BitwiseAnd,
            Relational,
            Colon,
            Additive,
            Multiplicative,
            Unary,
            WeirdPower,
            Power
        }

        private static Precedence GetPrecedence(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Assignment:
                    return Precedence.Assignment;
                case TokenKind.LogicalOr:
                    return Precedence.LogicalOr;
                case TokenKind.LogicalAnd:
                    return Precedence.LogicalAnd;
                case TokenKind.BitwiseOr:
                    return Precedence.BitwiseOr;
                case TokenKind.BitwiseAnd:
                    return Precedence.BitwiseAnd;
                case TokenKind.Less:
                case TokenKind.LessOrEqual:
                case TokenKind.Greater:
                case TokenKind.GreaterOrEqual:
                case TokenKind.Equality:
                case TokenKind.Inequality:
                    return Precedence.Relational;
                case TokenKind.Colon:
                    return Precedence.Colon;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return Precedence.Additive;
                case TokenKind.Multiply:
                case TokenKind.DotMultiply:
                case TokenKind.Divide:
                case TokenKind.DotDivide:
                case TokenKind.Backslash:
                case TokenKind.DotBackslash:
                    return Precedence.Multiplicative;
                case TokenKind.Not:
                    return Precedence.Unary;
                case TokenKind.Power:
                case TokenKind.DotPower:
                case TokenKind.Transpose:
                case TokenKind.DotTranspose:
                    return Precedence.Power;
                default:
                    return Precedence.Expression;
            }
        }
        
        private List<Token> Tokens { get; }
        private int _index;
        private Token CurrentToken => Tokens[_index];
        private Token PeekToken(int n) => Tokens[_index + n];
        private SyntaxFactory Factory { get; }
        
        public MParser(List<Token> tokens)
        {
            Tokens = tokens;
            _index = 0;
            Factory = new SyntaxFactory();
        }

        private Token EatToken()
        {
            var token = Tokens[_index];
            //Console.WriteLine($"{token} at {token.PureToken.Position}");
            _index++;
            return token;
        }

        private Token EatToken(TokenKind kind)
        {
            var token = Tokens[_index];
            //Console.WriteLine($"{token} at {token.PureToken.Position}");
            if (token.Kind != kind)
            {
                throw new ParsingException($"Unexpected token \"{token.PureToken}\" instead of {kind} at {token.PureToken.Position}.");
            }
            _index++;
            return token;
        }

        private Token EatIdentifier(string s)
        {
            var token = Tokens[_index];
            //Console.WriteLine($"{token} at {token.PureToken.Position}");
            if (token.PureToken.Kind != TokenKind.Identifier)
            {
                throw new ParsingException($"Unexpected token \"{token.PureToken}\" instead of identifier \"{s}\" at {token.PureToken.Position}.");
            }

            if (token.PureToken.LiteralText != s)
            {
                throw new ParsingException($"Unexpected identifier \"{token.PureToken.LiteralText}\" instead of \"{s}\" at {token.PureToken.Position}.");
            }
            _index++;
            return token;
        }

        private void EatAll()
        {
            _index = Tokens.Count - 1;
        }

        private List<SyntaxNode> ParseFunctionOutputList()
        {
            var outputs = new List<Token>();
            while (CurrentToken.Kind != TokenKind.ClosingSquareBracket)
            {
                if (outputs.Count > 0 && CurrentToken.Kind == TokenKind.Comma)
                {
                    outputs.Add(EatToken());
                }
                outputs.Add(EatToken(TokenKind.Identifier));
            }

            return outputs.Select(token => new TokenNode(token) as SyntaxNode).ToList();
        }

        private FunctionOutputDescriptionNode ParseFunctionOutputDescription()
        {
            if (CurrentToken.Kind == TokenKind.Identifier)
            {
                if (PeekToken(1).Kind == TokenKind.Assignment)
                {
                    var identifier = EatToken();
                    var assignmentSign = EatToken(TokenKind.Assignment);                    
                    return Factory.FunctionOutputDescription(
                        new List<SyntaxNode> { Factory.Token(identifier) },
                        Factory.Token(assignmentSign)
                        );
                }

                return null;
            } else if (CurrentToken.Kind == TokenKind.OpeningSquareBracket)
            {
                var leftBracket = EatToken();
                var outputs = ParseFunctionOutputList();
                var rightBracket = EatToken(TokenKind.ClosingSquareBracket);
                var nodes = new List<SyntaxNode> {Factory.Token(leftBracket)};
                nodes.AddRange(outputs);
                nodes.Add(Factory.Token(rightBracket));
                var assignmentSign = EatToken(TokenKind.Assignment);                    
                return Factory.FunctionOutputDescription(nodes, Factory.Token(assignmentSign));
            }
            throw new ParsingException($"Unexpected token {CurrentToken.PureToken} during parsing function output descritpion at {CurrentToken.PureToken.Position}.");
        }

        private ParameterListNode ParseParameterList()
        {
            var identifierTokens = new List<Token>();
            
            while (CurrentToken.Kind != TokenKind.ClosingBracket)
            {
                if (identifierTokens.Count > 0)
                {
                    identifierTokens.Add(EatToken(TokenKind.Comma));
                }

                if (CurrentToken.Kind == TokenKind.Not)
                {
                    var notToken = EatToken();
                    identifierTokens.Add(notToken);
                }
                else
                {
                    identifierTokens.Add(EatToken(TokenKind.Identifier));
                }
            }

            return Factory.ParameterList(identifierTokens.Select(token => Factory.Token(token) as SyntaxNode).ToList());
        }

        private FunctionInputDescriptionNode ParseFunctionInputDescription()
        {
            if (CurrentToken.Kind == TokenKind.OpeningBracket)
            {
                var openingBracket = EatToken(TokenKind.OpeningBracket);
                var parameterList = ParseParameterList();
                var closingBracket = EatToken(TokenKind.ClosingBracket);
                return Factory.FunctionInputDescription(
                    new TokenNode(openingBracket),
                    parameterList,
                    new TokenNode(closingBracket));
            }
            else
            {
                return null;
            }
        }

        private TokenNode PossibleSemicolonOrComma()
        {
            if (CurrentToken.Kind == TokenKind.Semicolon
                || CurrentToken.Kind == TokenKind.Comma)
            {
                return Factory.Token(EatToken());
            }

            return null;
        }
        
        private FunctionDeclarationNode ParseFunctionDeclaration()
        {
            var functionKeyword = EatIdentifier("function");
            var outputDescription = ParseFunctionOutputDescription();
            var name = EatToken(TokenKind.Identifier);
            var inputDescription = ParseFunctionInputDescription();
            var body = ParseStatements();
            TokenNode end = null;
            if (CurrentToken.Kind == TokenKind.Identifier
                && CurrentToken.PureToken.LiteralText == "end")
            {
                end = Factory.Token(EatIdentifier("end"));
            }

            var semicolonOrComma = PossibleSemicolonOrComma();
            return Factory.FunctionDeclaration(
                Factory.Token(functionKeyword),
                outputDescription,
                Factory.Token(name),
                inputDescription,
                body,
                end,
                semicolonOrComma);
        }
        
        private StatementNode ParseClassDeclaration()
        {
            var node = new TokenNode(CurrentToken);
            EatAll();
            return null;
        }
       
        private FunctionCallParameterListNode ParseFunctionCallParameterList()
        {
            var nodes = new List<SyntaxNode>();
            while (CurrentToken.Kind != TokenKind.ClosingBracket)
            {
                if (nodes.Count > 0)
                {
                    nodes.Add(Factory.Token(EatToken(TokenKind.Comma)));
                }

                nodes.Add(ParseExpression());
            }

            return Factory.FunctionCallParameterList(nodes);
        }

        private ExpressionNode ParseMember()
        {
            if (CurrentToken.Kind == TokenKind.Identifier)
            {
                return Factory.IdentifierName(EatToken());
            } else if (CurrentToken.Kind == TokenKind.OpeningBracket)
            {
                var openingBracket = EatToken();
                var indirectMember = ParseExpression();
                var closingBracket = EatToken(TokenKind.ClosingBracket);
                return Factory.IndirectMemberAccess(
                    Factory.Token(openingBracket),
                    indirectMember,
                    Factory.Token(closingBracket));
            }
            throw new ParsingException($"Unexpected token {CurrentToken.PureToken} at {CurrentToken.PureToken.Position}.");
        }

        private ExpressionNode ParsePostfix(ParseOptions options, ExpressionNode expression)
        {
            while (true)
            {
                var token = CurrentToken;
                switch(token.Kind) {
                    case TokenKind.OpeningBrace: // cell array element access
                        if (options.ParsingArrayElements && expression.TrailingTrivia.Any())
                        {
                            return expression;
                        }
                        var openingBrace = EatToken();
                        var indices = ParseArrayElementList(TokenKind.ClosingBrace);
                        var closingBrace = EatToken(TokenKind.ClosingBrace);
                        expression = Factory.CellArrayElementAccessExpression(
                            expression,
                            Factory.Token(openingBrace),
                            indices,
                            Factory.Token(closingBrace)
                        );
                        break;
                    case TokenKind.OpeningBracket: // function call
                        if (options.ParsingArrayElements && expression.TrailingTrivia.Any())
                        {
                            return expression;
                        }
                        var openingBracket = EatToken();
                        var parameters = ParseFunctionCallParameterList();
                        var closingBracket = EatToken(TokenKind.ClosingBracket);
                        expression = Factory.FunctionCallExpression(
                            expression,
                            Factory.Token(openingBracket),
                            parameters,
                            Factory.Token(closingBracket));
                        break;
                    case TokenKind.Dot: // member access
                        if (expression is IdentifierNameNode
                            || expression is MemberAccessNode
                            || expression is FunctionCallExpressionNode
                            || expression is CellArrayElementAccessExpressionNode)
                        {
                            var dot = EatToken();
                            var member = ParseMember();
                            expression = Factory.MemberAccess(expression, Factory.Token(dot), member);
                        }
                        else
                        {
                            throw new ParsingException(
                                $"Unexpected token {token.PureToken} at {token.PureToken.Position}.");
                        }

                        break;
                    case TokenKind.Transpose:
                    case TokenKind.DotTranspose:
                        var operation = Factory.Token(EatToken());
                        expression = Factory.UnaryPostfixOperationExpression(expression, operation);
                        break;
                    case TokenKind.UnquotedStringLiteral:
                        if (expression is IdentifierNameNode idNameNode)
                        {
                            var arguments = new List<UnquotedStringLiteralNode>();
                            while (CurrentToken.Kind == TokenKind.UnquotedStringLiteral)
                            {
                                arguments.Add(Factory.UnquotedStringLiteral(EatToken()));
                            }

                            return Factory.CommandExpression(idNameNode, arguments);
                        }
                        else
                        {
                            throw new ParsingException($"Unexpected token \"{CurrentToken.PureToken.LiteralText}\" while parsing expression \"{expression.FullText}\" at {CurrentToken.PureToken.Position}.");
                        }
                    case TokenKind.At:
                        if (expression is IdentifierNameNode idNameNode2
                            && !expression.TrailingTrivia.Any())
                        {
                            var atToken = Factory.Token(EatToken());
                            var baseClassNameWithArguments = ParseExpression();
                            return Factory.BaseClassInvokation(idNameNode2, atToken, baseClassNameWithArguments);
                        }
                        else
                        {
                            throw new ParsingException($"Unexpected token \"{CurrentToken.PureToken.LiteralText}\" at {CurrentToken.PureToken.Position}.");
                        }
                    default:
                        return expression;
                }
            }
        }

        private ArrayElementListNode ParseArrayElementList(TokenKind closing)
        {
            var nodes = new List<SyntaxNode> {};
            
            while (CurrentToken.Kind != closing)
            {
                if (nodes.Count > 0)
                {
                    if (CurrentToken.Kind == TokenKind.Comma
                        || CurrentToken.Kind == TokenKind.Semicolon)
                    {
                        nodes.Add(Factory.Token(EatToken()));
                    }
                }

                var expression = ParseExpression(new ParseOptions {ParsingArrayElements = true});
                if (expression != null)
                {
                    nodes.Add(expression);
                }
            }
            
            return Factory.ArrayElementList(nodes);            
        }

        private ArrayLiteralExpressionNode ParseArrayLiteral()
        {
            var openingSquareBracket = EatToken(TokenKind.OpeningSquareBracket);
            var elements = ParseArrayElementList(TokenKind.ClosingSquareBracket);
            var closingSquareBracket = EatToken(TokenKind.ClosingSquareBracket);
            return Factory.ArrayLiteralExpression(
                Factory.Token(openingSquareBracket),
                elements,
                Factory.Token(closingSquareBracket));
        }

        private CellArrayLiteralExpressionNode ParseCellArrayLiteral()
        {
            var openingBrace = EatToken(TokenKind.OpeningBrace);
            var elements = ParseArrayElementList(TokenKind.ClosingBrace);
            var closingBrace = EatToken(TokenKind.ClosingBrace);
            return Factory.CellArrayLiteralExpression(
                Factory.Token(openingBrace),
                elements,
                Factory.Token(closingBrace));
        }

        private ParenthesizedExpressionNode ParseParenthesizedExpression()
        {
            var openParen = Factory.Token(EatToken(TokenKind.OpeningBracket));
            var expression = ParseExpression();
            var closeParen = Factory.Token(EatToken(TokenKind.ClosingBracket));
            return Factory.ParenthesizedExpression(
                openParen,
                expression,
                closeParen);
        }

        private ExpressionNode ParseTerm(ParseOptions options)
        {
            var token = CurrentToken;
            ExpressionNode expression = null;
            if (token.Kind == TokenKind.Identifier)
            {
                var term = EatToken();
                expression = Factory.IdentifierName(term);
            }
            else if (token.Kind == TokenKind.NumberLiteral)
            {
                var number = EatToken();
                expression = Factory.NumberLiteral(number);
            }
            else if (token.Kind == TokenKind.StringLiteral)
            {
                var str = EatToken();
                expression = Factory.StringLiteral(str);
            }
            else if (token.Kind == TokenKind.DoubleQuotedStringLiteral)
            {
                var str = EatToken();
                expression = Factory.DoubleQuotedStringLiteral(str);
            }
            else if (token.Kind == TokenKind.OpeningSquareBracket) // array literal expression
            {
                expression = ParseArrayLiteral();
            }
            else if (token.Kind == TokenKind.OpeningBrace) // cell array literal expression
            {
                expression = ParseCellArrayLiteral();
            }
            else if (token.Kind == TokenKind.Colon) // for parsing things like a{:}
            {
                expression = Factory.EmptyExpression();
            }
            else if (token.Kind == TokenKind.OpeningBracket)
            {
                expression = ParseParenthesizedExpression();
            }

            if (expression == null)
            {
                return null;
            }
            return ParsePostfix(options, expression);
        }

        internal struct ParseOptions
        {
            public bool ParsingArrayElements { get; set; }
            
            public static ParseOptions Default = new ParseOptions { ParsingArrayElements = false };
        }

        public ExpressionNode ParseExpression()
        {
            return ParseExpression(ParseOptions.Default);
        }

        private ExpressionNode ParseExpression(ParseOptions options)
        {
            return ParseSubExpression(options, Precedence.Expression);
        }

        private bool IsUnaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Not:
                case TokenKind.QuestionMark:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsBinaryOperator(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Assignment:
                case TokenKind.LogicalOr:
                case TokenKind.LogicalAnd:
                case TokenKind.BitwiseOr:
                case TokenKind.BitwiseAnd:
                case TokenKind.Less:
                case TokenKind.LessOrEqual:
                case TokenKind.Greater:
                case TokenKind.GreaterOrEqual:
                case TokenKind.Equality:
                case TokenKind.Inequality:
                case TokenKind.Colon:
                case TokenKind.Plus:
                case TokenKind.Minus:
                case TokenKind.Multiply:
                case TokenKind.DotMultiply:
                case TokenKind.Divide:
                case TokenKind.DotDivide:
                case TokenKind.Backslash:
                case TokenKind.DotBackslash:
                case TokenKind.Not:
                case TokenKind.Power:
                case TokenKind.DotPower:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsLeftAssociative(TokenKind kind)
        {
            return true; // TODO: really?
        }

        private TokenKind ConvertToUnaryTokenKind(TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Plus:
                    return TokenKind.UnaryPlus;
                case TokenKind.Minus:
                    return TokenKind.UnaryMinus;
                case TokenKind.Not:
                    return TokenKind.UnaryNot;
                case TokenKind.QuestionMark:
                    return TokenKind.UnaryQuestionMark;
                default:
                    throw new ArgumentException(nameof(kind));
            }
        }

        private CompoundNameNode ParseCompoundName()
        {
            var lastToken = EatToken(TokenKind.Identifier);
            var firstName = Factory.IdentifierName(lastToken);
            var nodes = new List<SyntaxNode> {firstName};
            while (CurrentToken.Kind == TokenKind.Dot
                   && !lastToken.TrailingTrivia.Any())
            {
                var dot = Factory.Token(EatToken());
                nodes.Add(dot);
                lastToken = EatToken(TokenKind.Identifier);
                nodes.Add(Factory.IdentifierName(lastToken));
            }

            return Factory.CompoundName(nodes);
        }

        private FunctionHandleNode ParseFunctionHandle()
        {
            var atSign = EatToken();
            if (CurrentToken.Kind == TokenKind.Identifier)
            {
                var compoundName = ParseCompoundName();
                return Factory.NamedFunctionHandle(
                    Factory.Token(atSign),
                    compoundName);
            } else if (CurrentToken.Kind == TokenKind.OpeningBracket)
            {
                var inputs = ParseFunctionInputDescription();
                var body = ParseExpression();
                return Factory.Lambda(Factory.Token(atSign), inputs, body);
            }
            throw new ParsingException($"Unexpected token {CurrentToken.PureToken} while parsing function handle at {CurrentToken.PureToken.Position}.");
        }
        
        private ExpressionNode ParseSubExpression(ParseOptions options, Precedence precedence)
        {
            ExpressionNode lhs = null;
            if (IsUnaryOperator(CurrentToken.Kind))
            {
                var operation = EatToken();
                var unaryTokenKind = ConvertToUnaryTokenKind(operation.Kind);
                var newPrecedence = GetPrecedence(unaryTokenKind);
                var operand = ParseSubExpression(options, newPrecedence);
                if (operand == null)
                {
                    if (options.ParsingArrayElements && operation.Kind == TokenKind.Not)
                    {
                        operand = Factory.EmptyExpression();
                    }
                    else
                    {
                        throw new ParsingException($"Unexpected token {CurrentToken.Kind} at {operation.PureToken.Position}.");
                    }
                }
                lhs = Factory.UnaryPrefixOperationExpression(Factory.Token(operation), operand);
            }
            else if (CurrentToken.Kind == TokenKind.At)
            {
                return ParseFunctionHandle();
            }
            else
            {
                lhs = ParseTerm(options);
            }
            while (true)
            {
                var token = CurrentToken;
                if (IsBinaryOperator(token.Kind))
                {
                    var newPrecedence = GetPrecedence(token.Kind);
                    if (newPrecedence < precedence)
                    {
                        break;
                    }

                    if (newPrecedence == precedence && IsLeftAssociative(token.Kind))
                    {
                        break;
                    }

                    EatToken();
                    var rhs = ParseSubExpression(options, newPrecedence);
                    if (rhs == null && token.Kind == TokenKind.Colon) // for parsing things like a{:}
                    {
                        rhs = Factory.EmptyExpression();
                    }
                    if (token.Kind == TokenKind.Assignment)
                    {
                        lhs = Factory.AssignmentExpression(lhs, Factory.Token(token), rhs);
                    }
                    else
                    {
                        lhs = Factory.BinaryOperationExpression(lhs, Factory.Token(token), rhs);
                    }
                }
                else
                {
                    break;
                }
            }

            return lhs;
        }

        private List<TokenNode> ParseOptionalCommas()
        {
            var commas = new List<TokenNode>();
            while (CurrentToken.Kind == TokenKind.Comma)
            {
                commas.Add(Factory.Token(EatToken()));
            }

            return commas;
        }

        private List<TokenNode> ParseOptionalSemicolonsOrCommas()
        {
            var commas = new List<TokenNode>();
            while (CurrentToken.Kind == TokenKind.Comma
                   || CurrentToken.Kind == TokenKind.Semicolon)
            {
                commas.Add(Factory.Token(EatToken()));
            }

            return commas;
        }

        private SwitchCaseNode ParseSwitchCase()
        {
            var caseKeyword = EatIdentifier("case");
            var caseId = ParseExpression();
            var commas = ParseOptionalCommas();
            var statementList = ParseStatements();
            return Factory.SwitchCase(Factory.Token(caseKeyword), caseId, statementList, commas);
        }

        private SwitchStatementNode ParseSwitchStatement()
        {
            var switchKeyword = EatIdentifier("switch");
            var expression = ParseExpression();
            var commas = ParseOptionalCommas();
            var casesList = new List<SwitchCaseNode>();
            while (CurrentToken.Kind == TokenKind.Identifier
                   && CurrentToken.PureToken.LiteralText == "case")
            {
                casesList.Add(ParseSwitchCase());
            }

            var endKeyword = EatIdentifier("end");
            return Factory.SwitchStatement(
                Factory.Token(switchKeyword),
                expression,
                casesList,
                Factory.Token(endKeyword),
                commas);
        }

        public ExpressionStatementNode ParseExpressionStatement()
        {
            var statement = ParseExpression();
            var possibleSemicolonOrComma = PossibleSemicolonOrComma();
            return Factory.ExpressionStatement(statement, possibleSemicolonOrComma);
        }

        public WhileStatementNode ParseWhileStatement()
        {
            var whileKeyword = EatToken();
            var condition = ParseExpression();
            var commas = ParseOptionalCommas();
            var body = ParseStatements();
            var endKeyword = EatIdentifier("end");
            var semicolonOrComma = PossibleSemicolonOrComma();
            return Factory.WhileStatement(
                Factory.Token(whileKeyword),
                condition,
                commas,
                body,
                Factory.Token(endKeyword),
                semicolonOrComma);
        }

        public StatementNode ParseStatement()
        {
            var statement = ParseStatementCore();
            return statement;
        }

        public IfStatementNode ParseIfStatement()
        {
            var ifKeyword = Factory.Token(EatToken());
            var condition = ParseExpression();
            var commas = ParseOptionalSemicolonsOrCommas();
            var body = ParseStatements();
            TokenNode elseKeyword = null;
            StatementListNode elseBody = null;
            if (CurrentToken.Kind == TokenKind.Identifier
                && CurrentToken.PureToken.LiteralText == "else")
            {
                elseKeyword = Factory.Token(EatToken());
                elseBody = ParseStatements();
            }
            if (CurrentToken.Kind == TokenKind.Identifier
                && CurrentToken.PureToken.LiteralText == "elseif")
            {
                elseKeyword = null;
                var ifStatement = ParseIfStatement();
                elseBody = Factory.StatementList(new List<SyntaxNode> { ifStatement });
                return Factory.IfStatement(
                    ifKeyword,
                    condition,
                    commas,
                    body,
                    null,
                    elseBody,
                    null,
                    null);
            }

            var endKeyword = Factory.Token(EatIdentifier("end"));
            var possibleSemicolonOrComma = PossibleSemicolonOrComma();
            return Factory.IfStatement(
                ifKeyword,
                condition,
                commas,
                body,
                elseKeyword,
                elseBody,
                endKeyword,
                possibleSemicolonOrComma);
        }

        public ForStatementNode ParseForStatement()
        {
            var forKeyword = Factory.Token(EatIdentifier("for"));
            var expression = ParseExpression();
            if (!(expression is AssignmentExpressionNode))
            {
                throw new ParsingException($"Unexpected expression \"{expression.FullText}\" while parsing FOR statement at {CurrentToken.PureToken.Position}.");
            }

            var forAssignment = (AssignmentExpressionNode) expression;
            var commas = ParseOptionalSemicolonsOrCommas();

            var body = ParseStatements();
            var endKeyword = Factory.Token(EatIdentifier("end"));
            return Factory.ForStatement(forKeyword, forAssignment, body, endKeyword, commas);
        }

        public TryCatchStatementNode ParseTryCatchStatement()
        {
            var tryKeyword = Factory.Token(EatIdentifier("try"));
            var tryBody = ParseStatements();
            if (CurrentToken.PureToken.LiteralText == "catch")
            {
                var catchKeyword = Factory.Token(EatIdentifier("catch"));
                var catchBody = ParseStatements();
                var endKeyword = Factory.Token(EatIdentifier("end"));
                return Factory.TryCatchStatement(tryKeyword, tryBody, catchKeyword, catchBody, endKeyword);
            }
            else if (CurrentToken.PureToken.LiteralText == "end")
            {
                var endKeyword = Factory.Token(EatIdentifier("end"));
                return Factory.TryCatchStatement(tryKeyword, tryBody, endKeyword);
            }
            else
            {
                throw new ParsingException($"Unexpected token {CurrentToken.PureToken} while parsing try/catch statement at {CurrentToken.PureToken.Position}.");
            }
        }

        public StatementNode ParseStatementCore()
        {
            if (CurrentToken.Kind == TokenKind.Identifier)
            {
                if (CurrentToken.PureToken.LiteralText == "function")
                {
                    return ParseFunctionDeclaration();
                }
                else if (CurrentToken.PureToken.LiteralText == "classdef")
                {
                    return ParseClassDeclaration();
                }
                else if (CurrentToken.PureToken.LiteralText == "switch")
                {
                    return ParseSwitchStatement();
                }
                else if (CurrentToken.PureToken.LiteralText == "while")
                {
                    return ParseWhileStatement();
                }
                else if (CurrentToken.PureToken.LiteralText == "if")
                {
                    return ParseIfStatement();
                }
                else if (CurrentToken.PureToken.LiteralText == "case")
                {
                    return null;
                }
                else if (CurrentToken.PureToken.LiteralText == "else")
                {
                    return null;
                }
                else if (CurrentToken.PureToken.LiteralText == "elseif")
                {
                    return null;
                }
                else if (CurrentToken.PureToken.LiteralText == "end")
                {
                    return null;
                }
                else if (CurrentToken.PureToken.LiteralText == "for")
                {
                    return ParseForStatement();
                }
                else if (CurrentToken.PureToken.LiteralText == "try")
                {
                    return ParseTryCatchStatement();
                }
                else if (CurrentToken.PureToken.LiteralText == "catch")
                {
                    return null;
                }

                return ParseExpressionStatement();
            }

            if (CurrentToken.Kind == TokenKind.OpeningSquareBracket)
            {
                return ParseExpressionStatement();
            }

            if (CurrentToken.Kind == TokenKind.Semicolon)
            {
                return Factory.ExpressionStatement(Factory.EmptyExpression(), Factory.Token(EatToken()));
            }
            throw new ParsingException($"Unexpected token: \"{CurrentToken.PureToken}\" at {CurrentToken.PureToken.Position}");
        }

        private StatementListNode ParseStatements()
        {
            var statements = new List<SyntaxNode>();
            while (CurrentToken.PureToken.Kind != TokenKind.EndOfFile)
            {
                var node = ParseStatement();
                if (node == null)
                {
                    break;
                }
                statements.Add(node);
            }

            return Factory.StatementList(statements);
        }

        private StatementListNode ParseFile()
        {
            return ParseStatements();
        }
        
        public StatementListNode Parse()
        {
            return ParseFile();
        }
    }
}