using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Lexer;

namespace Parser
{
    public class SyntaxFactory
    {
        private static SyntaxNode SetParent(SyntaxNode parent)
        {
            foreach (var node in parent.Children)
            {
                node.Parent = parent;
            }

            return parent;
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
            var children = new List<SyntaxNode> {functionKeyword};
            if (outputDescription != null)
            {
                children.Add(outputDescription);
            }
            children.Add(name);
            children.Add(inputDescription);
            children.Add(body);
            if (end != null)
            {
                children.Add(end);
            }
            if (semicolonOrComma != null)
            {
                children.Add(semicolonOrComma);
            }
            var result =
                new FunctionDeclarationNode(
                    children,
                    functionKeyword,
                    outputDescription,
                    name,
                    inputDescription,
                    body,
                    end,
                    semicolonOrComma);
            SetParent(result);
            return result;
        }

        public FunctionOutputDescriptionNode FunctionOutputDescription(
            List<SyntaxNode> nodes,
            TokenNode equalitySign)
        {
            var children = new List<SyntaxNode>(nodes);
            nodes.Add(equalitySign);
            var result = new FunctionOutputDescriptionNode(
                nodes,
                nodes
                    .Where(node => node is TokenNode && ((TokenNode) node).Token.Kind == TokenKind.Identifier)
                    .Select(node => node as TokenNode)
                    .ToList(),
                equalitySign
                );
            SetParent(result);
            return result;
        }

        public ParameterListNode ParameterList(List<SyntaxNode> nodes)
        {
            var result = new ParameterListNode(
                nodes,
                nodes
                    .Where(
                        node => node is TokenNode && ((TokenNode) node).Token.Kind != TokenKind.Comma
                    ).ToList());
            SetParent(result);
            return result;
        }

        public StatementListNode StatementList(List<SyntaxNode> nodes)
        {
            var result = new StatementListNode(nodes);
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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
            TokenNode semicolonOrComma = null)
        {
            var children = new List<SyntaxNode> { switchKeyword, switchExpression };
            children.AddRange(cases);
            children.Add(endKeyword);
            if (semicolonOrComma != null)
            {
                children.Add(semicolonOrComma);
            }

            var result = new SwitchStatementNode(
                children,
                switchKeyword,
                switchExpression,
                cases,
                endKeyword,
                semicolonOrComma);
            SetParent(result);
            return result;
        }

        public SwitchCaseNode SwitchCase(
            TokenNode caseKeyword,
            TokenNode caseIdentifier,
            StatementListNode statementList)
        {
            var children = new List<SyntaxNode>
            {
                caseKeyword,
                caseIdentifier,
                statementList
            };
            var result = new SwitchCaseNode(children, caseKeyword, caseIdentifier, statementList);
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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

        public ExpressionStatementNode ExpressionStatement(ExpressionNode expression)
        {
            var children = new List<SyntaxNode> {expression};
            var result = new ExpressionStatementNode(children, expression, null);
            SetParent(result);
            return result;
        }

        public ExpressionStatementNode ExpressionStatement(ExpressionNode expression, TokenNode semicolonOrComma)
        {
            var children = new List<SyntaxNode> {expression, semicolonOrComma};
            var result = new ExpressionStatementNode(children, expression, semicolonOrComma);
            SetParent(result);
            return result;
        }

        public CellArrayElementAccessExpressionNode CellArrayElementAccessExpression(
            ExpressionNode cellArray,
            TokenNode openingBrace,
            ExpressionNode index,
            TokenNode closingBrace)
        {
            var children = new List<SyntaxNode> {cellArray, openingBrace, index, closingBrace};
            var result = new CellArrayElementAccessExpressionNode(
                children,
                cellArray,
                openingBrace,
                index,
                closingBrace);
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
        }
        
        public FunctionCallParameterListNode FunctionCallParameterList(List<SyntaxNode> nodes)
        {
            var result = new FunctionCallParameterListNode(
                nodes,
                nodes
                    .OfType<ExpressionNode>()
                    .ToList());
            SetParent(result);
            return result;
        }

        public ArrayElementListNode ArrayElementList(List<SyntaxNode> nodes)
        {
            var result = new ArrayElementListNode(
                nodes,
                nodes
                    .OfType<ExpressionNode>()
                    .ToList());
            SetParent(result);
            return result;            
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
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
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
            if (optionalCommasAfterCondition != null)
            {
                children.AddRange(optionalCommasAfterCondition);
            }

            children.Add(body);
            children.Add(end);
            if (semicolonOrComma != null)
            {
                children.Add(semicolonOrComma);
            }
            var result = new WhileStatementNode(
                children,
                whileKeyword,
                condition,
                optionalCommasAfterCondition,
                body,
                end,
                semicolonOrComma);
            SetParent(result);
            return result;
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
            if (optionalCommasAfterCondition != null)
            {
                children.AddRange(optionalCommasAfterCondition);
            }

            children.Add(body);
            if (elseKeyword != null)
            {
                children.Add(elseKeyword);
                children.Add(elseBody);
            }

            children.Add(endKeyword);
            var result = new IfStatementNode(
                children,
                ifKeyword,
                condition,
                optionalCommasAfterCondition,
                body,
                elseKeyword,
                elseBody,
                endKeyword);
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
        }

        public ForStatementNode ForStatement(
            TokenNode forKeyword,
            AssignmentExpressionNode forAssignment,
            StatementListNode body,
            TokenNode endKeyword)
        {
            var children = new List<SyntaxNode>
            {
                forKeyword,
                forAssignment,
                body,
                endKeyword
            };
            var result = new ForStatementNode(
                children,
                forKeyword,
                forAssignment,
                body,
                endKeyword);
            SetParent(result);
            return result;
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
            SetParent(result);
            return result;
        }
    }
}