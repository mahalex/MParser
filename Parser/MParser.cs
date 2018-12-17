using System;
using System.Collections.Generic;
using Parser.Internal;
using System.Linq;

namespace Parser
{
    public class MParser
    {
        private readonly ITextWindow _window;

        public MParser(ITextWindow window)
        {
            _window = window;
        }

        public SyntaxTree Parse()
        {
            var lexer = new Internal.MLexerGreen(_window);
            var lexerDiagnostics = lexer.Diagnostics;
            var tokens = lexer.ParseAll();
            var parser = new Internal.MParserGreen(tokens, new Internal.SyntaxFactory());
            var green = parser.ParseFile();
            var parserDiagnostics = parser.Diagnostics;
            var totalDiagnostics = new DiagnosticsBag(lexerDiagnostics.Concat(parserDiagnostics));
            var root = new FileSyntaxNode(null, green, 0);
            return new SyntaxTree(root, totalDiagnostics);
        }
    }

    public class SyntaxTree
    {
        public SyntaxTree(FileSyntaxNode root, DiagnosticsBag diagnostics)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        }

        public FileSyntaxNode Root { get; }
        public DiagnosticsBag Diagnostics { get; }
    }

}