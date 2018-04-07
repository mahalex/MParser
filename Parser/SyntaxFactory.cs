using System.Collections.Generic;
using System.Linq;
using Lexer;

namespace Parser
{
    public class SyntaxFactory
    {
        private static T SetParent<T>(T parent) where T : SyntaxNode
        {
            foreach (var node in parent.Children)
            {
                node.Parent = parent;
            }

            return parent;
        }

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
            var result =
                new FunctionDeclarationNode(
                    RemoveNulls(children),
                    functionKeyword,
                    outputDescription,
                    name,
                    inputDescription,
                    body,
                    end,
                    semicolonOrComma);
            return SetParent(result);
        }

        public FunctionOutputDescriptionNode FunctionOutputDescription(
            List<SyntaxNode> nodes,
            TokenNode equalitySign)
        {
            var children = new List<SyntaxNode>(nodes);
            children.Add(equalitySign);
            var result = new FunctionOutputDescriptionNode(
                children,
                children
                    .Where(node => node is TokenNode tokenNode && tokenNode.Token.Kind == TokenKind.Identifier)
                    .Select(node => (TokenNode)node)
                    .ToList(),
                equalitySign
                );
            return SetParent(result);
        }

        public ParameterListNode ParameterList(List<SyntaxNode> nodes)
        {
            var result = new ParameterListNode(
                nodes,
                nodes
                    .Where(
                        node => node is TokenNode tokenNode && tokenNode.Token.Kind != TokenKind.Comma
                    )
                    .ToList());
            return SetParent(result);
        }

        public StatementListNode StatementList(List<SyntaxNode> nodes)
        {
            var result = new StatementListNode(nodes);
            return SetParent(result);
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
            var result = new FunctionInputDescriptionNode(children, openingBracket, parameterList, closingBracket);
            return SetParent(result);
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

            var result = new SwitchStatementNode(
                RemoveNulls(children),
                switchKeyword,
                switchExpression,
                cases,
                endKeyword,
                semicolonOrComma,
                optionalCommasAfterExpression);
            return SetParent(result);
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
            var result = new SwitchCaseNode(
                RemoveNulls(children),
                caseKeyword,
                caseIdentifier,
                statementList,
                optionalCommasAfterIdentifier);
            return SetParent(result);
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
            var result = new AssignmentExpressionNode(children, lhs, assignmentSign, rhs);
            return SetParent(result);
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
            var result = new UnaryPrefixOperationExpressionNode(children, operation, operand);
            return SetParent(result);
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
            var result = new UnaryPostfixOperationExpressionNode(children, operand, operation);
            return SetParent(result);
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
            var result = new BinaryOperationExpressionNode(children, lhs, operation, rhs);
            return SetParent(result);
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

        public ExpressionStatementNode ExpressionStatement(ExpressionNode expression)
        {
            var children = new List<SyntaxNode> {expression};
            var result = new ExpressionStatementNode(children, expression, null);
            return SetParent(result);
        }

        public ExpressionStatementNode ExpressionStatement(ExpressionNode expression, TokenNode semicolonOrComma)
        {
            var children = new List<SyntaxNode> {expression, semicolonOrComma};
            var result = new ExpressionStatementNode(children, expression, semicolonOrComma);
            return SetParent(result);
        }

        public CellArrayElementAccessExpressionNode CellArrayElementAccessExpression(
            ExpressionNode cellArray,
            TokenNode openingBrace,
            ArrayElementListNode indices,
            TokenNode closingBrace)
        {
            var children = new List<SyntaxNode> {cellArray, openingBrace, indices, closingBrace};
            var result = new CellArrayElementAccessExpressionNode(
                children,
                cellArray,
                openingBrace,
                indices,
                closingBrace);
            return SetParent(result);
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
            var result = new FunctionCallExpressionNode(
                children,
                functionName,
                openingBracket,
                parameters,
                closingBracket);
            return SetParent(result);
        }
        
        public FunctionCallParameterListNode FunctionCallParameterList(List<SyntaxNode> nodes)
        {
            var result = new FunctionCallParameterListNode(
                nodes,
                nodes
                    .OfType<ExpressionNode>()
                    .ToList());
            return SetParent(result);
        }

        public ArrayElementListNode ArrayElementList(List<SyntaxNode> nodes)
        {
            var result = new ArrayElementListNode(
                nodes,
                nodes
                    .OfType<ExpressionNode>()
                    .ToList());
            return SetParent(result);
        }

        public CompoundNameNode CompoundName(List<SyntaxNode> nodes)
        {
            var result = new CompoundNameNode(
                nodes,
                nodes
                    .OfType<IdentifierNameNode>()
                    .ToList());
            return SetParent(result);
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
            var result = new ArrayLiteralExpressionNode(
                children,
                openingSquareBracket,
                elements,
                closingSquareBracket);
            return SetParent(result);
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
            var result = new CellArrayLiteralExpressionNode(
                children,
                openingBrace,
                elements,
                closingBrace);
            return SetParent(result);
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
            var result = new MemberAccessNode(
                children,
                leftOperand,
                dot,
                rightOperand);
            return SetParent(result);
        }

        public WhileStatementNode WhileStatement(
            TokenNode whileKeyword,
            ExpressionNode condition,
            StatementListNode body,
            TokenNode end,
            List<TokenNode> optionalCommasAfterCondition = null,
            TokenNode semicolonOrComma = null)
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
            var result = new WhileStatementNode(
                RemoveNulls(children),
                whileKeyword,
                condition,
                optionalCommasAfterCondition,
                body,
                end,
                semicolonOrComma);
            return SetParent(result);
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
            StatementListNode body,
            TokenNode elseKeyword,
            StatementListNode elseBody,
            TokenNode endKeyword,
            List<TokenNode> optionalCommasAfterCondition = null)
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

            var result = new IfStatementNode(
                RemoveNulls(children),
                ifKeyword,
                condition,
                optionalCommasAfterCondition,
                body,
                elseKeyword,
                elseBody,
                endKeyword);
            return SetParent(result);
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
            var result = new ParenthesizedExpressionNode(
                children,
                openParen,
                expression,
                closeParen);
            return SetParent(result);
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
            var result = new ForStatementNode(
                RemoveNulls(children),
                forKeyword,
                forAssignment,
                body,
                endKeyword,
                optionalCommasAfterAssignment);
            return SetParent(result);
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
            var result = new IndirectMemberAccessNode(
                children,
                openingBracket,
                indirectMemberName,
                closingBracket);
            return SetParent(result);
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
            var result = new NamedFunctionHandleNode(
                children,
                atSign,
                functionName);
            return SetParent(result);
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
            var result = new LambdaNode(
                children,
                atSign,
                input,
                body);
            return SetParent(result);
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
            var result = new TryCatchStatementNode(
                children,
                tryKeyword,
                tryBody,
                catchKeyword,
                catchBody,
                endKeyword);
            return SetParent(result);
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
            var result = new TryCatchStatementNode(
                children,
                tryKeyword,
                tryBody,
                null,
                null,
                endKeyword);
            return SetParent(result);
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
            var result = new CommandExpressionNode(
                children,
                identifierName,
                arguments);
            return SetParent(result);
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
            var result = new BaseClassInvokationNode(
                children,
                methodName,
                atToken,
                baseClassNameAndArguments);
            return SetParent(result);
        }
    }
}