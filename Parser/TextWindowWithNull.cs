namespace Parser
{
    public class TextWindowWithNull : TextWindow
    {
        public TextWindowWithNull(string text, string fileName = null) : base(text, fileName)
        {
        }

        public override char PeekChar()
        {
            return IsEof() ? '\0' : base.PeekChar();
        }

        public override char PeekChar(int n)
        {
            return Position.Offset + n >= Text.Length ? '\0' : base.PeekChar(n);
        }
    }
}
