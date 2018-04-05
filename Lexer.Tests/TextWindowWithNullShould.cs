using NUnit.Framework;

namespace Lexer.Tests
{
    [TestFixture]
    public class TestWindowWithNullShould
    {
        [Test]
        public void ReturnNullGivenEmptyText()
        {
            var sut = new TextWindowWithNull("");
            Assert.IsTrue(sut.PeekChar() == '\0');
        }

        [Test]
        public void ReturnCharsInCorrectOrder()
        {
            var text = "abc";
            var sut = new TextWindowWithNull(text);
            Assert.AreEqual('a', sut.PeekChar());
            sut.ConsumeChar();
            Assert.AreEqual('b', sut.PeekChar());
            sut.ConsumeChar();
            Assert.AreEqual('c', sut.PeekChar());
            sut.ConsumeChar();
            Assert.AreEqual('\0', sut.PeekChar());
        }
    }
}