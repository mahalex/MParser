#nullable enable
namespace Parser.Internal
{
    internal partial class SyntaxFactory
    {
        public FileSyntaxNode FileSyntax(BlockStatementSyntaxNode body, SyntaxToken endOfFile)
        {
            return new FileSyntaxNode(body, endOfFile);
        }

        public BlockStatementSyntaxNode BlockStatementSyntax(SyntaxList statements)
        {
            return new BlockStatementSyntaxNode(statements);
        }

        public FunctionDeclarationSyntaxNode FunctionDeclarationSyntax(SyntaxToken functionKeyword, FunctionOutputDescriptionSyntaxNode? outputDescription, SyntaxToken name, FunctionInputDescriptionSyntaxNode? inputDescription, SyntaxList<SyntaxToken> commas, StatementSyntaxNode body, EndKeywordSyntaxNode? endKeyword)
        {
            return new FunctionDeclarationSyntaxNode(functionKeyword, outputDescription, name, inputDescription, commas, body, endKeyword);
        }

        public FunctionOutputDescriptionSyntaxNode FunctionOutputDescriptionSyntax(SyntaxList outputList, SyntaxToken assignmentSign)
        {
            return new FunctionOutputDescriptionSyntaxNode(outputList, assignmentSign);
        }

        public FunctionInputDescriptionSyntaxNode FunctionInputDescriptionSyntax(SyntaxToken openingBracket, SyntaxList parameterList, SyntaxToken closingBracket)
        {
            return new FunctionInputDescriptionSyntaxNode(openingBracket, parameterList, closingBracket);
        }

        public SwitchStatementSyntaxNode SwitchStatementSyntax(SyntaxToken switchKeyword, ExpressionSyntaxNode switchExpression, SyntaxList<SyntaxToken> optionalCommas, SyntaxList<SwitchCaseSyntaxNode> cases, SyntaxToken endKeyword)
        {
            return new SwitchStatementSyntaxNode(switchKeyword, switchExpression, optionalCommas, cases, endKeyword);
        }

        public SwitchCaseSyntaxNode SwitchCaseSyntax(SyntaxToken caseKeyword, ExpressionSyntaxNode caseIdentifier, SyntaxList<SyntaxToken> optionalCommas, StatementSyntaxNode body)
        {
            return new SwitchCaseSyntaxNode(caseKeyword, caseIdentifier, optionalCommas, body);
        }

        public WhileStatementSyntaxNode WhileStatementSyntax(SyntaxToken whileKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, StatementSyntaxNode body, SyntaxToken endKeyword)
        {
            return new WhileStatementSyntaxNode(whileKeyword, condition, optionalCommas, body, endKeyword);
        }

        public ElseifClause ElseifClause(SyntaxToken elseifKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, StatementSyntaxNode body)
        {
            return new ElseifClause(elseifKeyword, condition, optionalCommas, body);
        }

        public ElseClause ElseClause(SyntaxToken elseKeyword, StatementSyntaxNode body)
        {
            return new ElseClause(elseKeyword, body);
        }

        public IfStatementSyntaxNode IfStatementSyntax(SyntaxToken ifKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, StatementSyntaxNode body, SyntaxList<ElseifClause> elseifClauses, ElseClause? elseClause, SyntaxToken endKeyword)
        {
            return new IfStatementSyntaxNode(ifKeyword, condition, optionalCommas, body, elseifClauses, elseClause, endKeyword);
        }

        public ForStatementSyntaxNode ForStatementSyntax(SyntaxToken forKeyword, AssignmentExpressionSyntaxNode assignment, SyntaxList<SyntaxToken> optionalCommas, StatementSyntaxNode body, SyntaxToken endKeyword)
        {
            return new ForStatementSyntaxNode(forKeyword, assignment, optionalCommas, body, endKeyword);
        }

        public AssignmentExpressionSyntaxNode AssignmentExpressionSyntax(ExpressionSyntaxNode lhs, SyntaxToken assignmentSign, ExpressionSyntaxNode rhs)
        {
            return new AssignmentExpressionSyntaxNode(lhs, assignmentSign, rhs);
        }

        public CatchClauseSyntaxNode CatchClauseSyntax(SyntaxToken catchKeyword, SyntaxList catchBody)
        {
            return new CatchClauseSyntaxNode(catchKeyword, catchBody);
        }

        public TryCatchStatementSyntaxNode TryCatchStatementSyntax(SyntaxToken tryKeyword, StatementSyntaxNode tryBody, CatchClauseSyntaxNode? catchClause, SyntaxToken endKeyword)
        {
            return new TryCatchStatementSyntaxNode(tryKeyword, tryBody, catchClause, endKeyword);
        }

        public ExpressionStatementSyntaxNode ExpressionStatementSyntax(ExpressionSyntaxNode expression)
        {
            return new ExpressionStatementSyntaxNode(expression);
        }

        public EmptyStatementSyntaxNode EmptyStatementSyntax(SyntaxToken semicolon)
        {
            return new EmptyStatementSyntaxNode(semicolon);
        }

