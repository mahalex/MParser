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
        public BoundExpressionStatement(SyntaxNode syntax, BoundExpression expression, bool discardResult)
            : base(syntax)
        {
            Expression = expression;
            DiscardResult = discardResult;
        }

        public BoundExpression Expression { get; }

        public bool DiscardResult { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;
    }

    public class BoundForStatement : BoundStatement
    {
        public BoundForStatement(
            SyntaxNode syntax,
            BoundIdentifierNameExpression loopVariable,
            BoundExpression loopedExpression,
            BoundStatement body)
            : base(syntax)
        {
            LoopVariable = loopVariable;
            LoopedExpression = loopedExpression;
            Body = body;
        }

        public BoundIdentifierNameExpression LoopVariable { get; }

        public BoundExpression LoopedExpression { get; }

        public BoundStatement Body { get; }

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

        public BoundFunctionDeclaration WithBody(BoundStatement body)
        {
            if (body == Body)
            {
                return this;
            }

            return new BoundFunctionDeclaration(
                Syntax,
                Name,
                InputDescription,
                OutputDescription,
                body);
        }
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
        public BoundWhileStatement(SyntaxNode syntax, BoundExpression condition, BoundStatement body)
            : base(syntax)
        {
            Condition = condition;
            Body = body;
        }

        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;

        public BoundExpression Condition { get; }

        public BoundStatement Body { get; }
    }

    public abstract class BoundExpression : BoundNode
    {
        public BoundExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public abstract TypeSymbol Type { get; }
    }

    public class BoundArrayLiteralExpression : BoundExpression
    {
        public BoundArrayLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ArrayLiteralExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
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

        public override TypeSymbol Type => Right.Type;
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

        public override TypeSymbol Type => Op.Result;
    }

    public class BoundCellArrayElementAccessExpression : BoundExpression
    {
        public BoundCellArrayElementAccessExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CellArrayElementAccessExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundCellArrayLiteralExpression : BoundExpression
    {
        public BoundCellArrayLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CellArrayLiteralExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundClassInvokationExpression : BoundExpression
    {
        public BoundClassInvokationExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ClassInvokationExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundCommandExpression : BoundExpression
    {
        public BoundCommandExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CommandExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundCompoundNameExpression : BoundExpression
    {
        public BoundCompoundNameExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.CompoundNameExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundDoubleQuotedStringLiteralExpression : BoundExpression
    {
        public BoundDoubleQuotedStringLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.DoubleQuotedStringLiteralExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundEmptyExpression : BoundExpression
    {
        public BoundEmptyExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.EmptyExpression;

        public override TypeSymbol Type => TypeSymbol.Null;
    }

    public class BoundErrorExpression : BoundExpression
    {
        public BoundErrorExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.ErrorExpression;

        public override TypeSymbol Type => TypeSymbol.Error;
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

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundTypedFunctionCallExpression : BoundExpression
    {
        public BoundTypedFunctionCallExpression(
            SyntaxNode syntax,
            TypedFunctionSymbol function,
            ImmutableArray<BoundExpression> arguments)
            : base(syntax)
        {
            Function = function;
            Arguments = arguments;
        }

        public TypedFunctionSymbol Function { get; }
        public ImmutableArray<BoundExpression> Arguments { get; }
        public override BoundNodeKind Kind => BoundNodeKind.TypedFunctionCallExpression;

        public override TypeSymbol Type => Function.ReturnType;
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

        public override TypeSymbol Type => TypeSymbol.MObject;
    }

    public class BoundIndirectMemberAccessExpression : BoundExpression
    {
        public BoundIndirectMemberAccessExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.IndirectMemberAccessExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundLambdaExpression : BoundExpression
    {
        public BoundLambdaExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.LambdaExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundMemberAccessExpression : BoundExpression
    {
        public BoundMemberAccessExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.MemberAccessExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundNamedFunctionHandleExpression : BoundExpression
    {
        public BoundNamedFunctionHandleExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.NamedFunctionHandleExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public abstract class BoundNumberLiteralExpression : BoundExpression
    {
        protected BoundNumberLiteralExpression(SyntaxNode syntax) : base(syntax)
        {
        }
    }

    public class BoundNumberDoubleLiteralExpression : BoundNumberLiteralExpression
    {
        public BoundNumberDoubleLiteralExpression(SyntaxNode syntax, double value)
            : base(syntax)
        {
            Value = value;
        }

        public double Value { get; }
        public override BoundNodeKind Kind => BoundNodeKind.NumberDoubleLiteralExpression;

        public override TypeSymbol Type => TypeSymbol.Double;
    }

    public class BoundNumberIntLiteralExpression : BoundNumberLiteralExpression
    {
        public BoundNumberIntLiteralExpression(SyntaxNode syntax, int value)
            : base(syntax)
        {
            Value = value;
        }

        public int Value { get; }
        public override BoundNodeKind Kind => BoundNodeKind.NumberIntLiteralExpression;

        public override TypeSymbol Type => TypeSymbol.Int;
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

        public override TypeSymbol Type => TypeSymbol.String;
    }

    public class BoundTypedVariableDeclaration : BoundStatement
    {
        public BoundTypedVariableDeclaration(SyntaxNode syntax, TypedVariableSymbol variable, BoundExpression initializer)
            : base(syntax)
        {
            Variable = variable;
            Initializer = initializer;
        }

        public TypedVariableSymbol Variable { get; }

        public BoundExpression Initializer { get; }

        public override BoundNodeKind Kind => BoundNodeKind.TypedVariableDeclaration;
    }

    public class BoundTypedVariableExpression : BoundExpression
    {
        public BoundTypedVariableExpression(SyntaxNode syntax, TypedVariableSymbol variable)
            : base(syntax)
        {
            Variable = variable;
        }

        public TypedVariableSymbol Variable { get; }

        public override BoundNodeKind Kind => BoundNodeKind.TypedVariableExpression;

        public override TypeSymbol Type => Variable.Type;
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

        public override TypeSymbol Type => Op.Result;
    }

    public class BoundUnquotedStringLiteralExpression : BoundExpression
    {
        public BoundUnquotedStringLiteralExpression(SyntaxNode syntax)
            : base(syntax)
        {
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnquotedStringLiteralExpression;

        public override TypeSymbol Type => throw new System.NotImplementedException();
    }

    public class BoundConversionExpression : BoundExpression
    {
        public BoundConversionExpression(SyntaxNode syntax, TypeSymbol targetType, BoundExpression expression)
            : base(syntax)
        {
            TargetType = targetType;
            Expression = expression;
        }

        public TypeSymbol TargetType { get; }

        public BoundExpression Expression { get; }

        public override BoundNodeKind Kind => BoundNodeKind.ConversionExpression;

        public override TypeSymbol Type => TargetType;
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
