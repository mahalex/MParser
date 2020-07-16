using System;
using System.Linq;
using Parser;

namespace Repl
{
    public class MRepl
    {
        private readonly CompilationContext _context;

        public MRepl()
        {
            _context = CompilationContext.Empty;
        }

        public void Run()
        {
            while (true)
            {
                var line = Read();
                if (line.StartsWith('#'))
                {
                    line = line.Trim();
                    if (line == "#q")
                    {
                        break;
                    }
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"Unknown command '{line}'.");
                    Console.ResetColor();
                    continue;
                }
                var result = Evaluate(line);
                Print(result);
            }
        }

        private void Print(string result)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(result);
            Console.ResetColor();
        }

        private string Read()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }

        private string Evaluate(string submission)
        {
            var tree = SyntaxTree.Parse(submission);
            if (tree.Diagnostics.Any())
            {
                foreach (var diagnostic in tree.Diagnostics.Diagnostics)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{diagnostic.Span}: {diagnostic.Message}");
                    Console.ResetColor();
                }
                TreeRenderer.RenderTree(tree);
                return string.Empty;
            }

            TreeRenderer.RenderTree(tree);
            var compilation = Compilation.Create(tree);
            var evaluationResult = compilation.Evaluate(_context, inRepl: true);

            foreach (var diagnostic in evaluationResult.Diagnostics)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{diagnostic.Span}: {diagnostic.Message}");
                Console.ResetColor();
            }

            return evaluationResult.Value?.ToString() ?? string.Empty;
        }
    }
}
