using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Parser
{
    public struct SyntaxToken : IEquatable<SyntaxToken>
    {
        private readonly SyntaxNode _parent;
        private readonly Internal.GreenNode _token;

        public TokenKind Kind => _token.Kind;

        public override string? ToString()
        {
            return _token.ToString();
        }

        internal SyntaxToken(SyntaxNode parent, Internal.GreenNode token, int position)
        {
            _parent = parent;
            _token = token ?? throw new ArgumentNullException(nameof(token));
            Position = position;
        }

        public SyntaxNode Parent => _parent;
        internal Internal.GreenNode Token => _token;

        public int Position { get; }

        public object? Value => _token.GetValue();

        public bool Equals(SyntaxToken other)
        {
            return Equals(_parent, other._parent) && Equals(_token, other._token);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SyntaxToken token && Equals(token);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_parent != null ? _parent.GetHashCode() : 0) * 397) ^ (_token != null ? _token.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SyntaxToken left, SyntaxToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SyntaxToken left, SyntaxToken right)
        {
            return !left.Equals(right);
        }

        public string Text => _token.Text;
        public string FullText => _token.FullText;
        public int FullWidth => _token.FullWidth;
        public bool IsMissing => _token.IsMissing;

        public IReadOnlyList<SyntaxTrivia> LeadingTrivia
        {
            get
            {
                var p = _parent;
                return _token.LeadingTrivia.Select(trivia => new SyntaxTrivia(p, trivia)).ToImmutableList();
            }
        }

        public IReadOnlyList<SyntaxTrivia> TrailingTrivia
        {
            get
            {
                var p = _parent;
                return _token.TrailingTrivia.Select(trivia => new SyntaxTrivia(p, trivia)).ToImmutableList();
            }
        }
    }
}