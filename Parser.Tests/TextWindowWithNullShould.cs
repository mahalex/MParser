using Xunit;

namespace Parser.Tests
{
    public class TestWindowWithNullShould
    {
        [Fact]
        public void ReturnNullGivenEmptyText()
        {
            var sut = new TextWindowWithNull("");
            Assert.True(sut.PeekChar() == '\0');
        }

        [Fact]
        public void ReturnCharsInCorrectOrder()
        {
            var text = "abc";
            var sut = new TextWindowWithNull(text);
            Assert.Equal('a', sut.PeekChar());
            sut.ConsumeChar();
            Assert.Equal('b', sut.PeekChar());
            sut.ConsumeChar();
            Assert.Equal('c', sut.PeekChar());
            sut.ConsumeChar();
            Assert.Equal('\0', sut.PeekChar());
        }
    }
}