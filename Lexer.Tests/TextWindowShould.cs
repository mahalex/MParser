using Lexer;
using NUnit.Framework;

namespace Parser.Tests
{
    [TestFixture]
    public class TestWindowShould
    {
        [Test]
        public void ReturnEofGivenEmptyText()
        {
            var sut = new TextWindow("");
            Assert.IsTrue(sut.IsEof());
        }

        [Test]
        public void ReturnNotEofGivenNonEmptyText()
        {
            var sut = new TextWindow("Text.");
            Assert.IsFalse(sut.IsEof());
        }

        [Test]
        public void ReturnCharsInCorrectOrder()
        {
            var text = "abc";
            var sut = new TextWindow(text);
            Assert.AreEqual('a', sut.PeekChar());
            sut.ConsumeChar();
            Assert.AreEqual('b', sut.PeekChar());
            sut.ConsumeChar();
            Assert.AreEqual('c', sut.PeekChar());
            sut.ConsumeChar();
            Assert.IsTrue(sut.IsEof());
        }
    }
}