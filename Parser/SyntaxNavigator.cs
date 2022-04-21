using System.Collections.Generic;

namespace Parser
{
    public class SyntaxNavigator
    {
        public static SyntaxNavigator Singleton = new SyntaxNavigator();

        public IEnumerable<SyntaxToken> EnumerateTokens(SyntaxNode node)
        {
            foreach (var child in node.GetChildNodesAndTokens())
            {
                if (child.IsNode)
                {
                    foreach (var token in EnumerateTokens(child.AsNode()!))
                    {
                        yield return token;
                    }
                }
                if (child.IsToken)
                {
                    yield return child.AsToken();
                }
            }
        }
    }
}