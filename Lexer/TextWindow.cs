namespace Lexer
{
    public class TextWindow : ITextWindow
    {
        protected readonly string Text;
        protected int Offset { get; set; }
        private PositionInsideFile _position;
        public IPosition Position => _position;

        public TextWindow(string text, string fileName = null)
        {
            Text = text;
            Offset = 0;
            _position = new PositionInsideFile
            {
                File = fileName,
                Line = 0,
                Column = 0
            };
        }

        public bool IsEof()
        {
            return Offset >= Text.Length;
        }

        public virtual char PeekChar()
        {
            return Text[Offset];
        }


        public virtual char PeekChar(int n)
        {
            return Text[Offset + n];
        }


        public void ConsumeChar()
        {
            if (Text[Offset] == '\n' || Text[Offset] == '\r')
            {
                _position.Line++;
                _position.Column = 0;
            }
            else
            {
                _position.Column++;
            }
            Offset++;
        }

        public void ConsumeChars(int n)
        {
            for (var i = 0; i < n; i++)
            {
                if (PeekChar(i) == '\n' || PeekChar(i) == '\r')
                {
                    _position.Line++;
                    _position.Column = 0;
                }
                else
                {
                    _position.Column++;
                }
            }
            Offset += n;
        }

        public char GetAndConsumeChar()
        {
            var c = Text[Offset];
            ConsumeChar();
            return c;
        }

        public string GetAndConsumeChars(int n)
        {
            var s = Text.Substring(Offset, n);
            ConsumeChars(n);
            return s;
        }

        public int CharactersLeft()
        {
            return Text.Length - Offset;
        }
    }
}
