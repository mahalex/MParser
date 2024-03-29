﻿using Xunit;
using FluentAssertions;
using System;

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
            Assert.IsType<BinaryOperationExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)statement!).Expression);
        }

        [Fact]
        public void ParseExpressionStatementWithSemicolon()
        {
            var text = "2 + 3;";
            var sut = GetSut(text);
            var actual = sut.Parse();
            Assert.Single(actual.Root.Body.Statements);
            var statement = actual.Root.Body.Statements[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(statement);
            Assert.IsType<BinaryOperationExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)statement!).Expression);
        }

        [Fact]
        public void ParseExpressionStatementWithSemicolonAfterNewLine()
        {
            var text = "2 + 3\n;";
            var sut = GetSut(text);
            var actual = sut.Parse();
            Assert.Equal(2, actual.Root.Body.Statements.Count);
            var statement1 = actual.Root.Body.Statements[0].AsNode();
            Assert.IsType<ExpressionStatementSyntaxNode>(statement1);
            Assert.IsType<BinaryOperationExpressionSyntaxNode>(((ExpressionStatementSyntaxNode)statement1!).Expression);
            var statement2 = actual.Root.Body.Statements[1].AsToken();
            Assert.Equal(TokenKind.SemicolonToken, statement2.Kind);
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
            var text = "% Comment\n  2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.Body.Statements[0].AsNode() as ExpressionStatementSyntaxNode;
            var expression = statement!.Expression as BinaryOperationExpressionSyntaxNode;
            var lhs = expression!.Lhs;
            var operation = expression.Operation;
            var rhs = expression.Rhs;
            Assert.Equal(0, lhs.Position);
            Assert.Equal(14, operation.Position);
            Assert.Equal(16, rhs.Position);
        }

        [Fact]
        public void ProvideFullSpan()
        {
            var text = "% Comment\n  2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.Body.Statements[0].AsNode() as ExpressionStatementSyntaxNode;
            var expression = statement!.Expression as BinaryOperationExpressionSyntaxNode;
            var lhs = expression!.Lhs;
            var operation = expression.Operation;
            var rhs = expression.Rhs;
            expression.FullSpan.Start.Should().Be(0);
            expression.FullSpan.End.Should().Be(17);
            lhs.FullSpan.Start.Should().Be(0);
            lhs.FullSpan.End.Should().Be(14);
            operation.FullSpan.Start.Should().Be(14);
            operation.FullSpan.End.Should().Be(16);
            rhs.FullSpan.Start.Should().Be(16);
            rhs.FullSpan.End.Should().Be(17);
        }

        [Fact]
        public void ProvideSpan()
        {
            var text = "% Comment\n  2 + 3";
            var sut = GetSut(text);
            var actual = sut.Parse();
            var statement = actual.Root.Body.Statements[0].AsNode() as ExpressionStatementSyntaxNode;
            var expression = statement!.Expression as BinaryOperationExpressionSyntaxNode;
            var lhs = expression!.Lhs;
            var operation = expression.Operation;
            var rhs = expression.Rhs;
            expression.Span.Start.Should().Be(12);
            expression.Span.End.Should().Be(17);
            lhs.Span.Start.Should().Be(12);
            lhs.Span.End.Should().Be(13);
            operation.Span.Start.Should().Be(14);
            operation.Span.End.Should().Be(15);
            rhs.Span.Start.Should().Be(16);
            rhs.Span.End.Should().Be(17);
        }

        [Fact]
        public void NotHangOnUnknownSyntax()
        {
            var text = @"
classdef myClass
    properties
        Channel;
        NodeID;
        Node;
    end
    methods
        function this = sendData(this, arg1, arg2)
            arguments
                this (1,1) myClass
                arg1 (1,1) double {mustBeNonnegative}
                arg2 (1,1) double {mustBeNonnegative}
            end
            If (arg1 = 0)
            this.NodeID = 3;
        end
    end
    function this = getData(this, arg1, arg2)
    end
end";
            var sut = GetSut(text);
            Func<SyntaxTree> action = sut.Parse;
            action.Should().Throw<ParsingException>();
        }
    }
}