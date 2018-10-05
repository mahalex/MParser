namespace Parser
{
    public abstract class SyntaxWalker : SyntaxVisitor
    {
        public override void DefaultVisit(SyntaxNode node)
        {
            foreach (var nodeOrToken in node.GetChildNodesAndTokens())
            {
                if (nodeOrToken.IsNode)
                {
                    Visit(nodeOrToken.AsNode());
                }
                else
                {
                    VisitToken(nodeOrToken.AsToken());
                }
            }
        }

        public virtual void VisitToken(SyntaxToken token)
        {
        }
    }
}