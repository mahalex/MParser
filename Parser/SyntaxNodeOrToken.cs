namespace Parser
{
    public struct SyntaxNodeOrToken
    {
        private readonly Internal.GreenNode _token;
        private readonly SyntaxNode _nodeOrParent;
        private readonly bool _isToken;

        internal SyntaxNodeOrToken(SyntaxNode node)
        {
            _token = null;
            _nodeOrParent = node;
            _isToken = false;
            Position = node.Position;
        }

        internal SyntaxNodeOrToken(SyntaxNode parent, Internal.GreenNode token, int position)
        {
            _token = token;
            _nodeOrParent = parent;
            _isToken = true;
            Position = position;
        }

        public bool IsToken => _isToken;
        public bool IsNode => !IsToken;

        public int Position { get; }

        public SyntaxNode AsNode()
        {
            if (_isToken)
            {
                return default(SyntaxNode);
            }

            return _nodeOrParent;
        }

        public SyntaxToken AsToken()
        {
            if (!_isToken)
            {
                return default(SyntaxToken);
            }
            return new SyntaxToken(_nodeOrParent, _token, Position);
        }

        public static implicit operator SyntaxNodeOrToken(SyntaxToken token)
        {
            return new SyntaxNodeOrToken(token.Parent, token.Token, token.Position);
        }

        public static implicit operator SyntaxNodeOrToken(SyntaxNode node)
        {
            return new SyntaxNodeOrToken(node);
        }
    }
}