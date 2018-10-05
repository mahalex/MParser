using System;

namespace Parser
{
    public struct SyntaxTrivia : IEquatable<SyntaxTrivia>
    {
        private readonly SyntaxNode _parent;
        private readonly Internal.GreenNode _trivia;

        internal SyntaxTrivia(SyntaxNode parent, Internal.GreenNode trivia)
        {
            _parent = parent;
            _trivia = trivia;
        }
        
        public SyntaxNode Parent => _parent;
        
        internal Internal.GreenNode Trivia => _trivia;

        public bool Equals(SyntaxTrivia other)
        {
            return Equals(_parent, other._parent) && Equals(_trivia, other._trivia);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SyntaxTrivia trivia && Equals(trivia);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_parent != null ? _parent.GetHashCode() : 0) * 397) ^ (_trivia != null ? _trivia.GetHashCode() : 0);
            }
        }

        public static bool operator ==(SyntaxTrivia left, SyntaxTrivia right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SyntaxTrivia left, SyntaxTrivia right)
        {
            return !left.Equals(right);
        }
        
        public string Text => _trivia.Text;
        public string FullText => _trivia.FullText;

        public override string ToString()
        {
            return Text;
        }
    }
}