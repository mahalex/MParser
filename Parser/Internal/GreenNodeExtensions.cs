using System.Collections.Generic;
using System.Linq;

namespace Parser.Internal
{
    internal static class GreenNodeExtensions
    {
        public static TNode WithDiagnostics<TNode>(this TNode node, params TokenDiagnostic[] diagnostics)
            where TNode : GreenNode
        {
            return (TNode)node.SetDiagnostics(diagnostics.ToArray());
        }
    }
}
