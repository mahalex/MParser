using System.Collections.Generic;
using System.Linq;
using Lexer;

namespace Parser
{
    public class SyntaxFactory
    {
        private static List<SyntaxNode> RemoveNulls(List<SyntaxNode> children)
        {
            return children.Where(x => x != null).ToList();
        }
        
        public FunctionDeclarationNode FunctionDeclaration(
            TokenNode functionKeyword,
            FunctionOutputDescriptionNode outputDescription,
            TokenNode name,
            FunctionInputDescriptionNode inputDescription,
            StatementListNode body,
            TokenNode end,
            TokenNode semicolonOrComma = null)
        {
            var children = new List<SyntaxNode>
            {
                functionKeyword,
                outputDescription,
                name,
                inputDescription,
                body,
                end,
                semicolonOrComma
            };
            return new FunctionDeclarationNode(
                    RemoveNulls(children),
                    functionKeyword,
                    outputDescription,
                    name,
                    inputDescription,
                    body,
                    end,
                    semicolonOrComma);
        }

        public FunctionOutputDescriptionNode FunctionOutputDescription(
            List<SyntaxNode> nodes,
            TokenNode equalitySign)
        {
            var children = new List<SyntaxNode>(nodes);
            children.Add(equalitySign);
            return new FunctionOutputDescriptionNode(
                children,
                children
                    .Where(node => node is TokenNode tokenNode && tokenNode.Token.Kind == TokenKind.Identifier)
                    .Select(node => (TokenNode)node)
                    .ToList(),
                equalitySign
                );
        }

        public ParameterListNode ParameterList(List<SyntaxNode> nodes)
        {
            return new ParameterListNode(
                nodes,
                nodes
                    .Where(
                        node => node is TokenNode tokenNode && tokenNode.Token.Kind != TokenKind.Comma
                    )
                    .ToList());
        }

        public StatementListNode StatementList(List<SyntaxNode> nodes)
        {
            return new StatementListNode(nodes);
        }

        public FunctionInputDescriptionNode FunctionInputDescription(
            TokenNode openingBracket,
            ParameterListNode parameterList,
            TokenNode closingBracket)
        {
            var children = new List<SyntaxNode>
            {
                openingBracket,
                parameterList,
                closingBracket
            };
            return new FunctionInputDescriptionNode(children, openingBracket, parameterList, closingBracket);
        }

        public TokenNode Token(Token token)
        {
            return new TokenNode(token);
        }

        public SwitchStatementNode SwitchStatement(
            TokenNode switchKeyword,
            ExpressionNode switchExpression,
            List<SwitchCaseNode> cases,
            TokenNode endKeyword,
            List<TokenNode> optionalCommasAfterExpression,
            TokenNode semicolonOrComma = null)
        {
            var children = new List<SyntaxNode> { switchKeyword, switchExpression };
            children.AddRange(optionalCommasAfterExpression);
            children.AddRange(cases);
            children.Add(endKeyword);
            children.Add(semicolonOrComma);

            return new SwitchStatementNode(
                RemoveNulls(children),
                switchKeyword,
                switchExpression,
                cases,
                endKeyword,
                semicolonOrComma,
                optionalCommasAfterExpression);
        }

        public SwitchCaseNode SwitchCase(
            TokenNode caseKeyword,
            ExpressionNode caseIdentifier,
            StatementListNode statementList,
            List<TokenNode> optionalCommasAfterIdentifier)
        {
            var children = new List<SyntaxNode>
            {
                caseKeyword,
                caseIdentifier
            };
            children.AddRange(optionalCommasAfterIdentifier);
            children.Add(statementList);
            return new SwitchCaseNode(
                RemoveNulls(children),
                caseKeyword,
                caseIdentifier,
                statementList,
                optionalCommasAfterIdentifier);
        }

        public AssignmentExpressionNode AssignmentExpression(
            ExpressionNode lhs,
            TokenNode assignmentSign,
            ExpressionNode rhs)
        {
            var children = new List<SyntaxNode>
            {
                lhs,
                assignmentSign,
                rhs
            };
            return new AssignmentExpressionNode(children, lhs, assignmentSign, rhs);
        }

        public UnaryPrefixOperationExpressionNode UnaryPrefixOperationExpression(
            TokenNode operation,
            ExpressionNode operand)
        {
            var children = new List<SyntaxNode>
            {
                operation,
                operand
            };
            return new UnaryPrefixOperationExpressionNode(children, operation, operand);
        }

        public UnaryPostfixOperationExpressionNode UnaryPostfixOperationExpression(
            ExpressionNode operand,
            TokenNode operation)
        {
            var children = new List<SyntaxNode>
            {
                operand,
                operation
            };
            return new UnaryPostfixOperationExpressionNode(children, operand, operation);
        }

        public BinaryOperationExpressionNode BinaryOperationExpression(
            ExpressionNode lhs,
            TokenNode operation,
            ExpressionNode rhs)
        {
            var children = new List<SyntaxNode>
            {
                lhs,
                operation,
                rhs
            };
            return new BinaryOperationExpressionNode(children, lhs, operation, rhs);
        }

