namespace Parser.Binding
{
    public enum BoundNodeKind
    {
        Root,
        File,

        // Statements
        
        AbstractMethodDeclaration,
        BlockStatement,
        ClassDeclaration,
        ConcreteMethodDeclaration,
        EmptyStatement,
        ExpressionStatement,
        ForStatement,
        FunctionDeclaration,
        IfStatement,
        SwitchStatement,
        TryCatchStatement,
        WhileStatement,

        // Expressions
        
        ArrayLiteralExpression,
        AssignmentExpression,
        BinaryOperationExpression,
        CellArrayElementAccessExpression,
        CellArrayLiteralExpression,
        ClassInvokationExpression,
        CommandExpression,
        CompoundNameExpression,
        DoubleQuotedStringLiteralExpression,
        EmptyExpression,
        FunctionCallExpression,
        IdentifierNameExpression,
        IndirectMemberAccessExpression,
        LambdaExpression,
        MemberAccessExpression,
        NamedFunctionHandleExpression,
        NumberLiteralExpression,
        ParenthesizedExpression,
        StringLiteralExpression,
        UnaryPrefixOperationExpression,
        UnaryPostfixOperationExpression,
        UnquotedStringLiteralExpression,

        // Parts
        ElseIfClause,
        ElseClause
    }
}