        public EmptyExpressionSyntaxNode EmptyExpressionSyntax()
        {
            return new EmptyExpressionSyntaxNode();
        }

        public UnaryPrefixOperationExpressionSyntaxNode UnaryPrefixOperationExpressionSyntax(SyntaxToken operation, ExpressionSyntaxNode operand)
        {
            return new UnaryPrefixOperationExpressionSyntaxNode(operation, operand);
        }

        public CompoundNameExpressionSyntaxNode CompoundNameExpressionSyntax(SyntaxList nodes)
        {
            return new CompoundNameExpressionSyntaxNode(nodes);
        }

        public NamedFunctionHandleExpressionSyntaxNode NamedFunctionHandleExpressionSyntax(SyntaxToken atSign, CompoundNameExpressionSyntaxNode functionName)
        {
            return new NamedFunctionHandleExpressionSyntaxNode(atSign, functionName);
        }

        public LambdaExpressionSyntaxNode LambdaExpressionSyntax(SyntaxToken atSign, FunctionInputDescriptionSyntaxNode input, ExpressionSyntaxNode body)
        {
            return new LambdaExpressionSyntaxNode(atSign, input, body);
        }

        public BinaryOperationExpressionSyntaxNode BinaryOperationExpressionSyntax(ExpressionSyntaxNode lhs, SyntaxToken operation, ExpressionSyntaxNode rhs)
        {
            return new BinaryOperationExpressionSyntaxNode(lhs, operation, rhs);
        }

        public IdentifierNameExpressionSyntaxNode IdentifierNameExpressionSyntax(SyntaxToken name)
        {
            return new IdentifierNameExpressionSyntaxNode(name);
        }

        public NumberLiteralExpressionSyntaxNode NumberLiteralExpressionSyntax(SyntaxToken number)
        {
            return new NumberLiteralExpressionSyntaxNode(number);
        }

        public StringLiteralExpressionSyntaxNode StringLiteralExpressionSyntax(SyntaxToken stringToken)
        {
            return new StringLiteralExpressionSyntaxNode(stringToken);
        }

        public DoubleQuotedStringLiteralExpressionSyntaxNode DoubleQuotedStringLiteralExpressionSyntax(SyntaxToken stringToken)
        {
            return new DoubleQuotedStringLiteralExpressionSyntaxNode(stringToken);
        }

        public UnquotedStringLiteralExpressionSyntaxNode UnquotedStringLiteralExpressionSyntax(SyntaxToken stringToken)
        {
            return new UnquotedStringLiteralExpressionSyntaxNode(stringToken);
        }

        public ArrayLiteralExpressionSyntaxNode ArrayLiteralExpressionSyntax(SyntaxToken openingSquareBracket, SyntaxList nodes, SyntaxToken closingSquareBracket)
        {
            return new ArrayLiteralExpressionSyntaxNode(openingSquareBracket, nodes, closingSquareBracket);
        }

        public CellArrayLiteralExpressionSyntaxNode CellArrayLiteralExpressionSyntax(SyntaxToken openingBrace, SyntaxList nodes, SyntaxToken closingBrace)
        {
            return new CellArrayLiteralExpressionSyntaxNode(openingBrace, nodes, closingBrace);
        }

        public ParenthesizedExpressionSyntaxNode ParenthesizedExpressionSyntax(SyntaxToken openingBracket, ExpressionSyntaxNode expression, SyntaxToken closingBracket)
        {
            return new ParenthesizedExpressionSyntaxNode(openingBracket, expression, closingBracket);
        }

        public CellArrayElementAccessExpressionSyntaxNode CellArrayElementAccessExpressionSyntax(ExpressionSyntaxNode expression, SyntaxToken openingBrace, SyntaxList nodes, SyntaxToken closingBrace)
        {
            return new CellArrayElementAccessExpressionSyntaxNode(expression, openingBrace, nodes, closingBrace);
        }

        public FunctionCallExpressionSyntaxNode FunctionCallExpressionSyntax(ExpressionSyntaxNode functionName, SyntaxToken openingBracket, SyntaxList nodes, SyntaxToken closingBracket)
        {
            return new FunctionCallExpressionSyntaxNode(functionName, openingBracket, nodes, closingBracket);
        }

        public MemberAccessExpressionSyntaxNode MemberAccessExpressionSyntax(SyntaxNode leftOperand, SyntaxToken dot, SyntaxNode rightOperand)
        {
            return new MemberAccessExpressionSyntaxNode(leftOperand, dot, rightOperand);
        }

        public UnaryPostfixOperationExpressionSyntaxNode UnaryPostfixOperationExpressionSyntax(ExpressionSyntaxNode operand, SyntaxToken operation)
        {
            return new UnaryPostfixOperationExpressionSyntaxNode(operand, operation);
        }

        public IndirectMemberAccessExpressionSyntaxNode IndirectMemberAccessExpressionSyntax(SyntaxToken openingBracket, ExpressionSyntaxNode expression, SyntaxToken closingBracket)
        {
            return new IndirectMemberAccessExpressionSyntaxNode(openingBracket, expression, closingBracket);
        }

