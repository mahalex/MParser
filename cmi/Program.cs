using Parser;
using System;
using System.IO;

namespace cmi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: cmi <file.m>");
            }

            var fileName = args[0];
            var text = File.ReadAllText(fileName);
            var tree = SyntaxTree.Parse(text);
            var compilation = Compilation.Create(tree);
            if (tree.Diagnostics.Diagnostics.Count > 0)
            {
                foreach (var diagnostic in tree.Diagnostics.Diagnostics)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{diagnostic.Span}: {diagnostic.Message}");
                    Console.ResetColor();
                }
            }

            var context = new CompilationContext();
            var evaluationResult = compilation.Evaluate(context);

            foreach (var diagnostic in evaluationResult.Diagnostics)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{diagnostic.Span}: {diagnostic.Message}");
                Console.ResetColor();
            }

            var result = evaluationResult.Value?.ToString();
            if (result is not null)
            {
                Console.WriteLine(result);
            }
        }
    }
}
