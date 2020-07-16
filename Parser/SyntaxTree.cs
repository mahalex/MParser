using System;
using Parser.Internal;

namespace Parser
{
    public class SyntaxTree
    {
        public SyntaxTree(RootSyntaxNode nullRoot, DiagnosticsBag diagnostics)
        {
            NullRoot = nullRoot ?? throw new ArgumentNullException(nameof(nullRoot));
            Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public RootSyntaxNode NullRoot { get; }
        public FileSyntaxNode Root => NullRoot.File;
        public DiagnosticsBag Diagnostics { get; }

        public static SyntaxTree Parse(string text)
        {
            var window = new TextWindowWithNull(text);
            var parser = new MParser(window);
            var tree = parser.Parse();
            return tree;
        }
    }

}