using Parser;
using System;

namespace cmi
{
    public class TreeRenderer
    {
        private static void RenderToken(SyntaxToken token, string indent, bool isLast)
        {
            Console.Write(indent + (isLast ? "└── " : "├── "));
            Console.Write($"<{token.Kind}>");
            Console.Write($" {token.Text}");
            Console.WriteLine();
        }

        private static void RenderNode(SyntaxNode node, string indent, bool isLast)
        {
            Console.Write(indent);
            Console.Write(isLast ? "└── " : "├── ");
            Console.Write($"<{node.Kind}>");
            Console.WriteLine();
            var children = node.GetChildNodesAndTokens();
            var last = children.Count - 1;
            indent += isLast ? "    " : "│   ";
            for (var index = 0; index <= last; index++)
            {
                var child = children[index];
                if (child.IsNode)
                {
                    RenderNode(child.AsNode()!, indent, index == last);
                }
                else if (child.IsToken)
                {
                    RenderToken(child.AsToken(), indent, index == last);
                }
            }
        }

        public static void RenderTree(SyntaxTree tree)
        {
            RenderNode(tree.Root, "", true);
        }
    }
}