        public CommandExpressionSyntaxNode CommandExpressionSyntax(SyntaxToken commandName, SyntaxList<UnquotedStringLiteralExpressionSyntaxNode> arguments)
        {
            return new CommandExpressionSyntaxNode(commandName, arguments);
        }

        public ClassInvokationExpressionSyntaxNode ClassInvokationExpressionSyntax(ExpressionSyntaxNode methodName, SyntaxToken atSign, ExpressionSyntaxNode baseClassNameAndArguments)
        {
            return new ClassInvokationExpressionSyntaxNode(methodName, atSign, baseClassNameAndArguments);
        }

        public AttributeAssignmentSyntaxNode AttributeAssignmentSyntax(SyntaxToken assignmentSign, ExpressionSyntaxNode value)
        {
            return new AttributeAssignmentSyntaxNode(assignmentSign, value);
        }

        public AttributeSyntaxNode AttributeSyntax(SyntaxToken name, AttributeAssignmentSyntaxNode? assignment)
        {
            return new AttributeSyntaxNode(name, assignment);
        }

        public AttributeListSyntaxNode AttributeListSyntax(SyntaxToken openingBracket, SyntaxList nodes, SyntaxToken closingBracket)
        {
            return new AttributeListSyntaxNode(openingBracket, nodes, closingBracket);
        }

        public ConcreteMethodDeclarationSyntaxNode ConcreteMethodDeclarationSyntax(SyntaxToken functionKeyword, FunctionOutputDescriptionSyntaxNode? outputDescription, CompoundNameExpressionSyntaxNode name, FunctionInputDescriptionSyntaxNode? inputDescription, SyntaxList<SyntaxToken> commas, StatementSyntaxNode body, EndKeywordSyntaxNode? endKeyword)
        {
            return new ConcreteMethodDeclarationSyntaxNode(functionKeyword, outputDescription, name, inputDescription, commas, body, endKeyword);
        }

        public AbstractMethodDeclarationSyntaxNode AbstractMethodDeclarationSyntax(FunctionOutputDescriptionSyntaxNode? outputDescription, CompoundNameExpressionSyntaxNode name, FunctionInputDescriptionSyntaxNode? inputDescription)
        {
            return new AbstractMethodDeclarationSyntaxNode(outputDescription, name, inputDescription);
        }

        public MethodsListSyntaxNode MethodsListSyntax(SyntaxToken methodsKeyword, AttributeListSyntaxNode? attributes, SyntaxList methods, SyntaxToken endKeyword)
        {
            return new MethodsListSyntaxNode(methodsKeyword, attributes, methods, endKeyword);
        }

        public PropertiesListSyntaxNode PropertiesListSyntax(SyntaxToken propertiesKeyword, AttributeListSyntaxNode? attributes, SyntaxList properties, SyntaxToken endKeyword)
        {
            return new PropertiesListSyntaxNode(propertiesKeyword, attributes, properties, endKeyword);
        }

        public BaseClassListSyntaxNode BaseClassListSyntax(SyntaxToken lessSign, SyntaxList baseClasses)
        {
            return new BaseClassListSyntaxNode(lessSign, baseClasses);
        }

        public ClassDeclarationSyntaxNode ClassDeclarationSyntax(SyntaxToken classdefKeyword, AttributeListSyntaxNode? attributes, SyntaxToken className, BaseClassListSyntaxNode? baseClassList, SyntaxList nodes, SyntaxToken endKeyword)
        {
            return new ClassDeclarationSyntaxNode(classdefKeyword, attributes, className, baseClassList, nodes, endKeyword);
        }

        public EnumerationItemValueSyntaxNode EnumerationItemValueSyntax(SyntaxToken openingBracket, SyntaxList values, SyntaxToken closingBracket)
        {
            return new EnumerationItemValueSyntaxNode(openingBracket, values, closingBracket);
        }

        public EnumerationItemSyntaxNode EnumerationItemSyntax(SyntaxToken name, EnumerationItemValueSyntaxNode? values, SyntaxList<SyntaxToken> commas)
        {
            return new EnumerationItemSyntaxNode(name, values, commas);
        }

        public EnumerationListSyntaxNode EnumerationListSyntax(SyntaxToken enumerationKeyword, AttributeListSyntaxNode? attributes, SyntaxList<EnumerationItemSyntaxNode> items, SyntaxToken endKeyword)
        {
            return new EnumerationListSyntaxNode(enumerationKeyword, attributes, items, endKeyword);
        }

        public EventsListSyntaxNode EventsListSyntax(SyntaxToken eventsKeyword, AttributeListSyntaxNode? attributes, SyntaxList events, SyntaxToken endKeyword)
        {
            return new EventsListSyntaxNode(eventsKeyword, attributes, events, endKeyword);
        }

        public EndKeywordSyntaxNode EndKeywordSyntax(SyntaxToken endKeyword)
        {
            return new EndKeywordSyntaxNode(endKeyword);
        }
    }
}