        public IdentifierNameNode IdentifierName(
            Token identifier)
        {
            return new IdentifierNameNode(identifier);
        }

        public NumberLiteralNode NumberLiteral(
            Token numberLiteral)
        {
            return new NumberLiteralNode(numberLiteral);
        }

        public StringLiteralNode StringLiteral(
            Token stringLiteral)
        {
            return new StringLiteralNode(stringLiteral);
        }

        public DoubleQuotedStringLiteralNode DoubleQuotedStringLiteral(
            Token stringLiteral)
        {
            return new DoubleQuotedStringLiteralNode(stringLiteral);
        }

        public UnquotedStringLiteralNode UnquotedStringLiteral(
            Token stringLiteral)
        {
            return new UnquotedStringLiteralNode(stringLiteral);
        }

        public ExpressionStatementNode ExpressionStatement(ExpressionNode expression, TokenNode semicolonOrComma)
        {
            var children = new List<SyntaxNode> {expression, semicolonOrComma};
            return new ExpressionStatementNode(
                RemoveNulls(children),
                expression,
                semicolonOrComma);
        }

        public CellArrayElementAccessExpressionNode CellArrayElementAccessExpression(
            ExpressionNode cellArray,
            TokenNode openingBrace,
            ArrayElementListNode indices,
            TokenNode closingBrace)
        {
            var children = new List<SyntaxNode> {cellArray, openingBrace, indices, closingBrace};
            return new CellArrayElementAccessExpressionNode(
                children,
                cellArray,
                openingBrace,
                indices,
                closingBrace);
        }

        public FunctionCallExpressionNode FunctionCallExpression(
            ExpressionNode functionName,
            TokenNode openingBracket,
            FunctionCallParameterListNode parameters,
            TokenNode closingBracket)
        {
            var children = new List<SyntaxNode>
            {
                functionName,
                openingBracket,
                parameters,
                closingBracket
            };
            return new FunctionCallExpressionNode(
                children,
                functionName,
                openingBracket,
                parameters,
                closingBracket);
        }
        
        public FunctionCallParameterListNode FunctionCallParameterList(List<SyntaxNode> nodes)
        {
            return new FunctionCallParameterListNode(
                nodes,
                nodes
                    .OfType<ExpressionNode>()
                    .ToList());
        }

        public ArrayElementListNode ArrayElementList(List<SyntaxNode> nodes)
        {
            return new ArrayElementListNode(
                nodes,
                nodes
                    .OfType<ExpressionNode>()
                    .ToList());
        }

        public CompoundNameNode CompoundName(List<SyntaxNode> nodes)
        {
            return new CompoundNameNode(
                nodes,
                nodes
                    .OfType<IdentifierNameNode>()
                    .ToList());
        }

        public ArrayLiteralExpressionNode ArrayLiteralExpression(
            TokenNode openingSquareBracket,
            ArrayElementListNode elements,
            TokenNode closingSquareBracket)
        {
            var children = new List<SyntaxNode>
            {
                openingSquareBracket,
                elements,
                closingSquareBracket
            };
            return new ArrayLiteralExpressionNode(
                children,
                openingSquareBracket,
                elements,
                closingSquareBracket);
        }

        public CellArrayLiteralExpressionNode CellArrayLiteralExpression(
            TokenNode openingBrace,
            ArrayElementListNode elements,
            TokenNode closingBrace)
        {
            var children = new List<SyntaxNode>
            {
                openingBrace,
                elements,
                closingBrace
            };
            return new CellArrayLiteralExpressionNode(
                children,
                openingBrace,
                elements,
                closingBrace);
        }

        public EmptyExpressionNode EmptyExpression()
        {
            return new EmptyExpressionNode();
        }

        public MemberAccessNode MemberAccess(
            SyntaxNode leftOperand,
            TokenNode dot,
            SyntaxNode rightOperand)
        {
            var children = new List<SyntaxNode>
            {
                leftOperand,
                dot,
                rightOperand
            };
            return new MemberAccessNode(
                children,
                leftOperand,
                dot,
                rightOperand);
        }

        public WhileStatementNode WhileStatement(
            TokenNode whileKeyword,
            ExpressionNode condition,
            List<TokenNode> optionalCommasAfterCondition,
            StatementListNode body,
            TokenNode end,
            TokenNode semicolonOrComma)
        {
            var children = new List<SyntaxNode>
            {
                whileKeyword,
                condition,
            };
            children.AddRange(optionalCommasAfterCondition);
            children.Add(body);
            children.Add(end);
            children.Add(semicolonOrComma);
            return new WhileStatementNode(
                RemoveNulls(children),
                whileKeyword,
                condition,
                optionalCommasAfterCondition,
                body,
                end,
                semicolonOrComma);
        }

