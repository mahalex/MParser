using Xunit;

namespace Parser.Tests
{
    public class TestWindowShould
    {
        [Fact]
        public void ReturnEofGivenEmptyText()
        {
            var sut = new TextWindow("");
            Assert.True(sut.IsEof());
        }

        [Fact]
        public void ReturnNotEofGivenNonEmptyText()
        {
            var sut = new TextWindow("Text.");
            Assert.False(sut.IsEof());
        }

        [Fact]
        public void ReturnCharsInCorrectOrder()
        {
            var text = "abc";
            var sut = new TextWindow(text);
            Assert.Equal('a', sut.PeekChar());
            sut.ConsumeChar();
            Assert.Equal('b', sut.PeekChar());
            sut.ConsumeChar();
            Assert.Equal('c', sut.PeekChar());
            sut.ConsumeChar();
            Assert.True(sut.IsEof());
        }
    }
}