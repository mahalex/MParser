namespace Lexer
{
    public struct PureToken
    {
        public TokenKind Kind { get; }
        public string LiteralText { get; }
        public object Value { get; }
        public IPosition Position { get; }

        public PureToken(TokenKind kind, string literalText, object value, IPosition position)
        {
            Kind = kind;
            LiteralText = literalText;
            Value = value;
            Position = position;
        }

        public override string ToString() => LiteralText;
    }
}