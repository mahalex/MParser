namespace Parser
{
    // We use the same set of kinds for syntax tokens & syntax nodes.
    public enum TokenKind
    {
        // *****************
        // * SYNTAX TOKENS *
        // *****************

        None = 0,
        BadToken = 1,
        // The lexer puts a virtual "end of file" token at the end of the parsed file.
        EndOfFileToken = 2,
        // Identifier: could be a reserved word, a variable name, a class name, etc.
        IdentifierToken = 3,
        // Number literal: 123, 45.678, 2e-5, etc.
        NumberLiteralToken = 4,
        // String literal: 'abc', '123', etc. The "usual" string literals are single-quoted and are just char arrays.
        StringLiteralToken = 5,
        // Double-quoted string literal: "abc", "123", etc. These are the "new" string literal that are more like strings
        // and less like char arrays (for example, char arrays could be columns instead of rows, or even multi-dimensional).
        DoubleQuotedStringLiteralToken = 6,
        // This is for supporting "command statements" like
        // > cd some/+folder/
        // In this example, "some/folder" should be treated as a string literal (for example, "+' there should be a part
        // of it, and not parsed as a binary operator).
        UnquotedStringLiteralToken = 7,
        
        // trivia
        
        // Spaces, tabs, etc.
        WhitespaceToken = 10,
        // New line characters.
        NewlineToken = 11,
        // There are three types of comments:
        // * comments starting with % and lasting till the end of the line;
        // * comments starting with %{ and ending with %}, possibly spanning several lines;
        // * comments starting with ... and lasting till the end of the line.
        // At the moment, this token is used to accomodate all of them, so the other two
        // (MultilineComment = 13 and DotDotDot = 56 are not used).
        CommentToken = 12,
        // Comments starting with %{ and ending with %}, possibly spanning several lines. Not used, see Comment = 12.
        MultilineCommentToken = 13,
        
        // operators

        // =
        EqualsToken = 20,
        // ==
        EqualsEqualsToken = 21,
        // ~=
        TildeEqualsToken = 22,
        // &&
        AmpersandAmpersandToken = 23,
        // ||
        PipePipeToken = 24,
        // &
        AmpersandToken = 25,
        // |
        PipeToken = 26,
        // <
        LessToken = 27,
        // <=
        LessOrEqualsToken = 28,
        // >
        GreaterToken = 29,
        // >=
        GreaterOrEqualsToken = 30,
        // ~
        TildeToken = 31,
        // +
        PlusToken = 32,
        // -
        MinusToken = 33,
        // *
        StarToken = 34,
        // /
        SlashToken = 35,
        // ^
        CaretToken = 36,
        // \
        BackslashToken = 37,
        // ' (this is the same as starting string literal; we'll have some fun distinguishing those).
        ApostropheToken = 38,
        // .*
        DotStarToken = 39,
        // ./
        DotSlashToken = 40,
        // .^
        DotCaretToken = 41,
        // .\
        DotBackslashToken = 42,
        // .'
        DotApostropheToken = 43,
        // @
        AtToken = 44,
        // :
        ColonToken = 45,
        // ?
        QuestionToken = 46,
        // ,
        CommaToken = 47,
        // ;
        SemicolonToken = 48,
        // {
        OpenBraceToken = 49,
        // }
        CloseBraceToken = 50,
        // [
        OpenSquareBracketToken = 51,
        // ]
        CloseSquareBracketToken = 52,
        // (
        OpenParenthesisToken = 53,
        // )
        CloseParenthesisToken = 54,
        // .
        DotToken = 55,
        // Comments starting with ... and lasting till the end of the line.  Not used, see Comment = 12.
        DotDotDotToken = 56,

        // Unary tokens are not recognized during lexing; they are contextually recognized while parsing.
        UnaryPlus = 57,
        UnaryMinus = 58,
        UnaryNot = 59,
        UnaryQuestionMark = 60,

        // ****************
        // * SYNTAX NODES *
        // ****************

        // The whole file.
        File = 100,

        // a list of syntax nodes and/or tokens.
        List,


        // STATEMENTS
        // The name ends with "Declaration" or "Statement".

        // result = abstractMethod(object)
        AbstractMethodDeclaration,

        // statement1
        // statement2;
        BlockStatement,

        // classdef MyClass < BaseClass, AnotherBaseClass
        //     properties
        //         y
        //     end
        //     methods
        //         <...>
        //     end
        // end
        ClassDeclaration,

        // function result = method(obj)
        //     <...>
        // end
        ConcreteMethodDeclaration,

        // 
        EmptyStatement,

        // a = 5;
        ExpressionStatement,

        // for a = 1:5
        //     process(a);
        // end
        ForStatement,

        // [output1, output2] = function(input1, input2)
        // <...>
        // end
        FunctionDeclaration,

        // if a < 5
        //     doSomething();
        // elseif a > 10
        //     doSomethingElse();
        // else
        //     GiveUp();
        // end
        IfStatement,

        // switch a
        //     case 1
        // <...>
        // end
        SwitchStatement,

        // try
        //     somethingWeird();
        // catch e
        //     dealWithIt(e);
        // end
        TryCatchStatement,

        // while a < 10
        //     doSomething();
        // end
        WhileStatement,


        // EXPRESSIONS
        // The name ends with "Expression".

        // [1, 2; 3 4]
        ArrayLiteralExpression,

        // a = 1:5
        AssignmentExpression,

        // +
        BinaryOperationExpression,

        // abc{2}
        CellArrayElementAccessExpression,

        // {1, 3, 'abc'}
        CellArrayLiteralExpression,

        // method@SuperClass(object)
        ClassInvokationExpression,

        // cd some/+folder/
        CommandExpression,

        // some.complex.name
        CompoundNameExpression,

        // "abc"
        DoubleQuotedStringLiteralExpression,

        // 
        EmptyExpression,

        // doSomething(5)
        FunctionCallExpression,

        // a
        IdentifierNameExpression,

        // struct.(field)
        IndirectMemberAccessExpression,

        // @(x) x + 1
        LambdaExpression,

        // object.member
        MemberAccessExpression,

        // @func
        NamedFunctionHandleExpression,

        // 123
        NumberLiteralExpression,

        // (1 + 2 * 3)
        ParenthesizedExpression,

        // 'abc'
        StringLiteralExpression,

        // -13
        UnaryPrefixOperationExpression,

        // [1 2 3]'
        UnaryPostfixOperationExpression,

        // abc
        UnquotedStringLiteralExpression,


        // PARTS OF STATEMENTS & EXPRESSIONS

        // (input1, input2)
        FunctionInputDescription,
        // [output1, output2] =
        FunctionOutputDescription,

        // case 1
        //     doSomething();
        SwitchCase,

        // elseif a > 10
        //     doSomethingElse();
        ElseifClause,
        // else
        //     GiveUp();
        ElseClause,
        // catch e
        //     dealWithIt(e);
        // end
        CatchClause,

        // = true
        AttributeAssignment,
        // Sealed = true
        Attribute,
        // (Sealed = true)
        AttributeList,

        // methods
        //     function result = method(obj)
        //     <...>
        //     end
        // end
        MethodsList,
        // properties
        //     x
        //     y
        // end
        PropertiesList,
        // < BaseClass, AnotherBaseClass
        BaseClassList,

        // (1)
        EnumerationItemValue,
        // One (1)
        EnumerationItem,
        // enumeration
        //     One (1)
        //     Two (2)
        // end
        EnumerationList,
        // events
        //     ToggleSomething
        // end
        EventsList,
        EndKeyword,
        Root
    }
}