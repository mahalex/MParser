using Parser;
using System;
using System.IO;
using Mono.Options;
using System.Collections.Generic;

namespace cmc
{
    class Program
    {
        static int Main(string[] args)
        {
            var referencePaths = new List<string>();
            var outputPath = (string?)null;
            var moduleName = (string?)null;
            var helpRequested = false;
            var sourcePaths = new List<string>();
            var options = new OptionSet
            {
                "usage: cmc <source-paths> [options]",
                { "r=", "The {path} of an assembly to reference", v => referencePaths.Add(v) },
                { "o=", "The output {path} of the assembly to create", v => outputPath = v },
                { "m=", "The {name} of the module", v => moduleName = v },
                { "?|h|help", "Prints help", v => helpRequested = true },
                { "<>", v => sourcePaths.Add(v) }
            };

            options.Parse(args);
            if (helpRequested)
            {
                options.WriteOptionDescriptions(Console.Out);
                return 0;
            }

            if (sourcePaths.Count > 1)
            {
                Console.Error.WriteLine("Cannot compile more than one file.");
                return -1;
            }
            
            if (outputPath == null)
            {
                outputPath = Path.ChangeExtension(sourcePaths[0], ".exe");
            }

            if (moduleName == null)
            {
                moduleName = Path.GetFileNameWithoutExtension(outputPath);
            }


            var sourcePath = sourcePaths[0];
            var text = File.ReadAllText(sourcePath);
            var tree = SyntaxTree.Parse(text);
            var compilation = Compilation.Create(tree);
            compilation.Emit(referencePaths.ToArray(), outputPath);
            return 0;
        }
    }
}
