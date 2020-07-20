using Parser.Binding;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static Parser.Binding.BoundNodeFactory;

namespace Parser.Lowering
{
    internal class Lowerer : BoundTreeRewriter
    {
        private int _labelNumber = 0;
        private int _localVariableNumber = 0;

        private Lowerer()
        {
        }

        private BoundLabel GenerateLabel()
        {
            var name = $"Label{++_labelNumber}";
            return new BoundLabel(name);
        }

        private TypedVariableSymbol GenerateTypedLocalVariable(TypeSymbol typeSymbol)
        {
            var name = $"#{++_localVariableNumber}";
            return new TypedVariableSymbol(name, typeSymbol);
        }

        public override BoundStatement RewriteIfStatement(BoundIfStatement node)
        {
            // if cond
            //     body
            // elseif cond1
            //     body1
            // elseif cond2
            //     body2
            // else
            //     body3
            //
            // gotoFalse cond Label1
            // body
            // goto LabelEnd
            // Label1:
            // gotoFalse cond1 Label2
            // body1
            // goto LabelEnd
            // Label2:
            // gotoFalse cond2 Label3
            // body2
            // goto LabelEnd
            // Label3:
            // body3
            // LabelEnd:

            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var numberOfLabelsNeeded = 1 + node.ElseifClauses.Length + (node.ElseClause is null ? 0 : 1);
            var labels = new BoundLabel[numberOfLabelsNeeded];
            for (var i = 0; i < numberOfLabelsNeeded; i++)
            {
                labels[i] = GenerateLabel();
            }

            builder.Add(GotoIfFalse(node.Syntax, node.Condition, labels[0]));
            builder.Add(node.Body);
            if (numberOfLabelsNeeded >= 2)
            {
                var counter = 0;
                foreach (var elseifClause in node.ElseifClauses)
                {
                    builder.Add(Goto(node.Syntax, labels[^1]));
                    builder.Add(LabelStatement(node.Syntax, labels[counter]));
                    counter++;
                    builder.Add(GotoIfFalse(node.Syntax, elseifClause.Condition, labels[counter]));
                    builder.Add(elseifClause.Body);
                }
                if (node.ElseClause is { } elseClause)
                {
                    builder.Add(Goto(node.Syntax, labels[^1]));
                    builder.Add(LabelStatement(node.Syntax, labels[counter]));
                    builder.Add(elseClause);
                }
            }

            builder.Add(LabelStatement(node.Syntax, labels[^1]));
            return RewriteBlockStatement(Block(node.Syntax, builder.ToArray()));
        }

        public override BoundStatement RewriteWhileStatement(BoundWhileStatement node)
        {
            // while cond
            //     body
            // end
            //
            //      |
            //      |
            //      V
            //
            // LabelLoop:
            //     gotoFalse cond LabelEnd
            //     body
            //     goto LabelLoop
            // LabelEnd:
            var labelLoop = GenerateLabel();
            var labelEnd = GenerateLabel();
            var result = Block(
                node.Syntax,
                // LabelLoop:
                LabelStatement(node.Syntax, labelLoop),
                //     gotoFalse cond LabelEnd
                GotoIfFalse(
                    node.Syntax,
                    node.Condition,
                    labelEnd),
                //     body
                node.Body,
                //     goto LabelLoop
                Goto(node.Syntax, labelLoop),
                // LabelEnd:
                LabelStatement(node.Syntax, labelEnd));
            return RewriteBlockStatement(result);
        }

        public override BoundStatement RewriteForStatement(BoundForStatement node)
        {
            // for i = expr
            //     body
            // end
            //
            //   #array = expr
            //   #length = len(#array)
            //   #index = 0
            //   i = #array(#index);
            // LabelLoop:
            //   gotoFalse (#index < #length) LabelEnd
            //   body
            //   #index = #index + 1
            //   goto LabelLoop
            // LabelEnd:
            var labelLoop = GenerateLabel();
            var labelEnd = GenerateLabel();
            var localArray = GenerateTypedLocalVariable(TypeSymbol.MObject);
            var localLength = GenerateTypedLocalVariable(TypeSymbol.Int);
            var localIndex = GenerateTypedLocalVariable(TypeSymbol.Int);
            return RewriteBlockStatement(Block(
                node.Syntax,
                //   #array = expr
                TypedVariableDeclaration(node.Syntax, localArray, node.LoopedExpression),
                //   #length = len(#array)
                TypedVariableDeclaration(
                    node.Syntax,
                    localLength,
                    FunctionCall(
                        node.Syntax,
                        Identifier(node.Syntax, "len"),
                        new[] { (BoundExpression)TypedVariableExpression(node.Syntax, localArray) }.ToImmutableArray())),
                //   #index = 0
                TypedVariableDeclaration(node.Syntax, localIndex, NumberDoubleLiteral(node.Syntax, 0.0)),
                //   i = #array(#index);
                ExpressionStatement(
                    node.Syntax,
                    Assignment(
                        node.Syntax,
                        node.LoopVariable,
                        FunctionCall(
                            node.Syntax,
                            TypedVariableExpression(node.Syntax, localArray),
                            new[] { (BoundExpression)TypedVariableExpression(node.Syntax, localIndex) }.ToImmutableArray())),
                    discardResult: true),
                // LabelLoop:
                LabelStatement(node.Syntax, labelLoop),
                //   gotoFalse (#index < #length) LabelEnd
                GotoIfFalse(
                    node.Syntax,
                    BinaryOperation(
                        node.Syntax,
                        TypedVariableExpression(node.Syntax, localIndex),
                        BoundBinaryOperator.GetOperator(TokenKind.LessToken, TypeSymbol.Int, TypeSymbol.Int)!,
                        TypedVariableExpression(node.Syntax, localLength)),
                    labelEnd),
                //   body
                node.Body,
                //   #index = #index + 1;
                ExpressionStatement(
                    node.Syntax,
                    Assignment(
                        node.Syntax,
                        TypedVariableExpression(node.Syntax, localIndex),
                        BinaryOperation(
                            node.Syntax,
                            TypedVariableExpression(node.Syntax, localIndex),
                            BoundBinaryOperator.GetOperator(TokenKind.PlusToken, TypeSymbol.Int, TypeSymbol.Int)!,
                            NumberIntLiteral(node.Syntax, 1))),
                    discardResult: true),
                //   goto LabelLoop
                Goto(node.Syntax, labelLoop),
                // LabelEnd:
                LabelStatement(node.Syntax, labelEnd)));
        }

        public static BoundBlockStatement Lower(BoundStatement statement)
        {
            var lowerer = new Lowerer();
            var result = lowerer.RewriteStatement(statement);
            var flatResult = lowerer.Flatten(result);
            return flatResult;
        }

        private BoundBlockStatement Flatten(BoundStatement statement)
        {
            var builder = ImmutableArray.CreateBuilder<BoundStatement>();
            var stack = new Stack<BoundStatement>();

            stack.Push(statement);
            while (stack.Count() > 0)
            {
                var current = stack.Pop();
                if (current is BoundBlockStatement block)
                {
                    foreach (var s in block.Statements.Reverse())
                    {
                        stack.Push(s);
                    }
                }
                else
                {
                    builder.Add(current);
                }
            }

            return Block(statement.Syntax, builder.ToImmutable());
        }
    }
}
