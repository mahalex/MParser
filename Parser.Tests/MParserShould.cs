﻿using NUnit.Framework;

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
        
        [Test]
        public void ParseAssignmentExpression()
        {
            var text = "a = b";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var assignment = actual.Root.StatementList[0].AsNode();
            Assert.IsInstanceOf<ExpressionStatementSyntaxNode>(assignment);
            Assert.IsInstanceOf<AssignmentExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)assignment).Expression);
        }

        [Test]
        public void ParseAssignmentExpression_Incomplete()
        {
            var text = "a = ";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var assignment = actual.Root.StatementList[0].AsNode();
            Assert.IsInstanceOf<ExpressionStatementSyntaxNode>(assignment);
            Assert.IsInstanceOf<AssignmentExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)assignment).Expression);
        }
    }
}