namespace Parser
{
    // We use the same set of kinds for syntax tokens & syntax nodes.
    public enum TokenKind
    {
        // SYNTAX TOKENS

        None = 0,
        BadToken = 1,
        // The lexer puts a virtual "end of file" token at the end of the parsed file.
        EndOfFile = 2,
        // Identifier: could be a reserved word, a variable name, a class name, etc.
        Identifier = 3,
        // Number literal: 123, 45.678, 2e-5, etc.
        NumberLiteral = 4,
        // String literal: 'abc', '123', etc. The "usual" string literals are single-quoted and are just char arrays.
        StringLiteral = 5,
        // Double-quoted string literal: "abc", "123", etc. These are the "new" string literal that are more like strings
        // and less like char arrays (for example, char arrays could be columns instead of rows, or even multi-dimensional).
        DoubleQuotedStringLiteral = 6,
        // This is for supporting "command statements" like
        // > cd some/+folder/
        // In this example, "some/folder" should be treated as a string literal (for example, "+' there should be a part
        // of it, and not parsed as a binary operator).
        UnquotedStringLiteral = 7,
        
        // trivia
        
        // Spaces, tabs, etc.
        Whitespace = 10,
        // New line characters.
        Newline = 11,
        // There are three types of comments:
        // * comments starting with % and lasting till the end of the line;
        // * comments starting with %{ and ending with %}, possibly spanning several lines;
        // * comments starting with ... and lasting till the end of the line.
        // At the moment, this token is used to accomodate all of them, so the other two
        // (MultilineComment = 13 and DotDotDot = 56 are not used).
        Comment = 12,
        // Comments starting with %{ and ending with %}, possibly spanning several lines. Not used, see Comment = 12.
        MultilineComment = 13,
        
        // operators

        // =
        Assignment = 20,
        // ==
        Equality = 21,
        // ~=
        Inequality = 22,
        // &&
        LogicalAnd = 23,
        // ||
        LogicalOr = 24,
        // &
        BitwiseAnd = 25,
        // |
        BitwiseOr = 26,
        // <
        Less = 27,
        // <=
        LessOrEqual = 28,
        // >
        Greater = 29,
        // >=
        GreaterOrEqual = 30,
        // ~
        Not = 31,
        // +
        Plus = 32,
        // -
        Minus = 33,
        // *
        Multiply = 34,
        // /
        Divide = 35,
        // ^
        Power = 36,
        // \
        Backslash = 37,
        // ' (this is the same as starting string literal; we'll have some fun distinguishing those).
        Transpose = 38,
        // .*
        DotMultiply = 39,
        // ./
        DotDivide = 40,
        // .^
        DotPower = 41,
        // .\
        DotBackslash = 42,
        // .'
        DotTranspose = 43,
        // @
        At = 44,
        // :
        Colon = 45,
        // ?
        QuestionMark = 46,
        // ,
        Comma = 47,
        // ;
        Semicolon = 48,
        // {
        OpeningBrace = 49,
        // }
        ClosingBrace = 50,
        // [
        OpeningSquareBracket = 51,
        // ]
        ClosingSquareBracket = 52,
        // (
        OpeningBracket = 53,
        // )
        ClosingBracket = 54,
        // .
        Dot = 55,
        // Comments starting with ... and lasting till the end of the line.  Not used, see Comment = 12.
        DotDotDot = 56,
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
    }
}