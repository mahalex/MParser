namespace Parser
{
    // We use the same set of kinds for syntax tokens & syntax nodes.
    public enum TokenKind
    {
        // SYNTAX TOKENS

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

        // SYNTAX NODES
        // The whole file.
        File = 100,
        // a list of syntax nodes and/or tokens.
        List,
        // [output1, output2] = function(input1, input2)
        // <...>
        // end
        FunctionDeclaration,
        // (input1, input2)
        FunctionInputDescription,
        // [output1, output2] =
        FunctionOutputDescription,
        // switch a
        //     case 1
        // <...>
        // end
        SwitchStatement,
        // case 1
        //     doSomething();
        SwitchCase,
        // while a < 10
        //     doSomething();
        // end
        WhileStatement,
        // if a < 5
        //     doSomething();
        // elseif a > 10
        //     doSomethingElse();
        // else
        //     GiveUp();
        // end
        IfStatement,
        // elseif a > 10
        //     doSomethingElse();
        ElseifClause,
        // else
        //     GiveUp();
        ElseClause,
        // for a = 1:5
        //     process(a);
        // end
        ForStatement,
        // a = 1:5
        AssignmentExpression,
        // catch e
        //     dealWithIt(e);
        // end
        CatchClause,
        // try
        //     somethingWeird();
        // catch e
        //     dealWithIt(e);
        // end
        TryCatchStatement,
        // a = 5;
        ExpressionStatement,
        // 
        EmptyStatement,
        // 
        EmptyExpression,
        // -13
        UnaryPrefixOperationExpression,
        // some.complex.name
        CompoundName,
        // @func
        NamedFunctionHandle,
        // @(x) x + 1
        Lambda,
        // +
        BinaryOperation,
        // a
        IdentifierName,
        // 123
        NumberLiteralExpression,
        // 'abc'
        StringLiteralExpression,
        // "abc"
        DoubleQuotedStringLiteralExpression,
        // abc
        UnquotedStringLiteralExpression,
        // [1, 2; 3 4]
        ArrayLiteralExpression,
        // {1, 3, 'abc'}
        CellArrayLiteralExpression,
        // (1 + 2 * 3)
        ParenthesizedExpression,
        // abc{2}
        CellArrayElementAccess,
        // doSomething(5)
        FunctionCall,
        // object.member
        MemberAccess,
        // [1 2 3]'
        UnaryPostfixOperationExpression,
        // struct.(field)
        IndirectMemberAccess,
        // cd some/+folder/
        Command,
        // method@SuperClass(object)
        ClassInvokation,
        // = true
        AttributeAssignment,
        // Sealed = true
        Attribute,
        // (Sealed = true)
        AttributeList,
        // function result = method(obj)
        //     <...>
        // end
        MethodDefinition,
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
        // classdef MyClass < BaseClass, AnotherBaseClass
        //     properties
        //         y
        //     end
        //     methods
        //         <...>
        //     end
        // end
        ClassDeclaration,
        // (1)
        EnumerationItemValue,
        // One (1)
        EnumerationItem,
        // enumeration
        //     One (1)
        //     Two (2)
        // end
        EnumerationList,
        // result = abstractMethod(object)
        AbstractMethodDeclaration,
        // events
        //     ToggleSomething
        // end
        EventsList,
        EndKeyword,
        Root
    }
}