using System.Collections.Generic;
using System.Linq;
using Lexer;

using NUnit.Framework;

namespace Parser.Tests
{
    public class MParserShould
    {
        private static MParser CreateParser(string text)
        {
            var window = new TextWindowWithNull(text);
            var lexer = new MLexer(window, new PureTokenFactory(window));
            var tokens = lexer.ParseAll();
            var parser = new MParser(tokens);
            return parser;
        }

        [Test]
        public void ParseAssignmentExpression()
        {
            var text = "a = b";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<AssignmentExpressionNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }
        
        [Test]
        public void ParseSimpleStatement()
        {
            var text = "a = b";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<ExpressionStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseFunctionCallExpression()
        {
            var text = "func(a, 2, 'abc', d)";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<FunctionCallExpressionNode>(actual);
            var f = actual as FunctionCallExpressionNode;
            Assert.AreEqual(4, f?.Parameters.Parameters.Count);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseArrayLiteralExpression()
        {
            var text = "[a, 2, 'text']";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var a = actual as ArrayLiteralExpressionNode;
            Assert.AreEqual(3, a?.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseLeftAssociativeSamePrecedence()
        {
            var text = "2 + 3 + 4";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(actual);
            var e = (BinaryOperationExpressionNode)actual;
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(e.Lhs);
            Assert.IsInstanceOf<NumberLiteralNode>(e.Rhs);
            Assert.AreEqual(text, actual.FullText);
        }
        
        [Test]
        public void ParseLeftAssociativeRaisingPrecedence()
        {
            var text = "2 + 3 * 4";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(actual);
            var e = (BinaryOperationExpressionNode) actual;
            Assert.AreEqual(TokenKind.Plus, e.Operation.Token.Kind);
            Assert.IsInstanceOf<NumberLiteralNode>(e.Lhs);
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(e.Rhs);
            Assert.AreEqual(text, actual.FullText);
        }
        
        [Test]
        public void ParseLeftAssociativeLoweringPrecedence()
        {
            var text = "2 * 3 + 4";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(actual);
            var e = (BinaryOperationExpressionNode) actual;
            Assert.AreEqual(TokenKind.Plus, e.Operation.Token.Kind);
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(e.Lhs);
            Assert.IsInstanceOf<NumberLiteralNode>(e.Rhs);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseUnaryOperators()
        {
            var text = "-42";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<UnaryPrefixOperationExpressionNode>(actual);
            var e = (UnaryPrefixOperationExpressionNode) actual;
            Assert.AreEqual(TokenKind.Minus, e.Operation.Token.Kind);
            Assert.IsInstanceOf<NumberLiteralNode>(e.Operand);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseMemberAccess()
        {
            var text = "a.b.c";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<MemberAccessNode>(actual);
            var m = (MemberAccessNode) actual;
            Assert.IsInstanceOf<MemberAccessNode>(m.LeftOperand);
            Assert.IsInstanceOf<IdentifierNameNode>(m.RightOperand);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseWhileStatement()
        {
            var text = "while a < b c = d end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<WhileStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseWhileStatementWithComma()
        {
            var text = "while a < b, c = d end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<WhileStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseIfStatement()
        {
            var text = "if 2 < 3 a = b end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<IfStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }
        
        [Test]
        public void ParseIfElseStatement()
        {
            var text = "if 2 < 3 a = b else c = d end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<IfStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseParenthesizedExpression()
        {
            var text = "2 * (3 + 4)";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(actual);
            var e = (BinaryOperationExpressionNode) actual;
            Assert.IsInstanceOf<NumberLiteralNode>(e.Lhs);
            Assert.IsInstanceOf<ParenthesizedExpressionNode>(e.Rhs);
            var p = (ParenthesizedExpressionNode) e.Rhs;
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(p.Expression);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseForStatement()
        {
            var text = "for i = 1:5 a = i end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<ForStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseEmptyArray()
        {
            var text = "[]";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var a = (ArrayLiteralExpressionNode) actual;
            Assert.AreEqual(0, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseCellArrayLiteral()
        {
            var text = "{ 1 2, 3 }";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<CellArrayLiteralExpressionNode>(actual);
            var a = (CellArrayLiteralExpressionNode) actual;
            Assert.AreEqual(3, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseIndirectMemberAccess()
        {
            var text = "abc.(def)";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<MemberAccessNode>(actual);
            var a = (MemberAccessNode) actual;
            Assert.IsInstanceOf<IdentifierNameNode>(a.LeftOperand);
            Assert.IsInstanceOf<IndirectMemberAccessNode>(a.RightOperand);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseMemberAccessAfterElementAccess()
        {
            var text = "a(1).b";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<MemberAccessNode>(actual);
            var m = (MemberAccessNode) actual;
            Assert.IsInstanceOf<FunctionCallExpressionNode>(m.LeftOperand);
            Assert.IsInstanceOf<IdentifierNameNode>(m.RightOperand);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseFunctionDeclarationWithoutInputs()
        {
            var text = "function a = b a = 1";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<FunctionDeclarationNode>(actual);
            var f = (FunctionDeclarationNode) actual;
            Assert.AreEqual(f.InputDescription, null);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseCellArrayWithCellArrayLiteralInside()
        {
            var text = "{1 2 a {3}}";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<CellArrayLiteralExpressionNode>(actual);
            var a = (CellArrayLiteralExpressionNode) actual;
            Assert.AreEqual(4, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }
        
        [Test]
        public void ParseCellArrayWithCellArrayAccessInside()
        {
            var text = "{1 2 a{3}}";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<CellArrayLiteralExpressionNode>(actual);
            var a = (CellArrayLiteralExpressionNode) actual;
            Assert.AreEqual(3, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseCellArrayWithElementInBracketsInside()
        {
            var text = "{1 2 a (3)}";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<CellArrayLiteralExpressionNode>(actual);
            var a = (CellArrayLiteralExpressionNode) actual;
            Assert.AreEqual(4, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }
        
        [Test]
        public void ParseCellArrayWithFunctionCallInside()
        {
            var text = "{1 2 a(3)}";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<CellArrayLiteralExpressionNode>(actual);
            var a = (CellArrayLiteralExpressionNode) actual;
            Assert.AreEqual(3, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseArrayWithCellArrayLiteralInside()
        {
            var text = "[1 2 a {3}]";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var a = (ArrayLiteralExpressionNode) actual;
            Assert.AreEqual(4, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }
        
        [Test]
        public void ParseArrayWithCellArrayAccessInside()
        {
            var text = "[1 2 a{3}]";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var a = (ArrayLiteralExpressionNode) actual;
            Assert.AreEqual(3, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseArrayWithElementInBracketsInside()
        {
            var text = "[1 2 a (3)]";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var a = (ArrayLiteralExpressionNode) actual;
            Assert.AreEqual(4, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }
        
        [Test]
        public void ParseArrayWithFunctionCallInside()
        {
            var text = "[1 2 a(3)]";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var a = (ArrayLiteralExpressionNode) actual;
            Assert.AreEqual(3, a.Elements.Elements.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseFunctionHandle()
        {
            var text = "@sqrt";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<NamedFunctionHandleNode>(actual);
            var f = (NamedFunctionHandleNode) actual;
            Assert.AreEqual(1, f.FunctionName.Names.Count);
            Assert.AreEqual("sqrt", f.FunctionName.Names[0].Token.PureToken.LiteralText);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseFunctionHandleWithCompoundName()
        {
            var text = "@a.b.c";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<NamedFunctionHandleNode>(actual);
            var f = (NamedFunctionHandleNode) actual;
            Assert.AreEqual(3, f.FunctionName.Names.Count);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseLambda()
        {
            var text = "@(x, y) x + y";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<LambdaNode>(actual);
            var f = (LambdaNode) actual;
            Assert.AreEqual(2, f.Input.Parameters.Parameters.Count);
            Assert.IsInstanceOf<BinaryOperationExpressionNode>(f.Body);
            Assert.AreEqual(text, actual.FullText);            
        }

        [Test]
        public void ParseTildeAsResultReplacement()
        {
            var text = "[a, ~, b]";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<ArrayLiteralExpressionNode>(actual);
            var f = (ArrayLiteralExpressionNode) actual;
            Assert.AreEqual(3, f.Elements.Elements.Count);
            Assert.IsInstanceOf<UnaryPrefixOperationExpressionNode>(f.Elements.Elements[1]);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseTildeAsFunctionInputReplacement()
        {
            var text = "function a(b, ~, c) end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<FunctionDeclarationNode>(actual);
            var f = (FunctionDeclarationNode) actual;
            Assert.AreEqual(3, f.InputDescription.Parameters.Parameters.Count);
            CollectionAssert.AreEqual(new[] { "b", "~", "c" }, f.InputDescription.Parameters.Parameters.Select(p => (p as TokenNode).Token.PureToken.LiteralText));
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseDotTranspose()
        {
            var text = "a.'";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<UnaryPostfixOperationExpressionNode>(actual);
            var e = (UnaryPostfixOperationExpressionNode) actual;
            Assert.AreEqual(TokenKind.DotTranspose, e.Operation.Token.Kind);
            Assert.AreEqual("a", (e.Operand as IdentifierNameNode)?.Token.PureToken.LiteralText);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseDoubleQuotedStringLiteral()
        {
            var text = "\"some string\"";
            var sut = CreateParser(text);
            var actual = sut.ParseExpression();
            Assert.IsInstanceOf<DoubleQuotedStringLiteralNode>(actual);
            var s = (DoubleQuotedStringLiteralNode) actual;
            Assert.AreEqual("some string", s.Token.PureToken.Value);
            Assert.AreEqual(text, actual.FullText);
        }

        [Test]
        public void ParseTryCatchStatement()
        {
            var text = "try a = b catch c = d end";
            var sut = CreateParser(text);
            var actual = sut.ParseStatement();
            Assert.IsInstanceOf<TryCatchStatementNode>(actual);
            Assert.AreEqual(text, actual.FullText);
        }
    }
}