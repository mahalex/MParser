namespace Lexer
{
    public class Trivia
    {
        public TriviaType Type { get; }
        public string LiteralText { get; }

        public Trivia(TriviaType type, string literalText)
        {
            Type = type;
            LiteralText = literalText;
        }
    }
}