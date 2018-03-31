using System.Collections.Generic;
using System.Linq;

namespace Lexer
{
    public class Token
    {
        public List<Trivia> LeadingTrivia { get; }
        public List<Trivia> TrailingTrivia { get; }
        public PureToken PureToken { get; }
        public string FullText { get; }
        public TokenKind Kind => PureToken.Kind;

        public Token(PureToken pureToken, List<Trivia> leadingTrivia, List<Trivia> trailingTrivia)
        {
            PureToken = pureToken;
            LeadingTrivia = leadingTrivia;
            TrailingTrivia = trailingTrivia;
            FullText = BuildFullText();
        }

        private string BuildFullText()
        {
            var leading = LeadingTrivia.Select(t => t.LiteralText);
            var token = PureToken.LiteralText;
            var trailing = TrailingTrivia.Select(t => t.LiteralText);
            return string.Join("", leading.Concat(new[] {token}).Concat(trailing));
        }

        public override string ToString() => FullText;
    }
}