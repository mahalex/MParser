using System;
using Parser;

namespace Repl
{
    public class MRepl
    {
        public void Run()
        {
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                var window = new TextWindowWithNull(line);
                var parser = new MParser(window);
                var tree = parser.Parse();
                if (tree.Diagnostics.Diagnostics.Count > 0)
                {
                    foreach (var diagnostic in tree.Diagnostics.Diagnostics)
                    {
                        Console.WriteLine($"{diagnostic.Span}: {diagnostic.Message}");
                    }
                }
                TreeRenderer.RenderTree(tree);
            }
        }
    }

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
                    RenderNode(child.AsNode(), indent, index == last);
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
