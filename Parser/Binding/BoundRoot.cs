using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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

    public enum BoundBinaryOperatorKind
    {
        Equals,
        PipePipe,
        AmpersandAmpersand,
        Pipe,
        Ampersand,
        Less,
        LessOrEquals,
        Greater,
        GreaterOrEquals,
        EqualsEquals,
        TildeEquals,
        Colon,
        Plus,
        Minus,
        Star,
        DotStar,
        Slash,
        DotSlash,
        Backslash,
        DotBackslash,
        Tilde,
        Caret,
        DotCaret,
    }

    public abstract class BoundNode
    {
        public BoundNode(SyntaxNode syntax)
        {
            Syntax = syntax;
        }

        public SyntaxNode Syntax { get; }
        public abstract BoundNodeKind Kind { get; }
    }

    public class BoundRoot : BoundNode
    {
        public BoundRoot(SyntaxNode syntax, BoundFile file)
            : base(syntax)
        {
            File = file;
        }

        public BoundFile File { get; }

        public override BoundNodeKind Kind => BoundNodeKind.Root;
    }

    public class BoundFile : BoundNode
    {
        public BoundFile(SyntaxNode syntax, ImmutableArray<BoundStatement> statements)
            : base(syntax)
        {
            Statements = statements;
        }

        public ImmutableArray<BoundStatement> Statements { get; }

        public override BoundNodeKind Kind => BoundNodeKind.File;
    }

    public abstract class BoundStatement : BoundNode
    {
        public BoundStatement(SyntaxNode syntax)
            : base(syntax)
        {
        }
    }

    public abstract class BoundMethodDeclaration : BoundStatement
    {
        public BoundMethodDeclaration(SyntaxNode syntax)
            : base(syntax)
        {
        }
    }

    public class BoundAbstractMethodDeclaration : BoundMethodDeclaration
    {
        public BoundAbstractMethodDeclaration(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.AbstractMethodDeclaration;
    }

    public class BoundBlockStatement : BoundStatement
    {
        public BoundBlockStatement(SyntaxNode syntax, ImmutableArray<BoundStatement> statements)
            : base(syntax)
        {
            Statements = statements;
        }

        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;

        public ImmutableArray<BoundStatement> Statements { get; }
    }

    public class BoundClassDeclaration : BoundStatement
    {
        public BoundClassDeclaration(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ClassDeclaration;
    }

    public class BoundConcreteMethodDeclaration : BoundMethodDeclaration
    {
        public BoundConcreteMethodDeclaration(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ConcreteMethodDeclaration;
    }

    public class BoundEmptyStatement : BoundStatement
    {
        public BoundEmptyStatement(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.EmptyStatement;
    }

    public class BoundExpressionStatement : BoundStatement
    {
        public BoundExpression Expression { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;

        public BoundExpressionStatement(SyntaxNode syntax, BoundExpression expression)
            : base(syntax)
        {
            Expression = expression;
        }
    }

    public class BoundForStatement : BoundStatement
    {
        public BoundForStatement(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;
    }

    public class BoundFunctionDeclaration : BoundStatement
    {
        public BoundFunctionDeclaration(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.FunctionDeclaration;
    }

    public class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(SyntaxNode syntax, BoundExpression condition, BoundBlockStatement body, IEnumerable<ElseifClause> elseIfClauses, BoundElseClause? elseClause)
            : base(syntax)
        {
            Condition = condition;
            Body = body;
            ElseIfClauses = elseIfClauses;
            ElseClause = elseClause;
        }

        public BoundExpression Condition { get; }
        public BoundBlockStatement Body { get; }
        public IEnumerable<ElseifClause> ElseIfClauses { get; }
        public BoundElseClause? ElseClause { get; }

        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
    }

    public class BoundSwitchStatement : BoundStatement
    {
        public BoundSwitchStatement(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.SwitchStatement;
    }

    public class BoundTryCatchStatement : BoundStatement
    {
        public BoundTryCatchStatement(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.TryCatchStatement;
    }

    public class BoundWhileStatement : BoundStatement
    {
        public BoundWhileStatement(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
    }

    public abstract class BoundExpression : BoundNode
    {
        public BoundExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }
    }

    public class BoundArrayLiteralExpression : BoundExpression
    {
        public BoundArrayLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ArrayLiteralExpression;
    }

    public class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(SyntaxNode syntax, BoundExpression left, BoundExpression right)
            : base(syntax)
        {
            Left = left;
            Right = right;
        }

        public BoundExpression Left { get; }
        public BoundExpression Right { get; }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
    }

    public class BoundBinaryOperationExpression : BoundExpression
    {
        public BoundBinaryOperationExpression(SyntaxNode syntax, BoundExpression left, BoundBinaryOperator op, BoundExpression right)
            : base(syntax)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public BoundExpression Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpression Right { get; }

        public override BoundNodeKind Kind => BoundNodeKind.BinaryOperationExpression;
    }

    public class BoundCellArrayElementAccessExpression : BoundExpression
    {
        public BoundCellArrayElementAccessExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CellArrayElementAccessExpression;
    }

    public class BoundCellArrayLiteralExpression : BoundExpression
    {
        public BoundCellArrayLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CellArrayLiteralExpression;
    }

    public class BoundClassInvokationExpression : BoundExpression
    {
        public BoundClassInvokationExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ClassInvokationExpression;
    }

    public class BoundCommandExpression : BoundExpression
    {
        public BoundCommandExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CommandExpression;
    }

    public class BoundCompoundNameExpression : BoundExpression
    {
        public BoundCompoundNameExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CompoundNameExpression;
    }

    public class BoundDoubleQuotedStringLiteralExpression : BoundExpression
    {
        public BoundDoubleQuotedStringLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.DoubleQuotedStringLiteralExpression;
    }

    public class BoundEmptyExpression : BoundExpression
    {
        public BoundEmptyExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.EmptyExpression;
    }

    public class BoundFunctionCallExpression : BoundExpression
    {
        public BoundFunctionCallExpression(SyntaxNode syntax, BoundExpression name, ImmutableArray<BoundExpression> arguments)
            : base(syntax)
        {
            Name = name;
            Arguments = arguments;
        }

        public BoundExpression Name { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }
        public override BoundNodeKind Kind => BoundNodeKind.FunctionCallExpression;
    }

    public class BoundIdentifierNameExpression : BoundExpression
    {
        public BoundIdentifierNameExpression(SyntaxNode syntax, string name)
            : base(syntax)
        {
            Name = name;
        }

        public string Name { get; }
        public override BoundNodeKind Kind => BoundNodeKind.IdentifierNameExpression;
    }

    public class BoundIndirectMemberAccessExpression : BoundExpression
    {
        public BoundIndirectMemberAccessExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.IndirectMemberAccessExpression;
    }

    public class BoundLambdaExpression : BoundExpression
    {
        public BoundLambdaExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.LambdaExpression;
    }

    public class BoundMemberAccessExpression : BoundExpression
    {
        public BoundMemberAccessExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.MemberAccessExpression;
    }

    public class BoundNamedFunctionHandleExpression : BoundExpression
    {
        public BoundNamedFunctionHandleExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.NamedFunctionHandleExpression;
    }

    public class BoundNumberLiteralExpression : BoundExpression
    {
        public BoundNumberLiteralExpression(SyntaxNode syntax, double value)
            : base(syntax)
        {
            Value = value;
        }

        public double Value { get; }
        public override BoundNodeKind Kind => BoundNodeKind.NumberLiteralExpression;
    }

    public class BoundParenthesizedExpression : BoundExpression
    {
        public BoundParenthesizedExpression(SyntaxNode syntax, BoundExpression expression)
            : base(syntax)
        {
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.ParenthesizedExpression;

        public BoundExpression Expression { get; }
    }

    public class BoundStringLiteralExpression : BoundExpression
    {
        public BoundStringLiteralExpression(SyntaxNode syntax, string value)
            : base(syntax)
        {
            Value = value;
        }

        public string Value { get; }
        public override BoundNodeKind Kind => BoundNodeKind.StringLiteralExpression;
    }

    public class BoundUnaryPrefixOperationExpression : BoundExpression
    {
        public BoundUnaryPrefixOperationExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryPrefixOperationExpression;
    }

    public class BoundUnaryPostfixOperationExpression : BoundExpression
    {
        public BoundUnaryPostfixOperationExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryPostfixOperationExpression;
    }

    public class BoundUnquotedStringLiteralExpression : BoundExpression
    {
        public BoundUnquotedStringLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnquotedStringLiteralExpression;
    }

    public class BoundElseIfClause : BoundNode
    {
        public BoundElseIfClause(SyntaxNode syntax, BoundExpression condition, BoundBlockStatement body)
            : base(syntax)
        {
            Condition = condition;
            Body = body;
        }

        public BoundExpression Condition { get; }
        public BoundBlockStatement Body { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ElseIfClause;
    }

    public class BoundElseClause : BoundNode
    {
        public BoundElseClause(SyntaxNode syntax, BoundBlockStatement body)
            : base(syntax)
        {
            Body = body;
        }

        public BoundBlockStatement Body { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ElseClause;
    }

    public class BoundBinaryOperator
    {
        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(TokenKind.EqualsToken, BoundBinaryOperatorKind.Equals),
            new BoundBinaryOperator(TokenKind.PipePipeToken, BoundBinaryOperatorKind.PipePipe),
            new BoundBinaryOperator(TokenKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.AmpersandAmpersand),
            new BoundBinaryOperator(TokenKind.PipeToken, BoundBinaryOperatorKind.Pipe),
            new BoundBinaryOperator(TokenKind.AmpersandToken, BoundBinaryOperatorKind.Ampersand),
            new BoundBinaryOperator(TokenKind.LessToken, BoundBinaryOperatorKind.Less),
            new BoundBinaryOperator(TokenKind.LessOrEqualsToken, BoundBinaryOperatorKind.LessOrEquals),
            new BoundBinaryOperator(TokenKind.GreaterToken, BoundBinaryOperatorKind.Greater),
            new BoundBinaryOperator(TokenKind.GreaterOrEqualsToken, BoundBinaryOperatorKind.GreaterOrEquals),
            new BoundBinaryOperator(TokenKind.EqualsEqualsToken, BoundBinaryOperatorKind.EqualsEquals),
            new BoundBinaryOperator(TokenKind.TildeEqualsToken, BoundBinaryOperatorKind.TildeEquals),
            new BoundBinaryOperator(TokenKind.ColonToken, BoundBinaryOperatorKind.Colon),
            new BoundBinaryOperator(TokenKind.PlusToken, BoundBinaryOperatorKind.Plus),
            new BoundBinaryOperator(TokenKind.MinusToken, BoundBinaryOperatorKind.Minus),
            new BoundBinaryOperator(TokenKind.StarToken, BoundBinaryOperatorKind.Star),
            new BoundBinaryOperator(TokenKind.DotStarToken, BoundBinaryOperatorKind.DotStar),
            new BoundBinaryOperator(TokenKind.SlashToken, BoundBinaryOperatorKind.Slash),
            new BoundBinaryOperator(TokenKind.DotSlashToken, BoundBinaryOperatorKind.DotSlash),
            new BoundBinaryOperator(TokenKind.BackslashToken, BoundBinaryOperatorKind.Backslash),
            new BoundBinaryOperator(TokenKind.DotBackslashToken, BoundBinaryOperatorKind.DotBackslash),
            new BoundBinaryOperator(TokenKind.TildeToken, BoundBinaryOperatorKind.Tilde),
            new BoundBinaryOperator(TokenKind.CaretToken, BoundBinaryOperatorKind.Caret),
            new BoundBinaryOperator(TokenKind.DotCaretToken, BoundBinaryOperatorKind.DotCaret),
        };

        public BoundBinaryOperator(TokenKind syntaxKind, BoundBinaryOperatorKind kind)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
        }

        public TokenKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }

        internal static BoundBinaryOperator? GetOperator(TokenKind kind)
        {
            return _operators.FirstOrDefault(op => op.SyntaxKind == kind);
        }
    }
}
