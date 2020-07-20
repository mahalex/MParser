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
        ConditionalGotoStatement,
        EmptyStatement,
        ExpressionStatement,
        ForStatement,
        FunctionDeclaration,
        GotoStatement,
        IfStatement,
        LabelStatement,
        SwitchStatement,
        TryCatchStatement,
        TypedVariableDeclaration,
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
        ConversionExpression,
        DoubleQuotedStringLiteralExpression,
        EmptyExpression,
        ErrorExpression,
        FunctionCallExpression,
        IdentifierNameExpression,
        IndirectMemberAccessExpression,
        LambdaExpression,
        MemberAccessExpression,
        NamedFunctionHandleExpression,
        NumberLiteralExpression,
        ParenthesizedExpression,
        StringLiteralExpression,
        TypedFunctionCallExpression,
        TypedVariableExpression,
        UnaryOperationExpression,
        UnquotedStringLiteralExpression,

        // Parts
        ElseIfClause,
        ElseClause,
    }
}
