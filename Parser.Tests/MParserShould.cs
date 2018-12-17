using Xunit;

namespace Parser.Tests
{
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
            var assignment = actual.Root.StatementList[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(assignment);
            Assert.IsType<AssignmentExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)assignment).Expression);
        }

        [Fact]
        public void ParseExpressionStatement()
        {
            var text = "2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.StatementList[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(statement);
            Assert.IsType<BinaryOperationExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)statement).Expression);
        }

        [Fact]
        public void ParseAssignmentExpression_Incomplete()
        {
            var text = "a = ";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var assignment = actual.Root.StatementList[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(assignment);
            Assert.IsType<AssignmentExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)assignment).Expression);
        }

        [Theory]
        [InlineData("2 + ;")]
        public void ParseStatement(string text)
        {
            var sut = GetSut(text);
            var actual = sut.Parse();
            Assert.Collection(actual.Diagnostics, item => Assert.Equal("Unexpected token 'SemicolonToken', expected 'IdentifierToken'.", item.Message));
        }

        [Fact]
        public void ProvidePosition()
        {
            var text = "2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.StatementList[0].AsNode() as ExpressionStatementSyntaxNode;
            var expression = statement.Expression as BinaryOperationExpressionSyntaxNode;
            var lhs = expression.Lhs;
            var operation = expression.Operation;
            var rhs = expression.Rhs;
            Assert.Equal(0, lhs.Position);
            Assert.Equal(2, operation.Position);
            Assert.Equal(4, rhs.Position);
        }
    }
}