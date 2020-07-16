using System.Collections.Immutable;

namespace Parser.Binding
{
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
        public BoundFile(SyntaxNode syntax, BoundStatement body)
            : base(syntax)
        {
            Body = body;
        }

        public BoundStatement Body { get; }

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

    public class BoundConditionalGotoStatement : BoundStatement
    {
        public BoundConditionalGotoStatement(SyntaxNode syntax, BoundExpression condition, BoundLabel label, bool gotoIfTrue = true)
            : base(syntax)
        {
            Condition = condition;
            Label = label;
            GotoIfTrue = gotoIfTrue;
        }

        public BoundExpression Condition { get; }

        public BoundLabel Label { get; }

        public bool GotoIfTrue { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ConditionalGotoStatement;
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
        public BoundFunctionDeclaration(SyntaxNode syntax, string name, ImmutableArray<ParameterSymbol> inputDescription, ImmutableArray<ParameterSymbol> outputDescription, BoundStatement body)
            : base(syntax)
        {
            Name = name;
            InputDescription = inputDescription;
            OutputDescription = outputDescription;
            Body = body;
        }

        public override BoundNodeKind Kind => BoundNodeKind.FunctionDeclaration;

        public string Name { get; }
        public ImmutableArray<ParameterSymbol> InputDescription { get; }
        public ImmutableArray<ParameterSymbol> OutputDescription { get; }
        public BoundStatement Body { get; }
    }

    public class BoundGotoStatement : BoundStatement
    {
        public BoundGotoStatement(SyntaxNode syntax, BoundLabel label)
            : base(syntax)
        {
            Label = label;
        }

        public BoundLabel Label { get; }

        public override BoundNodeKind Kind => BoundNodeKind.GotoStatement;
    }

    public class BoundIfStatement : BoundStatement
    {
        public BoundIfStatement(SyntaxNode syntax, BoundExpression condition, BoundStatement body, ImmutableArray<BoundElseifClause> elseifClauses, BoundStatement? elseClause)
            : base(syntax)
        {
            Condition = condition;
            Body = body;
            ElseifClauses = elseifClauses;
            ElseClause = elseClause;
        }

        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
        public ImmutableArray<BoundElseifClause> ElseifClauses { get; }
        public BoundStatement? ElseClause { get; }

        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
    }

    public class BoundLabelStatement : BoundStatement
    {
        public BoundLabelStatement(SyntaxNode syntax, BoundLabel label)
            : base(syntax)
        {
            Label = label;
        }

        public BoundLabel Label { get; }

        public override BoundNodeKind Kind => BoundNodeKind.LabelStatement;
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

    public class BoundUnaryOperationExpression : BoundExpression
    {
        public BoundUnaryOperationExpression(SyntaxNode syntax, BoundUnaryOperator op, BoundExpression operand)
            : base(syntax)
        {
            Op = op;
            Operand = operand;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryOperationExpression;

        public BoundUnaryOperator Op { get; }
        public BoundExpression Operand { get; }
    }

    public class BoundUnquotedStringLiteralExpression : BoundExpression
    {
        public BoundUnquotedStringLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnquotedStringLiteralExpression;
    }

    public class BoundElseifClause : BoundNode
    {
        public BoundElseifClause(SyntaxNode syntax, BoundExpression condition, BoundStatement body)
            : base(syntax)
        {
            Condition = condition;
            Body = body;
        }

        public BoundExpression Condition { get; }
        public BoundStatement Body { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ElseIfClause;
    }
}
