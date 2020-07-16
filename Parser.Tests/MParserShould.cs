using Xunit;

namespace Parser.Tests
{
    using static DiagnosticExtensions;

    public class MParserShould
    {
        private static MParser GetSut(string text)
        {
            var window = new TextWindowWithNull(text);
            var parser = new MParser(window);
            return parser;
        }
        
        [Fact]
        public void ParseAssignmentExpression()
        {
            var text = "a = b";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var assignment = actual.Root.Body.Statements[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(assignment);
            if (assignment is null)
            {
                throw new System.Exception();
            }

            Assert.IsType<AssignmentExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)assignment).Expression);
        }

        [Fact]
        public void ParseExpressionStatement()
        {
            var text = "2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.Body.Statements[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(statement);
            if (statement is null)
            {
                throw new System.Exception();
            }

            Assert.IsType<BinaryOperationExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)statement).Expression);
        }

        [Fact]
        public void ParseAssignmentExpression_Incomplete()
        {
            var text = "a = ";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var assignment = actual.Root.Body.Statements[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(assignment);
            if (assignment is null)
            {
                throw new System.Exception();
            }

            Assert.IsType<AssignmentExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)assignment).Expression);
        }

        [Theory]
        [InlineData("2 + ;")]
        public void ParseStatement(string text)
        {
            var sut = GetSut(text);
            var actual = sut.Parse();
            var diagnostics = actual.Root.GetDiagnostics();
            Assert.True(diagnostics.IsEquivalentTo(MissingToken(4, TokenKind.IdentifierToken)));
        }

        [Fact]
        public void ProvidePosition()
        {
            var text = "2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.Body.Statements[0].AsNode() as ExpressionStatementSyntaxNode;
            var expression = statement!.Expression as BinaryOperationExpressionSyntaxNode;
            var lhs = expression!.Lhs;
            var operation = expression.Operation;
            var rhs = expression.Rhs;
            Assert.Equal(0, lhs.Position);
            Assert.Equal(2, operation.Position);
            Assert.Equal(4, rhs.Position);
        }
    }
}