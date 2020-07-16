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
        UnaryOperationExpression,
        UnquotedStringLiteralExpression,

        // Parts
        ElseIfClause,
        ElseClause
    }
}
