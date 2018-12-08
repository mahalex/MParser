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
    }
}