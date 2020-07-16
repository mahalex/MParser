namespace Parser.Binding
{
    public abstract class BoundNode
    {
        public BoundNode(SyntaxNode syntax)
        {
            Syntax = syntax;
        }

        public SyntaxNode Syntax { get; }
        public abstract BoundNodeKind Kind { get; }
    }
}
