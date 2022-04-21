namespace Parser
{
    public abstract partial class SyntaxVisitor
    {
        public virtual void Visit(SyntaxNode? node)
        {
            node?.Accept(this);
        }

        public virtual void DefaultVisit(SyntaxNode node)
        {
        }

        public virtual void VisitList(SyntaxNodeOrTokenList list)
        {
            DefaultVisit(list);
        }

        public virtual void VisitRoot(RootSyntaxNode node)
        {
            DefaultVisit(node);
        }
    }
}