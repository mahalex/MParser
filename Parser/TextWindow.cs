namespace Parser
{
    public class TextWindow : ITextWindow
    {
        protected readonly string Text;
        private Position _position;
        public Position Position => _position;

        public TextWindow(string text, string fileName = null)
        {
            Text = text;
            _position = new Position
            {
                FileName = fileName,
                Line = 0,
                Column = 0,
                Offset = 0
            };
        }

        public bool IsEof()
        {
            return _position.Offset >= Text.Length;
        }

        public virtual char PeekChar()
        {
            return Text[_position.Offset];
        }

        public virtual char PeekChar(int n)
        {
            return Text[_position.Offset + n];
        }

        public void ConsumeChar()
        {
            if (Text[_position.Offset] == '\n' || Text[_position.Offset] == '\r')
            {
                _position.Line++;
                _position.Column = 0;
            }
            else
            {
                _position.Column++;
            }
            _position.Offset++;
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
            _position.Offset += n;
        }

        public char GetAndConsumeChar()
        {
            var c = Text[_position.Offset];
            ConsumeChar();
            return c;
        }

        public string GetAndConsumeChars(int n)
        {
            var s = Text.Substring(_position.Offset, n);
            ConsumeChars(n);
            return s;
        }

        public int CharactersLeft()
        {
            return Text.Length - _position.Offset;
        }
    }
}