        public StatementNode AppendSemicolonOrComma(StatementNode statement, TokenNode semicolonOrComma)
        {
            statement.SemicolonOrComma = semicolonOrComma;
            statement.Children.Add(semicolonOrComma);
            statement.Children[statement.Children.Count - 1].Parent = statement;
            return statement;
        }

        public IfStatementNode IfStatement(
            TokenNode ifKeyword,
            ExpressionNode condition,
            List<TokenNode> optionalCommasAfterCondition,
            StatementListNode body,
            TokenNode elseKeyword,
            StatementListNode elseBody,
            TokenNode endKeyword,
            TokenNode possibleSemicolonOrComma)
        {
            var children = new List<SyntaxNode>
            {
                ifKeyword,
                condition
            };
            children.AddRange(optionalCommasAfterCondition);
            children.Add(body);
            children.Add(elseKeyword);
            children.Add(elseBody);
            children.Add(endKeyword);
            children.Add(possibleSemicolonOrComma);

            return new IfStatementNode(
                RemoveNulls(children),
                ifKeyword,
                condition,
                optionalCommasAfterCondition,
                body,
                elseKeyword,
                elseBody,
                endKeyword,
                possibleSemicolonOrComma);
        }

        public ParenthesizedExpressionNode ParenthesizedExpression(
            TokenNode openParen,
            ExpressionNode expression,
            TokenNode closeParen)
        {
            var children = new List<SyntaxNode>
            {
                openParen,
                expression,
                closeParen
            };
            return new ParenthesizedExpressionNode(
                children,
                openParen,
                expression,
                closeParen);
        }

        public ForStatementNode ForStatement(
            TokenNode forKeyword,
            AssignmentExpressionNode forAssignment,
            StatementListNode body,
            TokenNode endKeyword,
            List<TokenNode> optionalCommasAfterAssignment)
        {
            var children = new List<SyntaxNode>
            {
                forKeyword,
                forAssignment,
            };
            children.AddRange(optionalCommasAfterAssignment);
            children.Add(body);
            children.Add(endKeyword);
            return new ForStatementNode(
                RemoveNulls(children),
                forKeyword,
                forAssignment,
                body,
                endKeyword,
                optionalCommasAfterAssignment);
        }

        public IndirectMemberAccessNode IndirectMemberAccess(
            TokenNode openingBracket,
            ExpressionNode indirectMemberName,
            TokenNode closingBracket)
        {
            var children = new List<SyntaxNode>
            {
                openingBracket,
                indirectMemberName,
                closingBracket
            };
            return new IndirectMemberAccessNode(
                children,
                openingBracket,
                indirectMemberName,
                closingBracket);
        }

        public NamedFunctionHandleNode NamedFunctionHandle(
            TokenNode atSign,
            CompoundNameNode functionName)
        {
            var children = new List<SyntaxNode>
            {
                atSign,
                functionName
            };
            return new NamedFunctionHandleNode(
                children,
                atSign,
                functionName);
        }

        public LambdaNode Lambda(
            TokenNode atSign,
            FunctionInputDescriptionNode input,
            ExpressionNode body)
        {
            var children = new List<SyntaxNode>
            {
                atSign,
                input,
                body
            };
            return new LambdaNode(
                children,
                atSign,
                input,
                body);
        }

        public TryCatchStatementNode TryCatchStatement(
            TokenNode tryKeyword,
            StatementListNode tryBody,
            TokenNode catchKeyword,
            StatementListNode catchBody,
            TokenNode endKeyword)
        {
            var children = new List<SyntaxNode>
            {
                tryKeyword,
                tryBody,
                catchKeyword,
                catchBody,
                endKeyword
            };
            return new TryCatchStatementNode(
                children,
                tryKeyword,
                tryBody,
                catchKeyword,
                catchBody,
                endKeyword);
        }

        public TryCatchStatementNode TryCatchStatement(
            TokenNode tryKeyword,
            StatementListNode tryBody,
            TokenNode endKeyword)
        {
            var children = new List<SyntaxNode>
            {
                tryKeyword,
                tryBody,
                endKeyword
            };
            return new TryCatchStatementNode(
                children,
                tryKeyword,
                tryBody,
                null,
                null,
                endKeyword);
        }

        public CommandExpressionNode CommandExpression(
            IdentifierNameNode identifierName,
            List<UnquotedStringLiteralNode> arguments)
        {
            var children = new List<SyntaxNode>
            {
                identifierName
            };
            children.AddRange(arguments);
            return new CommandExpressionNode(
                children,
                identifierName,
                arguments);
        }

        public BaseClassInvokationNode BaseClassInvokation(
            IdentifierNameNode methodName,
            TokenNode atToken,
            ExpressionNode baseClassNameAndArguments)
        {
            var children = new List<SyntaxNode>
            {
                methodName,
                atToken,
                baseClassNameAndArguments
            };
            return new BaseClassInvokationNode(
                children,
                methodName,
                atToken,
                baseClassNameAndArguments);
        }
    }
}